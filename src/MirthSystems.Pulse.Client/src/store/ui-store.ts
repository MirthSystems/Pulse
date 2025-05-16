import { create } from 'zustand';

interface UiState {
    isVenueDialogOpen: boolean;
    isSpecialDialogOpen: boolean;
    isDeleteDialogOpen: boolean;
    deleteEntityType: 'venue' | 'special' | null;
    deleteEntityId: string | null;
    openVenueDialog: () => void;
    closeVenueDialog: () => void;
    openSpecialDialog: () => void;
    closeSpecialDialog: () => void;
    openDeleteDialog: (entityType: 'venue' | 'special', entityId: string) => void;
    closeDeleteDialog: () => void;
}

export const useUiStore = create<UiState>()((set) => ({
    isVenueDialogOpen: false,
    isSpecialDialogOpen: false,
    isDeleteDialogOpen: false,
    deleteEntityType: null,
    deleteEntityId: null,
    openVenueDialog: () => set({ isVenueDialogOpen: true }),
    closeVenueDialog: () => set({ isVenueDialogOpen: false }),
    openSpecialDialog: () => set({ isSpecialDialogOpen: true }),
    closeSpecialDialog: () => set({ isSpecialDialogOpen: false }),
    openDeleteDialog: (entityType, entityId) => set({ isDeleteDialogOpen: true, deleteEntityType: entityType, deleteEntityId: entityId }),
    closeDeleteDialog: () => set({ isDeleteDialogOpen: false, deleteEntityType: null, deleteEntityId: null }),
}));
