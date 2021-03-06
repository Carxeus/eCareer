using AutoMapper;
using Career.Configuration;
using Career.EntityFramework;
using Career.IoC.IoCModule;
using Company.Application.Company;
using Company.Domain.Repositories;
using Company.Infrastructure;
using Company.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Application
{
    public class ApplicationModule : Module
    {
        protected override void Load(IServiceCollection services)
        {
            IConfiguration configuration = ConfigurationHelper.GetConfiguration();

            services.AddDbContext<CompanyDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("CompanyDatabase")));
            services.AddUnitOfWork<CompanyDbContext>();
            
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<ICompanyFollowerRepository, CompanyFollowerRepository>();

            services.AddAutoMapper(typeof(CompanyMappinProfile));
        }
    }
}