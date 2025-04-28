/**
 * Mock implementation of e2e-test-utils
 * This allows TypeScript to properly resolve imports in the test files
 */

export class Screenshot {
  constructor(_baseFolder: string) {}
  async takeScreenshot(_page: unknown, _label: string): Promise<void> {}
}

export async function setupCredentials(_envResponse: unknown, _labClient: unknown): Promise<[string, string]> {
  return ['test-username', 'test-password'];
}

export async function enterCredentials(_page: unknown, _screenshot: unknown, _username: string, _password: string): Promise<void> {}

export const RETRY_TIMES = 3;

export class LabClient {
  async getVarsByCloudEnvironment(_params: unknown): Promise<unknown[]> {
    return [{}];
  }
}

export interface LabApiQueryParams {
  azureEnvironment: AzureEnvironments;
  appType: AppTypes;
}

export enum AzureEnvironments {
  CLOUD = 'cloud'
}

export enum AppTypes {
  CLOUD = 'cloud'
}

export class BrowserCacheUtils {
  constructor(_page: unknown, _storageType: string) {}
  async getTokens(): Promise<{ accessTokens: unknown[]; idTokens: unknown[]; refreshTokens: unknown[] }> {
    return { accessTokens: [{scopes: ['User.Read']}], idTokens: [{}], refreshTokens: [{}] };
  }
  async getAccountFromCache(): Promise<unknown> { return {}; }
  async accessTokenForScopesExists(_accessTokens: unknown[], _scopes: string[]): Promise<boolean> { return true; }
  async getTelemetryCacheEntry(_clientId: string): Promise<unknown> { return { cacheHits: 1 }; }
  async verifyTokenStore(_options: { scopes: string[] }): Promise<void> {}
}