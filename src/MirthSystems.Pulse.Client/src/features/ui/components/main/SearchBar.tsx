import { FC, useState } from 'react';
import {  
  TextField, 
  InputAdornment, 
  IconButton, 
  Paper, 
  useTheme
} from '@mui/material';
import { Search as SearchIcon, Clear as ClearIcon } from '@mui/icons-material';

interface SearchBarProps {
  placeholder?: string;
  onSearch?: (query: string) => void;
  fullWidth?: boolean;
}

export const SearchBar: FC<SearchBarProps> = ({
  placeholder = 'Search for specials near you...',
  onSearch,
  fullWidth = true
}) => {
  const [query, setQuery] = useState('');
  const theme = useTheme();

  const handleSearch = () => {
    if (onSearch && query.trim()) {
      onSearch(query.trim());
    }
  };

  const handleKeyPress = (e: React.KeyboardEvent) => {
    if (e.key === 'Enter') {
      handleSearch();
    }
  };

  const handleClear = () => {
    setQuery('');
  };

  return (
    <Paper
      elevation={1}
      sx={{
        p: '2px 4px',
        display: 'flex',
        alignItems: 'center',
        width: fullWidth ? '100%' : '600px',
        maxWidth: '100%',
        borderRadius: 40,
        border: '1px solid',
        borderColor: theme.palette.divider,
        '&:hover': {
          boxShadow: theme.shadows[2],
        }
      }}
    >
      <InputAdornment position="start" sx={{ pl: 2 }}>
        <SearchIcon color="action" />
      </InputAdornment>
      
      <TextField
        fullWidth
        placeholder={placeholder}
        value={query}
        onChange={(e) => setQuery(e.target.value)}
        onKeyDown={handleKeyPress}
        variant="standard"
        InputProps={{
          disableUnderline: true,
          sx: { 
            ml: 1,
            flex: 1,
            fontSize: '1rem',
            fontWeight: 400,
          },
          endAdornment: query ? (
            <InputAdornment position="end">
              <IconButton size="small" onClick={handleClear}>
                <ClearIcon fontSize="small" />
              </IconButton>
            </InputAdornment>
          ) : null,
        }}
        sx={{ ml: 1 }}
      />
    </Paper>
  );
};