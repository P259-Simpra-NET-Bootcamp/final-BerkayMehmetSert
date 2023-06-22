using Application.Contracts.Constants.Cart;
using Application.Contracts.Requests.Payment;
using Application.Contracts.Responses.Cart;
using Application.Contracts.Services;
using Application.Services;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Domain.Entities;
using Infrastructure.Notification;
using Infrastructure.Notification.Model;
using Infrastructure.Payment;
using Infrastructure.Payment.Requests;
using Moq;
using Xunit;

namespace Application.Test.Services;

public class PaymentServiceTest
{
    private readonly Mock<IUserService> _userServiceMock = new();
    private readonly Mock<ICartService> _cartServiceMock = new();
    private readonly Mock<ICouponService> _couponServiceMock = new();
    private readonly Mock<IProductService> _productServiceMock = new();
    private readonly Mock<Func<NotificationType, INotificationService>> _notificationServiceMock = new();
    private readonly Mock<Func<PaymentMethod, IFakePaymentService>> _fakePaymentServiceFactoryMock = new();
    private readonly PaymentService _paymentService;

    public PaymentServiceTest()
    {
        _paymentService = new PaymentService(
            _userServiceMock.Object,
            _cartServiceMock.Object,
            _couponServiceMock.Object,
            _productServiceMock.Object,
            _notificationServiceMock.Object,
            _fakePaymentServiceFactoryMock.Object);
    }

    [Fact]
    public void ProcessPaymentShouldThrowCartItemNotFoundException()
    {
        var userId = new Guid("11111111-1111-1111-1111-111111111111");
        var user = new User { Id = userId, PointBalance = 100 };
        var cartItems = new CartResponse
        {
            CartItems = new List<CartItem>()
        };
        _userServiceMock.Setup(x => x.GetUserEntity(userId)).Returns(user);
        _cartServiceMock.Setup(x => x.GetCartItems()).Returns(cartItems);
        var request = new PaymentRequest
        {
            PaymentMethod = PaymentMethod.CreditCard,
            CreditCard = new CreditCardRequest()
        };
        var exception = Assert.Throws<BusinessException>(() => _paymentService.ProcessPayment(request));
        Assert.Equal(CartBusinessMessages.CartNotFound, exception.Message);
    }
}