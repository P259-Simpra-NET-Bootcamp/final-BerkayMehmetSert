using Application.Contracts.Requests.User;
using Application.Contracts.Responses.Token;
using Application.Contracts.Responses.User;
using Domain.Entities;

namespace Application.Contracts.Services;

public interface IUserService
{
    TokenResponse Login(LoginRequest request);
    void RegisterUser(RegisterUserRequest request);
    void RegisterAdmin(RegisterAdminRequest request);
    void UpdateUser(UpdateUserRequest request);
    void ChangePassword(ChangePasswordRequest request);
    void UpdateUserPointBalance(User user);
    void DeleteUser(Guid id);
    UserResponse GetUserById(Guid? id = null);
    UserPointResponse GetUserPointById(Guid? id = null);
    Guid GetUserIdFromToken();
    List<UserResponse> GetAllUsers();
    User GetUserEntity(Guid? id = null);
}