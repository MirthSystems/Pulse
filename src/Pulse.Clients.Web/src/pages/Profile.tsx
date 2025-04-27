import React, { useEffect, useState } from "react";

// Msal imports
import { MsalAuthenticationTemplate, useMsal } from "@azure/msal-react";
import { InteractionStatus, InteractionType, InteractionRequiredAuthError, AccountInfo } from "@azure/msal-browser";
import { loginRequest } from "../configs/auth";

// Sample app imports
import { ProfileData } from "../components/profile/ProfileData";
import Loading from "../components/common/Loading";
import ErrorComponent from "../components/common/ErrorComponent";
import { callMsGraph } from "../utils/MsGraphApiCall";

// Material-ui imports
import Paper from "@mui/material/Paper";

// Define GraphData interface
export interface GraphData {
  jobTitle?: string;
  mail?: string;
  businessPhones?: string[];
  officeLocation?: string;
  displayName?: string;
  id?: string;
}

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
                callMsGraph(response.accessToken)
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
        <Paper>
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