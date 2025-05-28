using Microsoft.EntityFrameworkCore;
using Voluta.Data;
using Voluta.Repositories;
using Voluta.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllers();

app.Run();
