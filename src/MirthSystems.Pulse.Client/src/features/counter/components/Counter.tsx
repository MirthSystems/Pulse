import { useState } from 'react';
import { Button, Typography, Box, TextField, Paper } from '@mui/material';
import { useAppDispatch, useAppSelector } from '../../../app/hooks';
import { decrement, increment, incrementByAmount } from '../counterSlice';

export const Counter = () => {
  const count = useAppSelector((state) => state.counter.value);
  const dispatch = useAppDispatch();
  const [incrementAmount, setIncrementAmount] = useState('2');

  const incrementValue = Number(incrementAmount) || 0;

  return (
    <Paper elevation={3} sx={{ p: 3, my: 3, textAlign: 'center' }}>
      <Typography variant="h5" gutterBottom>
        Redux Counter Example
      </Typography>
      
      <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'center', gap: 2 }}>
        <Button
          variant="contained"
          color="primary"
          aria-label="Decrement value"
          onClick={() => dispatch(decrement())}
        >
          -
        </Button>
        
        <Typography variant="h4" sx={{ mx: 2, minWidth: '50px' }}>
          {count}
        </Typography>
        
        <Button
          variant="contained"
          color="primary"
          aria-label="Increment value"
          onClick={() => dispatch(increment())}
        >
          +
        </Button>
      </Box>
      
      <Box sx={{ mt: 3, display: 'flex', alignItems: 'center', justifyContent: 'center', gap: 2 }}>
        <TextField
          label="Amount"
          variant="outlined"
          size="small"
          value={incrementAmount}
          onChange={(e) => setIncrementAmount(e.target.value)}
          sx={{ width: '100px' }}
        />
        
        <Button
          variant="contained"
          color="secondary"
          onClick={() => dispatch(incrementByAmount(incrementValue))}
        >
          Add Amount
        </Button>
      </Box>
    </Paper>
  );
};