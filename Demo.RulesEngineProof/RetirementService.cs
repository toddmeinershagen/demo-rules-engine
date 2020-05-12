using System.Collections.Generic;
using System.Linq;
using RulesEngine.Interfaces;
using RulesEngine.Models;

namespace Demo.RulesEngineProof
{
    public class RetirementService
    {
        private readonly IRulesEngine _engine;

        public RetirementService(IRulesRepository repository)
        {
            var rules = repository.GetRules("RetirementEligibility.json");
            _engine = new RulesEngine.RulesEngine(rules, null);
        }

        public bool IsEligible(Employee employee)
        {
            List<RuleResultTree> results = _engine.ExecuteRule("RetirementEligibility", employee);
            return results.FirstOrDefault()?.IsSuccess ?? false;
        }
    }
}
