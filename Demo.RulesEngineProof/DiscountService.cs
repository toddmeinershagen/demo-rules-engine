using System;
using System.Collections.Generic;
using RulesEngine.Extensions;
using RulesEngine.Interfaces;
using RulesEngine.Models;

namespace Demo.RulesEngineProof
{
    public class DiscountService
    {
        private readonly IRulesEngine _engine;

        public DiscountService(IRulesRepository repository)
        {
            var rules = repository.GetRules("Discount.json");
            _engine = new RulesEngine.RulesEngine(rules, null);
        }

        public decimal CalculateDiscount(dynamic[] inputs)
        {
            decimal discount = 0m;
            List<RuleResultTree> resultList = _engine.ExecuteRule("Discount", inputs);

            resultList.OnSuccess((eventName) =>
            {
                discount = Convert.ToDecimal(eventName);
            });

            return discount; ;
        }
    }
}
