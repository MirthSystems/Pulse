import { useAuth } from '../../user/hooks/useAuth';
import { useAppSelector } from '../../../app/hooks';

/**
 * Custom hook that provides access to API-related state and functionality
 */
export const useApi = () => {
  const { getToken } = useAuth();
  const { token, isAuthenticated, userRoles } = useAppSelector((state) => state.user);
  
  /**
   * Check if the current user has a specific role
   */
  const hasRole = (role: string): boolean => {
    return userRoles.includes(role);
  };

  /**
   * Check if the current user has any of the specified roles
   */
  const hasAnyRole = (roles: string[]): boolean => {
    return roles.some(role => userRoles.includes(role));
  };

  return {
    isAuthenticated,
    token,
    getToken,
    hasRole,
    hasAnyRole,
    userRoles
  };
};