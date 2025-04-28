import React from 'react';
import { Button, IconButton, Tooltip, SxProps, Theme } from '@mui/material';
import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import { useNavigation } from '../../utils/navigationUtils';

interface BackButtonProps {
  /** Text to display (when variant is "text" or "contained") */
  text?: string;
  /** Button variant */
  variant?: 'icon' | 'text' | 'contained' | 'outlined';
  /** Optional tooltip text (only used for icon variant) */
  tooltip?: string;
  /** Whether to fallback to home page if no history */
  fallbackToHome?: boolean;
  /** Optional callback to run before navigation */
  onBeforeNavigate?: () => boolean | void;
  /** Additional styles */
  sx?: SxProps<Theme>;
  /** The path to navigate to (overrides history-based navigation) */
  to?: string;
}

/**
 * Back button that intelligently navigates to the previous page or home
 */
const BackButton: React.FC<BackButtonProps> = ({
  text = 'Back',
  variant = 'icon',
  tooltip = 'Go back',
  fallbackToHome = true,
  onBeforeNavigate,
  sx = {},
  to
}) => {
  const { goBack, canGoBack, navigateTo } = useNavigation();

  const handleClick = () => {
    // Run the pre-navigation callback if provided
    if (onBeforeNavigate) {
      const result = onBeforeNavigate();
      // If callback returns false explicitly, cancel navigation
      if (result === false) return;
    }

    // If a specific destination is provided, use that
    if (to) {
      navigateTo(to);
      return;
    }

    // Otherwise use intelligent history-based navigation
    if (canGoBack || fallbackToHome) {
      goBack();
    }
  };

  // Don't render if we can't go back and fallbackToHome is false
  if (!canGoBack && !fallbackToHome && !to) {
    return null;
  }

  // Render as icon button
  if (variant === 'icon') {
    const button = (
      <IconButton
        onClick={handleClick}
        size="medium"
        aria-label={tooltip}
        sx={{
          ...sx
        }}
      >
        <ArrowBackIcon />
      </IconButton>
    );

    return tooltip ? (
      <Tooltip title={tooltip}>{button}</Tooltip>
    ) : (
      button
    );
  }

  // Render as text or contained button
  return (
    <Button
      onClick={handleClick}
      variant={variant}
      startIcon={<ArrowBackIcon />}
      sx={{
        ...sx
      }}
    >
      {text}
    </Button>
  );
};

export default BackButton;