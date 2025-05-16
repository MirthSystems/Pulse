import { Box, Container, Typography } from '@mui/material';

export const Footer = () => {
  return (
    <Box
      component="footer"
      sx={{
        py: 3,
        mt: 'auto',
        backgroundColor: (theme) => theme.palette.mode === 'light'
          ? theme.palette.grey[200]
          : theme.palette.grey[800],
      }}
    >
      <Container maxWidth="sm">
        <Typography variant="body2" color="text.secondary" align="center">
          © {new Date().getFullYear()} Mirth Systems - Pulse
        </Typography>
      </Container>
    </Box>
  );
};
