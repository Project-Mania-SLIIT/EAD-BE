using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using StudentManagement.Models;
using StudentManagement.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<StudentStoreDatabaseSettings>(
            builder.Configuration.GetSection(nameof(StudentStoreDatabaseSettings)));

builder.Services.AddSingleton<IStudentStoreDatabaseSettings>(sp =>
    sp.GetRequiredService<IOptions<StudentStoreDatabaseSettings>>().Value);

builder.Services.AddSingleton<IMongoClient>(s =>
        new MongoClient(builder.Configuration.GetValue<string>("StudentStoreDatabaseSettings:ConnectionString")));

builder.Services.AddScoped<IStudentService, StudentService>();

//user setting 
// Add services to the container.
builder.Services.Configure<UserStoreDatabaseSettings>(
                builder.Configuration.GetSection(nameof(StudentStoreDatabaseSettings)));

builder.Services.AddSingleton<IUserStoreDatabaseSettings>(sp =>
    sp.GetRequiredService<IOptions<UserStoreDatabaseSettings>>().Value);

builder.Services.AddSingleton<IMongoClient>(s =>
        new MongoClient(builder.Configuration.GetValue<string>("UserStoreDatabaseSettings:ConnectionString")));

builder.Services.AddScoped<IUserService, UserService>();
//

// Add services to the train.
builder.Services.Configure<TrainStoreDatabaseSetting>(
                builder.Configuration.GetSection(nameof(TrainStoreDatabaseSetting)));

builder.Services.AddSingleton<ITrainStoreDatabaseSetting>(sp =>
    sp.GetRequiredService<IOptions<TrainStoreDatabaseSetting>>().Value);

builder.Services.AddSingleton<IMongoClient>(s =>
        new MongoClient(builder.Configuration.GetValue<string>("TrainStoreDatabaseSetting:ConnectionString")));

builder.Services.AddScoped<ITrainService, TrainService>();

//


// Add services to the booking.
builder.Services.Configure<BookingStoreDatabaseSetting>(
                builder.Configuration.GetSection(nameof(BookingStoreDatabaseSetting)));

builder.Services.AddSingleton<IBookingStoreDatabaseSetting>(sp =>
    sp.GetRequiredService<IOptions<BookingStoreDatabaseSetting>>().Value);

builder.Services.AddSingleton<IMongoClient>(s =>
        new MongoClient(builder.Configuration.GetValue<string>("BookingStoreDatabaseSetting:ConnectionString")));

builder.Services.AddScoped<IBookingService, BookingService>();

//

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        }
        );

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
