using OpenDoors.Api.Interfaces.Candidaturas;
using OpenDoors.Api.Interfaces.CandidaturasHistorico;
using OpenDoors.Api.Interfaces.Empresas;
using OpenDoors.Api.Interfaces.Estudantes;
using OpenDoors.Api.Interfaces.Matches;
using OpenDoors.Api.Interfaces.Notificacoes;
using OpenDoors.Api.Interfaces.TestesRespostas;
using OpenDoors.Api.Interfaces.TestesVocacionais;
using OpenDoors.Api.Interfaces.Vagas;
using OpenDoors.Api.Middleware;
using OpenDoors.Api.Repositories.CandidaturaHistoricos;
using OpenDoors.Api.Repositories.Candidaturas;
using OpenDoors.Api.Repositories.Empresas;
using OpenDoors.Api.Repositories.Estudantes;
using OpenDoors.Api.Repositories.Matchs;
using OpenDoors.Api.Repositories.Notificacoes;
using OpenDoors.Api.Repositories.TesteRespostas;
using OpenDoors.Api.Repositories.TesteVocacionais;
using OpenDoors.Api.Repositories.Vagas;
using OpenDoors.Api.Services;
using OpenDoors.Api.Services.CandidaturaHistoricos;
using OpenDoors.Api.Services.Candidaturas;
using OpenDoors.Api.Services.Empresas;
using OpenDoors.Api.Services.Estudantes;
using OpenDoors.Api.Services.Matchs;
using OpenDoors.Api.Services.Notificacoes;
using OpenDoors.Api.Services.TesteRespostas;
using OpenDoors.Api.Services.TesteVocacionais;
using OpenDoors.Api.Services.Vagas;

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
// CONFIGURAÇÃO DA IA (Groq + Services)
// ============================================

builder.Services.AddHttpClient<OpenDoors.Api.Services.GroqService>();

builder.Services.AddScoped<OpenDoors.Api.Services.GroqService>(sp =>
{
    var http = sp.GetRequiredService<IHttpClientFactory>().CreateClient();
    var config = sp.GetRequiredService<IConfiguration>();
    return new OpenDoors.Api.Services.GroqService(config, http);
});

builder.Services.AddScoped<IEstudanteRepository, EstudanteRepositorySupabase>();
builder.Services.AddScoped<IEstudanteService, EstudanteService>();

builder.Services.AddScoped<OpenDoors.Api.Services.AnalisarCurriculoService>();
builder.Services.AddScoped<OpenDoors.Api.Services.AnalisarTesteService>();
builder.Services.AddScoped<OpenDoors.Api.Services.GerarScoreService>();

builder.Services.AddScoped<IEmpresaRepository, EmpresaRepositorySupabase>();
builder.Services.AddScoped<IEmpresaService, EmpresaService>();

builder.Services.AddScoped<ICandidaturaRepository, CandidaturaRepositorySupabase>();
builder.Services.AddScoped<ICandidaturaService, CandidaturaService>();

// CandidaturaHistorico
builder.Services.AddScoped<ICandidaturaHistoricoRepository, CandidaturaHistoricoRepositorySupabase>();
builder.Services.AddScoped<ICandidaturaHistoricoService, CandidaturaHistoricoService>();

// Match
builder.Services.AddScoped<IMatchRepository, MatchRepositorySupabase>();
builder.Services.AddScoped<IMatchService, MatchService>();

// Notificacao
builder.Services.AddScoped<INotificacaoRepository, NotificacaoRepositorySupabase>();
builder.Services.AddScoped<INotificacaoService, NotificacaoService>();

// TesteResposta
builder.Services.AddScoped<ITesteRespostaRepository, TesteRespostaRepositorySupabase>();
builder.Services.AddScoped<ITesteRespostaService, TesteRespostaService>();

// TesteVocacional
builder.Services.AddScoped<ITesteVocacionalRepository, TesteVocacionalRepositorySupabase>();
builder.Services.AddScoped<ITesteVocacionalService, TesteVocacionalService>();

// Vaga
builder.Services.AddScoped<IVagaRepository, VagaRepositorySupabase>();
builder.Services.AddScoped<IVagaService, VagaService>();

// ============================================
// CONSTRUÇÃO E EXECUÇÃO DO APP
// ============================================

var app = builder.Build();

app.UseMiddleware<ExceptionHandlerMiddleware>();

// Em ambiente de desenvolvimento, mostra o Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("PermitirFrontend");
app.UseAuthorization();
app.MapControllers();

app.Run();