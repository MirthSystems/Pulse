/**
 * UserProfile is a unified type for profile data throughout the app
 */
export type UserProfile = {
  id: string;
  displayName: string;
  email?: string;
  jobTitle?: string;
  phoneNumber?: string;
  location?: string;
  avatar?: string;
};