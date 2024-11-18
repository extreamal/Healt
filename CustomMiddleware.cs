using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using HealthLifeProject.Entities;
using Microsoft.EntityFrameworkCore;
using HealthLifeProject.Models;

namespace HealthLifeProject
{
    internal class CustomMiddleware
    {
        //делегат, который содержит указатель на следующий метод в конвере запросов
        private readonly RequestDelegate _next;
        private readonly HealthLifeDBContext _healthLifeDBContext;
        private readonly BenefactorsRepository _benefactorsRepository;
        private readonly HospitalsRepresentativesRepository _hospitalsRepresentativesRepository;

        public CustomMiddleware(RequestDelegate next)
        {
            _next = next;
            _healthLifeDBContext = new HealthLifeDBContext(new DbContextOptions<HealthLifeDBContext>());
            _benefactorsRepository = new BenefactorsRepository(_healthLifeDBContext);
            _hospitalsRepresentativesRepository = new HospitalsRepresentativesRepository(_healthLifeDBContext);
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if(context.User.Identity.IsAuthenticated)
            {
                var userEmail = context.User.Identity.Name;
                Benefactors benefactor = _benefactorsRepository.GetBenefactorByEmail(userEmail);
                HospitalsRepresentatives hospitalsRepresentative = _hospitalsRepresentativesRepository.GetHospitalsRepresentativeByEmail(userEmail);

                if (benefactor != null)
                {
                    context.Items["UserName"] = $"{_healthLifeDBContext.Names.FirstAsync(u => u.Id == benefactor.NameId).Result.Name} {_healthLifeDBContext.Surnames.FirstAsync(u => u.Id == benefactor.SurnameId).Result.Surname}";
                    context.Items["Avatar"] = benefactor.Avatar;
                }
                else if (hospitalsRepresentative != null)
                {
                    context.Items["UserName"] = $"{_healthLifeDBContext.Names.FirstAsync(u => u.Id == hospitalsRepresentative.NameId).Result.Name} {_healthLifeDBContext.Surnames.FirstAsync(u => u.Id == hospitalsRepresentative.SurnameId).Result.Surname}";
                    context.Items["Avatar"] = hospitalsRepresentative.Avatar;
                }
            }

            string method = context.Request.Method;
            switch (method)
            {
                case "GET":
                    {


                        break;
                    }
                case "POST":
                    {


                        break;
                    }
                case "PUT":
                    {


                        break;
                    }
                case "DELETE":
                    {


                        break;
                    }
            }
            //Передаем контекст управления следующему middleware
            await _next.Invoke(context);
        }
    }
}