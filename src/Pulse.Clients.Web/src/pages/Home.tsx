import { useNavigate } from 'react-router-dom';
import { Button, Box, TextField, Paper, Typography } from '@mui/material';
import React, { useState } from 'react';

export default function Home() {
  const navigate = useNavigate();
  const [address, setAddress] = useState('');
  const [radius, setRadius] = useState(5);
  const [results, setResults] = useState<any[]>([]); // Placeholder for search results

  const handleSearch = (e: React.FormEvent) => {
    e.preventDefault();
    // TODO: Implement search logic
    setResults([{ id: 1, name: 'Sample Venue', specials: ['Sample Special'] }]);
  };

  return (
    <Box sx={{ minHeight: '100vh', display: 'flex', flexDirection: 'column' }}>
      <Box sx={{ display: 'flex', justifyContent: 'flex-end', p: 2 }}>
        <Button variant="contained" onClick={() => navigate('/admin')}>Admin Login</Button>
      </Box>
      <Box sx={{ flex: 1, display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
        <Paper elevation={3} sx={{ p: 4, minWidth: 350 }}>
          <Typography variant="h5" align="center" gutterBottom>Find Specials Near You</Typography>
          <form onSubmit={handleSearch}>
            <TextField
              label="Address"
              value={address}
              onChange={e => setAddress(e.target.value)}
              fullWidth
              margin="normal"
              required
            />
            <TextField
              label="Radius (miles)"
              type="number"
              value={radius}
              onChange={e => setRadius(Number(e.target.value))}
              fullWidth
              margin="normal"
              inputProps={{ min: 1 }}
              required
            />
            <Button type="submit" variant="contained" color="primary" fullWidth sx={{ mt: 2 }}>
              Search Specials
            </Button>
          </form>
          {/* Placeholder for search results */}
          {results.length > 0 && (
            <Box sx={{ mt: 4 }}>
              <Typography variant="h6">Results</Typography>
              {results.map((venue) => (
                <Paper key={venue.id} sx={{ p: 2, mt: 1 }}>
                  <Typography variant="subtitle1">{venue.name}</Typography>
                  <Typography variant="body2">Specials: {venue.specials.join(', ')}</Typography>
                </Paper>
              ))}
            </Box>
          )}
        </Paper>
      </Box>
    </Box>
  );
}