import { graphConfig } from "../configs/auth";
import { GraphData } from "../types/graph-data";

export class GraphService {
  /**
   * Calls the MS Graph API with the provided access token
   * @param accessToken - The access token acquired from MSAL
   * @returns Promise containing the response data
   */
  static async getUserInfo(accessToken: string): Promise<GraphData> {
    const headers = new Headers();
    const bearer = `Bearer ${accessToken}`;

    headers.append("Authorization", bearer);

    const options = {
      method: "GET",
      headers: headers
    };

    return fetch(graphConfig.graphMeEndpoint, options)
      .then(response => {
        if (!response.ok) {
          throw new Error(`Graph API returned ${response.status}`);
        }
        return response.json();
      })
      .catch(error => {
        console.error("Error calling MS Graph API:", error);
        throw error;
      });
  }
}