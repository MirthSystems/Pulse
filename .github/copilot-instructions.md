# Copilot Instructions for Project

This project consists of a C# backend API and a Vite SPA frontend built with React, TypeScript, Redux, MUI, and Luxon. When generating code or providing suggestions, adhere to the following guidelines:

## Backend (C# API)

- Use ASP.NET Core to build the API.
- Follow RESTful API design principles:
    - Use appropriate HTTP methods (GET, POST, PUT, DELETE, etc.).
    - Return standard HTTP status codes (e.g., 200, 404, 500).
    - Structure endpoints logically (e.g., `/api/resource`).
- Implement dependency injection for services and repositories.
- Use data transfer objects (DTOs) for request and response models.
- Validate input data using model validation attributes or FluentValidation.
- Handle errors gracefully and return meaningful JSON error messages.
- Use logging to track application behavior and errors.

## Frontend (Vite SPA with React, TypeScript, Redux, MUI, Luxon)

- Use functional components and React hooks.
- Write TypeScript code with strict typing; avoid using `any`.
- Manage application state with Redux:
    - Define actions, reducers, and selectors properly.
    - Use Redux Toolkit for simplified setup.
- Utilize MUI components for the user interface:
    - Follow MUI’s guidelines for accessibility and theming.
- Handle all date and time operations with Luxon:
    - Use Luxon for parsing, formatting, and manipulating dates.
    - Ensure consistent time zone handling.
- Optimize performance using `React.memo`, `useCallback`, and other techniques as needed.
- Handle API calls with proper error handling and loading states.

## Integration

- Ensure seamless communication between frontend and backend:
    - Match API endpoints with frontend routes and expected data structures.
    - Handle authentication if required (e.g., JWT or OAuth).
- Display user-friendly error messages on the frontend when API calls fail.

## General

- Write unit tests for both backend and frontend code.
- Follow the principle of least astonishment; keep code readable and maintainable.
- Use meaningful variable and function names.
- Keep code DRY (Don’t Repeat Yourself); extract reusable functions and components.

## Command-Line Instructions

- All command-line operations must be performed using **Windows PowerShell**.
- Use PowerShell cmdlets for file system operations (e.g., `New-Item`, `Copy-Item`, `Set-Location`).
- Run .NET CLI commands for the backend (e.g., `dotnet build`, `dotnet run`) in the backend directory.
- Run npm or yarn commands for the frontend (e.g., `npm install`, `npm run dev`) in the frontend directory.
- When writing scripts, use PowerShell (.ps1) files and syntax.
- Handle environment variables with `$env:VARIABLE_NAME`.
- Ensure that commands are run in the correct directories by using `Set-Location` or specifying the working directory.
- If any Unix-specific commands are encountered (e.g., in scripts or documentation), translate them to PowerShell equivalents (e.g., use `Remove-Item -Recurse -Force` instead of `rm -rf`).