import { FC, ReactNode } from 'react';
import { Link as RouterLink } from 'react-router-dom';
import {
  Box,
  ButtonBase,
  ListItem,
  ListItemText,
  Typography,
} from '@mui/material';

export interface SidebarNavItemProps {
  active?: boolean;
  icon?: ReactNode;
  path: string;
  title: string;
  children?: ReactNode;
}

export const SidebarNavItem: FC<SidebarNavItemProps> = ({
  active = false,
  icon,
  path,
  title,
  children
}) => {

  return (
    <ListItem
      disableGutters
      sx={{
        display: 'flex',
        py: 0
      }}
    >
      <ButtonBase
        component={RouterLink}
        to={path}
        sx={{
          alignItems: 'center',
          borderRadius: 1,
          display: 'flex',
          justifyContent: 'flex-start',
          pl: '16px',
          pr: '16px',
          py: '12px',
          textAlign: 'left',
          width: '100%',
          ...(active && {
            backgroundColor: 'rgba(255, 255, 255, 0.08)',
            color: 'primary.main',
          }),
          '&:hover': {
            backgroundColor: 'rgba(255, 255, 255, 0.05)'
          }
        }}
      >
        {icon && (
          <Box
            component="span"
            sx={{
              alignItems: 'center',
              color: active ? 'primary.main' : 'text.secondary',
              display: 'inline-flex',
              justifyContent: 'center',
              mr: 2,
              '& svg': {
                fontSize: 24
              }
            }}
          >
            {icon}
          </Box>
        )}
        <ListItemText
          primary={
            <Typography
              color={active ? 'primary.main' : 'text.primary'}
              variant="subtitle2"
              fontWeight={active ? 600 : 400}
            >
              {title}
            </Typography>
          }
        />
        {children}
      </ButtonBase>
    </ListItem>
  );
};