import { useState } from 'react';
import { 
  Box, 
  Typography, 
  IconButton,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  Button,
  Paper
} from '@mui/material';
import LocationOnIcon from '@mui/icons-material/LocationOn';
import DeleteIcon from '@mui/icons-material/Delete';
import VisibilityIcon from '@mui/icons-material/Visibility';
import { type VenueItem } from '../../models';

interface VenueItemCardProps {
  venue: VenueItem;
  onView: (id: string) => void;
  onDelete: (id: string) => void;
}

export const VenueItemCard = ({ venue, onView, onDelete }: VenueItemCardProps) => {
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  
  const handleDeleteClick = () => {
    setDeleteDialogOpen(true);
  };

  const handleCloseDeleteDialog = () => {
    setDeleteDialogOpen(false);
  };

  const handleConfirmDelete = () => {
    onDelete(venue.id);
    setDeleteDialogOpen(false);
  };
  
  return (
    <>
      <Paper 
        sx={{ 
          mb: 2,
          display: 'flex',
          position: 'relative',
          borderRadius: 1,
          overflow: 'hidden',
          transition: 'transform 0.2s, box-shadow 0.2s',
          '&:hover': {
            transform: 'translateY(-2px)',
            boxShadow: (theme) => theme.shadows[3]
          }
        }}
        elevation={1}
      >
        {/* Action buttons at top right */}
        <Box 
          sx={{ 
            position: 'absolute',
            top: 0,
            right: 0,
            display: 'flex',
            p: 0.5,
          }}
        >
          <IconButton 
            size="small" 
            onClick={() => onView(venue.id)}
            aria-label="View venue"
          >
            <VisibilityIcon fontSize="small" />
          </IconButton>
          <IconButton 
            size="small" 
            color="error"
            onClick={handleDeleteClick}
            aria-label="Delete venue"
          >
            <DeleteIcon fontSize="small" />
          </IconButton>
        </Box>
        
        {/* Square venue image or placeholder */}
        <Box 
          sx={{ 
            width: '100px', 
            height: '100px', 
            flexShrink: 0,
            display: 'flex',
            justifyContent: 'center',
            alignItems: 'center',
            bgcolor: 'primary.main',
            color: 'white'
          }}
        >
          {venue.profileImage ? (
            <Box
              component="img"
              sx={{
                width: '100%',
                height: '100%',
                objectFit: 'cover',
              }}
              src={venue.profileImage}
              alt={venue.name}
            />
          ) : (
            <Typography variant="h3" sx={{ fontWeight: 'bold' }}>
              {venue.name.charAt(0)}
            </Typography>
          )}
        </Box>
        
        {/* Venue information with equal padding */}
        <Box sx={{ 
          p: 2, 
          flexGrow: 1, 
          display: 'flex', 
          flexDirection: 'column',
          justifyContent: 'center'
        }}>
          <Typography variant="h6" sx={{ mb: 0.5 }}>
            {venue.name}
          </Typography>
          
          <Box sx={{ display: 'flex', alignItems: 'center' }}>
            <LocationOnIcon fontSize="small" color="primary" sx={{ mr: 0.5 }} />
            <Typography variant="body2" color="text.secondary">
              {venue.locality}, {venue.region}
            </Typography>
          </Box>
          
          {venue.description && (
            <Typography variant="body2" color="text.secondary" sx={{ mt: 0.5 }}>
              {venue.description}
            </Typography>
          )}
        </Box>
      </Paper>
      
      {/* Delete confirmation dialog */}
      <Dialog
        open={deleteDialogOpen}
        onClose={handleCloseDeleteDialog}
      >
        <DialogTitle>Delete Venue</DialogTitle>
        <DialogContent>
          <DialogContentText>
            Are you sure you want to delete {venue.name}? This action cannot be undone.
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCloseDeleteDialog}>Cancel</Button>
          <Button onClick={handleConfirmDelete} color="error" autoFocus>
            Delete
          </Button>
        </DialogActions>
      </Dialog>
    </>
  );
};
