import { IProblemDetails } from '../types/models';

export interface ApiConfig {
  baseUrl: string;
  timeout: number;
}

export class ApiError extends Error {
  status: number;
  problemDetails?: IProblemDetails;

  constructor(message: string, status: number, problemDetails?: IProblemDetails) {
    super(message);
    this.name = 'ApiError';
    this.status = status;
    this.problemDetails = problemDetails;
  }

  /**
   * Returns true if this error represents an authentication error
   */
  isAuthError(): boolean {
    return this.status === 401;
  }

  /**
   * Returns true if this error represents a forbidden error
   */
  isForbiddenError(): boolean {
    return this.status === 403;
  }

  /**
   * Returns true if this error represents a not found error
   */
  isNotFoundError(): boolean {
    return this.status === 404;
  }
}

/**
 * Core API client service that handles HTTP requests with authentication
 */
export class ApiClient {
  private config: ApiConfig;
  private authToken?: string;

  constructor(config?: Partial<ApiConfig>) {
    this.config = {
      baseUrl: import.meta.env.VITE_API_DOMAIN || 'http://localhost:5000',
      timeout: 30000,
      ...config
    };
  }

  /**
   * Sets the authorization token for API requests
   */
  setAuthToken(token: string): void {
    this.authToken = token;
  }

  /**
   * Clears the authorization token
   */
  clearAuthToken(): void {
    this.authToken = undefined;
  }

  /**
   * Performs a GET request
   */
  async get<T>(endpoint: string, queryParams?: Record<string, unknown>, requireAuth: boolean = false): Promise<T> {
    const url = this.buildUrl(endpoint, queryParams);
    return this.request<T>('GET', url, undefined, requireAuth);
  }

  /**
   * Performs a POST request
   */
  async post<T>(endpoint: string, data?: unknown, requireAuth: boolean = false): Promise<T> {
    const url = this.buildUrl(endpoint);
    return this.request<T>('POST', url, data, requireAuth);
  }

  /**
   * Performs a PUT request
   */
  async put<T>(endpoint: string, data?: unknown, requireAuth: boolean = false): Promise<T> {
    const url = this.buildUrl(endpoint);
    return this.request<T>('PUT', url, data, requireAuth);
  }

  /**
   * Performs a DELETE request
   */
  async delete<T>(endpoint: string, requireAuth: boolean = false): Promise<T> {
    const url = this.buildUrl(endpoint);
    return this.request<T>('DELETE', url, undefined, requireAuth);
  }

  /**
   * Builds a URL with optional query parameters
   */
  private buildUrl(endpoint: string, queryParams?: Record<string, unknown>): string {
    const baseUrl = this.config.baseUrl;
    
    // Ensure endpoint starts with a slash
    const normalizedEndpoint = endpoint.startsWith('/') ? endpoint : `/${endpoint}`;
    
    let url = `${baseUrl}${normalizedEndpoint}`;
    
    if (queryParams && Object.keys(queryParams).length > 0) {
      const searchParams = new URLSearchParams();
      
      Object.entries(queryParams).forEach(([key, value]) => {
        if (value !== undefined && value !== null) {
          if (typeof value === 'boolean') {
            searchParams.append(key, value ? 'true' : 'false');
          } else {
            searchParams.append(key, String(value));
          }
        }
      });
      
      url += `?${searchParams.toString()}`;
    }
    
    return url;
  }

  /**
   * Makes an HTTP request with the specified method, URL, and optional data
   */
  private async request<T>(
    method: string, 
    url: string, 
    data?: unknown,
    requireAuth: boolean = false
  ): Promise<T> {
    const headers: Record<string, string> = {
      'Content-Type': 'application/json',
    };

    // Only add auth header if we're making an authenticated request and we have a token
    if (requireAuth && this.authToken) {
      headers['Authorization'] = `Bearer ${this.authToken}`;
    }

    const controller = new AbortController();
    const timeoutId = setTimeout(() => controller.abort(), this.config.timeout);

    try {
      const response = await fetch(url, {
        method,
        headers,
        body: data ? JSON.stringify(data) : undefined,
        signal: controller.signal,
      });

      return this.parseResponse<T>(response);
    } catch (error) {
      if (error instanceof DOMException && error.name === 'AbortError') {
        throw new ApiError('Request timed out', 408);
      }
      throw error;
    } finally {
      clearTimeout(timeoutId);
    }
  }

  /**
   * Parses the response from the server
   */
  private async parseResponse<T>(response: Response): Promise<T> {
    if (response.ok) {
      const contentType = response.headers.get('content-type');
      if (contentType && contentType.includes('application/json')) {
        return response.json() as Promise<T>;
      } else {
        return {} as T;
      }
    }

    // Handle specific error status codes
    if (response.status === 401) {
      // Unauthorized - trigger auth flow or redirect to login
      console.warn('Authentication token is invalid or expired');
    } else if (response.status === 403) {
      // Forbidden - user doesn't have permission
      console.warn('Access denied - insufficient permissions');
    } else if (response.status === 404) {
      // Not found
      console.warn(`Resource not found: ${response.url}`);
    } else if (response.status === 429) {
      // Too many requests
      console.warn('Rate limit exceeded - too many requests');
    } else if (response.status >= 500) {
      // Server errors
      console.error('Server error occurred');
    }

    let errorBody: IProblemDetails | undefined;
    try {
      errorBody = await response.json() as IProblemDetails;
    } catch {
      errorBody = undefined;
    }

    const errorMessage = errorBody?.detail || errorBody?.title || `API error: ${response.status} ${response.statusText}`;
    throw new ApiError(errorMessage, response.status, errorBody);
  }
}