/**
 * Standard problem details object for error responses.
 * Based on RFC 7807 - Problem Details for HTTP APIs.
 */
export interface ProblemDetails {
  type?: string;
  title?: string;
  status?: number;
  detail?: string;
  instance?: string;
  [key: string]: unknown;
}
