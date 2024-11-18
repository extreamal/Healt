using HealthLifeProject.Commons;
using HealthLifeProject.Entities;
using HealthLifeProject.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthLifeProject
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<HealthLifeDBContext>();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Account/AccessDenied");
            });

            //добавление в сервисы приложения нашего кастомного CustomHttpContetx
            services.AddHttpContextAccessor();

            //services.AddDbContext<HealthLifeDBContext>(
            //    options => options.UseSqlServer(@"Data Source=DESKTOP-HH8M6J0\MSSQLSERVERRR;Initial Catalog=lifeDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False")
            //    );

            //services.AddScoped<BenefactorsRepository, BenefactorsRepository>();
            //services.AddScoped<CategoryRepository, CategoryRepository>();
            //services.AddScoped<CharitableContributionsRepository, CharitableContributionsRepository>();
            //services.AddScoped<ContactPhonesRepository, ContactPhonesRepository>();
            //services.AddScoped<DiagnosesRepository, DiagnosesRepository>();
            //services.AddScoped<EntrancesRepository, EntrancesRepository>();
            //services.AddScoped<EventsRepository, EventsRepository>();
            //services.AddScoped<FundraisingStatusesRepository, FundraisingStatusesRepository>();
            //services.AddScoped<GenderRepository, GenderRepository>();
            //services.AddScoped<HospitalsCharitableContributionsRepository, HospitalsCharitableContributionsRepository>();
            //services.AddScoped<HospitalsRepository, HospitalsRepository>();
            //services.AddScoped<HospitalsRepresentativesRepository, HospitalsRepresentativesRepository>();
            //services.AddScoped<HousesRepository, HousesRepository>();
            //services.AddScoped<MainInfoRepository, MainInfoRepository>();
            //services.AddScoped<NamesRepository, NamesRepository>();
            //services.AddScoped<NewsRepository, NewsRepository>();
            //services.AddScoped<PartnersRepository, PartnersRepository>();
            //services.AddScoped<PartnersRepresentativesRepository, PartnersRepresentativesRepository>();
            //services.AddScoped<PatientsCharitableContributionsRepository, PatientsCharitableContributionsRepository>();
            //services.AddScoped<PatientsRepository, PatientsRepository>();
            //services.AddScoped<PatientsRepository, PatientsRepository>();
            //services.AddScoped<PatronymicsRepository, PatronymicsRepository>();
            //services.AddScoped<PositionsRepository, PositionsRepository>();
            //services.AddScoped<RolesRepository, RolesRepository>();
            //services.AddScoped<StreetsRepository, StreetsRepository>();
            //services.AddScoped<StreetTypesRepository, StreetTypesRepository>();
            //services.AddScoped<SurnamesRepository, SurnamesRepository>();
            //services.AddScoped<TreatmentCategoryRepository, TreatmentCategoryRepository>();
            //services.AddScoped<UsersInfoRepository, UsersInfoRepository>();
            //services.AddScoped<WardsRepository, WardsRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.Use(async (context, next) => {
                await next();
                if (context.Response.StatusCode == 404)
                {
                    context.Request.Path = "/Error/PageNotFound";
                    await next();
                }
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            StaticHttpContextExensions.UseStaticCustomHttpContext(app);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<CustomMiddleware>();

            StripeConfiguration.ApiKey = Configuration.GetSection("Stripe:SecretKey").Get<string>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
