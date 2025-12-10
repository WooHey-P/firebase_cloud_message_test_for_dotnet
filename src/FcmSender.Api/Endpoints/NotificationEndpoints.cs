namespace FcmSender.Api.Endpoints;

using System.ComponentModel.DataAnnotations;
using FcmSender.Api.Contracts.Requests;
using FcmSender.Api.Contracts.Responses;
using FcmSender.Core.Abstractions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

public static class NotificationEndpoints
{
    public static RouteGroupBuilder MapNotificationEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/notifications")
            .WithTags("Notifications");

        group.MapPost("/send", SendAsync)
            .WithName("SendNotification")
            .WithSummary("FCM 단일 메시지 전송")
            .WithDescription("Firebase Cloud Messaging HTTP v1 API를 사용해 단일 기기, 토픽 또는 조건에 메시지를 전송합니다.")
            .Produces<SendNotificationResponse>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .ProducesValidationProblem()
            .WithOpenApi(operation =>
            {
                operation.RequestBody = new OpenApiRequestBody
                {
                    Required = false,
                    Content =
                    {
                        ["application/json"] = new OpenApiMediaType
                        {
                            Schema = new OpenApiSchema
                            {
                                Type = "object",
                                Properties =
                                {
                                    ["title"] = new OpenApiSchema { Type = "string", Description = "알림 제목", Nullable = true },
                                    ["body"] = new OpenApiSchema { Type = "string", Description = "알림 본문", Nullable = true },
                                    ["imageUrl"] = new OpenApiSchema { Type = "string", Description = "이미지 URL (선택)", Nullable = true },
                                    ["token"] = new OpenApiSchema { Type = "string", Description = "FCM 디바이스 토큰 (비어있으면 기본값 사용)", Nullable = true },
                                    ["topic"] = new OpenApiSchema { Type = "string", Description = "토픽 이름 (선택)", Nullable = true },
                                    ["condition"] = new OpenApiSchema { Type = "string", Description = "조건식 (선택)", Nullable = true },
                                    ["data"] = new OpenApiSchema { Type = "object", Description = "커스텀 데이터 (선택)", Nullable = true },
                                    ["validateOnly"] = new OpenApiSchema { Type = "boolean", Description = "검증만 수행 (기본값: false)", Nullable = true }
                                }
                            },
                            Examples = new Dictionary<string, OpenApiExample>
                            {
                                ["기본 (토큰 발송)"] = new()
                                {
                                    Summary = "기본 토큰 발송",
                                    Description = "기본 디바이스 토큰으로 알림을 발송합니다. token을 비워두면 .env의 FIREBASE_DEFAULTDEVICETOKEN이 사용됩니다.",
                                    Value = new OpenApiObject
                                    {
                                        ["title"] = new OpenApiString("Hello from server"),
                                        ["body"] = new OpenApiString("This message was triggered via the FCM sender API.")
                                    }
                                },
                                ["토큰 지정"] = new()
                                {
                                    Summary = "특정 토큰으로 발송",
                                    Description = "특정 디바이스 토큰으로 알림을 발송합니다.",
                                    Value = new OpenApiObject
                                    {
                                        ["title"] = new OpenApiString("알림 제목"),
                                        ["body"] = new OpenApiString("알림 본문 내용입니다."),
                                        ["token"] = new OpenApiString("YOUR_FCM_DEVICE_TOKEN_HERE")
                                    }
                                },
                                ["커스텀 데이터 포함"] = new()
                                {
                                    Summary = "커스텀 데이터 포함",
                                    Description = "알림과 함께 커스텀 데이터를 전송합니다.",
                                    Value = new OpenApiObject
                                    {
                                        ["title"] = new OpenApiString("새 메시지"),
                                        ["body"] = new OpenApiString("새로운 메시지가 도착했습니다."),
                                        ["data"] = new OpenApiObject
                                        {
                                            ["screen"] = new OpenApiString("chat"),
                                            ["messageId"] = new OpenApiString("12345")
                                        }
                                    }
                                },
                                ["토픽 발송"] = new()
                                {
                                    Summary = "토픽으로 발송",
                                    Description = "특정 토픽을 구독한 모든 디바이스에 알림을 발송합니다.",
                                    Value = new OpenApiObject
                                    {
                                        ["title"] = new OpenApiString("공지사항"),
                                        ["body"] = new OpenApiString("새로운 공지사항이 등록되었습니다."),
                                        ["topic"] = new OpenApiString("announcements")
                                    }
                                },
                                ["검증만 수행"] = new()
                                {
                                    Summary = "Dry-run (검증만)",
                                    Description = "실제 발송 없이 메시지 유효성만 검증합니다.",
                                    Value = new OpenApiObject
                                    {
                                        ["title"] = new OpenApiString("테스트"),
                                        ["body"] = new OpenApiString("검증 테스트입니다."),
                                        ["validateOnly"] = new OpenApiBoolean(true)
                                    }
                                },
                                ["플랫폼별 설정 (Android/iOS)"] = new()
                                {
                                    Summary = "Android/iOS 플랫폼별 설정 포함",
                                    Description = "Android와 iOS 플랫폼별 우선순위, TTL, APNs 설정을 포함한 전체 요청 예시입니다.",
                                    Value = new OpenApiObject
                                    {
                                        ["token"] = new OpenApiString("YOUR_FCM_DEVICE_TOKEN_HERE"),
                                        ["notification"] = new OpenApiObject
                                        {
                                            ["title"] = new OpenApiString("알림 제목"),
                                            ["body"] = new OpenApiString("알림 내용")
                                        },
                                        ["android"] = new OpenApiObject
                                        {
                                            ["priority"] = new OpenApiString("high"),
                                            ["ttl"] = new OpenApiString("3600s")
                                        },
                                        ["apns"] = new OpenApiObject
                                        {
                                            ["headers"] = new OpenApiObject
                                            {
                                                ["apns-priority"] = new OpenApiString("10"),
                                                ["apns-expiration"] = new OpenApiString("1702234567")
                                            },
                                            ["payload"] = new OpenApiObject
                                            {
                                                ["aps"] = new OpenApiObject
                                                {
                                                    ["content-available"] = new OpenApiInteger(1)
                                                }
                                            }
                                        },
                                        ["data"] = new OpenApiObject
                                        {
                                            ["key1"] = new OpenApiString("value1"),
                                            ["key2"] = new OpenApiString("value2")
                                        }
                                    }
                                }
                            }
                        }
                    }
                };

                operation.Responses["200"] = CreateSuccessResponse();
                return operation;
            });

        return group;
    }

    private static OpenApiResponse CreateSuccessResponse() => new()
    {
        Description = "메시지 전송 성공",
        Content =
        {
            ["application/json"] = new OpenApiMediaType
            {
                Schema = new OpenApiSchema
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.Schema,
                        Id = nameof(SendNotificationResponse)
                    }
                },
                Example = new OpenApiObject
                {
                    ["messageName"] = new OpenApiString("projects/my-project/messages/1234567890"),
                    ["dryRun"] = new OpenApiBoolean(false)
                }
            }
        }
    };

    private static async Task<Results<Ok<SendNotificationResponse>, ValidationProblem>> SendAsync(
        SendNotificationRequest request,
        IFcmSender sender,
        CancellationToken cancellationToken)
    {
        var validationErrors = ValidateRequest(request);
        if (validationErrors.Count > 0)
        {
            return TypedResults.ValidationProblem(validationErrors);
        }

        try
        {
            var result = await sender.SendAsync(request.ToDomainRequest(), cancellationToken).ConfigureAwait(false);
            return TypedResults.Ok(new SendNotificationResponse(result.MessageName, result.IsDryRun));
        }
        catch (InvalidOperationException ex)
        {
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
            {
                ["target"] = new[] { ex.Message }
            });
        }
    }

    private static Dictionary<string, string[]> ValidateRequest(SendNotificationRequest request)
    {
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(request);

        Validator.TryValidateObject(request, context, validationResults, validateAllProperties: true);

        if (validationResults.Count == 0)
        {
            return new Dictionary<string, string[]>();
        }

        return validationResults
            .GroupBy(r => r.MemberNames.FirstOrDefault() ?? string.Empty)
            .ToDictionary(
                g => string.IsNullOrWhiteSpace(g.Key) ? "request" : g.Key,
                g => g.Select(r => r.ErrorMessage ?? "유효성 검사 실패").ToArray());
    }
}
