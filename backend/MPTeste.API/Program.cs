using Microsoft.EntityFrameworkCore;
using MPTeste.API.Data;
using MPTeste.API.Middleware;
using MPTeste.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuração de Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Configuração do Banco de Dados
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// Configuração de CORS com origens específicas (seguro)
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirOrigesLocais", policy =>
    {
        var origins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? ["http://localhost:3000"];

        policy.WithOrigins(origins)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Adiciona os controladores
builder.Services.AddControllers();

// Configuração do Swagger (Swashbuckle)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Injeção de Dependências
builder.Services.AddScoped<CategoriaService>();
builder.Services.AddScoped<PessoaService>();
builder.Services.AddScoped<TransacaoService>();

var app = builder.Build();

// Middleware de tratamento global de exceções
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("PermitirOrigesLocais");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();