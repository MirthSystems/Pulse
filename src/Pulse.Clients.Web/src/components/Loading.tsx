import React from 'react';
import { Box, CircularProgress, Typography, SxProps, Theme } from '@mui/material';

interface LoadingProps {
  /** The message to display below the loading spinner */
  message?: string;
  /** Optional size for the CircularProgress component (default: 40) */
  size?: number;
  /** Optional full-height flag to make the loading component fill its container */
  fullHeight?: boolean;
  /** Optional color for the CircularProgress (default: 'primary') */
  color?: 'primary' | 'secondary' | 'error' | 'info' | 'success' | 'warning' | 'inherit';
  /** Optional additional styles for the container */
  sx?: SxProps<Theme>;
}

/**
 * A reusable loading component that displays a circular progress and optional message
 */
const Loading: React.FC<LoadingProps> = React.memo(({ 
  message = 'Loading...', 
  size = 40,
  fullHeight = false,
  color = 'primary',
  sx = {}
}) => {
  return (
    <Box
      sx={{
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        justifyContent: 'center',
        padding: 4,
        gap: 2,
        height: fullHeight ? '100%' : 'auto',
        minHeight: fullHeight ? '200px' : 'auto',
        ...sx
      }}
      role="status"
      aria-live="polite"
    >
      <CircularProgress color={color} size={size} aria-label="Loading" />
      {message && (
        <Typography variant="body1" color="text.secondary">
          {message}
        </Typography>
      )}
    </Box>
  );
});

Loading.displayName = 'Loading';

export default Loading;