using Microsoft.EntityFrameworkCore;
using Sprint1_2semestre.Data;
using Sprint1_2semestre.Services; // Adicionando o ConfigManager

var builder = WebApplication.CreateBuilder(args);

// Configura a string de conexão (use sua string de conexão real aqui)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Adiciona o ApplicationDbContext ao contêiner de injeção de dependências
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseOracle(connectionString));  // Certifique-se de ter o pacote Oracle.EntityFrameworkCore instalado

// Adiciona o ConfigManager como Singleton no contêiner de serviços
builder.Services.AddSingleton<ConfigManager>();

// Adiciona serviços de controladores e configura JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// Configura o CORS para permitir requisições de qualquer origem
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Adiciona o Swagger para a documentação da API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurações de desenvolvimento (Swagger e SwaggerUI)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Habilita o CORS com a política definida
app.UseCors("AllowAll");

// Adiciona middleware para autorização
app.UseAuthorization();

// Mapeia os controladores da API
app.MapControllers();

// Executa a aplicação
app.Run();
