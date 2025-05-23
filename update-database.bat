@echo off
echo ===== Update Database =====
echo This will apply all pending migrations to the database.
set /p Confirm="Proceed? (Y/N): "
if /i "%Confirm%"=="Y" (
  dotnet ef database update --project src/Pulse --startup-project src/Pulse.API --context ApplicationDbContext
  if %ERRORLEVEL% EQU 0 (
    echo Database updated successfully.
  ) else (
    echo Error updating database. See errors above.
  )
) else (
  echo Operation cancelled.
)
pause
