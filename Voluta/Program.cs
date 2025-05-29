using Microsoft.EntityFrameworkCore;
using Voluta.Data;
using Voluta.Repositories;
using Voluta.Services;
using Voluta.Middleware;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.AspNetCore;
using System.Reflection;
using Voluta.Filters;

var builder = WebApplication.CreateBuilder(args);

// Adiciona os controllers e configura o filtro de validação
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
})
.AddFluentValidation(fv => 
{
    fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    fv.AutomaticValidationEnabled = true;
    fv.ImplicitlyValidateChildProperties = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Voluta API", 
        Version = "v1",
        Description = "API do projeto Voluta"
    });
});

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});

// Registra todos os validadores automaticamente
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddDbContext<VolutaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexaoLocal")));

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IOngRepository, OngRepository>();
builder.Services.AddScoped<ISolicitacaoVoluntariadoRepository, SolicitacaoVoluntariadoRepository>();

builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IOngService, OngService>();
builder.Services.AddScoped<ISolicitacaoService, SolicitacaoService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Voluta API v1"));
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseMiddleware<MiddlewareDeErros>();
app.UseResponseCompression();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
