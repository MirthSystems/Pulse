@echo off
echo ===== Add EF Core Migration =====
set /p MigrationName="Enter migration name: "
dotnet ef migrations add %MigrationName% --project src/MirthSystems.Pulse.Infrastructure --startup-project src/MirthSystems.Pulse.Services.DatabaseMigrations --output-dir Data/Migrations --context ApplicationDbContext
if %ERRORLEVEL% EQU 0 (
  echo Migration '%MigrationName%' created successfully.
) else (
  echo Error creating migration. See errors above.
)
pause