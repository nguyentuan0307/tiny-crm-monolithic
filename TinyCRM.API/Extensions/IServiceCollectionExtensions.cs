﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TinyCRM.Application.Interfaces.IServices;
using TinyCRM.Application.Models;
using TinyCRM.Domain.Entities.Accounts;
using TinyCRM.Domain.Entities.Contacts;
using TinyCRM.Domain.Entities.Deals;
using TinyCRM.Domain.Entities.Leads;
using TinyCRM.Domain.Entities.ProductDeals;
using TinyCRM.Domain.Entities.Products;
using TinyCRM.Domain.Entities.Roles;
using TinyCRM.Domain.Interfaces;
using TinyCRM.Infrastructure;
using TinyCRM.Infrastructure.Identity;
using TinyCRM.Infrastructure.Identity.Repository;
using TinyCRM.Infrastructure.Identity.Service;
using TinyCRM.Infrastructure.Identity.Users;
using TinyCRM.Infrastructure.Repositories;
using TinyCRM.Infrastructure.Service;

namespace TinyCRM.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDataContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Default"), b => b.MigrationsAssembly("TinyCRM.API"));
            }
            );

            services.AddScoped<Func<AppDataContext>>((provider) => () => provider.GetService<AppDataContext>()
                                                                         ?? throw new InvalidOperationException());
            services.AddScoped<DbFactory>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<DataContributor>();
            return services.AddScoped<IAccountRepository, AccountRepository>()
                .AddScoped<IContactRepository, ContactRepository>()
                .AddScoped<IProductRepository, ProductRepository>()
                .AddScoped<ILeadRepository, LeadRepository>()
                .AddScoped<IDealRepository, DealRepository>()
                .AddScoped<IProductDealRepository, ProductDealRepository>()
                .AddScoped<IUserRepository, UserRepository>();
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services.AddScoped<IAccountService, AccountService>()
                .AddScoped<IContactService, ContactService>()
                .AddScoped<IProductService, ProductService>()
                .AddScoped<ILeadService, LeadService>()
                .AddScoped<IDealService, DealService>()
                .AddScoped<IProductDealService, ProductDealService>()
                .AddScoped<IUserService, UserService>();
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Tiny CRM",
                    Version = "v1"
                });

                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    BearerFormat = "JWT",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
            return services;
        }

        public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    // Password settings.
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequiredLength = 6;
                    options.Password.RequiredUniqueChars = 1;

                    // Lockout settings.
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.AllowedForNewUsers = true;

                    // User settings.
                    options.User.AllowedUserNameCharacters =
                        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<AppDataContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = configuration["JWT:ValidAudience"],
                    ValidIssuer = configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                        configuration["JWT:SecretKey"] ?? string.Empty))
                };
            });
            return services;
        }

        public static IServiceCollection AddAuthorizations(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policy.SuperAdminPolicy,
                    policy => policy.RequireRole(Role.SuperAdmin));
                options.AddPolicy(Policy.AdminPolicy,
                    policy => policy.RequireRole(Role.SuperAdmin, Role.Admin));
                options.AddPolicy(Policy.UserPolicy,
                    policy => policy.RequireRole(Role.User, Role.Admin, Role.SuperAdmin));
            });

            return services;
        }

        public static async Task SeedDataAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var seedData = scope.ServiceProvider.GetRequiredService<DataContributor>();
            await seedData.SeedAsync();
        }

        public static void ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection("JWT"));
        }
    }
}