using System;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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

            var discountService = new DiscountService();
            var discountOffered = discountService.CalculateDiscount(inputs);
            var discountMessage = discountOffered == 0m 
                ? "The user is not eligible for any discount." 
                : $"Discount offered is {discountOffered * 100}% over MRP.";
            Console.WriteLine(discountMessage);
            Console.WriteLine();


            var service = new RetirementService();
            var employee = new Employee { LengthOfServiceInDays = 25, IsOverridden = true };
            var isEligible = service.IsEligible(employee);

            Console.WriteLine($"Eligible for Retirement?:  {isEligible}");
            Console.ReadLine();
        }
    }
}
