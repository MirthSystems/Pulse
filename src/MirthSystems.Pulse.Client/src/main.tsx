import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.tsx'
import { Provider as ReduxProvider } from 'react-redux';
import { store, DateTimeProvider, AuthProvider } from './app/index';

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <ReduxProvider store={store}>
      <AuthProvider>
        <DateTimeProvider>
          <App />
        </DateTimeProvider>
      </AuthProvider>
    </ReduxProvider>
  </StrictMode>,
)
