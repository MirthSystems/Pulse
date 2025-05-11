import type { Theme } from '@mui/material/styles';

// ----------------------------------------------------------------------

export function backofficeLayoutVars(theme: Theme) {
  return {
    '--layout-transition-easing': 'linear',
    '--layout-transition-duration': '120ms',
    '--layout-nav-vertical-width': '300px',
    '--layout-backoffice-content-pt': theme.spacing(1),
    '--layout-backoffice-content-pb': theme.spacing(8),
    '--layout-backoffice-content-px': theme.spacing(5),
  };
}
