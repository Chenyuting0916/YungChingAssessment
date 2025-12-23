# YungChing Assessment - Product API

這是一個使用 ASP.NET Core 7.0 開發的產品管理 API 專案。本專案採用分層架構 (Layered Architecture)，並嚴格遵循 SOLID 原則，整合了 AutoMapper、FluentValidation 與 Unit of Work 模式。

## 專案特色

*   **分層架構 (Clean/Layered Architecture)**：
    *   `Api`: 負責 HTTP 請求處理、DTO 映射與輸入驗證。
    *   `Core`: 核心業務邏輯、實體 (Entities) 與介面定義。
    *   `Infrastructure`: 資料存取層，包含 DbContext、Repository 與 Migrations。
*   **設計模式**：
    *   **Repository Pattern**: 統一的資料存取介面。
    *   **Unit of Work**: 確保資料操作的原子性。
*   **自動化工具**：
    *   **AutoMapper**: 自動處理 Entity 與 DTO 之間的轉換。
    *   **FluentValidation**: 強大的輸入欄位驗證機制。
*   **開發效率**：
    *   **SQLite**: 輕量化資料庫，無需額外安裝 SQL Server 即可運行。
    *   **Swagger/OpenAPI**: 提供互動式的 API 測試文件。

## 下載與執行步驟

### 1. 複製專案 (Clone)
```bash
git clone <repository-url>
cd YungChingProject
```

### 2. 資料庫初始化 (Migrations)
本專案使用 SQLite。在第一次執行前，請確保已套用遷移：
```bash
dotnet ef database update --project YungChingAssessment.Infrastructure --startup-project YungChingAssessment.Api
```
*(注意：專案中已包含種子資料，執行後會自動建立 `app.db`)*

### 3. 執行專案
```bash
dotnet run --project YungChingAssessment.Api
```

### 4. 存取 API 文件
開啟瀏覽器並造訪：
`http://localhost:<port>/swagger` (Port 通常會顯示在終端機輸出中)

## 單元測試
執行以下指令來驗證系統邏輯：
```bash
dotnet test
```

## API 端點摘要
*   `GET /api/products`: 取得所有產品。
*   `GET /api/products/{id}`: 取得特定產品詳情。
*   `POST /api/products`: 新增產品。
*   `PUT /api/products/{id}`: 更新產品。
*   `DELETE /api/products/{id}`: 刪除產品。
*   `GET /api/products/{id}/price-details`: 取得優化後的產品價格詳情（單次資料庫查詢）。
