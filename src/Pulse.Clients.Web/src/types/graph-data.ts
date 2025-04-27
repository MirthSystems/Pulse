/**
 * GraphData interface defines the structure of user data from Microsoft Graph
 */
export interface GraphData {
  jobTitle?: string;
  mail?: string;
  businessPhones?: string[];
  officeLocation?: string;
  displayName?: string;
  id?: string;
}