using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API_trabajo_clase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET: api/values?count=5
        // Returns a list of random basic math operations.
        [HttpGet]
        public IEnumerable<OperationResult> Get([FromQuery] int count = 5)
        {
            if (count < 1) count = 1;
            if (count > 100) count = 100; // limit to avoid huge responses

            var rnd = new Random();
            var results = new List<OperationResult>(count);

            for (int i = 0; i < count; i++)
            {
                int a = rnd.Next(0, 101); // 0..100
                int b = rnd.Next(0, 101);
                var op = (Operator)rnd.Next(0, 4);

                double value;
                string expr;

                switch (op)
                {
                    case Operator.Add:
                        value = a + b;
                        expr = $"{a} + {b}";
                        break;
                    case Operator.Subtract:
                        value = a - b;
                        expr = $"{a} - {b}";
                        break;
                    case Operator.Multiply:
                        value = a * b;
                        expr = $"{a} * {b}";
                        break;
                    case Operator.Divide:
                    default:
                        // avoid division by zero
                        if (b == 0) b = 1;
                        value = Math.Round((double)a / b, 4);
                        expr = $"{a} / {b}";
                        break;
                }

                results.Add(new OperationResult
                {
                    Left = a,
                    Right = b,
                    Operator = OperatorToSymbol(op),
                    Expression = expr,
                    Result = value
                });
            }

            return results;
        }

        // Simple DTO returned by the endpoint
        public record OperationResult
        {
            public int Left { get; init; }
            public int Right { get; init; }
            public string Operator { get; init; } = string.Empty;
            public string Expression { get; init; } = string.Empty;
            public double Result { get; init; }
        }

        enum Operator
        {
            Add,
            Subtract,
            Multiply,
            Divide
        }

        private static string OperatorToSymbol(Operator op) => op switch
        {
            Operator.Add => "+",
            Operator.Subtract => "-",
            Operator.Multiply => "*",
            Operator.Divide => "/",
            _ => "?"
        };
    }
}
