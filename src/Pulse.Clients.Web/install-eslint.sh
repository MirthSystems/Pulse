#!/usr/bin/env bash

# Install ESLint dependencies
npm install --save-dev \
  eslint \
  @typescript-eslint/eslint-plugin \
  @typescript-eslint/parser \
  eslint-plugin-react \
  eslint-plugin-react-hooks \
  eslint-plugin-react-refresh

echo "ESLint dependencies installed successfully!"
echo "You can now run 'npm run lint' to lint your code."