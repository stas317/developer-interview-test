using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Runner
{
    public class Application
    {
        private readonly IRebateService _rebateService;

        public Application(IRebateService rebateService)
        {
            _rebateService = rebateService;
        }

        public void Run(string[] args)
        {
            var calculateRebateRequest = new CalculateRebateRequest
            {
                RebateIdentifier = "test RebateIdentifier",
                ProductIdentifier = "test ProductIdentifier",
                Volume = 10
            };

            _rebateService.Calculate(calculateRebateRequest);
        }
    }
}
