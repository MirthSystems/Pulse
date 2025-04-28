import React from 'react';
import LocalBarIcon from '@mui/icons-material/LocalBar';
import RestaurantIcon from '@mui/icons-material/Restaurant';
import MusicNoteIcon from '@mui/icons-material/MusicNote';

/**
 * Returns an icon component based on a tag string (e.g., "beer", "music").
 */
export function getTagIcon(tag: string): React.ReactElement {
  const lowerTag = tag.toLowerCase();
  if (lowerTag.includes('beer') || lowerTag.includes('happy') || lowerTag.includes('wine')) {
    return <LocalBarIcon fontSize="small" />;
  } else if (lowerTag.includes('music') || lowerTag.includes('jazz') || lowerTag.includes('live')) {
    return <MusicNoteIcon fontSize="small" />;
  } else {
    return <RestaurantIcon fontSize="small" />;
  }
}
