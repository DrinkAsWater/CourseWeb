# WebMVC 課程管理系統

ASP.NET Core 8.0 多層式架構課程管理系統，包含 Web API、MVC 前端、資料存取與業務邏輯層，以及教學示範專案。

---

## 方案結構

```
WebMVC.sln
├── WebMVC              # ASP.NET MVC 教學示範專案
├── CourseWeb           # 課程管理 MVC 前端（Cookie 認證）
├── CourseAPI           # 課程管理 REST API（JWT 認證）
├── CourseData          # 資料存取層（EF Core + SQL Server）
├── CourseService       # 業務邏輯層（Service / Repository 介面）
└── ConsoleAppClient    # API 呼叫測試用主控台程式
```

---

## 各專案說明

### WebMVC — MVC 教學示範

**用途**：示範 ASP.NET Core MVC 的各項核心概念，與資料庫無關。

| 功能 | 說明 |
|------|------|
| `BooksController` | 書籍 CRUD，示範 ServiceFilter、屬性路由 |
| `TagHelperTutorialController` | Tag Helper 用法示範 |
| `BaseController` | 共用基底 Controller，封裝 HandleException、ShowNotFound |
| `AuthorizationFilter` | 自訂 `IAsyncAuthorizationFilter`，模擬角色驗證 |
| `TimestampActionFilter` | 動作過濾器，注入時間戳記 |
| `RequestLoggingMiddleware` | 自訂中介軟體，記錄每個 HTTP 請求 |
| `IBookService / BookService_V1` | 介面 + DI 示範，切換不同實作版本 |

---

### CourseWeb — 課程管理 MVC 前端

**用途**：提供學員操作介面，使用 **Cookie 認證**。

| Controller | 路由 | 功能 |
|---|---|---|
| `HomeController` | `/` | 首頁、課程列表 |
| `MemberController` | `/Member/*` | 註冊、登入、登出、修改密碼、修改個人資料 |
| `ShopController` | `/Shop/*` | 查看與登記課程 |

**認證方式**：`CookieAuthenticationDefaults`，Cookie 名稱 `UserLoginCookie`，未登入導向 `/Member/Login`。

**ViewModel**：`LoginViewModel`、`UserRegisterViewModel`、`UserChgPwdViewModel`、`UserChgInfoViewModel`、`CourseScheduleViewModel`、`UserCourseScheduleViewModel`

---

### CourseAPI — 課程管理 REST API

**用途**：提供 RESTful API，使用 **JWT Bearer 認證**，整合 Swagger UI。

| Controller | 端點 | 認證 | 功能 |
|---|---|---|---|
| `AuthController` | `POST /api/auth/login` | 公開 | 登入，回傳 JWT Token |
| `AuthController` | `POST /api/auth/logout` | 需認證 | 登出（Token 加入黑名單） |
| `CourseScheduleController` | `GET /api/courseschedule` | 公開 | 查詢課程列表（支援篩選） |
| `MemberController` | `GET/PUT /api/member` | 需認證 | 查詢與更新會員資料 |
| `ShopController` | `GET /api/shop` | 需認證 | 查詢已登記課程 |
| `ShopController` | `POST /api/shop` | 需認證 | 登記課程 |
| `ShopController` | `DELETE /api/shop/{id}` | 需認證 | 取消課程 |
| `BooksController` | `GET /api/books` | 公開 | 書籍資料 API（靜態） |

**安全機制**：
- JWT 驗證（Issuer / Audience / 過期時間 / 簽章金鑰）
- `ITokenBlacklistService`（Singleton）：記憶體黑名單，登出後 Token 立即失效
- `JwtBlacklistMiddleware`：在 `UseAuthentication` 之後、`UseAuthorization` 之前攔截已撤銷 Token
- `GlobalExceptionMiddleware`：全域例外處理，統一錯誤回應格式

---

### CourseData — 資料存取層

**用途**：EF Core 反向工程產生的 DbContext 與 Entity Model，直接對應 SQL Server 資料庫。

**DbContext**：`KhNetCourseContext`（資料庫：`KhNetCourseDB`）

| Entity | 資料表 | 說明 |
|---|---|---|
| `Course` | `course` | 課程（代碼唯一索引） |
| `Courseschedule` | `courseschedule` | 課程場次（關聯 Course、Teacher） |
| `Stucourseschedule` | `stucourseschedule` | 學員課程報名紀錄 |
| `Student` | `student` | 學員（email、密碼雜湊） |
| `Teacher` | `teacher` | 教師資料 |
| `Sysadmin` | `sysadmin` | 系統管理員 |

**Repository**：
- `CourseScheduleRepository`：查詢課程場次
- `MemberRepository`：學員 CRUD、密碼更新
- `MemberCourseScheduleRepository`：學員報名 CRUD

---

### CourseService — 業務邏輯層

**用途**：定義 Repository / Service 介面，實作業務規則，不直接依賴資料存取細節。

**介面（Interface）**：

| 介面 | 說明 |
|---|---|
| `ICourseScheduleRepository` | 課程場次資料存取 |
| `ICourseScheduleService` | 課程場次查詢業務邏輯 |
| `IMemberRepository` | 學員資料存取 |
| `IMemberService` | 學員業務邏輯（註冊、登入、修改） |
| `IMemberCourseScheduleRepository` | 學員報名資料存取 |
| `IShopService` | 購物（報名課程）業務邏輯 |

**主要業務規則**：
- 密碼以 `SHA256(密碼 + UserId大寫)` 雜湊儲存（`PwdHelper.PwdSHA256Hash`）
- 修改密碼須先驗證舊密碼
- 註冊時以 email 去重

---

### ConsoleAppClient — API 測試主控台

**用途**：示範如何以 `HttpClient` 呼叫 `CourseAPI`，目標位址 `https://localhost:7224/`。

---

## 架構關係圖

```
ConsoleAppClient ──────────────────────────┐
                                           ↓
CourseWeb (MVC) ─── (直接 DI) ──┐      CourseAPI (REST API)
                                 ↓           ↓
                           CourseService (業務邏輯 + 介面)
                                 ↓
                           CourseData (EF Core)
                                 ↓
                          SQL Server (KhNetCourseDB)
```

**依賴方向**：

```
CourseAPI / CourseWeb
    → CourseData  → CourseService
    → CourseService
```

---

## 技術棧

| 技術 | 版本 | 用途 |
|---|---|---|
| ASP.NET Core | 8.0 | Web 框架 |
| Entity Framework Core | 8.0 | ORM（Code-First Reverse Engineering） |
| EF Core SQL Server | 8.0 | SQL Server Provider |
| JWT Bearer | 8.0 | API 認證 |
| Swashbuckle (Swagger) | 6.6 | API 文件 |
| Bootstrap | lib | 前端樣式 |
| jQuery | lib | 前端互動 |

---

## 快速啟動

### 前置需求

- .NET 8 SDK
- SQL Server（還原 `KhNetCourse.bak`）

### 設定連線字串

在 `CourseAPI/appsettings.json` 與 `CourseWeb/appsettings.json` 設定：

```json
{
  "ConnectionStrings": {
    "KhNetCourseDB": "Server=.;Database=KhNetCourse;Trusted_Connection=True;"
  }
}
```

在 `CourseAPI/appsettings.json` 設定 JWT：

```json
{
  "JwtTokenSettings": {
    "Issuer": "your-issuer",
    "Audience": "your-audience",
    "IssuerSigningKey": "your-secret-key-min-32-chars"
  }
}
```

### 執行

```bash
# 同時啟動 API 與 MVC 前端（Visual Studio 設定多個啟動專案）
# 或分別執行：

dotnet run --project CourseAPI
dotnet run --project CourseWeb
```

Swagger UI：`https://localhost:{port}/swagger`

---

## 資料庫備份

根目錄的 `KhNetCourse.bak` 為 SQL Server 備份檔，可直接還原使用。
