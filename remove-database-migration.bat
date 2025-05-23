@echo off
echo ===== Remove Last EF Core Migration =====
echo WARNING: This will remove the last migration. Are you sure?
set /p Confirm="Proceed? (Y/N): "
if /i "%Confirm%"=="Y" (
  dotnet ef migrations remove --project src/Pulse --startup-project src/Pulse.API --context ApplicationDbContext
  if %ERRORLEVEL% EQU 0 (
    echo Last migration removed successfully.
  ) else (
    echo Error removing migration. See errors above.
  )
) else (
  echo Operation cancelled.
)
pause