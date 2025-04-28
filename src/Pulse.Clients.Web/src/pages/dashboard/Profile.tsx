import React, { useEffect } from "react";

// Msal imports
import { MsalAuthenticationTemplate } from "@azure/msal-react";
import { InteractionType } from "@azure/msal-browser";
import { loginRequest } from "../../configs/auth";

// App imports
import { ProfileData } from "@/components/profile/ProfileData";
import Loading from "@/components/Loading";
import ErrorComponent from "@/components/ErrorComponent";
import { useGraph } from "@/hooks/useGraph";

// Material-ui imports
import { Paper, Alert } from "@mui/material";

const ProfileContent: React.FC = () => {
    const { getUserProfile, userProfile, error, isLoading } = useGraph();

    useEffect(() => {
        const fetchUserProfile = async () => {
            try {
                await getUserProfile();
            } catch (error) {
                console.error("Error fetching user profile:", error);
            }
        };

        if (!userProfile && !isLoading) {
            fetchUserProfile();
        }
    }, [userProfile, isLoading, getUserProfile]);
  
    if (isLoading) {
        return <Loading />;
    }

    if (error) {
        return <Alert severity="error">Error loading profile data: {error.message}</Alert>;
    }
  
    return (
        <Paper sx={{ p: 3 }}>
            {userProfile ? <ProfileData graphData={userProfile} /> : <p>No profile data found</p>}
        </Paper>
    );
};

// Wrapper components for MSAL authentication to fix type issues
const MsalErrorComponent: React.FC<unknown> = () => (
  <ErrorComponent 
    title="Authentication Error" 
    message="There was a problem authenticating you. Please try again." 
  />
);
const MsalLoadingComponent = () => <Loading />;

const Profile: React.FC = () => {
    const authRequest = {
        ...loginRequest
    };

    return (
        <MsalAuthenticationTemplate 
            interactionType={InteractionType.Redirect} 
            authenticationRequest={authRequest} 
            errorComponent={MsalErrorComponent} 
            loadingComponent={MsalLoadingComponent}
        >
            <ProfileContent />
        </MsalAuthenticationTemplate>
    );
};

export default Profile;