using API.Utils;
using Azure.Storage.Blobs;
using BusinessLogic;
using BusinessLogic.Domain;
using BusinessLogic.Repository;
using BusinessLogic.Services;
using BusinessLogic.SubSystem;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Repository.EntityRepositories;
using Repository.Logging;
using System.Text;
using System.Text.Json.Serialization;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var key = builder.Configuration["Jwt:Key"];

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = StatusCodes.Status403Forbidden;
                            context.Response.ContentType = "application/json";
                            return context.Response.WriteAsync("{\"error\": \"Acceso denegado: no dispone de los permisos necesarios para realizar esta acción.\"}");
                        },
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.ContentType = "application/json";
                            return context.Response.WriteAsync("{\"error\": \"No autenticado: debe iniciar sesión para acceder a este recurso.\"}");
                        }
                    };
                });



            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Escribí: 'Bearer {tu token}'"
                });

                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("ManageImages", policy =>
                    policy.RequireAssertion(context =>
                    {
                        var allowedPermissions = new[]
                        {
                            ((int)Permission.CREATE_PRODUCTS).ToString(),
                            ((int)Permission.CREATE_ZONES).ToString(),
                            ((int)Permission.EDIT_PRODUCTS).ToString(),
                            ((int)Permission.EDIT_ZONES).ToString()
                        };

                        return context.User.HasClaim(c =>
                            c.Type == "permission" && allowedPermissions.Contains(c.Value));
                    }));

                foreach (var value in Enum.GetValues(typeof(Permission)))
                {
                    var intValue = ((int)value).ToString();
                    var name = Enum.GetName(typeof(Permission), value);

                    options.AddPolicy(name, policy =>
                        policy.RequireClaim("permission", intValue));
                }
            });


            builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<TokenUtils>();

            var connectionString = builder.Configuration.GetConnectionString("Database");
            builder.Services.AddSingleton(connectionString);
            builder.Services.AddSingleton(new BlobServiceClient(builder.Configuration.GetConnectionString("AzureBlob")));
            builder.Services.AddScoped<IAzureBlobService, AzureBlobService>();


            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IClientRepository, ClientRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IStockMovementRepository, StockMovementRepository>();
            builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
            builder.Services.AddScoped<IWarehouseRepository, WarehouseRepository>();
            builder.Services.AddScoped<IZoneRepository, ZoneRepository>();
            builder.Services.AddScoped<ISubCategoryRepository, SubCategoryRepository>();
            builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
            builder.Services.AddScoped<IVehicleCostRepository, VehicleCostRepository>();
            builder.Services.AddScoped<IShelveRepository, ShelveRepository>();
            builder.Services.AddScoped<IBrandRepository, BrandRepository>();
            builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
            builder.Services.AddScoped<IOrderRequestRepository, OrderRequestRepository>();

            builder.Services.AddScoped<IAzureBlobService, AzureBlobService>();

            builder.Services.AddScoped<RolesSubSystem>();
            builder.Services.AddScoped<UserSubSystem>();
            builder.Services.AddScoped<AuthSubSystem>();
            builder.Services.AddScoped<CategoriesSubSystem>();
            builder.Services.AddScoped<ClientsSubSystem>();
            builder.Services.AddScoped<ProductsSubSystem>();
            builder.Services.AddScoped<StockSubSystem>();
            builder.Services.AddScoped<SuppliersSubSystem>();
            builder.Services.AddScoped<WarehousesSubSystem>();
            builder.Services.AddScoped<ZonesSubSystem>();
            builder.Services.AddScoped<VehicleSubSystem>();
            builder.Services.AddScoped<BrandSubSystem>();
            builder.Services.AddScoped<DiscountsSubSystem>();
            builder.Services.AddScoped<OrderRequestSubSystem>();

            builder.Services.AddScoped<AuditLogger>();
            builder.Services.AddScoped<Facade>();

            //var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("PermitirFrontend", policy =>
                 {
                     // policy.WithOrigins(allowedOrigins)
                     policy.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                 });
            });

            var app = builder.Build();

            // if (app.Environment.IsDevelopment())
            // {
            app.UseSwagger();
            app.UseSwaggerUI();
            // }
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseCors("PermitirFrontend");
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}
