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
// CONSTRUÇÃO E EXECUÇÃO DO APP
// ============================================

var app = builder.Build();

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