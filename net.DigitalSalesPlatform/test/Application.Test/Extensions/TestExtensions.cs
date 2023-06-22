using Application.Contracts.Services;
using Application.Contracts.Validations.Category;
using Application.Contracts.Validations.Coupon;
using Application.Contracts.Validations.Product;
using Application.Contracts.Validations.User;
using Application.Services;
using Application.Test.Mocks.FakeData;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Test.Extensions;

public static class TestExtensions
{
    public static void AddTestMockServices(this IServiceCollection services)
    {
        services.AddTransient<CategoryFakeData>();
        services.AddTransient<CouponFakeData>();
        services.AddTransient<OrderDetailFakeData>();
        services.AddTransient<OrderReportFakeData>();
        services.AddTransient<OrderFakeData>();
        services.AddTransient<UserFakeData>();
        services.AddTransient<ProductFakeData>();
        services.AddScoped<ICategoryService, CategoryService>();

        services.AddTransient<CreateCategoryRequestValidator>();
        services.AddTransient<UpdateCategoryRequestValidator>();
        services.AddTransient<CreateCouponRequestValidator>();
        services.AddTransient<LoginRequestRequestValidator>();
        services.AddTransient<RegisterUserRequestValidator>();
        services.AddTransient<RegisterAdminRequestValidator>();
        services.AddTransient<UpdateUserRequestValidator>();
        services.AddTransient<ChangePasswordRequestValidator>();
        services.AddTransient<UpdateProductRequestValidator>();
        services.AddTransient<UpdateProductPriceRequestValidator>();
    }
}