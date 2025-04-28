import React, { useState, useEffect, ReactNode } from 'react';
import { useMsal } from '@azure/msal-react';
import { InteractionRequiredAuthError, InteractionType, AccountInfo } from '@azure/msal-browser';
import * as MicrosoftGraph from '@microsoft/microsoft-graph-types';
import { Client } from '@microsoft/microsoft-graph-client';
import { AuthCodeMSALBrowserAuthenticationProvider } from '@microsoft/microsoft-graph-client/authProviders/authCodeMsalBrowser';
import { loginRequest, graphConfig } from '../../configs/auth';
import { GraphContextType } from '../../types/graph-context-type';
import { GraphContext } from '../graph-context';

// Provider props
interface GraphProviderProps {
  children: ReactNode;
}

// Provider component
export const GraphProvider: React.FC<GraphProviderProps> = ({ children }) => {
  const { instance, accounts } = useMsal();
  const [graphClient, setGraphClient] = useState<Client | null>(null);
  const [userProfile, setUserProfile] = useState<MicrosoftGraph.User | null>(null);
  const [error, setError] = useState<Error | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(false);

  // Initialize the Graph client when the component mounts or accounts change
  useEffect(() => {
    const initializeGraphClient = async () => {
      try {
        // Ensure we have an active account
        const activeAccount = instance.getActiveAccount() || accounts[0];
        if (!activeAccount) return;

        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        const authProvider = new AuthCodeMSALBrowserAuthenticationProvider(instance as any, {
          account: activeAccount,
          scopes: loginRequest.scopes,
          interactionType: InteractionType.Popup
        });

        // Initialize Graph client
        const client = Client.initWithMiddleware({
          authProvider
        });

        setGraphClient(client);
      } catch (err) {
        setError(err as Error);
        console.error("Error initializing Graph client:", err);
      }
    };

    initializeGraphClient();
  }, [instance, accounts]);

  // Ensure we have valid tokens before making Graph calls
  const ensureTokens = async (account: AccountInfo) => {
    try {
      await instance.acquireTokenSilent({
        ...loginRequest,
        account
      });
    } catch (error) {
      if (error instanceof InteractionRequiredAuthError) {
        try {
          await instance.acquireTokenPopup({
            ...loginRequest,
            account
          });
        } catch (err) {
          setError(err as Error);
          throw err;
        }
      } else {
        setError(error as Error);
        throw error;
      }
    }
  };

  // Get user profile from Graph API
  const getUserProfile = async (): Promise<MicrosoftGraph.User> => {
    if (!graphClient) {
      throw new Error('Graph client not initialized');
    }
    
    setIsLoading(true);
    setError(null);
    
    try {
      const account = instance.getActiveAccount() || accounts[0];
      if (!account) {
        throw new Error('No active account');
      }
      
      await ensureTokens(account);
      
      const user = await graphClient.api(graphConfig.graphMeEndpoint)
        .select('displayName,mail,userPrincipalName,id,jobTitle,businessPhones,mobilePhone,officeLocation')
        .get();
      
      setUserProfile(user);
      return user;
    } catch (err) {
      setError(err as Error);
      throw err;
    } finally {
      setIsLoading(false);
    }
  };

  // Update user profile
  const updateUserProfile = async (userDetails: Partial<MicrosoftGraph.User>): Promise<MicrosoftGraph.User> => {
    if (!graphClient) {
      throw new Error('Graph client not initialized');
    }
    
    setIsLoading(true);
    setError(null);
    
    try {
      const account = instance.getActiveAccount() || accounts[0];
      if (!account) {
        throw new Error('No active account');
      }
      
      await ensureTokens(account);
      
      const updatedUser = await graphClient.api(graphConfig.graphMeEndpoint)
        .update(userDetails);
      
      // Update local cache
      setUserProfile(prevProfile => ({
        ...prevProfile,
        ...updatedUser
      }));
      
      return updatedUser;
    } catch (err) {
      setError(err as Error);
      throw err;
    } finally {
      setIsLoading(false);
    }
  };

  // Clear any error
  const clearError = () => {
    setError(null);
  };

  // Force refresh the profile data
  const refreshProfile = async () => {
    try {
      await getUserProfile();
    } catch (err) {
      console.error("Failed to refresh profile:", err);
    }
  };

  // Context value
  const contextValue: GraphContextType = {
    graphClient,
    userProfile,
    error,
    isLoading,
    getUserProfile,
    updateUserProfile,
    clearError,
    refreshProfile
  };

  return (
    <GraphContext.Provider value={contextValue}>
      {children}
    </GraphContext.Provider>
  );
};