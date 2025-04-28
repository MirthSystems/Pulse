/**
 * Represents an item in the breadcrumb navigation
 */
export type BreadcrumbItem = {
  /** The path for this breadcrumb */
  path: string;
  /** The display text for this breadcrumb */
  label: string;
  /** Whether this is the active/current breadcrumb */
  isActive: boolean;
};