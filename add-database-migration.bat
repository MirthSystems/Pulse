@echo off
echo ===== Add EF Core Migration =====
set /p MigrationName="Enter migration name: "
dotnet ef migrations add %MigrationName% --project src/Pulse --startup-project src/Pulse.AppHost --output-dir Data/Migrations --context ApplicationDbContext
if %ERRORLEVEL% EQU 0 (
  echo Migration '%MigrationName%' created successfully.
) else (
  echo Error creating migration. See errors above.
)
pause