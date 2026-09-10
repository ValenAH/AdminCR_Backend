using AutoMapper;
using Infraestructure.Database;
using Infraestructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Domain.Config
{
    public class Container
    {
        public static void Register(IServiceCollection services, IConfiguration config)
        {

            //Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IIdentificationTypeRepository, IdentificationTypeRepository>();
            services.AddScoped<ISaleRepository, SaleRepository>();
            services.AddScoped<ISaleDetailsRepository, SaleDetailsRepository>();
            services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IQuotaRepository, QuotaRepository>();

            //AutoMapper
            var configMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });

            var mapper = configMapper.CreateMapper();

            services.AddSingleton(mapper);

            //Connections
            var dbConnectionString = config.GetConnectionString("DbCon");

            if (string.IsNullOrWhiteSpace(dbConnectionString))
            {
                dbConnectionString = Environment.GetEnvironmentVariable("DB__ACCESS");
            }

            if (string.IsNullOrWhiteSpace(dbConnectionString))
            {
                throw new InvalidOperationException("No database connection string found. Set 'ConnectionStrings:DbCon' or the 'DB__ACCESS' environment variable.");
            }

            services.AddDbContext<Context>(
                options => options
                        .UseNpgsql(dbConnectionString)
                        // The following three options help with debugging, but should
                        // be changed or removed for production.
                        .LogTo(Console.WriteLine, LogLevel.Information)
                        .EnableSensitiveDataLogging()
                        .EnableDetailedErrors()
                );
        }
    }
}
