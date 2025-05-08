import { ApiConfig, defaultApiConfig } from '../config/apiConfig';
import { IProblemDetails } from '../types/models';

export class ApiError extends Error {
  status: number;
  problemDetails?: IProblemDetails;

  constructor(message: string, status: number, problemDetails?: IProblemDetails) {
    super(message);
    this.name = 'ApiError';
    this.status = status;
    this.problemDetails = problemDetails;
  }
}

export class ApiService {
  protected config: ApiConfig;
  protected authToken?: string;

  constructor(config: Partial<ApiConfig> = {}) {
    this.config = { ...defaultApiConfig, ...config };
  }

  setAuthToken(token: string): void {
    this.authToken = token;
  }

  clearAuthToken(): void {
    this.authToken = undefined;
  }

  protected getBaseUrl(): string {
    return this.config.baseUrl;
  }

  protected getHeaders(additionalHeaders: Record<string, string> = {}): Record<string, string> {
    const headers: Record<string, string> = {
      'Content-Type': 'application/json',
      ...additionalHeaders
    };

    if (this.authToken) {
      headers['Authorization'] = `Bearer ${this.authToken}`;
    }

    return headers;
  }

  protected async parseResponse<T>(response: Response): Promise<T> {
    if (response.ok) {
      const contentType = response.headers.get('content-type');
      if (contentType && contentType.includes('application/json')) {
        return response.json() as Promise<T>;
      } else {
        return {} as T;
      }
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

  protected buildQueryString(params: Record<string, unknown>): string {
    const searchParams = new URLSearchParams();
    
    Object.entries(params).forEach(([key, value]) => {
      if (value !== undefined && value !== null) {
        if (typeof value === 'boolean') {
          searchParams.append(key, value ? 'true' : 'false');
        } else {
          searchParams.append(key, String(value));
        }
      }
    });
    
    const queryString = searchParams.toString();
    return queryString ? `?${queryString}` : '';
  }

  protected async get<T>(
    endpoint: string, 
    queryParams: Record<string, unknown> = {}, 
    additionalHeaders: Record<string, string> = {}
  ): Promise<T> {
    const queryString = this.buildQueryString(queryParams);
    const url = `${this.getBaseUrl()}${endpoint}${queryString}`;
    
    const headers = this.getHeaders(additionalHeaders);
    
    const controller = new AbortController();
    const timeoutId = setTimeout(() => controller.abort(), this.config.timeout);
    
    try {
      const response = await fetch(url, {
        method: 'GET',
        headers,
        signal: controller.signal
      });
      
      return await this.parseResponse<T>(response);
    } catch (error) {
      if (error instanceof DOMException && error.name === 'AbortError') {
        throw new ApiError('Request timed out', 408);
      }
      throw error;
    } finally {
      clearTimeout(timeoutId);
    }
  }
  
  protected async post<T>(
    endpoint: string, 
    body: unknown, 
    additionalHeaders: Record<string, string> = {}
  ): Promise<T> {
    const url = `${this.getBaseUrl()}${endpoint}`;
    const headers = this.getHeaders(additionalHeaders);
    
    const controller = new AbortController();
    const timeoutId = setTimeout(() => controller.abort(), this.config.timeout);
    
    try {
      const response = await fetch(url, {
        method: 'POST',
        headers,
        body: JSON.stringify(body),
        signal: controller.signal
      });
      
      return await this.parseResponse<T>(response);
    } catch (error) {
      if (error instanceof DOMException && error.name === 'AbortError') {
        throw new ApiError('Request timed out', 408);
      }
      throw error;
    } finally {
      clearTimeout(timeoutId);
    }
  }
  
  protected async put<T>(
    endpoint: string, 
    body: unknown, 
    additionalHeaders: Record<string, string> = {}
  ): Promise<T> {
    const url = `${this.getBaseUrl()}${endpoint}`;
    const headers = this.getHeaders(additionalHeaders);
    
    const controller = new AbortController();
    const timeoutId = setTimeout(() => controller.abort(), this.config.timeout);
    
    try {
      const response = await fetch(url, {
        method: 'PUT',
        headers,
        body: JSON.stringify(body),
        signal: controller.signal
      });
      
      return await this.parseResponse<T>(response);
    } catch (error) {
      if (error instanceof DOMException && error.name === 'AbortError') {
        throw new ApiError('Request timed out', 408);
      }
      throw error;
    } finally {
      clearTimeout(timeoutId);
    }
  }
  
  protected async delete<T>(
    endpoint: string, 
    additionalHeaders: Record<string, string> = {}
  ): Promise<T> {
    const url = `${this.getBaseUrl()}${endpoint}`;
    const headers = this.getHeaders(additionalHeaders);
    
    const controller = new AbortController();
    const timeoutId = setTimeout(() => controller.abort(), this.config.timeout);
    
    try {
      const response = await fetch(url, {
        method: 'DELETE',
        headers,
        signal: controller.signal
      });
      
      return await this.parseResponse<T>(response);
    } catch (error) {
      if (error instanceof DOMException && error.name === 'AbortError') {
        throw new ApiError('Request timed out', 408);
      }
      throw error;
    } finally {
      clearTimeout(timeoutId);
    }
  }
}