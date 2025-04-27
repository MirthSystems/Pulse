import React, { useEffect, useState } from "react";

// Msal imports
import { MsalAuthenticationTemplate, useMsal } from "@azure/msal-react";
import { InteractionStatus, InteractionType, InteractionRequiredAuthError, AccountInfo } from "@azure/msal-browser";
import { loginRequest } from "../configs/auth";

// App imports
import { ProfileData } from "../components/profile/ProfileData";
import Loading from "../components/Loading";
import ErrorComponent from "../components/ErrorComponent";
import { GraphService } from "../services/graphService";
import { GraphData } from "../types/graph-data";

// Material-ui imports
import Paper from "@mui/material/Paper";

const ProfileContent: React.FC = () => {
    const { instance, inProgress } = useMsal();
    const [graphData, setGraphData] = useState<GraphData | null>(null);

    useEffect(() => {
        if (!graphData && inProgress === InteractionStatus.None) {
            // Get the active account
            const account = instance.getActiveAccount();
            if (!account) {
                instance.setActiveAccount(instance.getAllAccounts()[0]);
            }
            
            // Get token silently then call MS Graph
            instance.acquireTokenSilent({
                ...loginRequest,
                account: instance.getActiveAccount() as AccountInfo
            }).then((response) => {
                GraphService.getUserInfo(response.accessToken)
                    .then(response => setGraphData(response))
                    .catch(error => console.log(error));
            }).catch((e) => {
                if (e instanceof InteractionRequiredAuthError) {
                    instance.acquireTokenRedirect({
                        ...loginRequest,
                        account: instance.getActiveAccount() as AccountInfo
                    });
                }
            });
        }
    }, [inProgress, graphData, instance]);
  
    return (
        <Paper sx={{ p: 3 }}>
            {graphData ? <ProfileData graphData={graphData} /> : <p>No profile data found</p>}
        </Paper>
    );
};

const Profile: React.FC = () => {
    const authRequest = {
        ...loginRequest
    };

    return (
        <MsalAuthenticationTemplate 
            interactionType={InteractionType.Redirect} 
            authenticationRequest={authRequest} 
            errorComponent={ErrorComponent} 
            loadingComponent={Loading}
        >
            <ProfileContent />
        </MsalAuthenticationTemplate>
    );
};

export default Profile;