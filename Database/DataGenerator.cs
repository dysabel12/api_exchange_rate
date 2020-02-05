using api_exchange_rate.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_exchange_rate.Database
{
    public class DataGenerator
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ExchangeRateDBContext(
                serviceProvider.GetRequiredService<DbContextOptions<ExchangeRateDBContext>>()))
            {
                if (context.ExchangeRates.Any())
                {
                    return;
                }

                context.ExchangeRates.AddRange(
                    new ExchangeRate
                    {
                        id=1,
                        currency_input = "SOLES",
                        currency_output = "DOLARES",
                        exchange_rate = 0.3M
                    },
                    new ExchangeRate
                    {
                        id=2,
                        currency_input = "DOLARES",
                        currency_output = "SOLES",
                        exchange_rate = 3M
                    });

                context.SaveChanges();
            }
        }
    }
}
