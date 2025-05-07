import { ReactNode } from 'react';
import { AdapterLuxon } from '@mui/x-date-pickers/AdapterLuxon';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';

interface DateTimeProviderProps {
  children: ReactNode;
}

/**
 * DateTimeProvider component that provides MUI date pickers with Luxon adapter
 */
export const DateTimeProvider = ({ children }: DateTimeProviderProps) => {
  return (
    <LocalizationProvider dateAdapter={AdapterLuxon}>
      {children}
    </LocalizationProvider>
  );
};