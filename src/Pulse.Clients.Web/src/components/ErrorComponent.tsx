import React from 'react';
import { Box, Typography, Button, Paper, SxProps, Theme } from '@mui/material';
import ErrorOutlineIcon from '@mui/icons-material/ErrorOutline';
import WarningAmberIcon from '@mui/icons-material/WarningAmber';
import InfoIcon from '@mui/icons-material/Info';
import ReportProblemIcon from '@mui/icons-material/ReportProblem';
import { useNavigate } from 'react-router-dom';
import { AppError, ErrorSeverity } from '../types/error-types';

interface ErrorComponentProps {
  /** The error object or error message to display */
  error?: AppError | Error | string;
  /** The title of the error component */
  title?: string;
  /** The message to display (used if no error is provided) */
  message?: string;
  /** Whether to show a retry button */
  showRetry?: boolean;
  /** Whether to show a home button */
  showHome?: boolean;
  /** Custom action button */
  actionButton?: React.ReactNode;
  /** Additional styles for the container */
  sx?: SxProps<Theme>;
}

/**
 * A reusable error component that displays error information to the user
 */
const ErrorComponent: React.FC<ErrorComponentProps> = ({
  error,
  title,
  message,
  showRetry = true,
  showHome = true,
  actionButton,
  sx = {}
}) => {
  const navigate = useNavigate();
  
  // Determine error details
  let errorTitle = title;
  let errorMessage = message;
  let severity: ErrorSeverity = ErrorSeverity.MEDIUM;
  
  if (error) {
    if (typeof error === 'string') {
      errorMessage = error;
    } else if ('severity' in error && error.severity) {
      // Handle AppError type
      errorMessage = error.message;
      severity = error.severity;
      
      if (!title) {
        switch (error.severity) {
          case ErrorSeverity.FATAL:
            errorTitle = 'Critical Error';
            break;
          case ErrorSeverity.HIGH:
            errorTitle = 'Error';
            break;
          case ErrorSeverity.MEDIUM:
            errorTitle = 'Warning';
            break;
          case ErrorSeverity.LOW:
            errorTitle = 'Notice';
            break;
        }
      }
    } else if (error instanceof Error) {
      errorMessage = error.message;
    }
  }
  
  if (!errorTitle) {
    errorTitle = 'Error';
  }
  
  if (!errorMessage) {
    errorMessage = 'Something went wrong. Please try again later.';
  }
  
  // Get the appropriate icon based on severity
  const getIcon = () => {
    switch (severity) {
      case ErrorSeverity.FATAL:
      case ErrorSeverity.HIGH:
        return <ErrorOutlineIcon color="error" sx={{ fontSize: 64, mb: 2 }} />;
      case ErrorSeverity.MEDIUM:
        return <WarningAmberIcon color="warning" sx={{ fontSize: 64, mb: 2 }} />;
      case ErrorSeverity.LOW:
        return <InfoIcon color="info" sx={{ fontSize: 64, mb: 2 }} />;
      default:
        return <ReportProblemIcon color="action" sx={{ fontSize: 64, mb: 2 }} />;
    }
  };

  return (
    <Paper
      elevation={0}
      sx={{
        p: 4,
        borderRadius: 2,
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        textAlign: 'center',
        maxWidth: 500,
        mx: 'auto',
        my: 4,
        border: '1px solid',
        borderColor: 'divider',
        ...sx
      }}
    >
      {getIcon()}
      <Typography variant="h5" component="h2" gutterBottom sx={{ fontWeight: 600 }}>
        {errorTitle}
      </Typography>
      <Typography variant="body1" color="text.secondary" paragraph>
        {errorMessage}
      </Typography>
      <Box sx={{ mt: 2, display: 'flex', gap: 2 }}>
        {showRetry && (
          <Button
            variant="contained"
            color="primary"
            onClick={() => window.location.reload()}
          >
            Retry
          </Button>
        )}
        {showHome && (
          <Button
            variant="outlined"
            onClick={() => navigate('/')}
          >
            Go Home
          </Button>
        )}
        {actionButton}
      </Box>
    </Paper>
  );
};

export default ErrorComponent;