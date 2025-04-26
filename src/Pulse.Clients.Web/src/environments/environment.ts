export const environment = {
  production: false,
  apiUrl: 'https://localhost:7001/api/',
  auth: {
    clientId: '1ea2773e-e10a-4e8c-b050-14574337ac7e',
    authority: 'https://login.microsoftonline.com/64727719-5b2f-457f-b30a-4c77d23ba226',
    redirectUri: window.location.origin,
  },
  microsoftGraph: {
    domain: 'https://graph.microsoft.com',
    version: 'v1.0',
    scopes: ['user.read'],
  },
  downstreamApi: {
    baseUrl: 'https://localhost:7001/api/',
    scopes: ['api://20e5aada-0b67-4db5-9646-1b0316b2a242/access_as_user'],
  }
};
