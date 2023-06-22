using Core.Utilities.Code;
using Xunit;

namespace Core.Test.Utilities.Code;

public class CouponCodeGeneratorTest
{
    [Fact]
    public void GenerateShouldReturnCouponCodeWithCorrectLength()
    {
        var couponCode = CouponCodeGenerator.GenerateCouponCode();
        Assert.Equal(8, couponCode.Length);
    }
    
    [Fact]
    public void GenerateShouldReturnCouponCodeWithCorrectCharacters()
    {
        var couponCode = CouponCodeGenerator.GenerateCouponCode();
        Assert.Matches("^[A-Z0-9]*$", couponCode);
    }
    
    [Fact]
    public void GenerateShouldReturnCouponCodeWithCorrectCharactersAndLength()
    {
        var couponCode = CouponCodeGenerator.GenerateCouponCode();
        Assert.Matches("^[A-Z0-9]{8}$", couponCode);
    }
}