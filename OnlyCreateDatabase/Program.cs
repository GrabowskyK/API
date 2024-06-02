using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OnlyCreateDatabase.Database;
using OnlyCreateDatabase.Services.CourseServ;
using OnlyCreateDatabase.Services.EnrollmentServ;
using OnlyCreateDatabase.Services.ExerciseServ;
using OnlyCreateDatabase.Services.FileUploadServ;
using OnlyCreateDatabase.Services.GradeServ;
using OnlyCreateDatabase.Services.UserServ;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())); ;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<CourseService, CourseService>();
builder.Services.AddScoped<EnrollmentService, EnrollmentService>();
<<<<<<< HEAD
builder.Services.AddScoped<GradeService, GradeService>();
builder.Services.AddScoped<FileUploadService, FileUploadService>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IExerciseService, ExerciseService>();
=======
builder.Services.AddScoped<ExerciseService, ExerciseService>();
>>>>>>> 368f421727a5ccf9043c3168a21c89744671e07f
builder.Services.AddScoped<IFileUploadService, FileUploadService>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Moje API", Version = "v1" });

    // Konfiguracja autoryzacji JWT w Swaggerze
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = (builder.Configuration.GetSection("AppSettings:Token").Value),
        Name = "Authorization",
        In = ParameterLocation.Header,
        //Type = SecuritySchemeType.ApiKey,
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });


    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    }

    );

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("TeacherPolicy", policy => policy.RequireRole("Teacher"));
    options.AddPolicy("StudentPolicy", policy => policy.RequireRole("Student"));
});


builder.Services.AddCors();
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("*"));
app.UseCors("AllowAll");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
