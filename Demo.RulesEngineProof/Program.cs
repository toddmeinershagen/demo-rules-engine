using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Demo.RulesEngineProof
{
    class Program
    {
        static void Main(string[] args)
        {
            WithTimer(RunDiscountScenario);
            Console.WriteLine();
            WithTimer(RunRetirementScenario);

            Console.ReadLine();
        }

        private static void WithTimer(Action action)
        {
            var watch = Stopwatch.StartNew();
            action();
            watch.Stop();
            Console.WriteLine($"Duration:  {watch.Elapsed.TotalMilliseconds} ms");
        }

        private static void RunRetirementScenario()
        {
            var repository = new RulesRepository();
            var service = new RetirementService(repository);
            var employee = new Employee { LengthOfServiceInDays = 25, IsOverridden = true };
            var isEligible = service.IsEligible(employee);

            Console.WriteLine($"Eligible for Retirement?:  {isEligible}");
        }

        private static void RunDiscountScenario()
        {
            var basicInfo = "{\"name\": \"Dishant\",\"email\": \"dishantmunjal@live.com\",\"creditHistory\": \"good\",\"country\": \"india\",\"loyalityFactor\": 3,\"totalPurchasesToDate\": 10000}";
            var orderInfo = "{\"totalOrders\": 5,\"recurringItems\": 2}";
            var telemetryInfo = "{\"noOfVisitsPerMonth\": 10,\"percentageOfBuyingToVisit\": 15}";
            var employeeInfo = "{\"employees\":[{ \"Name\":\"Siddiq\",\"Age\":31}, { \"Name\":\"Jason\",\"Age\":45}]}"; //never used because the dynamic interpreter can't handle nested levels of lists.

            var converter = new ExpandoObjectConverter();
            dynamic input1 = JsonConvert.DeserializeObject<ExpandoObject>(basicInfo, converter);
            dynamic input2 = JsonConvert.DeserializeObject<ExpandoObject>(orderInfo, converter);
            dynamic input3 = JsonConvert.DeserializeObject<ExpandoObject>(telemetryInfo, converter);
            var input4 = new List<Input.Employee> 
            { 
                new Input.Employee { Name = "Siddiq", Age = 31 }, 
                new Input.Employee { Name = "Jason", Age = 45 } 
            };

            var inputs = new dynamic[]
                {
                    input1,
                    input2,
                    input3,
                    input4
                };

            var repository = new RulesRepository();
            var discountService = new DiscountService(repository);
            var discountOffered = discountService.CalculateDiscount(inputs);
            var discountMessage = discountOffered == 0m 
                ? "The user is not eligible for any discount." 
                : $"Discount offered is {discountOffered * 100}% over MRP.";
            Console.WriteLine(discountMessage);
        }
    }

    /// <summary>
    /// This is just a demonstration of strongly typed objects and that the Engine can handle these.
    /// </summary>
    public class Input
    {
        public class Employee
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

        public List<Employee> Employees { get; set; }
    }
}
