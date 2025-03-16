using Microsoft.EntityFrameworkCore;
using task_back.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TaskDbContext>(opt => opt.UseInMemoryDatabase("TaskDB"));
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();
app.UseCors("AllowAngularApp");
app.MapControllers();
app.Run();