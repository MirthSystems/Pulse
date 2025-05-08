import { Middleware, Action } from '@reduxjs/toolkit';
import { apiClient } from '../client';

/**
 * Action metadata type for identifying actions that require authentication
 */
interface ActionWithAuthMeta extends Action {
  meta?: {
    requiresAuth?: boolean;
    authToken?: string;
    [key: string]: unknown;
  };
}

/**
 * Redux middleware that centralizes authentication token management
 * and automatically adds authentication token to actions that need it
 */
export const authMiddleware: Middleware = store => next => (action) => {
  // Cast the unknown action to our extended type for type checking
  const typedAction = action as ActionWithAuthMeta;
  
  // Skip actions without meta information or that don't require auth
  if (!typedAction.meta?.requiresAuth) {
    return next(action);
  }

  const state = store.getState();
  const token = state.user?.token || localStorage.getItem('auth_token');
  
  // If no token is available, dispatch auth failed action
  if (!token) {
    console.warn('Authenticated action attempted without a valid token');
    return next({
      type: 'AUTH_REQUIRED',
      payload: { originalAction: action },
      error: true
    });
  }

  // Update token in API client if needed
  apiClient.setAuthToken(token);
  
  // Add token to action metadata
  const actionWithAuth = {
    ...typedAction,
    meta: {
      ...typedAction.meta,
      authToken: token
    }
  };
  
  return next(actionWithAuth);
};