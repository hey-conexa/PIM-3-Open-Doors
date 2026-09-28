using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using OpenDoors.Api.Middleware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OpenDoors.Api.Interfaces.TestesVocacionais;

var builder = WebApplication.CreateBuilder(args);

// ============================================
// CONFIGURAÇÕES DOS SERVIÇOS
// ============================================

// Adiciona suporte a Controllers (as classes que vão receber as requisições HTTP)
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

// Suporte ao Swagger (documentação automática da API)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Habilita CORS para o frontend conseguir acessar essa API depois
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFrontend", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// ============================================
// CONFIGURAÇÃO DO SUPABASE
// ============================================

// Lê as configurações do appsettings.json + appsettings.Development.json
var supabaseUrl = builder.Configuration["Supabase:Url"];
var supabaseKey = builder.Configuration["Supabase:Key"];

// Valida se as configurações foram carregadas
if (string.IsNullOrEmpty(supabaseUrl) || string.IsNullOrEmpty(supabaseKey))
{
    throw new Exception("Configurações do Supabase não foram encontradas! Verifique appsettings.json e appsettings.Development.json");
}

// Cria a instância do cliente Supabase e registra como Singleton
// (Singleton = uma única instância compartilhada na aplicação inteira, padrão de injeção de dependência)
builder.Services.AddSingleton<Supabase.Client>(_ =>
{
    var options = new Supabase.SupabaseOptions
    {
        AutoConnectRealtime = true
    };
    var client = new Supabase.Client(supabaseUrl, supabaseKey, options);
    client.InitializeAsync().Wait();
    return client;
});

// ============================================
// AUTENTICAÇÃO JWT (Supabase)
// ============================================

// Busca as chaves públicas do Supabase (ES256) e faz cache em memória
var jwksUri = $"{supabaseUrl}/auth/v1/.well-known/jwks.json";
IList<SecurityKey>? cachedJwksKeys = null;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            IssuerSigningKeyResolver = (_, _, _, _) =>
            {
                if (cachedJwksKeys == null)
                {
                    using var http = new System.Net.Http.HttpClient();
                    var jwksJson = http.GetStringAsync(jwksUri).Result;
                    cachedJwksKeys = new JsonWebKeySet(jwksJson).GetSigningKeys();
                }
                return cachedJwksKeys;
            }
        };
    });

// ============================================
// CONFIGURAÇÃO DA IA (Groq + Services)
// ============================================

// Registro correto do GroqService — permite injeção automática em outros services
builder.Services.AddHttpClient<OpenDoors.Api.Services.IA.GroqService>();
builder.Services.AddScoped<OpenDoors.Api.Services.IA.GroqService>();

builder.Services.AddScoped<OpenDoors.Api.Services.IA.AnalisarCurriculoService>();
builder.Services.AddScoped<OpenDoors.Api.Services.IA.AnalisarTesteService>();
builder.Services.AddScoped<OpenDoors.Api.Services.GerarScoreService>();
builder.Services.AddScoped<OpenDoors.Api.Services.GerarPerguntasMensaisService>();

builder.Services.AddHttpClient<OpenDoors.Api.Services.JoobleService>();
builder.Services.AddScoped<OpenDoors.Api.Services.JoobleService>();

// ============================================
// REPOSITÓRIOS E SERVIÇOS - IA (REFATORADO)
// ============================================

// Repositório para Testes Vocacionais
builder.Services.AddScoped<ITesteVocacionalRepository, 
    OpenDoors.Api.Repositories.IA.TesteVocacionalRepository>();

// Serviço que orquestra análises de IA
builder.Services.AddScoped<OpenDoors.Api.Interfaces.IA.IAnalisarIaService, 
    OpenDoors.Api.Services.IA.AnalisarIaService>();

// ============================================
// REPOSITÓRIOS E SERVIÇOS - VAGAS (REFATORADO)
// ============================================

// Repositório para Vagas
builder.Services.AddScoped<OpenDoors.Api.Interfaces.Vagas.IVagaRepository, 
    OpenDoors.Api.Repositories.Vagas.VagaRepository>();

// Serviço de Vagas
builder.Services.AddScoped<OpenDoors.Api.Interfaces.Vagas.IVagaService, 
    OpenDoors.Api.Services.Vagas.VagaService>();

// ============================================
// REPOSITÓRIOS E SERVIÇOS - PERGUNTAS TESTE (REFATORADO)
// ============================================

// Repositório para Perguntas de Teste
builder.Services.AddScoped<OpenDoors.Api.Interfaces.PerguntasTeste.IPerguntaTesteRepository, 
    OpenDoors.Api.Repositories.PerguntasTeste.PerguntaTesteRepository>();

// Serviço de Perguntas de Teste
builder.Services.AddScoped<OpenDoors.Api.Interfaces.PerguntasTeste.IPerguntaTesteService, 
    OpenDoors.Api.Services.PerguntasTeste.PerguntaTesteService>();

// ============================================
// REPOSITÓRIOS E SERVIÇOS - JOOBLE (REFATORADO)
// ============================================

// Repositório para Jooble
builder.Services.AddScoped<OpenDoors.Api.Interfaces.Jooble.IJoobleRepository, 
    OpenDoors.Api.Repositories.Jooble.JoobleRepository>();

// Serviço de Integração Jooble
builder.Services.AddScoped<OpenDoors.Api.Interfaces.Jooble.IJoobleService, 
    OpenDoors.Api.Services.Jooble.JoobleIntegrationService>();

// ============================================
// CONSTRUÇÃO E EXECUÇÃO DO APP
// ============================================

var app = builder.Build();

// Em ambiente de desenvolvimento, mostra o Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Registra o middleware de tratamento global de exceções
app.UseExceptionHandler();

app.UseCors("PermitirFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
