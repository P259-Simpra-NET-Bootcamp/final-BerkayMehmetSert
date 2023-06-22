using Core.Utilities.Date;
using Xunit;

namespace Core.Test.Utilities.Date;

public class DateHelperTest
{
    [Fact]
    public void GetCurrentDateShouldReturnCurrentDate()
    {
        var currentDate = DateTime.Now;
        var result = DateHelper.GetCurrentDate();
        Assert.Equal(currentDate.Date, result.Date);
    }
    
    [Fact]
    public void GetPreviousDateShouldReturnPreviousDate()
    {
        var currentDate = DateTime.Now.AddDays(-1);
        var result = DateHelper.GetPreviousDate();
        Assert.Equal(currentDate.Date, result.Date);
    }
}