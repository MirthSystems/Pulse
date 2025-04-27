import React from 'react';

interface WelcomeNameProps {
  name: string;
}

const WelcomeName: React.FC<WelcomeNameProps> = ({ name }) => {
  return <h1>Welcome, {name}!</h1>;
};

export default WelcomeName;