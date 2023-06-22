using System.Security.Claims;
using Application.Contracts.Constants.User;
using Application.Contracts.Repositories;
using Application.Contracts.Requests.User;
using Application.Contracts.Responses.Token;
using Application.Contracts.Responses.User;
using Application.Contracts.Services;
using AutoMapper;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Core.Persistence.Repositories;
using Core.Utilities.Date;
using Core.Utilities.Security;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _contextAccessor;

    public UserService(
        IUserRepository userRepository,
        ITokenService tokenService,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IHttpContextAccessor contextAccessor)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _contextAccessor = contextAccessor;
    }

    public TokenResponse Login(LoginRequest request)
    {
        var user = _userRepository.Get(x => x.Email.Equals(request.Email));

        if (user is null)
            throw new NotFoundException(UserBusinessMessages.UserNotFoundByEmail);
        if (!user.IsActive)
            throw new BusinessException(UserBusinessMessages.UserIsNotActive);
        if (!HashingHelper.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            throw new BusinessException(UserBusinessMessages.InvalidPassword);
        
        return _tokenService.CreateToken(user);
    }

    public void RegisterUser(RegisterUserRequest request) => RegisterUserInternal(request, "User");

    public void RegisterAdmin(RegisterAdminRequest request) => RegisterUserInternal(request, "Admin");
    
    public void UpdateUser(UpdateUserRequest request)
    {
        var user = GetUserEntity();

        var updatedUser = _mapper.Map(request, user);

        _userRepository.Update(updatedUser);
        _unitOfWork.SaveChanges();
    }

    public void ChangePassword(ChangePasswordRequest request)
    {
        var user = GetUserEntity();

        if (!HashingHelper.VerifyPasswordHash(request.OldPassword, user.PasswordHash, user.PasswordSalt))
            throw new BusinessException(UserBusinessMessages.PasswordNotMatchWithOldPassword);
        if (request.NewPassword != request.ConfirmPassword)
            throw new BusinessException(UserBusinessMessages.PasswordNotMatchWithConfirmPassword);

        HashingHelper.CreatePasswordHash(request.NewPassword, out var passwordHash, out var passwordSalt);

        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

        _userRepository.Update(user);
        _unitOfWork.SaveChanges();
    }

    public void UpdateUserPointBalance(User user)
    {
        user.LastTransactionDate = DateHelper.GetCurrentDate();
        _userRepository.Update(user);
        _unitOfWork.SaveChanges();
    }

    public void DeleteUser(Guid id)
    {
        var user = GetUserEntity(id);
        user.IsActive = false;

        _userRepository.SoftDelete(user);
        _unitOfWork.SaveChanges();
    }

    public UserResponse GetUserById(Guid? id = null)
    {
        var user = GetUserEntity(id);
        return _mapper.Map<UserResponse>(user);
    }

    public UserPointResponse GetUserPointById(Guid? id = null)
    {
        var user = GetUserEntity(id);
        return _mapper.Map<UserPointResponse>(user);
    }

    public List<UserResponse> GetAllUsers()
    {
        var users = _userRepository.GetAll(x => x.IsActive);
        return _mapper.Map<List<UserResponse>>(users);
    }

    public Guid GetUserIdFromToken()
    {
        var userId = _contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            throw new NotFoundException(UserBusinessMessages.UserNotFoundById);

        return Guid.Parse(userId);
    }

    public User GetUserEntity(Guid? id = null)
    {
        var userId = id ?? GetUserIdFromToken();
        var user = _userRepository.Get(x => x.Id.Equals(userId));

        if (user is null)
            throw new NotFoundException(UserBusinessMessages.UserNotFoundById);
        if (!user.IsActive)
            throw new BusinessException(UserBusinessMessages.UserIsNotActive);

        return user;
    }

    private void RegisterUserInternal<T>(T request, string role) where T : RegisterUserRequest
    {
        CheckIfUserExistByEmail(request.Email);

        HashingHelper.CreatePasswordHash(request.Password, out var passwordHash, out var passwordSalt);

        var user = _mapper.Map<User>(request);
        user.Role = role;
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;
        user.PointBalance = role.Equals("User") ? 100 : 0;

        _userRepository.Add(user);
        _unitOfWork.SaveChanges();
    }

    private void CheckIfUserExistByEmail(string email)
    {
        var user = _userRepository.Get(x => x.Email.Equals(email) && x.IsActive);
        if (user is not null)
            throw new NotFoundException(UserBusinessMessages.UserAlreadyExistByEmail);
    }
}