using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MoviePoint.Hubs;
using MoviePoint.logic.Repository;
using MoviePoint.Models;
using MoviePoint.Repository;
using System.Security.Principal;

namespace MoviePoint
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<MoviePointContext>(option =>
            {
                //option.UseSqlServer(builder.Configuration.GetConnectionString("Esraa"));
                option.UseSqlServer(builder.Configuration.GetConnectionString("Hadeer"));
                //option.UseSqlServer(builder.Configuration.GetConnectionString("Ghada"));

                //option.UseSqlServer(builder.Configuration.GetConnectionString("Asmaa"));
                //option.UseSqlServer(builder.Configuration.GetConnectionString("Alaa"));


            });






            builder.Services.AddSignalR(options =>
			{
				options.EnableDetailedErrors = true;
			});
			builder.Services.AddCors(
                options =>

                    options.AddDefaultPolicy(cong => {
                        cong.AllowAnyMethod().
                        SetIsOriginAllowed((host) => true)
                        .AllowAnyHeader()
                        .AllowCredentials();
                    })
                );

            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
            }
            ).AddEntityFrameworkStores<MoviePointContext>();

            builder.Services.AddScoped<IActorRepository, ActorRepository>();
            builder.Services.AddScoped<ICinemaRepository, CinemaRepository>();
            builder.Services.AddScoped<IProducerRepository, ProducerRepository>();
            builder.Services.AddScoped<IMovieRepository, MovieRepository>();
            builder.Services.AddScoped<IActorMovieRepository, ActorMovieRepository>();
            builder.Services.AddScoped<ICommentsRepository, CommentsRepository>();
			builder.Services.AddScoped<ITicketsRepository, TicketsRepository>();


          

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors();

            app.UseAuthentication();

            app.UseAuthorization();
            app.MapHub<MovieHub>("/MovieHub");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=home}/{id?}");

            app.Run();
        }
    }
}