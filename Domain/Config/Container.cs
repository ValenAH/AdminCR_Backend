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

namespace Domain.Config
{
    public class Container
    {
        public static void Register(IServiceCollection services, IConfiguration config)
        {

            //Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ISaleRepository, SaleRepository>();
            //AutoMapper
            var configMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });

            var mapper = configMapper.CreateMapper();

            services.AddSingleton(mapper);

            //Connections

            services.AddDbContext<Context>(
                options => options.UseSqlServer(config.GetConnectionString("DbCon"),
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure();
                })
                );
        }
    }
}
