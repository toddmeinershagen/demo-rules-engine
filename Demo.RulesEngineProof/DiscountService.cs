using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using RulesEngine.Extensions;
using RulesEngine.Interfaces;
using RulesEngine.Models;

namespace Demo.RulesEngineProof
{
    public class DiscountService
    {
        private readonly IRulesEngine _engine;

        public DiscountService()
        {
            var files = Directory.GetFiles(Directory.GetCurrentDirectory(), "Discount.json", SearchOption.AllDirectories);
            if (files == null || files.Length == 0)
                throw new Exception("Rules not found.");

            var fileData = File.ReadAllText(files[0]);
            var rules = JsonConvert.DeserializeObject<WorkflowRules[]>(fileData);
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
