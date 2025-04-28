import { createContext } from 'react';
import { AuthContextType } from '../types/auth-context-type';

// Create the context with a default value
export const AuthContext = createContext<AuthContextType | undefined>(undefined);
