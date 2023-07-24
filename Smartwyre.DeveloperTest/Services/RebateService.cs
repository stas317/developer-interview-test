using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Data.Interfaces;
using Smartwyre.DeveloperTest.Types;
using System.Collections.Generic;
using System;

namespace Smartwyre.DeveloperTest.Services;

public class RebateService : IRebateService
{
    private readonly IProductDataStore _productDataStore;
    private readonly IRebateDataStore _rebateDataStore;
    private Dictionary<IncentiveType, Func<decimal, decimal, decimal, decimal, Tuple<decimal, bool>>> rebateCalculation
        = new Dictionary<IncentiveType, Func<decimal, decimal, decimal, decimal, Tuple<decimal, bool>>>
    {
        { IncentiveType.FixedRateRebate, (decimal volume, decimal price, decimal percentage, decimal amount) => new Tuple<decimal, bool>(price * percentage * volume, !(percentage == 0 || price == 0 || volume == 0))},
        { IncentiveType.AmountPerUom, (decimal volume, decimal price, decimal percentage, decimal amount) => new Tuple<decimal, bool>(amount * volume, !(amount == 0 || volume == 0))},
        { IncentiveType.FixedCashAmount, (decimal volume, decimal price, decimal percentage, decimal amount) => new Tuple<decimal, bool>(amount, amount != 0)}
    };

    public RebateService(
        IProductDataStore productDataStore,
        IRebateDataStore rebateDataStore
        )
    {
        _productDataStore = productDataStore;
        _rebateDataStore = rebateDataStore;
    }

    public CalculateRebateResult Calculate(CalculateRebateRequest request)
    {
        var rebate = _rebateDataStore.GetRebate(request.RebateIdentifier);
        var product = _productDataStore.GetProduct(request.ProductIdentifier);

        var result = new CalculateRebateResult();

        if (rebate == null
            || product == null
            || !SupportedIncentivesCompare(rebate?.Incentive, product?.SupportedIncentives))
        {
            result.Success = false;
            return result;
        }

        var calculationResult = rebateCalculation[rebate.Incentive]
            (request.Volume, product.Price, rebate.Percentage, rebate.Amount);

        if (calculationResult.Item2)
        {
            result.Success = true;
            var storeRebateDataStore = new RebateDataStore();
            storeRebateDataStore.StoreCalculationResult(rebate, calculationResult.Item1);
        }

        return result;
    }

    private bool SupportedIncentivesCompare(
        IncentiveType? incentiveType,
        SupportedIncentiveType? supportedIncentiveType
    )
    {
        if (!incentiveType.HasValue || !supportedIncentiveType.HasValue)
            return false;

        return 1 << (int)incentiveType == (int)supportedIncentiveType;
    }
}
