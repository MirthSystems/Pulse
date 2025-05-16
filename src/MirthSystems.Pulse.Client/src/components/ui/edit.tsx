import { Button, type ButtonProps } from '@mui/material';
import EditIcon from '@mui/icons-material/Edit';
import SaveIcon from '@mui/icons-material/Save';
import CancelIcon from '@mui/icons-material/Cancel';
import React from 'react';

export interface EditButtonProps extends ButtonProps {
  editing: boolean;
  onEdit: () => void;
  onSave: () => void;
  onCancel: () => void;
}

export const EditButton: React.FC<EditButtonProps> = ({ editing, onEdit, onSave, onCancel, ...props }) => {
  if (editing) {
    return (
      <>
        <Button
          startIcon={<SaveIcon />}
          color="primary"
          variant="contained"
          onClick={onSave}
          sx={{ mr: 1 }}
          {...props}
        >
          Save
        </Button>
        <Button
          startIcon={<CancelIcon />}
          color="inherit"
          variant="outlined"
          onClick={onCancel}
          {...props}
        >
          Cancel
        </Button>
      </>
    );
  }
  return (
    <Button
      startIcon={<EditIcon />}
      color="primary"
      variant="contained"
      onClick={onEdit}
      {...props}
    >
      Edit
    </Button>
  );
};
