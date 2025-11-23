
using FreeExam.Application.AutoMapper;
using FreeExam.Application.Contracts.Services;
using FreeExam.Application.Services;
using FreeExam.Infrastructure;
using FreeExam.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FreeExam
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.


            builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IQuestionService, QuestionService>();
            builder.Services.AddScoped<ISubjectService, SubjectService>();
            builder.Services.AddScoped<IExamService, ExamService>();
            builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();


            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                );

            builder.Services.AddAutoMapper(typeof(Mapping));

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("policy", policy => {
                    policy.AllowAnyOrigin()         // ==> (3)
                    .AllowAnyMethod()
                    .AllowAnyHeader();

                });
            });


            //==============================
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
                app.UseSwagger();
                app.UseSwaggerUI();
            //}
            app.UseCors("policy"); // ==> (2)

            app.UseStaticFiles();//==> (1)


            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
