import * as MicrosoftGraph from '@microsoft/microsoft-graph-types';
import { Client } from '@microsoft/microsoft-graph-client';

export interface GraphContextType {
  graphClient: Client | null;
  userProfile: MicrosoftGraph.User | null;
  error: Error | null;
  isLoading: boolean;
  getUserProfile: () => Promise<MicrosoftGraph.User>;
  updateUserProfile: (userDetails: Partial<MicrosoftGraph.User>) => Promise<MicrosoftGraph.User>;
  clearError: () => void;
  refreshProfile: () => Promise<void>;
}