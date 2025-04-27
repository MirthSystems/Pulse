import { NavigateFunction } from "react-router-dom";
import { INavigationClient, NavigationOptions } from "@azure/msal-browser";

/**
 * Custom navigation client for MSAL.js integration with React Router
 */
export class NavigationService implements INavigationClient {
  private navigate: NavigateFunction;

  constructor(navigate: NavigateFunction) {
    this.navigate = navigate;
  }

  /**
   * Navigates to other pages within the same web application
   * Uses react-router-dom's useNavigate hook for client-side routing
   * @param url
   * @param options
   */
  async navigateInternal(url: string, options: NavigationOptions) {
    const relativePath = url.replace(window.location.origin, "");
    if (options.noHistory) {
      this.navigate(relativePath, { replace: true });
    } else {
      this.navigate(relativePath);
    }

    return false;
  }

  /**
   * Navigates to external pages outside the web application
   * @param url
   * @param options
   */
  async navigateExternal(url: string, options: NavigationOptions) {
    window.location.assign(url);
    return true;
  }
}