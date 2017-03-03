namespace GatewayWithTokenAuth
{
    using System;
    using System.Security.Claims;
    using Configuration;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Tokens;

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            var options = new TokenProviderSettings();
            Configuration.GetSection("TokenProviderSettings").Bind(options);
            services.AddSingleton(options);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            ConfigureAuthentication(app);
            app.UseMvc();
        }

        private void ConfigureAuthentication(IApplicationBuilder app)
        {
            var tokenProviderSettings = app.ApplicationServices.GetService<TokenProviderSettings>();

            var tokenValidationParameters = new TokenValidationParameters
            {
                // Token signature will be verified using a private key.
                ValidateIssuerSigningKey = true,
                RequireSignedTokens = true,
                IssuerSigningKey = tokenProviderSettings.SigningCredentials.Key,

                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = tokenProviderSettings.Issuer,

                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = tokenProviderSettings.Audience,

                //// Token will only be valid if not expired yet, with 5 minutes clock skew.
                ValidateLifetime = true,
                RequireExpirationTime = true,
                ClockSkew = TimeSpan.Zero,
            };

            var jwtBearerOptions = new JwtBearerOptions { AutomaticAuthenticate = true, TokenValidationParameters = tokenValidationParameters, };
            app.UseJwtBearerAuthentication(jwtBearerOptions);
        }
    }
}
