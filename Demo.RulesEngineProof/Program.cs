﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RulesEngine.Extensions;
using RulesEngine.Models;

namespace Demo.RulesEngineProof
{
    class Program
    {
        static void Main(string[] args)
        {
            var basicInfo = "{\"name\": \"Dishant\",\"email\": \"dishantmunjal@live.com\",\"creditHistory\": \"good\",\"country\": \"india\",\"loyalityFactor\": 3,\"totalPurchasesToDate\": 10000}";
            var orderInfo = "{\"totalOrders\": 5,\"recurringItems\": 2}";
            var telemetryInfo = "{\"noOfVisitsPerMonth\": 10,\"percentageOfBuyingToVisit\": 15}";

            var converter = new ExpandoObjectConverter();

            dynamic input1 = JsonConvert.DeserializeObject<ExpandoObject>(basicInfo, converter);
            dynamic input2 = JsonConvert.DeserializeObject<ExpandoObject>(orderInfo, converter);
            dynamic input3 = JsonConvert.DeserializeObject<ExpandoObject>(telemetryInfo, converter);

            var inputs = new dynamic[]
                {
                    input1,
                    input2,
                    input3
                };

            var files = Directory.GetFiles(Directory.GetCurrentDirectory(), "Discount.json", SearchOption.AllDirectories);
            if (files == null || files.Length == 0)
                throw new Exception("Rules not found.");

            var fileData = File.ReadAllText(files[0]);
            var workflowRules = JsonConvert.DeserializeObject<List<WorkflowRules>>(fileData);
            var bre = new RulesEngine.RulesEngine(workflowRules.ToArray(), null);

            string discountOffered = "No discount offered.";

            List<RuleResultTree> resultList = bre.ExecuteRule("Discount", inputs);

            resultList.OnSuccess((eventName) =>
            {
                discountOffered = $"Discount offered is {eventName} % over MRP.";
            });

            resultList.OnFail(() =>
            {
                discountOffered = "The user is not eligible for any discount.";
            });

            Console.WriteLine(discountOffered);


            files = Directory.GetFiles(Directory.GetCurrentDirectory(), "RetirementEligibility.json", SearchOption.AllDirectories);
            if (files == null || files.Length == 0)
                throw new Exception("Rules not found.");

            fileData = File.ReadAllText(files[0]);
            workflowRules = JsonConvert.DeserializeObject<List<WorkflowRules>>(fileData);

            var engine = new RulesEngine.RulesEngine(workflowRules.ToArray(), null);
            var input = new Employee { LengthOfServiceInDays = 70, IsOverridden = false };

            List<RuleResultTree> results = engine.ExecuteRule("RetirementEligibility", input);
            results.OnSuccess(e =>
            {
                Console.WriteLine($"You hit the '{e}' rule.");
            });

            Console.WriteLine($"Eligible for Retirement?:  {results.ToList().FirstOrDefault()?.IsSuccess}");
            Console.ReadLine();
        }
    }

    public class Employee
    {
        public int LengthOfServiceInDays { get; set; }
        public bool IsOverridden { get; set; }
    }
}
