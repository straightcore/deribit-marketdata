using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MarketData.Deribit.WebAPI.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Data.SQLite;

namespace MarketData.Deribit.WebAPI
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
            services.AddCors(options =>
                                {
                                    options.AddPolicy("CorsPolicy",
                                        builder => builder.SetIsOriginAllowed(_ => true)
                                            .AllowAnyOrigin()
                                            .AllowAnyMethod()
                                            .AllowAnyHeader());
                                });
            services.AddControllers();
            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
            });
            services.AddSwaggerGen();
            // services.AddCors(options => options.AddPolicy(builder => builder.WithOrigins("http://*/api",  "https://*/api")));
            services.AddTransient<ITradeRepository, DapperTradeRepository>();
            services.AddTransient<IInstrumentRepository, DapperInstrumentRepository>();
            services.AddSingleton<SQLiteConnectionStringBuilder>(serviceProvider => 
                { 
                    var builder = new SQLiteConnectionStringBuilder();
                    builder.DataSource = Configuration.GetSection("SqliteFile").Value;
                    return builder;
                });
            services.AddTransient<IDbConnection>(provider => 
            {
                var builder = provider.GetRequiredService<SQLiteConnectionStringBuilder>();
                var connection = SQLiteFactory.Instance.CreateConnection();
                connection.ConnectionString = builder.ConnectionString;
                connection.Open();
                return connection;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Historical MarketData WebAPI v1");
                c.RoutePrefix = string.Empty;
            });
            app.UseHttpsRedirection();

            app.UseRouting();

            // app.UseAuthorization();
            // app.UseCors(
            //     options => options.SetIsOriginAllowed(x => _ = true).AllowAnyMethod().AllowAnyHeader().AllowCredentials()
            // );
            app.UseCors("CorsPolicy");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
