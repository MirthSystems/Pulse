import * as MicrosoftGraph from '@microsoft/microsoft-graph-types';
import { Client } from '@microsoft/microsoft-graph-client';

/**
 * Interface for Microsoft Graph context value.
 * Provides Graph client, user profile, error/loading state, and Graph methods.
 */
export interface GraphContextType {
  /** The Microsoft Graph client instance, or null if not initialized. */
  graphClient: Client | null;
  /** The current user's Microsoft Graph profile, or null if not loaded. */
  userProfile: MicrosoftGraph.User | null;
  /** Any error encountered during Graph operations. */
  error: Error | null;
  /** Indicates if a Graph operation is in progress. */
  isLoading: boolean;
  /** Retrieves the current user's profile from Microsoft Graph. */
  getUserProfile: () => Promise<MicrosoftGraph.User>;
  /** Updates the current user's profile in Microsoft Graph. */
  updateUserProfile: (userDetails: Partial<MicrosoftGraph.User>) => Promise<MicrosoftGraph.User>;
  /** Clears any error in the Graph context. */
  clearError: () => void;
  /** Forces a refresh of the user profile from Microsoft Graph. */
  refreshProfile: () => Promise<void>;
}