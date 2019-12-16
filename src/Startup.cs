using System.Net.Http;
using System.Security.Claims;
using Bijector.Infrastructure.Discovery;
using Bijector.Infrastructure.Dispatchers;
using Bijector.Infrastructure.Queues;
using Bijector.Infrastructure.Repositories;
using Bijector.Workflows.Executor;
using Bijector.Workflows.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Bijector.Workflows
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<TimeStartWorkflowNode>();
            MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<CommandWorkflowNode>();
            MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<Messages.Commands.RenameDriveEntity>();
            MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<WorkflowNodeTimeStamps>();

            services.AddConsul(Configuration);
            services.AddConsulDiscover();

            services.AddRabbitMQ(Configuration);

            services.AddMongoDb(Configuration);
            services.AddMongoDbRepository<Workflow>("Workflows");

            services.AddHandleDispatchers();

            services.AddTransient<IWorkflowExecutor, WorkflowExecutor>();

            var discover = services.BuildServiceProvider().GetService<IServiceDiscover>();
            var accountsUrl = discover.ResolveServicePath("Bijector Accounts");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;              
                options.Authority = $"https://{accountsUrl}";
                options.Audience = "api.v1";
                options.BackchannelHttpHandler = new HttpClientHandler{ServerCertificateCustomValidationCallback = (f1, s, t, f2) => true};
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = ClaimTypes.NameIdentifier
                };
            });
            
            services.AddAuthorization();            

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Bijector Workflows API", Version = "v1" });
            });

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseHsts();
            app.UseHttpsRedirection();

            app.UseConsul(lifetime);

            app.UseRouting();            

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseRabbitMQ().SubscribeAllEvenets();

            app.UseSwagger();
            
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Bijector Workflows API");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
