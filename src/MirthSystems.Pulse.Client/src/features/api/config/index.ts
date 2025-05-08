export interface IApiConfig {
  baseUrl: string;
  apiVersion: string;
  timeout: number;
}

export const apiConfig: IApiConfig = {
  baseUrl: import.meta.env.VITE_API_DOMAIN || 'http://localhost:5000',
  apiVersion: import.meta.env.VITE_API_VERSION || 'v1.0.0',
  timeout: 30000
};