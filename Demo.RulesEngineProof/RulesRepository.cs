using System.IO;
using Newtonsoft.Json;
using RulesEngine.Models;

namespace Demo.RulesEngineProof
{
    public class RulesRepository : IRulesRepository
    {
        public WorkflowRules[] GetRules(string filename)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), filename);
            var fileData = File.ReadAllText(filePath);
            var rules = JsonConvert.DeserializeObject<WorkflowRules[]>(fileData);

            return rules;
        }
    }

    public interface IRulesRepository
    {
        WorkflowRules[] GetRules(string filename);
    }
}