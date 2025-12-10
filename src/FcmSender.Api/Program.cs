using DotNetEnv;
using FcmSender.Api.Endpoints;
using FcmSender.Core.Abstractions;
using FcmSender.Core.Options;
using FcmSender.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

TryLoadDotEnv();

var builder = WebApplication.CreateBuilder(args);

// 환경 변수를 Configuration에 추가
builder.Configuration.AddEnvironmentVariables();

// FIREBASE_PROJECTID, FIREBASE_DEFAULTDEVICETOKEN 환경 변수를 Firebase 섹션에 매핑
MapFirebaseEnvironmentVariables(builder.Configuration);

builder.Services.AddOptions<FirebaseOptions>()
    .BindConfiguration(FirebaseOptions.SectionName)
    .ValidateDataAnnotations()
    .Validate(
        options => !string.IsNullOrWhiteSpace(options.ProjectId),
        $"{FirebaseOptions.SectionName}:ProjectId 설정은 필수입니다.");

builder.Services.AddSingleton<IFirebaseCredentialProvider, FirebaseCredentialProvider>();
builder.Services.AddSingleton<IFcmMessageFactory, FcmMessageFactory>();
builder.Services.AddScoped<IFcmSender, FirebaseMessagingSender>();

builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "FCM Sender API",
        Version = "v1",
        Description = "Firebase Cloud Messaging(Firebase) 서버 메시지 전송 API"
    });
    options.SupportNonNullableReferenceTypes();
});

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();
app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "FCM Sender API v1");
    options.DocumentTitle = "FCM Sender API";
});

app.MapNotificationEndpoints();

app.Run();

static void TryLoadDotEnv()
{
    try
    {
        Env.TraversePath().Load();
    }
    catch (FileNotFoundException)
    {
        // Ignore when .env is absent.
    }
}

static void MapFirebaseEnvironmentVariables(IConfigurationManager configuration)
{
    var projectId = Environment.GetEnvironmentVariable("FIREBASE_PROJECTID");
    var defaultDeviceToken = Environment.GetEnvironmentVariable("FIREBASE_DEFAULTDEVICETOKEN");

    if (!string.IsNullOrWhiteSpace(projectId))
    {
        configuration[$"{FirebaseOptions.SectionName}:ProjectId"] = projectId;
    }

    if (!string.IsNullOrWhiteSpace(defaultDeviceToken))
    {
        configuration[$"{FirebaseOptions.SectionName}:DefaultDeviceToken"] = defaultDeviceToken;
    }
}
