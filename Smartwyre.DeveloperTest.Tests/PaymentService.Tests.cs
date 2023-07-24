using Moq;
using Smartwyre.DeveloperTest.Data.Interfaces;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests;

public class PaymentServiceTests
{
    [Fact]
    public void PositiveFixedRateRebateTest()
    {
        CalculateRebateRequest request = new CalculateRebateRequest { Volume = 100 };
        var productDataStoreMock = new Mock<IProductDataStore>();
        productDataStoreMock.Setup(s => s.GetProduct(It.IsAny<string>())).Returns(new Product
        {
            Price = 100,
            SupportedIncentives = SupportedIncentiveType.FixedRateRebate,
        });
        var rebateDataStoreMock = new Mock<IRebateDataStore>();
        rebateDataStoreMock.Setup(s => s.GetRebate(It.IsAny<string>())).Returns(new Rebate
        {
            Incentive = IncentiveType.FixedRateRebate,
            Percentage = 5,
            Amount = 100,
        });
        RebateService service = new RebateService(productDataStoreMock.Object, rebateDataStoreMock.Object);

        var result = service.Calculate(request);

        var serviceResult = Assert.IsType<CalculateRebateResult>(result);
        Assert.True(result.Success);
    }

    [Fact]
    public void PositiveAmountPerUomTest()
    {
        CalculateRebateRequest request = new CalculateRebateRequest { Volume = 100 };
        var productDataStoreMock = new Mock<IProductDataStore>();
        productDataStoreMock.Setup(s => s.GetProduct(It.IsAny<string>())).Returns(new Product
        {
            Price = 100,
            SupportedIncentives = SupportedIncentiveType.AmountPerUom,
        });
        var rebateDataStoreMock = new Mock<IRebateDataStore>();
        rebateDataStoreMock.Setup(s => s.GetRebate(It.IsAny<string>())).Returns(new Rebate
        {
            Incentive = IncentiveType.AmountPerUom,
            Percentage = 5,
            Amount = 100,
        });
        RebateService service = new RebateService(productDataStoreMock.Object, rebateDataStoreMock.Object);

        var result = service.Calculate(request);

        var serviceResult = Assert.IsType<CalculateRebateResult>(result);
        Assert.True(result.Success);
    }

    [Fact]
    public void PositiveFixedCashAmountTest()
    {
        CalculateRebateRequest request = new CalculateRebateRequest { Volume = 100 };
        var productDataStoreMock = new Mock<IProductDataStore>();
        productDataStoreMock.Setup(s => s.GetProduct(It.IsAny<string>())).Returns(new Product
        {
            Price = 100,
            SupportedIncentives = SupportedIncentiveType.FixedCashAmount,
        });
        var rebateDataStoreMock = new Mock<IRebateDataStore>();
        rebateDataStoreMock.Setup(s => s.GetRebate(It.IsAny<string>())).Returns(new Rebate
        {
            Incentive = IncentiveType.FixedCashAmount,
            Percentage = 5,
            Amount = 100,
        });
        RebateService service = new RebateService(productDataStoreMock.Object, rebateDataStoreMock.Object);

        var result = service.Calculate(request);

        var serviceResult = Assert.IsType<CalculateRebateResult>(result);
        Assert.True(result.Success);
    }

    [Fact]
    public void NegativeNoRebateInDatabaseTest()
    {
        CalculateRebateRequest request = new CalculateRebateRequest { Volume = 100 };
        var productDataStoreMock = new Mock<IProductDataStore>();
        productDataStoreMock.Setup(s => s.GetProduct(It.IsAny<string>())).Returns(new Product
        {
            Price = 100,
            SupportedIncentives = SupportedIncentiveType.FixedCashAmount,
        });
        var rebateDataStoreMock = new Mock<IRebateDataStore>();
        rebateDataStoreMock.Setup(s => s.GetRebate(It.IsAny<string>())).Returns((Rebate)null);
        RebateService service = new RebateService(productDataStoreMock.Object, rebateDataStoreMock.Object);

        var result = service.Calculate(request);

        var serviceResult = Assert.IsType<CalculateRebateResult>(result);
        Assert.False(result.Success);
    }

    [Fact]
    public void NegativeNoProductInDatabaseTest()
    {
        CalculateRebateRequest request = new CalculateRebateRequest { Volume = 100 };
        var productDataStoreMock = new Mock<IProductDataStore>();
        productDataStoreMock.Setup(s => s.GetProduct(It.IsAny<string>())).Returns((Product)null);
        var rebateDataStoreMock = new Mock<IRebateDataStore>();
        rebateDataStoreMock.Setup(s => s.GetRebate(It.IsAny<string>())).Returns(new Rebate
        {
            Incentive = IncentiveType.FixedCashAmount,
            Percentage = 5,
            Amount = 100,
        });
        RebateService service = new RebateService(productDataStoreMock.Object, rebateDataStoreMock.Object);

        var result = service.Calculate(request);

        var serviceResult = Assert.IsType<CalculateRebateResult>(result);
        Assert.False(result.Success);
    }

    [Fact]
    public void NegativeDifferentIncentiveTypeTest()
    {
        CalculateRebateRequest request = new CalculateRebateRequest { Volume = 100 };
        var productDataStoreMock = new Mock<IProductDataStore>();
        productDataStoreMock.Setup(s => s.GetProduct(It.IsAny<string>())).Returns(new Product
        {
            Price = 100,
            SupportedIncentives = SupportedIncentiveType.AmountPerUom,
        });
        var rebateDataStoreMock = new Mock<IRebateDataStore>();
        rebateDataStoreMock.Setup(s => s.GetRebate(It.IsAny<string>())).Returns(new Rebate
        {
            Incentive = IncentiveType.FixedCashAmount,
            Percentage = 5,
            Amount = 100,
        });
        RebateService service = new RebateService(productDataStoreMock.Object, rebateDataStoreMock.Object);

        var result = service.Calculate(request);

        var serviceResult = Assert.IsType<CalculateRebateResult>(result);
        Assert.False(result.Success);
    }

    [Fact]
    public void NegativeAmountIsZeroTest()
    {
        CalculateRebateRequest request = new CalculateRebateRequest { Volume = 100 };
        var productDataStoreMock = new Mock<IProductDataStore>();
        productDataStoreMock.Setup(s => s.GetProduct(It.IsAny<string>())).Returns(new Product
        {
            Price = 100,
            SupportedIncentives = SupportedIncentiveType.FixedCashAmount,
        });
        var rebateDataStoreMock = new Mock<IRebateDataStore>();
        rebateDataStoreMock.Setup(s => s.GetRebate(It.IsAny<string>())).Returns(new Rebate
        {
            Incentive = IncentiveType.FixedCashAmount,
            Percentage = 5,
            Amount = 0,
        });
        RebateService service = new RebateService(productDataStoreMock.Object, rebateDataStoreMock.Object);

        var result = service.Calculate(request);

        var serviceResult = Assert.IsType<CalculateRebateResult>(result);
        Assert.False(result.Success);
    }

    [Fact]
    public void NegativePercentageIsZeroTest()
    {
        CalculateRebateRequest request = new CalculateRebateRequest { Volume = 100 };
        var productDataStoreMock = new Mock<IProductDataStore>();
        productDataStoreMock.Setup(s => s.GetProduct(It.IsAny<string>())).Returns(new Product
        {
            Price = 100,
            SupportedIncentives = SupportedIncentiveType.FixedRateRebate,
        });
        var rebateDataStoreMock = new Mock<IRebateDataStore>();
        rebateDataStoreMock.Setup(s => s.GetRebate(It.IsAny<string>())).Returns(new Rebate
        {
            Incentive = IncentiveType.FixedRateRebate,
            Percentage = 0,
            Amount = 110,
        });
        RebateService service = new RebateService(productDataStoreMock.Object, rebateDataStoreMock.Object);

        var result = service.Calculate(request);

        var serviceResult = Assert.IsType<CalculateRebateResult>(result);
        Assert.False(result.Success);
    }

    [Fact]
    public void NegativePriceIsZeroTest()
    {
        CalculateRebateRequest request = new CalculateRebateRequest { Volume = 100 };
        var productDataStoreMock = new Mock<IProductDataStore>();
        productDataStoreMock.Setup(s => s.GetProduct(It.IsAny<string>())).Returns(new Product
        {
            Price = 0,
            SupportedIncentives = SupportedIncentiveType.FixedRateRebate,
        });
        var rebateDataStoreMock = new Mock<IRebateDataStore>();
        rebateDataStoreMock.Setup(s => s.GetRebate(It.IsAny<string>())).Returns(new Rebate
        {
            Incentive = IncentiveType.FixedRateRebate,
            Percentage = 5,
            Amount = 110,
        });
        RebateService service = new RebateService(productDataStoreMock.Object, rebateDataStoreMock.Object);

        var result = service.Calculate(request);

        var serviceResult = Assert.IsType<CalculateRebateResult>(result);
        Assert.False(result.Success);
    }
}
