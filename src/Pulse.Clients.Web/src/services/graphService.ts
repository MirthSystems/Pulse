import { Client } from '@microsoft/microsoft-graph-client';
import { AuthCodeMSALBrowserAuthenticationProvider } from '@microsoft/microsoft-graph-client/authProviders/authCodeMsalBrowser';
import { InteractionType } from '@azure/msal-browser';
import * as MicrosoftGraph from '@microsoft/microsoft-graph-types';
import { graphConfig, loginRequest, pca } from "../configs/auth";

/**
 * Service for interacting with Microsoft Graph API
 */
export class GraphService {
  private static graphClient: Client | null = null;
  private static isInitializing: boolean = false;
  private static initializationPromise: Promise<void> | null = null;

  /**
   * Initialize the Graph client authentication provider
   */
  private static async ensureClient() {
    // If initialization is already in progress, wait for it
    if (this.initializationPromise) {
      await this.initializationPromise;
    }
    
    // If the client is already initialized, return it
    if (this.graphClient) {
      return this.graphClient;
    }

    // Start initialization if not already in progress
    if (!this.isInitializing) {
      this.isInitializing = true;
      
      // Create a promise to handle initialization
      this.initializationPromise = new Promise<void>((resolve, reject) => {
        // Use a separate async function to handle initialization
        const initializeGraphClient = async () => {
          try {
            // Make sure MSAL is initialized
            if (!pca.getActiveAccount() && pca.getAllAccounts().length > 0) {
              pca.setActiveAccount(pca.getAllAccounts()[0]);
            }

            // Create an authentication provider
            const authProvider = new AuthCodeMSALBrowserAuthenticationProvider(pca, {
              account: pca.getActiveAccount()!,
              scopes: loginRequest.scopes,
              interactionType: InteractionType.Popup
            });

            // Initialize the Graph client
            this.graphClient = Client.initWithMiddleware({
              authProvider
            });
            
            resolve();
          } catch (error) {
            console.error('Error initializing Graph client:', error);
            this.isInitializing = false;
            reject(error);
          }
        };
        
        // Start the async initialization process
        initializeGraphClient();
      });
      
      try {
        await this.initializationPromise;
      } catch (error) {
        console.error('Graph client initialization failed:', error);
      } finally {
        this.isInitializing = false;
      }
    }

    return this.graphClient;
  }

  /**
   * Gets information about the current user
   * @returns Promise containing user data from Microsoft Graph
   */
  static async getUserInfo(): Promise<MicrosoftGraph.User> {
    const client = await this.ensureClient();
    
    if (!client) {
      throw new Error('Graph client could not be initialized');
    }
    
    return client
      .api(graphConfig.graphMeEndpoint)
      .select('displayName,mail,jobTitle,businessPhones,officeLocation,userPrincipalName,id')
      .get();
  }

  /**
   * Updates user profile information
   * @param userDetails User profile details to update
   * @returns Updated user profile
   */
  static async updateUserProfile(userDetails: Partial<MicrosoftGraph.User>): Promise<MicrosoftGraph.User> {
    const client = await this.ensureClient();
    
    if (!client) {
      throw new Error('Graph client could not be initialized');
    }
    
    return client
      .api(graphConfig.graphMeEndpoint)
      .update(userDetails);
  }
}