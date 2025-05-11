import type { Breakpoint } from '@mui/material/styles';
import type { ContainerProps } from '@mui/material/Container';

import { mergeClasses } from 'minimal-shared/utils';

import Container from '@mui/material/Container';

import { layoutClasses } from '../core/classes';

// ----------------------------------------------------------------------

export type BackofficeContentProps = ContainerProps & {
  layoutQuery?: Breakpoint;
  disablePadding?: boolean;
};

export function BackofficeContent({
  sx,
  children,
  className,
  disablePadding,
  maxWidth = 'lg',
  layoutQuery = 'lg',
  ...other
}: BackofficeContentProps) {
  return (
    <Container
      className={mergeClasses([layoutClasses.content, className])}
      maxWidth={maxWidth}
      sx={[
        (theme) => ({
          display: 'flex',
          flex: '1 1 auto',
          flexDirection: 'column',
          pt: 'var(--layout-backoffice-content-pt)',
          pb: 'var(--layout-backoffice-content-pb)',
          [theme.breakpoints.up(layoutQuery)]: {
            px: 'var(--layout-backoffice-content-px)',
          },
          ...(disablePadding && {
            p: {
              xs: 0,
              sm: 0,
              md: 0,
              lg: 0,
              xl: 0,
            },
          }),
        }),
        ...(Array.isArray(sx) ? sx : [sx]),
      ]}
      {...other}
    >
      {children}
    </Container>
  );
}
