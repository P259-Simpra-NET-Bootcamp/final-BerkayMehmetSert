using System.Security.Claims;
using Application.Contracts.Constants.User;
using Application.Contracts.Requests.User;
using Application.Contracts.Responses.Token;
using Application.Contracts.Services;
using Application.Contracts.Validations.User;
using Application.Services;
using Application.Test.Mocks.FakeData;
using Application.Test.Mocks.Repositories;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace Application.Test.Services;

public class UserServiceTest : UserMockRepository
{
    private readonly UserService _userService;
    private readonly Mock<ITokenService> _tokenServiceMock = new();
    private readonly Mock<IHttpContextAccessor> _contextAccessorMock = new();
    private readonly LoginRequestRequestValidator _loginRequestRequestValidator;
    private readonly RegisterUserRequestValidator _registerUserRequestValidator;
    private readonly RegisterAdminRequestValidator _registerAdminRequestValidator;
    private readonly UpdateUserRequestValidator _updateUserRequestValidator;
    private readonly ChangePasswordRequestValidator _changePasswordRequestValidator;

    public UserServiceTest(
        UserFakeData fakeData,
        LoginRequestRequestValidator loginRequestRequestValidator,
        RegisterUserRequestValidator registerUserRequestValidator,
        RegisterAdminRequestValidator registerAdminRequestValidator,
        UpdateUserRequestValidator updateUserRequestValidator,
        ChangePasswordRequestValidator changePasswordRequestValidator) :
        base(fakeData)
    {
        _loginRequestRequestValidator = loginRequestRequestValidator;
        _registerUserRequestValidator = registerUserRequestValidator;
        _registerAdminRequestValidator = registerAdminRequestValidator;
        _updateUserRequestValidator = updateUserRequestValidator;
        _changePasswordRequestValidator = changePasswordRequestValidator;
        _tokenServiceMock.Setup(x => x.CreateToken(It.IsAny<User>()))
            .Returns(new TokenResponse() { AccessToken = "1" });
        _contextAccessorMock.Setup(x => x.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier))
            .Returns(new Claim(ClaimTypes.NameIdentifier, "11111111-1111-1111-1111-111111111111"));
        _userService = new UserService(
            MockRepository.Object,
            _tokenServiceMock.Object,
            UnitOfWork.Object,
            Mapper,
            _contextAccessorMock.Object
        );
    }

    [Fact]
    public void LoginValidRequestShouldReturnSuccess()
    {
        var request = new LoginRequest { Email = "user2@system.com", Password = "1234" };
        var result = _userService.Login(request);
        Assert.NotNull(result);
        Assert.Equal("1", result.AccessToken);
    }

    [Fact]
    public void LoginShouldValidRequestThrowNotFoundException()
    {
        var request = new LoginRequest { Email = "system@system.com", Password = "1234" };
        var exception = Assert.Throws<NotFoundException>(() => _userService.Login(request));
        Assert.Equal(UserBusinessMessages.UserNotFoundByEmail, exception.Message);
    }

    [Fact]
    public void LoginShouldValidRequestThrowNotActiveException()
    {
        var request = new LoginRequest { Email = "user@system.com", Password = "1234" };
        var exception = Assert.Throws<BusinessException>(() => _userService.Login(request));
        Assert.Equal(UserBusinessMessages.UserIsNotActive, exception.Message);
    }

    [Fact]
    public void LoginShouldValidRequestThrowInValidPasswordException()
    {
        var request = new LoginRequest { Email = "user2@system.com", Password = "123456" };
        var exception = Assert.Throws<BusinessException>(() => _userService.Login(request));
        Assert.Equal(UserBusinessMessages.InvalidPassword, exception.Message);
    }

    [Fact]
    public void LoginShouldInValidRequestThrowValidationException()
    {
        var request = new LoginRequest { Email = "", Password = "1234" };
        var result = _loginRequestRequestValidator
            .Validate(request)
            .Errors
            .Where(x => x.PropertyName.Equals("Email"))
            .Select(x => x.ErrorMessage).FirstOrDefault();
        Assert.NotNull(request);
        Assert.Equal(UserValidationMessages.EmailRequired, result);
    }

    [Fact]
    public void RegisterUserValidRequestShouldReturnSuccess()
    {
        var request = new RegisterUserRequest { Email = "system@system.com", Password = "1234" };
        _userService.RegisterUser(request);
        MockRepository.Verify(x => x.Add(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public void RegisterUserValidRequestShouldThrowAlreadyExistsException()
    {
        var request = new RegisterUserRequest { Email = "admin@system.com", Password = "1234" };
        var exception = Assert.Throws<NotFoundException>(() => _userService.RegisterUser(request));
        Assert.Equal(UserBusinessMessages.UserAlreadyExistByEmail, exception.Message);
    }

    [Fact]
    public void RegisterUserShouldInValidRequestThrowValidationException()
    {
        var request = new RegisterUserRequest { Email = "", Password = "1234" };
        var result = _registerUserRequestValidator
            .Validate(request)
            .Errors
            .Where(x => x.PropertyName.Equals("Email"))
            .Select(x => x.ErrorMessage).FirstOrDefault();
        Assert.NotNull(request);
        Assert.Equal(UserValidationMessages.EmailRequired, result);
    }

    [Fact]
    public void RegisterAdminValidRequestShouldReturnSuccess()
    {
        var request = new RegisterAdminRequest { Email = "system@system.com", Password = "1234" };
        _userService.RegisterAdmin(request);
        MockRepository.Verify(x => x.Add(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public void RegisterAdminValidRequestShouldThrowAlreadyExistsException()
    {
        var request = new RegisterAdminRequest { Email = "admin@system.com", Password = "1234" };
        var exception = Assert.Throws<NotFoundException>(() => _userService.RegisterAdmin(request));
        Assert.Equal(UserBusinessMessages.UserAlreadyExistByEmail, exception.Message);
    }

    [Fact]
    public void RegisterAdminInValidRequestShouldThrowValidationExistsException()
    {
        var request = new RegisterAdminRequest { Email = "", Password = "1234" };
        var result = _registerAdminRequestValidator
            .Validate(request)
            .Errors
            .Where(x => x.PropertyName.Equals("Email"))
            .Select(x => x.ErrorMessage).FirstOrDefault();
        Assert.NotNull(request);
        Assert.Equal(UserValidationMessages.EmailRequired, result);
    }

    [Fact]
    public void UpdateUserValidRequestShouldReturnSuccess()
    {
        var request = new UpdateUserRequest { FirstName = "System", LastName = "System" };
        _userService.UpdateUser(request);
        MockRepository.Verify(x => x.Update(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public void UpdateUserValidRequestShouldThrowNotFoundException()
    {
        var userId = Guid.Empty;
        var request = new UpdateUserRequest { FirstName = "System", LastName = "System" };
        _contextAccessorMock.Setup(x => x.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier))
            .Returns(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));
        var exception = Assert.Throws<NotFoundException>(() => _userService.UpdateUser(request));
        Assert.Equal(UserBusinessMessages.UserNotFoundById, exception.Message);
    }

    [Fact]
    public void UpdateUserValidRequestShouldThrowNotActiveException()
    {
        var userId = new Guid("22222222-2222-2222-2222-222222222222");
        var exception = Assert.Throws<BusinessException>(() => _userService.GetUserEntity(userId));
        Assert.Equal(UserBusinessMessages.UserIsNotActive, exception.Message);
    }

    [Fact]
    public void UpdateUserInValidRequestShouldThrowValidationException()
    {
        var request = new UpdateUserRequest { FirstName = "", LastName = "" };
        var result = _updateUserRequestValidator
            .Validate(request)
            .Errors
            .Where(x => x.PropertyName.Equals("FirstName"))
            .Select(x => x.ErrorMessage).FirstOrDefault();
        Assert.NotNull(request);
        Assert.Equal(UserValidationMessages.FirstNameRequired, result);
    }

    [Fact]
    public void ChangePasswordValidRequestShouldReturnSuccess()
    {
        var request = new ChangePasswordRequest
        {
            OldPassword = "1234",
            NewPassword = "12345",
            ConfirmPassword = "12345"
        };
        _userService.ChangePassword(request);
        MockRepository.Verify(x => x.Update(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public void ChangePasswordValidRequestShouldThrowPasswordNotMatchWithOldPassword()
    {
        var request = new ChangePasswordRequest
        {
            OldPassword = "12345",
            NewPassword = "12345",
            ConfirmPassword = "12345"
        };
        var exception = Assert.Throws<BusinessException>(() => _userService.ChangePassword(request));
        Assert.Equal(UserBusinessMessages.PasswordNotMatchWithOldPassword, exception.Message);
    }

    [Fact]
    public void ChangePasswordValidRequestShouldThrowPasswordNotMatchWithConfirmPassword()
    {
        var request = new ChangePasswordRequest
        {
            OldPassword = "1234",
            NewPassword = "12345",
            ConfirmPassword = "123456"
        };
        var exception = Assert.Throws<BusinessException>(() => _userService.ChangePassword(request));
        Assert.Equal(UserBusinessMessages.PasswordNotMatchWithConfirmPassword, exception.Message);
    }

    [Fact]
    public void ChangePasswordInValidRequestShouldThrowValidationException()
    {
        var request = new ChangePasswordRequest { OldPassword = "", };
        var result = _changePasswordRequestValidator
            .Validate(request)
            .Errors
            .Where(x => x.PropertyName.Equals("OldPassword"))
            .Select(x => x.ErrorMessage).FirstOrDefault();
        Assert.NotNull(request);
        Assert.Equal(UserValidationMessages.PasswordRequired, result);
    }

    [Fact]
    public void UpdateUserPointBalanceShouldReturnSuccess()
    {
        var request = new User { Id = new Guid("11111111-1111-1111-1111-111111111111"), PointBalance = 100 };
        _userService.UpdateUserPointBalance(request);
        MockRepository.Verify(x => x.Update(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public void DeleteUserShouldReturnSuccess()
    {
        var userId = new Guid("11111111-1111-1111-1111-111111111111");
        _userService.DeleteUser(userId);
        MockRepository.Verify(x => x.SoftDelete(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public void DeleteUserShouldThrowNotFoundException()
    {
        var userId = new Guid("44444444-4444-4444-4444-444444444444");
        _contextAccessorMock.Setup(x => x.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier))
            .Returns(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));
        var exception = Assert.Throws<NotFoundException>(() => _userService.DeleteUser(userId));
        Assert.Equal(UserBusinessMessages.UserNotFoundById, exception.Message);
    }

    [Fact]
    public void DeleteUserShouldThrowNotActiveException()
    {
        var userId = new Guid("22222222-2222-2222-2222-222222222222");
        var exception = Assert.Throws<BusinessException>(() => _userService.DeleteUser(userId));
        Assert.Equal(UserBusinessMessages.UserIsNotActive, exception.Message);
    }

    [Fact]
    public void GetUserByIdShouldReturnSuccess()
    {
        var userId = new Guid("11111111-1111-1111-1111-111111111111");
        var result = _userService.GetUserById(userId);
        Assert.NotNull(result);
        Assert.Equal(userId, result.Id);
    }

    [Fact]
    public void GetUserByIdShouldReturnSuccessWithNoParameters()
    {
        var userId = new Guid("11111111-1111-1111-1111-111111111111");
        var result = _userService.GetUserById();
        Assert.NotNull(result);
        Assert.Equal(userId, result.Id);
    }

    [Fact]
    public void GetUserByIdShouldThrowNotFoundException()
    {
        var userId = Guid.Empty;
        var exception = Assert.Throws<NotFoundException>(() => _userService.GetUserById(userId));
        Assert.Equal(UserBusinessMessages.UserNotFoundById, exception.Message);
    }

    [Fact]
    public void GetUserByIdShouldThrowNotActiveException()
    {
        var userId = new Guid("22222222-2222-2222-2222-222222222222");
        var exception = Assert.Throws<BusinessException>(() => _userService.GetUserById(userId));
        Assert.Equal(UserBusinessMessages.UserIsNotActive, exception.Message);
    }

    [Fact]
    public void GetUserPointByIdShouldReturnSuccess()
    {
        var userId = new Guid("11111111-1111-1111-1111-111111111111");
        var result = _userService.GetUserPointById(userId);
        Assert.NotNull(result);
        Assert.Equal(userId, result.Id);
    }

    [Fact]
    public void GetUserPointByIdShouldReturnSuccessWithNoParameters()
    {
        var userId = new Guid("11111111-1111-1111-1111-111111111111");
        var result = _userService.GetUserPointById();
        Assert.NotNull(result);
        Assert.Equal(userId, result.Id);
    }

    [Fact]
    public void GetUserPointByIdShouldThrowNotFoundException()
    {
        var userId = Guid.Empty;
        var exception = Assert.Throws<NotFoundException>(() => _userService.GetUserPointById(userId));
        Assert.Equal(UserBusinessMessages.UserNotFoundById, exception.Message);
    }

    [Fact]
    public void GetUserPointByIdShouldThrowNotActiveException()
    {
        var userId = new Guid("22222222-2222-2222-2222-222222222222");
        var exception = Assert.Throws<BusinessException>(() => _userService.GetUserPointById(userId));
        Assert.Equal(UserBusinessMessages.UserIsNotActive, exception.Message);
    }

    [Fact]
    public void GetAllUsersShouldReturnSuccess()
    {
        var result = _userService.GetAllUsers();
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void GetUserIdFromTokenShouldReturnSuccess()
    {
        var userId = new Guid("11111111-1111-1111-1111-111111111111");
        var result = _userService.GetUserIdFromToken();
        Assert.Equal(userId, result);
    }

    [Fact]
    public void GetUserEntityShouldReturnSuccess()
    {
        var userId = new Guid("11111111-1111-1111-1111-111111111111");
        var result = _userService.GetUserEntity(userId);
        Assert.NotNull(result);
        Assert.Equal(userId, result.Id);
    }

    [Fact]
    public void GetUserEntityShouldReturnSuccessWithNoParameters()
    {
        var userId = new Guid("11111111-1111-1111-1111-111111111111");
        var result = _userService.GetUserEntity();
        Assert.NotNull(result);
        Assert.Equal(userId, result.Id);
    }

    [Fact]
    public void GetUserEntityShouldThrowNotFoundException()
    {
        var userId = Guid.Empty;
        var exception = Assert.Throws<NotFoundException>(() => _userService.GetUserEntity(userId));
        Assert.Equal(UserBusinessMessages.UserNotFoundById, exception.Message);
    }

    [Fact]
    public void GetUserEntityShouldThrowNotActiveException()
    {
        var userId = new Guid("22222222-2222-2222-2222-222222222222");
        var exception = Assert.Throws<BusinessException>(() => _userService.GetUserEntity(userId));
        Assert.Equal(UserBusinessMessages.UserIsNotActive, exception.Message);
    }
}