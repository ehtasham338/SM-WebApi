using Scalar.AspNetCore;
using StudentManagement.Domain.Repositories;
using StudentManagement.Infrastructure.Repositories;
using StudentManagement.Infrastructure.Services;
using StudentManagement.Domain.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using StudentManagement.API.Validators;
using StudentManagement.API.Dtos;
using StudentManagement.API.Middleware;
using Serilog;
using Serilog.Sinks.MSSqlServer;

// --- 1. SERILOG SETUP (Console + File + Database) ---
var builder = WebApplication.CreateBuilder(args);

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console() // Console par logs
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day) // Roz nayi file banegi
    .WriteTo.MSSqlServer(
        connectionString: connectionString,
        sinkOptions: new MSSqlServerSinkOptions
        {
            TableName = "AppLogs", // SQL Server mein table ka naam
            AutoCreateSqlTable = true // Table apne aap ban jayega
        })
    .MinimumLevel.Information()
    .CreateLogger();

builder.Host.UseSerilog(); // .NET ko batana ke Serilog use karna hai

// --- 2. CORE SERVICES ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// --- 3. FLUENTVALIDATION ---
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<StudentCreateDtoValidator>();

builder.Services.AddValidatorsFromAssemblyContaining<UserRegisterDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UserLoginDtoValidator>();

// --- 4. AUTOMAPPER ---
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// --- 5. SERVICE LAYER DI ---
builder.Services.AddScoped<IStudentService, StudentService>();
// 8. Auth Service DI (Yeh line add karein)
builder.Services.AddScoped<IAuthService, AuthService>();

// --- 6. REPOSITORY LAYER DI (ADO.NET) ---
builder.Services.AddScoped<IStudentRepository>(sp =>
{
    string connStr = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string not found");

    return new StudentRepository(connStr);
});


//  User Repository DI (ADO.NET)
builder.Services.AddScoped<IUserRepository>(sp =>
{
    string connStr = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string not found");
    return new UserRepository(connStr);
});

// Token Service DI
builder.Services.AddScoped<ITokenService, TokenService>();

// ----------------------------------------------------

var app = builder.Build();

// --- 7. MIDDLEWARE PIPELINE ---

//  Correlation ID middleware requet unique id 
app.UseMiddleware<CorrelationIdMiddleware>();


app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();