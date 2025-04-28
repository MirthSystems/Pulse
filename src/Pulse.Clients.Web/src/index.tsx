import React from "react";
import ReactDOM from "react-dom/client";
import { BrowserRouter } from "react-router-dom";
import App from "./App";
import { EventType, EventMessage, AuthenticationResult } from "@azure/msal-browser";
import { pca } from "./configs/auth";

// Import global styles
import "./styles/global.css";

// Initialize MSAL before rendering the app
const initializeMsal = async () => {
  try {
    await pca.initialize();
    
    // Set active account if one exists but none is currently active
    if (!pca.getActiveAccount() && pca.getAllAccounts().length > 0) {
      pca.setActiveAccount(pca.getAllAccounts()[0]);
    }
    
    // Listen for sign-in events and set active account
    pca.addEventCallback((event: EventMessage) => {
      if (event.eventType === EventType.LOGIN_SUCCESS && event.payload) {
        const payload = event.payload as AuthenticationResult;
        pca.setActiveAccount(payload.account);
      }
    });
    
    // Render the app
    ReactDOM.createRoot(document.getElementById("root") as HTMLElement).render(
      <React.StrictMode>
        <BrowserRouter>
          <App />
        </BrowserRouter>
      </React.StrictMode>
    );
  } catch (error) {
    console.error("MSAL Initialization Error:", error);
    
    // Render a basic error message if MSAL fails to initialize
    const rootElement = document.getElementById("root");
    if (rootElement) {
      rootElement.innerHTML = `
        <div style="padding: 20px; text-align: center; font-family: sans-serif;">
          <h1>Authentication Error</h1>
          <p>There was a problem initializing the authentication service. Please try refreshing the page.</p>
          <button onclick="window.location.reload()">Refresh Page</button>
        </div>
      `;
    }
  }
};

// Start the application
initializeMsal();
