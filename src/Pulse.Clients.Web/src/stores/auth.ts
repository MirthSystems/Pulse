import { defineStore } from 'pinia';
import { computed, ref } from 'vue';
import { useAuth0 } from '@auth0/auth0-vue';

export const useAuthStore = defineStore('auth', () => {
  const {
    loginWithRedirect,
    logout,
    user,
    isAuthenticated,
    isLoading,
    getAccessTokenSilently,
  } = useAuth0();

  const accessToken = ref<string | null>(null);

  const getToken = async (): Promise<string | null> => {
    if (isAuthenticated.value) {
      try {
        if (!accessToken.value) {
          const audience = import.meta.env.VITE_AUTH0_AUDIENCE;
          accessToken.value = await getAccessTokenSilently({
            authorizationParams: {
              audience,
            },
          });
        }
        return accessToken.value;
      } catch (error) {
        console.error('Error getting access token:', error);
        return null;
      }
    }
    return null;
  };

  const login = () => {
    loginWithRedirect();
  };

  const logoutUser = () => {
    logout({
      logoutParams: {
        returnTo: window.location.origin,
      },
    });
    accessToken.value = null;
  };

  return {
    login,
    logout: logoutUser,
    user: computed(() => user.value),
    isAuthenticated: computed(() => isAuthenticated.value),
    isLoading: computed(() => isLoading.value),
    getToken,
    accessToken,
  };
});
