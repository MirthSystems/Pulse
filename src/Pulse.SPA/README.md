# Pulse SPA

Modern search application built with React, Vite, and HeroUI.

## Technologies Used

- [Vite](https://vitejs.dev/guide/)
- [React Router 7](https://reactrouter.com/)
- [HeroUI](https://heroui.com)
- [Auth0](https://auth0.com/)
- [Tailwind CSS](https://tailwindcss.com)
- [Tailwind Variants](https://tailwind-variants.org)
- [TypeScript](https://www.typescriptlang.org)
- [Framer Motion](https://www.framer.com/motion)

## How to Use

To clone the project, run the following command:

```bash
git clone https://github.com/frontio-ai/vite-template.git
```

### Install dependencies

You can use one of them `npm`, `yarn`, `pnpm`, `bun`, Example using `npm`:

```bash
npm install
```

### Run the development server

```bash
npm run dev
```

### Setup pnpm (optional)

If you are using `pnpm`, you need to add the following code to your `.npmrc` file:

```bash
public-hoist-pattern[]=*@heroui/*
```

After modifying the `.npmrc` file, you need to run `pnpm install` again to ensure that the dependencies are installed correctly.

## Auth0 Configuration

This application uses Auth0 for authentication. To connect to your Auth0 account:

1. Create an Auth0 application at [Auth0 Dashboard](https://manage.auth0.com/)
2. Copy your Auth0 domain and client ID
3. Set up a `.env` file with the following variables:

```bash
VITE_AUTH0_DOMAIN=your-domain.auth0.com
VITE_AUTH0_CLIENT_ID=your-client-id
VITE_AUTH0_AUDIENCE=your-api-identifier (optional)
```

The application is already configured to use Auth0 for authentication with the following features:
- Login/logout functionality
- Protected routes for backoffice area
- User profile information display

## Application Structure

- `src/components` - UI components including auth components
- `src/layouts` - Page layouts
- `src/pages` - Application pages
  - `index.tsx` - Home page with search
  - `search.tsx` - Search results
  - `backoffice/` - Protected admin area
- `src/provider.tsx` - Auth0 and HeroUI providers
- `src/main.tsx` - Application entry point with React Router 7 configuration

## Route Structure

This application uses React Router 7's declarative routing with protected routes:

```
/ (public)
├── / (home page with search)
├── /search (search results)
└── /backoffice/* (protected area)
    └── /backoffice (dashboard)
```

## Authentication

Authentication is handled via Auth0. The application includes:

- Public routes accessible to all users
- Protected routes (/backoffice/*) requiring authentication
- Context-aware navigation (menu changes based on location)
- Proper error handling for unauthorized access

## Documentation

For more details on the application structure and testing:

- `ROUTER_FIX.md` - Documentation on React Router 7 integration fixes
- `TESTING.md` - Comprehensive testing guide and checklist
- `MIGRATION.md` - Details on the migration from file-based routing

## License

Licensed under the [MIT license](https://github.com/frontio-ai/vite-template/blob/main/LICENSE).
