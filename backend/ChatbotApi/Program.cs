using ChatbotApi;
using ChatbotApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the ChatbotService as a singleton
builder.Services.AddSingleton<ChatbotService>(serviceProvider => {
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var modelPath = configuration.GetValue<string>("ModelPath") ?? "chatbot_model.zip";
    return new ChatbotService(modelPath);
});

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // Ensure this matches your React app's URL
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Apply CORS Middleware before authorization
app.UseCors("AllowReactApp");

app.UseAuthorization();
app.MapControllers();

app.Run();
