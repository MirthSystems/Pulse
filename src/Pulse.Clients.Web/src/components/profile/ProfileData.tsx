import React from 'react';
import { 
  Box,
  Typography,
  List,
  ListItem,
  ListItemIcon,
  ListItemText,
  Card,
  CardContent,
  Divider,
  Avatar
} from '@mui/material';
import * as MicrosoftGraph from '@microsoft/microsoft-graph-types';
import EmailIcon from '@mui/icons-material/Email';
import PhoneIcon from '@mui/icons-material/Phone';
import WorkIcon from '@mui/icons-material/Work';
import LocationOnIcon from '@mui/icons-material/LocationOn';

interface ProfileDataProps {
  graphData: MicrosoftGraph.User;
}

export const ProfileData: React.FC<ProfileDataProps> = ({ graphData }) => {
  const { displayName, mail, businessPhones, jobTitle, officeLocation, userPrincipalName } = graphData;
  
  return (
    <Card>
      <CardContent>
        <Box sx={{ display: 'flex', alignItems: 'center', mb: 3 }}>
          <Avatar 
            sx={{ 
              width: 64, 
              height: 64, 
              bgcolor: 'primary.main', 
              fontSize: '1.5rem',
              mr: 2
            }}
          >
            {displayName && displayName.charAt(0).toUpperCase()}
          </Avatar>
          <Box>
            <Typography variant="h5" component="h1" gutterBottom>
              {displayName}
            </Typography>
            {jobTitle && (
              <Typography variant="subtitle1" color="text.secondary">
                {jobTitle}
              </Typography>
            )}
          </Box>
        </Box>
        
        <Divider sx={{ mb: 3 }} />
        
        <Typography variant="h6" component="h2" gutterBottom>
          Contact Information
        </Typography>
        
        <List>
          {mail && (
            <ListItem disablePadding sx={{ mb: 1 }}>
              <ListItemIcon>
                <EmailIcon color="primary" />
              </ListItemIcon>
              <ListItemText 
                primary="Email"
                secondary={mail}
              />
            </ListItem>
          )}
          
          {!mail && userPrincipalName && (
            <ListItem disablePadding sx={{ mb: 1 }}>
              <ListItemIcon>
                <EmailIcon color="primary" />
              </ListItemIcon>
              <ListItemText 
                primary="Email"
                secondary={userPrincipalName}
              />
            </ListItem>
          )}
          
          {businessPhones && businessPhones.length > 0 && (
            <ListItem disablePadding sx={{ mb: 1 }}>
              <ListItemIcon>
                <PhoneIcon color="primary" />
              </ListItemIcon>
              <ListItemText 
                primary="Phone"
                secondary={businessPhones[0]}
              />
            </ListItem>
          )}
          
          {jobTitle && (
            <ListItem disablePadding sx={{ mb: 1 }}>
              <ListItemIcon>
                <WorkIcon color="primary" />
              </ListItemIcon>
              <ListItemText 
                primary="Job Title"
                secondary={jobTitle}
              />
            </ListItem>
          )}
          
          {officeLocation && (
            <ListItem disablePadding sx={{ mb: 1 }}>
              <ListItemIcon>
                <LocationOnIcon color="primary" />
              </ListItemIcon>
              <ListItemText 
                primary="Office Location"
                secondary={officeLocation}
              />
            </ListItem>
          )}
        </List>
      </CardContent>
    </Card>
  );
};