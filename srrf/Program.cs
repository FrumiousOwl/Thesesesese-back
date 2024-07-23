using Microsoft.EntityFrameworkCore;
using srrf.Data;
using srrf.Interface;
using srrf.Repository;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddCors();
builder.Services.AddDbContext<SrrfContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SrrfContext")));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<ICategory, CategoryRepository> ();
builder.Services.AddScoped<IServiceRequest, ServiceRequestRepository> ();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000"));

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
