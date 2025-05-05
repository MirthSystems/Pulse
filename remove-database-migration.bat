@echo off
echo ===== Remove Last EF Core Migration =====
echo WARNING: This will remove the last migration. Are you sure?
set /p Confirm="Proceed? (Y/N): "
if /i "%Confirm%"=="Y" (
  dotnet ef migrations remove --project src/MirthSystems.Pulse.Infrastructure --startup-project src/MirthSystems.Pulse.Services.DatabaseMigrations --context ApplicationDbContext
  if %ERRORLEVEL% EQU 0 (
    echo Last migration removed successfully.
  ) else (
    echo Error removing migration. See errors above.
  )
) else (
  echo Operation cancelled.
)
pause