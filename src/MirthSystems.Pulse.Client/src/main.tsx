import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.tsx'
import { Provider as ReduxProvider } from 'react-redux';
import { store, DateTimeProvider, AuthProvider, AppThemeProvider } from './app/index';
import { BrowserRouter } from 'react-router-dom';

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <ReduxProvider store={store}>
      <BrowserRouter>
        <AuthProvider>
          <AppThemeProvider>
            <DateTimeProvider>
              <App />
            </DateTimeProvider>
          </AppThemeProvider>
        </AuthProvider>
      </BrowserRouter>
    </ReduxProvider>
  </StrictMode>,
)
