import { Dialog, DialogTitle, DialogContent, DialogActions, Button, Typography } from '@mui/material';

interface DeleteEntityDialogProps {
    open: boolean;
    entityType: 'venue' | 'special' | null;
    entityName?: string;
    onConfirm: () => void;
    onCancel: () => void;
}

export const DeleteEntityDialog = ({ open, entityType, entityName, onConfirm, onCancel }: DeleteEntityDialogProps) => (
    <Dialog open={open} onClose={onCancel}>
        <DialogTitle>Delete {entityType === 'venue' ? 'Venue' : 'Special'}?</DialogTitle>
        <DialogContent>
            <Typography>
                Are you sure you want to delete {entityType} {entityName ? `"${entityName}"` : ''}? This action cannot be undone.
            </Typography>
        </DialogContent>
        <DialogActions>
            <Button onClick={onCancel} color="primary">Cancel</Button>
            <Button onClick={onConfirm} color="error" variant="contained">Delete</Button>
        </DialogActions>
    </Dialog>
);
