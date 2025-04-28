/**
 * Represents an entry in the navigation history
 */
export type NavigationHistoryEntry = {
  /** The path of the navigation entry */
  path: string;
  /** The title of the page */
  title: string;
  /** Timestamp when navigation occurred */
  timestamp: number;
  /** Any state passed during navigation */
  state?: Record<string, unknown>;
};