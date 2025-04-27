import React from 'react';
import { useMsal } from '@azure/msal-react';
import { loginRequest } from '../../configs/auth';

const SignInButton: React.FC = () => {
  const { instance } = useMsal();

  const handleLogin = () => {
    instance.loginRedirect(loginRequest);
  };

  return (
    <button onClick={handleLogin}>
      Sign In
    </button>
  );
};

export default SignInButton;