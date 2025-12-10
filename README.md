# FCM Sender (.NET 8 Web API)

Firebase Cloud Messaging(FCM) 푸시 메시지 발송을 위한 .NET 8 기반 API 서버입니다.

## 빠른 시작

### 1. 저장소 클론

```bash
git clone <repository-url>
cd firebase_cloud_message_test_for_dotnet
```

### 2. 초기화 스크립트 실행

```bash
./init.sh
```

스크립트가 다음을 안내합니다:
- Firebase 프로젝트 ID 입력
- 서비스 계정 JSON 파일 경로 입력
- `.env` 파일 자동 생성

### 3. 실행

```bash
dotnet restore
dotnet run --project src/FcmSender.Api
```

### 4. 테스트

브라우저에서 `http://localhost:5130/swagger` 접속

## 사전 준비

### Firebase 서비스 계정 키 발급

1. [Firebase Console](https://console.firebase.google.com/) 접속
2. 프로젝트 설정 > **서비스 계정** 탭
3. **새 비공개 키 생성** 클릭
4. 다운로드된 JSON 파일 경로를 init.sh 실행 시 입력

### 프로젝트 ID 확인

Firebase Console > 프로젝트 설정 > **일반** 탭에서 확인

> **주의**: 프로젝트 ID는 `my-app-12345` 형식입니다. `1:793178809063:android:...` 같은 앱 ID와 다릅니다.

## 환경 변수

| 변수 | 필수 | 설명 |
|------|------|------|
| `FIREBASE_PROJECTID` | O | Firebase 프로젝트 ID |
| `GOOGLE_APPLICATION_CREDENTIALS` | O | 서비스 계정 JSON 경로 |
| `FIREBASE_DEFAULTDEVICETOKEN` | X | 기본 FCM 토큰 |

## 파일 구조

```
.
├── init.sh                   # 초기화 스크립트
├── .env.example              # 환경 변수 템플릿
├── .env                      # 환경 변수 (Git 무시)
├── secrets/                  # 서비스 계정 JSON (Git 무시)
├── src/
│   ├── FcmSender.Api/        # Minimal API + Swagger
│   └── FcmSender.Core/       # FCM 메시지 전송 로직
└── tests/
    └── FcmSender.Tests/      # 단위 테스트
```

## 참고

- [Firebase Cloud Messaging HTTP v1](https://firebase.google.com/docs/cloud-messaging/send-message)
- [.NET Minimal API](https://learn.microsoft.com/aspnet/core/tutorials/min-web-api)
