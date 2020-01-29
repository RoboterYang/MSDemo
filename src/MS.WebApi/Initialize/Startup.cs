using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MS.DbContexts;
using MS.Models.Automapper;
using MS.Services;
using MS.UnitOfWork;
using MS.WebCore;

namespace MS.WebApi
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
            services.AddControllers();

            //注册跨域策略
            services.AddCorsPolicy(Configuration);
            //注册webcore服务（网站主要配置）
            services.AddWebCoreService(Configuration);

            //注册工作单元（同时注册了DBContext）
            services.AddUnitOfWorkService<MSDbContext>(options => { options.UseMySql(Configuration.GetSection("ConectionStrings:MSDbContext").Value); });

            //注册automapper服务
            services.AddAutomapperService();

            //注册IBaseService和IRoleService接口及对应的实现类
            services.AddScoped<IBaseService, BaseService>();
            services.AddScoped<IRoleService, RoleService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
