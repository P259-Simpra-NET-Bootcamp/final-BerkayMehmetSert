using Application.Contracts.Constants.Coupon;
using Application.Contracts.Repositories;
using Application.Contracts.Requests.Coupon;
using Application.Contracts.Responses.Coupon;
using Application.Contracts.Services;
using AutoMapper;
using Core.Application.Caching;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Core.Persistence.Repositories;
using Core.Utilities.Code;
using Core.Utilities.Date;
using Domain.Entities;

namespace Application.Services;

public class CouponService : ICouponService
{
    private readonly ICouponRepository _couponRepository;
    private readonly Func<CacheType, ICacheService> _cacheService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    private const CacheType Cache = CacheType.Redis;

    public CouponService(
        ICouponRepository couponRepository,
        Func<CacheType, ICacheService> cacheService,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _couponRepository = couponRepository;
        _cacheService = cacheService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public void CreateCoupon(CreateCouponRequest request)
    {
        if (request.ExpirationDate < DateHelper.GetCurrentDate())
            throw new BusinessException(CouponBusinessMessages.ExpirationDateIsGreaterThanToday);
        
        var cacheKey = GetCacheKey();
        var coupon = _mapper.Map<Coupon>(request);
        coupon.Code = CouponCodeGenerator.GenerateCouponCode();
        coupon.IsActive = true;
        
        _couponRepository.Add(coupon);
        _unitOfWork.SaveChanges();

        RemoveCouponsFromCache(cacheKey);
    }


    public void UpdateCouponIsUsed(string code)
    {
        var coupon = GetCouponEntityByCode(code);
        var cacheKey = GetCacheKey();

        coupon.IsUsed = true;
        _couponRepository.Update(coupon);
        _unitOfWork.SaveChanges();

        RemoveCouponsFromCache(cacheKey);
    }

    public void UpdateCouponChangeUsed(Guid id)
    {
        var coupon = GetCouponEntity(id);
        var cacheKey = GetCacheKey();

        coupon.IsUsed = false;

        _couponRepository.Update(coupon);
        _unitOfWork.SaveChanges();

        RemoveCouponsFromCache(cacheKey);
    }

    public void UpdateCouponStatus(Guid id, bool isActive)
    {
        var coupon = GetCouponEntity(id);
        var cacheKey = GetCacheKey();

        coupon.IsActive = isActive;
        _couponRepository.Update(coupon);
        _unitOfWork.SaveChanges();

        RemoveCouponsFromCache(cacheKey);
    }

    public CouponResponse GetCouponByCode(string code)
    {
        var coupon = GetCouponEntityByCode(code);
        var cacheKey = GetCacheKey(code);

        var couponCache = GetCouponsFromCache(cacheKey);
        if (couponCache is not null)
            return couponCache.FirstOrDefault()!;
        
        var response = _mapper.Map<CouponResponse>(coupon);

        SetCouponsToCache(cacheKey, new List<CouponResponse> { response });

        return response;
    }

    public List<CouponResponse> GetAllCoupons()
    {
        var cacheKey = GetCacheKey();
        var couponCache = GetCouponsFromCache(cacheKey);

        if (couponCache is not null)
            return couponCache;

        var coupons = _couponRepository.GetAll();
        var response = _mapper.Map<List<CouponResponse>>(coupons);

        SetCouponsToCache(cacheKey, response);
        return response;
    }

    public List<CouponResponse> GetAllActiveCoupons()
    {
        var coupons = _couponRepository.GetAll(predicate: x => x.IsActive);
        return _mapper.Map<List<CouponResponse>>(coupons);
    }

    public Coupon GetCouponEntityByCode(string code)
    {
        var coupon = _couponRepository.Get(predicate: x => x.Code.Equals(code));

        if (coupon is null)
            throw new NotFoundException(CouponBusinessMessages.CouponNotFoundByCode);
        if (!coupon.IsActive)
            throw new BusinessException(CouponBusinessMessages.CouponIsNotActive);
        if (coupon.ExpirationDate < DateHelper.GetCurrentDate())
            throw new BusinessException(CouponBusinessMessages.CouponExpired);
        if (coupon.IsUsed)
            throw new BusinessException(CouponBusinessMessages.CouponIsUsed);

        return coupon;
    }

    private Coupon GetCouponEntity(Guid id)
    {
        var coupon = _couponRepository.Get(predicate: x => x.Id.Equals(id));

        if (coupon is null)
            throw new NotFoundException(CouponBusinessMessages.CouponNotFoundById);

        return coupon;
    }

    private string GetCacheKey() => "Coupon:All";
    private string GetCacheKey(string code) => $"Coupon:{code}";

    private List<CouponResponse>? GetCouponsFromCache(string cacheKey)
    {
        return _cacheService(Cache).TryGet(cacheKey, out List<CouponResponse>? coupons) ? coupons : null;
    }

    private void SetCouponsToCache(string cacheKey, List<CouponResponse> coupons)
    {
        _cacheService(Cache).Set(cacheKey, coupons);
    }

    private void RemoveCouponsFromCache(string cacheKey) => _cacheService(Cache).Remove(cacheKey);
}