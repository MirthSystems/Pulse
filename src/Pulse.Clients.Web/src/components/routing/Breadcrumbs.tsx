import React from 'react';
import { 
  Breadcrumbs as MuiBreadcrumbs, 
  Link, 
  Typography, 
  Box, 
  useTheme
} from '@mui/material';
import NavigateNextIcon from '@mui/icons-material/NavigateNext';
import HomeIcon from '@mui/icons-material/Home';
import { Link as RouterLink } from 'react-router-dom';
import { useNavigation } from '@/hooks/useNavigation';
import { BreadcrumbItem } from '@/types/breadcrumb-item';

interface BreadcrumbsProps {
  /** Maximum number of items to show before truncating */
  maxItems?: number;
  /** Custom separator between breadcrumb items */
  separator?: React.ReactNode;
  /** Whether to show icons for home/default items */
  showIcons?: boolean;
  /** CSS class to apply to the breadcrumbs container */
  className?: string;
}

/**
 * Dynamic breadcrumbs component that shows the current navigation path
 * Automatically generates breadcrumbs based on the current route
 */
const Breadcrumbs: React.FC<BreadcrumbsProps> = ({
  maxItems = 4,
  separator = <NavigateNextIcon fontSize="small" />,
  showIcons = true,
  className
}) => {
  const { getBreadcrumbs } = useNavigation();
  const breadcrumbs = getBreadcrumbs();
  const theme = useTheme();
  
  // Don't show breadcrumbs on the home page
  if (breadcrumbs.length === 1 && breadcrumbs[0].path === '/') {
    return null;
  }

  return (
    <Box 
      sx={{ 
        mb: 2,
        pl: 1, 
        py: 1,
        backgroundColor: theme.palette.mode === 'light' 
          ? 'rgba(0, 0, 0, 0.02)' 
          : 'rgba(255, 255, 255, 0.02)',
        borderRadius: 1
      }}
      className={className}
      aria-label="breadcrumb navigation"
    >
      <MuiBreadcrumbs 
        separator={separator}
        maxItems={maxItems}
        aria-label="breadcrumb"
      >
        {breadcrumbs.map((crumb, index) => 
          renderBreadcrumbItem(crumb, index, showIcons, breadcrumbs.length)
        )}
      </MuiBreadcrumbs>
    </Box>
  );
};

/**
 * Renders an individual breadcrumb item
 */
const renderBreadcrumbItem = (
  crumb: BreadcrumbItem, 
  index: number, 
  showIcons: boolean,
  totalCrumbs: number
) => {
  const isLast = index === totalCrumbs - 1;
  const isHome = crumb.path === '/';
  
  if (isLast) {
    return (
      <Typography 
        key={crumb.path} 
        color="text.primary"
        aria-current="page"
        sx={{ display: 'flex', alignItems: 'center' }}
      >
        {isHome && showIcons && <HomeIcon fontSize="small" sx={{ mr: 0.5 }} />}
        {crumb.label}
      </Typography>
    );
  }
  
  return (
    <Link
      key={crumb.path}
      component={RouterLink}
      to={crumb.path}
      color="inherit"
      sx={{ 
        display: 'flex', 
        alignItems: 'center',
        textDecoration: 'none',
        '&:hover': {
          textDecoration: 'underline'
        }
      }}
    >
      {isHome && showIcons && <HomeIcon fontSize="small" sx={{ mr: 0.5 }} />}
      {crumb.label}
    </Link>
  );
};

export default Breadcrumbs;