using AiCodeReviewAgent.Agents;
using AiCodeReviewAgent.Services;
using AiCodeReviewAgent.Tools;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Agent dependencies
builder.Services.AddScoped<GitDiffAnalysisTool>();
builder.Services.AddScoped<CodeReviewAgent>();

// OpenAI HTTP client
//builder.Services.AddHttpClient<OpenAIService>(client =>
//{
//    client.Timeout = TimeSpan.FromSeconds(90);
//});
//builder.Services.AddHttpClient<GeminiService>(
//    (serviceProvider, client) =>
//    {
//        var configuration = serviceProvider
//            .GetRequiredService<IConfiguration>();

//        var baseUrl = configuration["Gemini:BaseUrl"];
//        client.BaseAddress = new Uri(baseUrl!);
//        client.Timeout = TimeSpan.FromMinutes(2);
//    });


builder.Services.AddScoped<SecurityAnalysisTool>();

builder.Services.AddScoped<TestAnalysisTool>();



builder.Services.AddHttpClient<GeminiService>(
    (serviceProvider, client) =>
    {
        var configuration = serviceProvider
            .GetRequiredService<IConfiguration>();

        var baseUrl = configuration["Gemini:BaseUrl"];

        if (string.IsNullOrWhiteSpace(baseUrl))
            throw new InvalidOperationException(
                "Gemini BaseUrl is missing from appsettings.json.");

        client.BaseAddress = new Uri(baseUrl);
        client.Timeout = TimeSpan.FromMinutes(5);
    });
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
