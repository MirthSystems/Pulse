# Pulse Web Client

A modern web application for the Pulse platform built with React, TypeScript, and Vite.

## Features

- Real-time nightlife discovery platform
- Microsoft Authentication Library (MSAL) integration
- Material UI components
- Dark/light theme support
- Responsive design for all devices

## Quick Start

### Prerequisites

- Node.js 16.x or later
- npm 7.x or later

### Installation

```bash
# Install dependencies
npm install
```

### Development

```bash
# Start the development server
npm run dev
```

The app will be available at http://localhost:3000

### Building for Production

```bash
# Create a production build
npm run build

# Preview the production build
npm run preview
```

## Project Structure

```
src/
├── components/     # Reusable UI components
├── configs/        # Configuration files
├── contexts/       # React context providers
├── hooks/          # Custom React hooks
├── layouts/        # Page layout components
├── pages/          # Application pages
├── services/       # API and other services
├── styles/         # Global styles and themes
├── types/          # TypeScript type definitions
├── utils/          # Utility functions
├── App.tsx         # Main App component
└── index.tsx       # Application entry point
```

## Environment Variables

Create a `.env` file in the root directory with the following variables:

```
# Authentication (MSAL.js)
REACT_APP_AUTH_CLIENT_ID=your-client-id
REACT_APP_AUTH_AUTHORITY=https://login.microsoftonline.com/common

# Microsoft Graph
REACT_APP_MICROSOFT_GRAPH_DOMAIN=https://graph.microsoft.com/
REACT_APP_MICROSOFT_GRAPH_VERSION=v1.0
REACT_APP_MICROSOFT_GRAPH_SCOPES=user.read

# Backend API
REACT_APP_PULSE_API_URL=http://localhost:3000/api
REACT_APP_PULSE_API_SCOPES=your-api-scopes
```

## Testing

```bash
# Run tests
npm run test:e2e
```