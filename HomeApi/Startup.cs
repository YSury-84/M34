using FluentValidation.AspNetCore;
using HomeApi.Configuration;
using HomeApi.Models;
using HomeApi.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace HomeApi
{
    public class Startup
    {
        /// <summary>
        /// �������� ������������ �� ����� Json
        /// </summary>
        private IConfiguration Configuration
        { get; } = new ConfigurationBuilder()
          .AddJsonFile("HomeOptions.json")
          .Build();

        public void ConfigureServices(IServiceCollection services)
        {
            // ��������� ����� ������
            services.Configure<HomeOptions>(Configuration);

            services.AddControllers();
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "HomeApi",
                    Version = "v1"
                });
            });
            // ��������� ����� ������
            services.Configure<HomeOptions>(Configuration);
            services.Configure<HomeOptions>(opt =>
            {
                opt.Area = 120;
            });
            // ��������� ������ ����� (��������� Json-������) 
            services.Configure<Address>(Configuration.GetSection("Address"));
            // ���������� �����������
            var assembly = Assembly.GetAssembly(typeof(MappingProfile));
            services.AddAutoMapper(assembly);
            // ���������� ���������
            services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<AddDeviceRequestValidator>());

        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // ����������� ����������� ��� ������� ��� ���������� ��������
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HomeApi v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            // ������������ �������� � �������������
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }

}
