import { Paper, Box } from '@mui/material';

interface VenueMapProps {
  latitude: number;
  longitude: number;
  venueName: string;
}

const VenueMap = ({ latitude, longitude, venueName }: VenueMapProps) => {
  // Create a proper Google Maps embed URL
  const mapUrl = `https://maps.google.com/maps?q=${latitude},${longitude}&z=15&output=embed&title=${encodeURIComponent(venueName)}`;

  return (
    <Paper elevation={1} sx={{ width: '100%', height: '100%', overflow: 'hidden', borderRadius: 1 }}>
      <Box
        component="iframe"
        src={mapUrl}
        sx={{
          border: 0,
          width: '100%',
          height: '100%'
        }}
        title={`Map of ${venueName}`}
        loading="lazy"
        allowFullScreen
      />
    </Paper>
  );
};

export default VenueMap;
