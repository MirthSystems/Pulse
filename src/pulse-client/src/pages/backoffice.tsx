import { Box, Container, Typography, Paper, Tabs, Tab } from '@mui/material';
import { useState } from 'react';

export const BackofficePage = () => {
  const [activeTab, setActiveTab] = useState(0);

  const handleTabChange = (_event: React.SyntheticEvent, newValue: number) => {
    setActiveTab(newValue);
  };

  return (
    <Container maxWidth="lg">
      <Typography variant="h4" component="h1" gutterBottom>
        Backoffice Management
      </Typography>

      <Paper sx={{ width: '100%', mb: 2 }}>
        <Tabs
          value={activeTab}
          onChange={handleTabChange}
          indicatorColor="primary"
          textColor="primary"
        >
          <Tab label="Venues" />
          <Tab label="Specials" />
          <Tab label="Operating Schedules" />
        </Tabs>
      </Paper>

      <TabPanel value={activeTab} index={0}>
        <Typography variant="h5" component="h2" gutterBottom>
          Venues Management
        </Typography>
        <Typography variant="body1">
          Here you can manage your venues. This section will include venue listing, creation, editing, and deletion functionality.
        </Typography>
      </TabPanel>

      <TabPanel value={activeTab} index={1}>
        <Typography variant="h5" component="h2" gutterBottom>
          Specials Management
        </Typography>
        <Typography variant="body1">
          Here you can manage all specials across your venues. This section will include creation, editing, and scheduling of special offers.
        </Typography>
      </TabPanel>

      <TabPanel value={activeTab} index={2}>
        <Typography variant="h5" component="h2" gutterBottom>
          Operating Schedules Management
        </Typography>
        <Typography variant="body1">
          Here you can manage operating hours for all venues. This section will allow setting business hours, special closures, and holiday schedules.
        </Typography>
      </TabPanel>
    </Container>
  );
};

interface TabPanelProps {
  children?: React.ReactNode;
  index: number;
  value: number;
}

function TabPanel(props: TabPanelProps) {
  const { children, value, index, ...other } = props;

  return (
    <div
      role="tabpanel"
      hidden={value !== index}
      id={`simple-tabpanel-${index}`}
      aria-labelledby={`simple-tab-${index}`}
      {...other}
    >
      {value === index && (
        <Box sx={{ pt: 3 }}>
          {children}
        </Box>
      )}
    </div>
  );
}
