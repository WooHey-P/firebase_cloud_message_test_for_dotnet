# FCM Sender (.NET 8 Web API)

A .NET 8 based API server for sending Firebase Cloud Messaging (FCM) push notifications.

[한국어](README.ko.md)

## Quick Start

### 1. Clone the repository

```bash
git clone <repository-url>
cd firebase_cloud_message_test_for_dotnet
```

### 2. Run the initialization script

```bash
./init.sh
```

The script will guide you through:

- Entering your Firebase Project ID
- Specifying the service account JSON filename
- Automatically generating the `.env` file

### 3. Run the server

```bash
dotnet restore
dotnet run --project src/FcmSender.Api
```

### 4. Test

Open `http://localhost:5130/swagger` in your browser

## Prerequisites

### Get Firebase Service Account Key

1. Go to [Firebase Console](https://console.firebase.google.com/)
2. Project Settings > **Service accounts** tab
3. Click **Generate new private key**
4. Save the downloaded JSON file to `secrets/` folder

### Find Your Project ID

Firebase Console > Project Settings > **General** tab

> **Note**: Project ID looks like `my-app-12345`. It's NOT the App ID like `1:793178809063:android:...`

## Environment Variables

| Variable | Required | Description |
|----------|----------|-------------|
| `FIREBASE_PROJECTID` | Yes | Firebase Project ID |
| `GOOGLE_APPLICATION_CREDENTIALS` | Yes | Path to service account JSON |
| `FIREBASE_DEFAULTDEVICETOKEN` | No | Default FCM device token |

## Project Structure

```
.
├── init.sh                   # Initialization script
├── .env.example              # Environment variable template
├── .env                      # Environment variables (git ignored)
├── secrets/                  # Service account JSON (git ignored)
├── src/
│   ├── FcmSender.Api/        # Minimal API + Swagger
│   └── FcmSender.Core/       # FCM message sending logic
└── tests/
    └── FcmSender.Tests/      # Unit tests
```

## References

- [Firebase Cloud Messaging HTTP v1](https://firebase.google.com/docs/cloud-messaging/send-message)
- [.NET Minimal API](https://learn.microsoft.com/aspnet/core/tutorials/min-web-api)
