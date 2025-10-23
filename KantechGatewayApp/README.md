# KantechGatewayApp

WinForms app (convertible to Windows Service) that ingests CSV files for Kantech-related workflows:
Deactivate, Reactivate, Resignee, Profile/Access updates, New Profile/Access, Vendor Daily.

## Build
- .NET 8, VB.NET, Windows
- Open `KantechGatewayApp.vbproj` in Visual Studio 2022+
- Update paths in `App.config`
- Press F5 (WinForms). For service: build Release and install using `sc` or NSSM:
  ```
  sc create KantechGatewayApp binPath= "C:\path\KantechGatewayApp.exe --service"
  sc start KantechGatewayApp
  ```

## Configure
- All paths, schedules, and required fields in `App.config`.
- Place inbound CSVs in job-specific `Root.Inbound\<Source>` folders.

## Extend
- Implement real integration in each `Jobs/*Processor.vb` (write Kantech CSV/API/DB calls).