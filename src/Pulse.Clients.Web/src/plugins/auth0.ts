import authConfig from '../../auth_config.json';
import { createAuth0 } from '@auth0/auth0-vue';

export default createAuth0({
  domain: authConfig.domain,
  clientId: authConfig.clientId,
  authorizationParams: {
    redirect_uri: window.location.origin,
    audience: authConfig.audience,
  },
});
