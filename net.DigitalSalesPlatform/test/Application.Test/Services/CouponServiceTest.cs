using Application.Contracts.Constants.Coupon;
using Application.Contracts.Requests.Coupon;
using Application.Contracts.Validations.Coupon;
using Application.Services;
using Application.Test.Mocks.FakeData;
using Application.Test.Mocks.Repositories;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Core.Test.Helper;
using Xunit;

namespace Application.Test.Services;

public class CouponServiceTest : CouponMockRepository
{
    private readonly CouponService _couponService;
    private readonly CreateCouponRequestValidator _createCouponRequestValidator;

    public CouponServiceTest(
        CouponFakeData fakeData,
        CreateCouponRequestValidator createCouponRequestValidator) : base(fakeData)
    {
        _createCouponRequestValidator = createCouponRequestValidator;
        var cacheService = CacheMockHelper.GetCacheService();
        _couponService = new CouponService(MockRepository.Object, cacheService, UnitOfWork.Object,
            Mapper);
    }

    [Fact]
    public void CreateCouponValidRequestShouldReturnSuccess()
    {
        var request = new CreateCouponRequest { Amount = 1, ExpirationDate = DateTime.Now.AddDays(10) };
        _couponService.CreateCoupon(request);
        var coupon = _couponService.GetAllCoupons().Find(x => x.Amount == request.Amount);
        Assert.NotNull(coupon);
        Assert.Equal(request.Amount, coupon.Amount);
    }

    [Fact]
    public void CreateCouponValidRequestShouldThrowExpirationDateException()
    {
        var request = new CreateCouponRequest { Amount = 10, ExpirationDate = DateTime.Now.AddDays(-10) };
        var exception = Assert.Throws<BusinessException>(() => _couponService.CreateCoupon(request));
        Assert.Equal(CouponBusinessMessages.ExpirationDateIsGreaterThanToday, exception.Message);
    }

    [Fact]
    public void CreateCouponInValidRequestShouldThrowValidationException()
    {
        var request = new CreateCouponRequest { Amount = -5, ExpirationDate = DateTime.Now.AddDays(1) };
        var result = _createCouponRequestValidator
            .Validate(request)
            .Errors
            .Where(x => x.PropertyName == "Amount")
            .Select(x => x.ErrorMessage).FirstOrDefault();
        Assert.NotNull(request);
        Assert.Equal(CouponValidationMessages.AmountGreaterThanZero, result);
    }

    [Fact]
    public void UpdateCouponIsUsedShouldReturnSuccess()
    {
        const string couponCode = "11111111";
        _couponService.UpdateCouponIsUsed(couponCode);
        var coupon = _couponService.GetAllCoupons().Find(x => x.Code == couponCode);
        Assert.NotNull(coupon);
        Assert.True(coupon.IsUsed);
    }

    [Fact]
    public void UpdateCouponIsUsedShouldThrowNotFoundException()
    {
        const string couponCode = "00000000";
        var exception = Assert.Throws<NotFoundException>(() => _couponService.UpdateCouponIsUsed(couponCode));
        Assert.Equal(CouponBusinessMessages.CouponNotFoundByCode, exception.Message);
    }

    [Fact]
    public void UpdateCouponIsUsedShouldThrowAlreadyUsedException()
    {
        const string couponCode = "44444444";
        var exception = Assert.Throws<BusinessException>(() => _couponService.UpdateCouponIsUsed(couponCode));
        Assert.Equal(CouponBusinessMessages.CouponIsUsed, exception.Message);
    }

    [Fact]
    public void UpdateCouponIsUsedShouldThrowExpiredException()
    {
        const string couponCode = "22222222";
        var exception = Assert.Throws<BusinessException>(() => _couponService.UpdateCouponIsUsed(couponCode));
        Assert.Equal(CouponBusinessMessages.CouponExpired, exception.Message);
    }

    [Fact]
    public void UpdateCouponChangeUsedShouldReturnSuccess()
    {
        var id = new Guid("11111111-1111-1111-1111-111111111111");
        _couponService.UpdateCouponChangeUsed(id);
        var coupon = _couponService.GetAllCoupons().Find(x => x.Id == id);
        Assert.NotNull(coupon);
        Assert.False(coupon.IsUsed);
    }

    [Fact]
    public void UpdateCouponChangeUsedShouldThrowNotFoundException()
    {
        var id = Guid.Empty;
        var exception = Assert.Throws<NotFoundException>(() => _couponService.UpdateCouponChangeUsed(id));
        Assert.Equal(CouponBusinessMessages.CouponNotFoundById, exception.Message);
    }

    [Fact]
    public void UpdateCouponStatusShouldReturnSuccess()
    {
        var id = new Guid("11111111-1111-1111-1111-111111111111");
        const bool isActive = false;
        _couponService.UpdateCouponStatus(id, isActive);
        var coupon = _couponService.GetAllCoupons().Find(x => x.Id == id);
        Assert.NotNull(coupon);
        Assert.False(coupon.IsActive);
    }

    [Fact]
    public void UpdateCouponStatusShouldThrowNotFoundException()
    {
        var id = Guid.Empty;
        const bool isActive = false;
        var exception = Assert.Throws<NotFoundException>(() => _couponService.UpdateCouponStatus(id, isActive));
        Assert.Equal(CouponBusinessMessages.CouponNotFoundById, exception.Message);
    }

    [Fact]
    public void GetCouponByCodeShouldReturnSuccess()
    {
        const string couponCode = "11111111";
        var coupon = _couponService.GetCouponByCode(couponCode);
        Assert.NotNull(coupon);
        Assert.Equal(couponCode, coupon.Code);
    }

    [Fact]
    public void GetCouponByCodeShouldThrowNotFoundException()
    {
        const string couponCode = "00000000";
        var exception = Assert.Throws<NotFoundException>(() => _couponService.GetCouponByCode(couponCode));
        Assert.Equal(CouponBusinessMessages.CouponNotFoundByCode, exception.Message);
    }

    [Fact]
    public void GetCouponByCodeShouldThrowAlreadyUsedException()
    {
        const string couponCode = "44444444";
        var exception = Assert.Throws<BusinessException>(() => _couponService.GetCouponByCode(couponCode));
        Assert.Equal(CouponBusinessMessages.CouponIsUsed, exception.Message);
    }

    [Fact]
    public void GetCouponByCodeShouldThrowExpiredException()
    {
        const string couponCode = "22222222";
        var exception = Assert.Throws<BusinessException>(() => _couponService.GetCouponByCode(couponCode));
        Assert.Equal(CouponBusinessMessages.CouponExpired, exception.Message);
    }

    [Fact]
    public void GetCouponByCodeShouldThrowInactiveException()
    {
        const string couponCode = "33333333";
        var exception = Assert.Throws<BusinessException>(() => _couponService.GetCouponByCode(couponCode));
        Assert.Equal(CouponBusinessMessages.CouponIsNotActive, exception.Message);
    }

    [Fact]
    public void GetAllCouponsShouldReturnSuccess()
    {
        var coupons = _couponService.GetAllCoupons();
        Assert.NotNull(coupons);
        Assert.Equal(4, coupons.Count);
    }

    [Fact]
    public void GetAllActiveCouponsShouldReturnSuccess()
    {
        var coupons = _couponService.GetAllActiveCoupons();
        Assert.NotNull(coupons);
        Assert.Equal(3, coupons.Count);
    }
}