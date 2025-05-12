import { useState } from 'react';
import {
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
  FormControlLabel,
  Switch,
  TextField,
  Box
} from '@mui/material';
import { OperatingHours } from '@models/venue';

interface BusinessHoursEditorProps {
  businessHours: OperatingHours[];
  onChange: (dayIndex: number, field: keyof OperatingHours, value: any) => void;
  disabled?: boolean;
}

const BusinessHoursEditor = ({ businessHours, onChange, disabled = false }: BusinessHoursEditorProps) => {
  // Helper function to get the day name
  const getDayName = (day: number): string => {
    const days = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
    return days[day] || '';
  };

  return (
    <TableContainer component={Paper} variant="outlined">
      <Table>
        <TableHead>
          <TableRow>
            <TableCell>Day</TableCell>
            <TableCell>Open</TableCell>
            <TableCell>Open Time</TableCell>
            <TableCell>Close Time</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {businessHours.map((hours, index) => (
            <TableRow key={index}>
              <TableCell component="th" scope="row">
                {getDayName(hours.dayOfWeek)}
              </TableCell>
              <TableCell>
                <FormControlLabel
                  control={
                    <Switch
                      checked={!hours.isClosed}
                      onChange={(e) => onChange(index, 'isClosed', !e.target.checked)}
                      disabled={disabled}
                    />
                  }
                  label={hours.isClosed ? "Closed" : "Open"}
                />
              </TableCell>
              <TableCell>
                <TextField
                  type="time"
                  value={hours.timeOfOpen}
                  onChange={(e) => onChange(index, 'timeOfOpen', e.target.value)}
                  disabled={hours.isClosed || disabled}
                  InputLabelProps={{
                    shrink: true,
                  }}
                  inputProps={{
                    step: 300, // 5 min
                  }}
                  sx={{ width: 120 }}
                />
              </TableCell>
              <TableCell>
                <TextField
                  type="time"
                  value={hours.timeOfClose}
                  onChange={(e) => onChange(index, 'timeOfClose', e.target.value)}
                  disabled={hours.isClosed || disabled}
                  InputLabelProps={{
                    shrink: true,
                  }}
                  inputProps={{
                    step: 300, // 5 min
                  }}
                  sx={{ width: 120 }}
                />
              </TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
      <Box p={2} sx={{ color: 'text.secondary', fontSize: '0.875rem' }}>
        Note: For hours that extend past midnight, enter the closing time for the next day.
        For example, if you close at 2 AM on Monday, use 02:00 as the closing time for Sunday.
      </Box>
    </TableContainer>
  );
};

export default BusinessHoursEditor;
