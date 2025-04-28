/**
 * GraphData type defines the structure of user data from Microsoft Graph
 */
export type GraphData = {
  jobTitle?: string;
  mail?: string;
  businessPhones?: string[];
  officeLocation?: string;
  displayName?: string;
  id?: string;
};