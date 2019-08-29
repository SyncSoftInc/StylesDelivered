using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SyncSoft.ECP;
using SyncSoft.ECP.AspNetCore.Hosting;
using SyncSoft.ECP.AspNetCore.Mvc.ActionFilters;

namespace SyncSoft.StylesDelivered.WebSite
{
    public class Startup : SerilogStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddWebSiteServer(o =>
            {
                o.WholeSiteAuthPolicy = CONSTANTs.Policies.BearerOpenId;
                o.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);

                o.ConfigureMvcOptions = a =>
                {
                    a.Filters.Add(new MessageHandlerAttribute());
                    //a.Conventions.Add(new FromBodyBindingSourceConvention());
                };
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseReverseProxy();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseWebSiteServer();
        }
    }
}
