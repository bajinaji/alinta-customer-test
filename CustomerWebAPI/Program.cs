using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlintaTestModels;
using Microsoft.Extensions.DependencyInjection;

namespace CustomerWebAPI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var host = CreateHostBuilder(args).Build();

            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                using (var context = services.GetRequiredService<AlintaTestContext>())
                {
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();

                    var customers = new List<Customer>();
                    var c = new Customer()
                        {FirstName = "Rob", LastName = "Clayton", DateOfBirth = new DateTime(1976, 06, 19)};
                    customers.Add(c);
                    c = new Customer()
                        {FirstName = "Jess", LastName = "Clayton", DateOfBirth = new DateTime(1989, 12, 27)};
                    customers.Add(c);
                    c = new Customer()
                        {FirstName = "First1", LastName = "Last1", DateOfBirth = new DateTime(1920, 01, 01)};
                    customers.Add(c);
                    c = new Customer()
                        {FirstName = "First2", LastName = "Last2", DateOfBirth = new DateTime(2010, 07, 4)};
                    customers.Add(c);
                    c = new Customer()
                        {FirstName = "First3", LastName = "Last3", DateOfBirth = new DateTime(2001, 03, 07)};
                    customers.Add(c);
                    c = new Customer() {FirstName = "First4", LastName = "Last4"};
                    customers.Add(c);
                    c = new Customer() {FirstName = "Rob2", LastName = "Clayton2"};
                    customers.Add(c);
                    c = new Customer() {FirstName = "Rob3", LastName = "Clayton3"};
                    customers.Add(c);
                    c = new Customer() {FirstName = "Rob4", LastName = "Clayton4"};
                    customers.Add(c);


                    context.AddRange(customers);

                    context.SaveChanges();
                }
            }

            host.Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
	}
}
