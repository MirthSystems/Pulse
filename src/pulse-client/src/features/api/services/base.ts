export abstract class ApiService {
  protected baseUrl: string;
  protected authToken?: string;
  
  constructor(baseUrl: string, authToken?: string) {
    this.baseUrl = baseUrl;
    this.authToken = authToken;
  }

  protected async fetch<T>(
    endpoint: string,
    method: string,
    body?: unknown,
    requiresAuth = false
  ): Promise<T> {
    const headers: HeadersInit = {
      "Content-Type": "application/json",
    };

    if (requiresAuth) {
      if (!this.authToken) {
        throw new ApiError(
          "Authentication token is required for this operation"
        );
      }
      headers["Authorization"] = `Bearer ${this.authToken}`;
    }

    try {
      const response = await fetch(`${this.baseUrl}${endpoint}`, {
        method,
        headers,
        body: body ? JSON.stringify(body) : undefined,
      });

      if (!response.ok) {
        const errorData = await response.json().catch(() => null);
        throw new ApiError(
          `HTTP error! status: ${response.status}`,
          response.status,
          errorData
        );
      }

      return (await response.json()) as T;
    } catch (error) {
      if (error instanceof ApiError) {
        throw error;
      }
      throw new ApiError(`Network error: ${(error as Error).message}`);
    }
  }

  protected objectToQueryParams<T extends object>(request: T): string {
    const params = new URLSearchParams();

    Object.entries(request).forEach(([key, value]) => {
      if (value !== undefined) {
        params.append(key, String(value));
      }
    });

    return params.toString();
  }
}

export class ApiError extends Error {
  statusCode?: number;
  responseData?: unknown;

  constructor(message: string, statusCode?: number, responseData?: unknown) {
    super(message);
    this.name = "ApiError";
    this.statusCode = statusCode;
    this.responseData = responseData;
  }
}
