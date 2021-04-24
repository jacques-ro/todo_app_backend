using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Todo.Backend.Persistence.Configuration;
using Todo.Backend.Persistence.Context;

namespace backend
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        private DbConnection _connection;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            if(_configuration.GetValue<bool>("UseSqliteInMemoryDatabase"))
            {
                services.AddDbContext<TodoItemContext>(options =>                    
                    options.UseSqlite(CreateSqliteInMemoryConnection())
                );
            }
            else if (_configuration.GetValue<bool>("HerokuEnvironment"))
            {
                services.AddDbContext<TodoItemContext>(options =>
                    options.UseNpgsql(
                        GetHerokuConnectionString(_configuration.GetValue<string>("DATABASE_URL"))
                    )
                );                
            }
            else
            {
                services.AddDbContext<TodoItemContext>(options =>
                    options.UseNpgsql(_configuration.GetConnectionString("database"))                    
                );
            }       

            services.AddControllers();

            services.AddSwaggerGen();

            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddReadRepository();

            services.AddWriteRepository();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {            
            if(_configuration.GetValue<bool>("HerokuEnvironment"))
            {
                logger.LogDebug("Detected that the app runs on Heroku");
            }
            if (env.IsDevelopment())
            {
                logger.LogDebug("Detected development environment");                
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API");
            });

            app.UseRouting();            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private DbConnection CreateSqliteInMemoryConnection()
        {
            if(_connection == null)
            {
                var connection = new SqliteConnection("Filename=:memory:");

                connection.Open();
                _connection = connection;
            }               

            return _connection;
        }

        private static string GetHerokuConnectionString(string herokuDatabaseUrlString)
        {
            var uri = new Uri(herokuDatabaseUrlString);

            var db = uri.LocalPath.TrimStart('/');
            var userInfo = uri.UserInfo.Split(':', StringSplitOptions.RemoveEmptyEntries);

            return string.Join(';',
                $"User ID={userInfo[0]}",
                $"Password={userInfo[1]}",
                $"Host={uri.Host}",
                $"Port={uri.Port}",
                $"Database={db}",
                $"Pooling=true",
                "SSL Mode=Require",
                "Trust Server Certificate=True;"
            );
        }

        public void Dispose() => _connection.Dispose();
    }
}
