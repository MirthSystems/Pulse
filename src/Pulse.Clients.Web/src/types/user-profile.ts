/**
 * UserProfile is a unified interface for profile data throughout the app
 */
export interface UserProfile {
  id: string;
  displayName: string;
  email?: string;
  jobTitle?: string;
  phoneNumber?: string;
  location?: string;
  avatar?: string;
}