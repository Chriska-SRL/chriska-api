
using BusinessLogic;
using BusinessLogic.Repository;
using BusinessLogic.SubSystem;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Acá se registran servicios
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Acá es donde vas a registrar tus repositorios, subsystems y la Facade 👇
            // Ejemplo:
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<UserSubSystem>();
            builder.Services.AddScoped<Facade>();

            var app = builder.Build();

            // Configuración de middlewares
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
