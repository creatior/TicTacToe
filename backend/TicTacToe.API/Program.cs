using Microsoft.EntityFrameworkCore;
using TicTacToe.Application.Services;
using TicTacToe.DataAccess;
using TicTacToe.DataAccess.Repositories;

namespace TicTacToe
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.WebHost.UseUrls("http://+:5000");

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost3000",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:3000")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            builder.Services.AddDbContext<TicTacToeDbContext>(
                options =>
                {
                    options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(TicTacToeDbContext)));
                });

            builder.Services.AddScoped<IUsersService, UsersService>();
            builder.Services.AddScoped<IUsersRepository, UsersRepository>();

            builder.Services.AddScoped<IGamesService, GamesService>();
            builder.Services.AddScoped<IGamesRepository, GamesRepository>();

            var app = builder.Build();

            // auto migrations
            using (var scope = app.Services.CreateScope())
            {
                 var dbContext = scope.ServiceProvider.GetRequiredService<TicTacToeDbContext>();
                 dbContext.Database.Migrate();
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowLocalhost3000");

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
