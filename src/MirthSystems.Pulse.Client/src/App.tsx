import { useState } from 'react'
import { Container, CssBaseline, AppBar, Toolbar, Typography, Box, ThemeProvider, createTheme, Divider } from '@mui/material';
import { Provider as ReduxProvider } from 'react-redux';
import { LoginButton, LogoutButton, Profile } from './features/user';
import { Counter } from './features/counter';
import { store, DateTimeProvider, AuthProvider } from './app/index';
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import './App.css'

// Create a responsive theme with light mode support
const theme = createTheme({
  palette: {
    mode: 'light',
    primary: {
      main: '#1976d2',
    },
    secondary: {
      main: '#dc004e',
    },
  },
});

function App() {
  const [count, setCount] = useState(0)

  return (
    <ReduxProvider store={store}>
      <AuthProvider>
        <DateTimeProvider>
          <ThemeProvider theme={theme}>
            <CssBaseline />
            <Box sx={{ display: 'flex', flexDirection: 'column', minHeight: '100vh' }}>
              <AppBar position="static">
                <Toolbar>
                  <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
                    Mirth Systems Pulse
                  </Typography>
                  <LoginButton />
                  <LogoutButton />
                </Toolbar>
              </AppBar>
              
              <Container component="main" sx={{ mt: 4, mb: 4, flex: '1 0 auto' }}>
                <Profile />
                
                {/* Redux Counter Example */}
                <Counter />
                
                <Divider sx={{ my: 4 }} />
                
                <div>
                  <a href="https://vite.dev" target="_blank" rel="noopener noreferrer">
                    <img src={viteLogo} className="logo" alt="Vite logo" />
                  </a>
                  <a href="https://react.dev" target="_blank" rel="noopener noreferrer">
                    <img src={reactLogo} className="logo react" alt="React logo" />
                  </a>
                </div>
                <h1>Vite + React</h1>
                <div className="card">
                  <button onClick={() => setCount((count) => count + 1)}>
                    count is {count}
                  </button>
                  <p>
                    Edit <code>src/App.tsx</code> and save to test HMR
                  </p>
                </div>
                <p className="read-the-docs">
                  Click on the Vite and React logos to learn more
                </p>
              </Container>
              
              <Box component="footer" sx={{ py: 3, px: 2, mt: 'auto', backgroundColor: 'primary.light' }}>
                <Container maxWidth="sm">
                  <Typography variant="body2" color="white" align="center">
                    © {new Date().getFullYear()} Mirth Systems
                  </Typography>
                </Container>
              </Box>
            </Box>
          </ThemeProvider>
        </DateTimeProvider>
      </AuthProvider>
    </ReduxProvider>
  )
}

export default App
