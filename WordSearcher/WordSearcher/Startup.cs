using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WordSearcher.Repository.RabbitMQ;
using WordSearcher.Repository.RabbitMQ.Interfaces;
using WordSearcher.Repository.Redis;
using WordSearcher.Repository.Redis.Interfaces;

namespace WordSearcher
{
	public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
		{
			this.RegisterDependencies(services);
			services.AddMvc();
		}

		private void RegisterDependencies(IServiceCollection services)
		{
			services.AddTransient<IRedisRepository, RedisRepository>();
			services.AddTransient<IRabbitProducer, RabbitProducer>();
			services.AddTransient<IRabbitConsumer, RabbitConsumer>();
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

			app.UseMvc();
        }
    }
}
