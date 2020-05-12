using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using RulesEngine.Extensions;
using RulesEngine.Interfaces;
using RulesEngine.Models;

namespace Demo.RulesEngineProof
{
    public class RetirementService
    {
        private readonly IRulesEngine _engine;

        public RetirementService()
        {
            var files = Directory.GetFiles(Directory.GetCurrentDirectory(), "RetirementEligibility.json", SearchOption.AllDirectories);
            if (files == null || files.Length == 0)
                throw new Exception("Rules not found.");

            var fileData = File.ReadAllText(files[0]);
            var rules = JsonConvert.DeserializeObject<WorkflowRules[]>(fileData);

            _engine = new RulesEngine.RulesEngine(rules, null);
        }

        public bool IsEligible(Employee employee)
        {
            List<RuleResultTree> results = _engine.ExecuteRule("RetirementEligibility", employee);
            return results.FirstOrDefault()?.IsSuccess ?? false;
        }
    }
}
