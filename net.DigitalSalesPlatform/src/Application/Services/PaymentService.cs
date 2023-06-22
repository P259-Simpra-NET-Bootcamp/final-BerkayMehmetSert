using System.Transactions;
using Application.Contracts.Constants.Cart;
using Application.Contracts.Requests.Payment;
using Application.Contracts.Responses.Cart;
using Application.Contracts.Responses.Payment;
using Application.Contracts.Services;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Domain.Entities;
using Infrastructure.Notification;
using Infrastructure.Notification.Model;
using Infrastructure.Payment;
using Infrastructure.Payment.Constants;
using Infrastructure.Payment.Requests;

namespace Application.Services;

public class PaymentService : IPaymentService
{
    private readonly IUserService _userService;
    private readonly ICartService _cartService;
    private readonly ICouponService _couponService;
    private readonly IProductService _productService;
    private readonly Func<NotificationType, INotificationService> _notificationService;
    private readonly Func<PaymentMethod, IFakePaymentService> _fakePaymentService;

    private const NotificationType Type = NotificationType.RabbitMq;

    public PaymentService(
        IUserService userService,
        ICartService cartService,
        ICouponService couponService,
        IProductService productService,
        Func<NotificationType, INotificationService> notificationService,
        Func<PaymentMethod, IFakePaymentService> fakePaymentService)
    {
        _userService = userService;
        _cartService = cartService;
        _couponService = couponService;
        _productService = productService;
        _notificationService = notificationService;
        _fakePaymentService = fakePaymentService;
    }

    public PaymentResponse ProcessPayment(PaymentRequest request)
    {
        var user = _userService.GetUserEntity();
        var cartItems = GetCartItems();
        var fakePaymentService = _fakePaymentService(request.PaymentMethod);
        var remainingAmount = cartItems.TotalPrice;
        var rollbackActions = new List<Action>();

        using var transaction = new TransactionScope();
        try
        {
            var couponAmount = ApplyCoupon(request.CouponCode, ref remainingAmount, rollbackActions);
            var spentPoint = ProcessPaymentWithPoints(user, ref remainingAmount, rollbackActions);
            var earnedPoint = CalculatePoints(user, cartItems, remainingAmount, rollbackActions);
            var isPaid = MakePayment(fakePaymentService, remainingAmount, request.CreditCard, rollbackActions);

            UpdateProductStock(cartItems, isPaid);
            _userService.UpdateUserPointBalance(user);

            var result = new PaymentResponse
            {
                UserId = user.Id,
                CouponCode = request.CouponCode,
                CouponAmount = couponAmount,
                SpentPoint = spentPoint,
                EarnedPoint = earnedPoint,
                Cart = cartItems
            };

            _cartService.ClearCart();
            transaction.Complete();

            var notification = new Notification
            {
                EarnedPoint = earnedPoint.ToString(),
                CustomerName = user.FirstName,
                CustomerEmail = user.Email,
                Message = PaymentBusinessMessages.PaymentCompleted
            };

            _notificationService(Type).SendNotification(notification);
            return result;
        }
        catch (Exception exception)
        {
            RollbackActions(rollbackActions);
            throw;
        }
    }

    // Bu method sepetteki ürünleri getirir. Sepette ürün yoksa hata fırlatır.
    private CartResponse GetCartItems()
    {
        var cartItems = _cartService.GetCartItems();
        if (!cartItems.CartItems.Any())
            throw new BusinessException(CartBusinessMessages.CartNotFound);

        return cartItems;
    }


    // Bu method kupon ile ödeme işlemini gerçekleştirir. Kupon kodu boş ise kupon uygulanmaz.
    // Kupon tutarı sepet tutarını karşılıyorsa kalan tutar 0 olur.
    private decimal ApplyCoupon(string couponCode, ref decimal remainingAmount, List<Action> rollbackActions)
    {
        if (!string.IsNullOrEmpty(couponCode) && !string.IsNullOrWhiteSpace(couponCode))
        {
            var coupon = _couponService.GetCouponEntityByCode(couponCode);
            remainingAmount -= coupon.Amount;
            _couponService.UpdateCouponIsUsed(couponCode);
            rollbackActions.Add(() => _couponService.UpdateCouponChangeUsed(coupon.Id));
            return coupon.Amount;
        }

        return 0;
    }

    // Bu method puan ile ödeme işlemini gerçekleştirir.
    // Puan tutarı sepet tutarını karşılıyorsa kalan tutar 0 olur.
    // Kullanıcı puanı yetersiz ise puan ile ödeme yapılmaz.
    // Kullanıcının puanı harcanan puan kadar azaltılır.
    private decimal ProcessPaymentWithPoints(User user, ref decimal remainingAmount, List<Action> rollbackActions)
    {
        var paymentWithPoints = Math.Min(remainingAmount, user.PointBalance);
        remainingAmount -= paymentWithPoints;
        user.PointBalance -= paymentWithPoints;
        rollbackActions.Add(() =>
        {
            user.PointBalance += paymentWithPoints;
            _userService.UpdateUserPointBalance(user);
        });

        return paymentWithPoints;
    }

    // Bu method siparişten kazanılan puanı hesaplar.
    // Her ürün için kazanılan puan hesaplanır.
    // Kazanılan puan kullanıcının puan bakiyesine eklenir.
    private decimal CalculatePoints(User user, CartResponse cartItems, decimal remainingAmount,
        List<Action> rollbackActions)
    {
        decimal earnedPoints = 0;
        foreach (var cartItem in cartItems.CartItems)
        {
            var product = cartItem.Product;
            var earningRate = product.ScorePercentage;
            var maximumPoints = product.MaxScore;

            earnedPoints = remainingAmount * earningRate;
            earnedPoints = Math.Min(earnedPoints, maximumPoints);
            user.PointBalance += earnedPoints;
            rollbackActions.Add(() =>
            {
                user.PointBalance -= earnedPoints;
                _userService.UpdateUserPointBalance(user);
            });
        }

        return earnedPoints;
    }

    // Bu method ödeme işlemini gerçekleştirir.
    // Ödeme başarısız olursa hata fırlatılır.
    private bool MakePayment(IFakePaymentService fakePaymentService, decimal remainingAmount,
        CreditCardRequest creditCard, List<Action> rollbackActions)
    {
        var isPaid = true;
        if (remainingAmount > 0)
        {
            isPaid = fakePaymentService.Pay(remainingAmount, creditCard);
            rollbackActions.Add(() => fakePaymentService.Refund(remainingAmount, creditCard));

            if (!isPaid)
                throw new BusinessException(PaymentBusinessMessages.PaymentFailed);
        }

        return isPaid;
    }

    // Bu method ürün stoklarını günceller.
    private void UpdateProductStock(CartResponse cartItems, bool isPaid)
    {
        foreach (var item in cartItems.CartItems)
        {
            var productId = item.Product.Id;
            var quantity = item.Quantity;
            if (isPaid)
            {
                _productService.UpdateProductStockDecrement(productId, quantity);
            }
            else
            {
                _productService.UpdateProductStockIncrement(productId, quantity);
            }
        }
    }

    // Bu method işlemleri geri alır.
    private void RollbackActions(List<Action> rollbackActions)
    {
        foreach (var rollbackAction in rollbackActions)
        {
            rollbackAction();
        }
    }
}