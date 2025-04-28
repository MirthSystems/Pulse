import { Client } from '@microsoft/microsoft-graph-client';
import { AuthCodeMSALBrowserAuthenticationProvider } from '@microsoft/microsoft-graph-client/authProviders/authCodeMsalBrowser';
import { InteractionType } from '@azure/msal-browser';
import * as MicrosoftGraph from '@microsoft/microsoft-graph-types';
import { graphConfig, loginRequest, pca } from "../configs/auth";

/**
 * Service for interacting with Microsoft Graph API.
 * Handles Graph client initialization, user info retrieval, and profile updates.
 */
export class GraphService {
  private static graphClient: Client | null = null;
  private static isInitializing: boolean = false;
  private static initializationPromise: Promise<void> | null = null;

  /**
   * Ensures the Graph client is initialized and ready to use.
   * Handles MSAL account setup and authentication provider creation.
   * @returns Promise resolving to the initialized Graph client.
   */
  private static async ensureClient() {
    if (this.initializationPromise) {
      await this.initializationPromise;
    }
    if (this.graphClient) {
      return this.graphClient;
    }
    if (!this.isInitializing) {
      this.isInitializing = true;
      this.initializationPromise = new Promise<void>((resolve, reject) => {
        const initializeGraphClient = async () => {
          try {
            if (!pca.getActiveAccount() && pca.getAllAccounts().length > 0) {
              pca.setActiveAccount(pca.getAllAccounts()[0]);
            }
            const authProvider = new AuthCodeMSALBrowserAuthenticationProvider(pca, {
              account: pca.getActiveAccount()!,
              scopes: loginRequest.scopes,
              interactionType: InteractionType.Popup
            });
            this.graphClient = Client.initWithMiddleware({ authProvider });
            resolve();
          } catch (error) {
            console.error('Error initializing Graph client:', error);
            this.isInitializing = false;
            reject(error);
          }
        };
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
   * Gets information about the current user from Microsoft Graph.
   * @returns Promise containing user data from Microsoft Graph.
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
   * Updates user profile information in Microsoft Graph.
   * @param userDetails User profile details to update.
   * @returns Promise resolving to the updated user profile.
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