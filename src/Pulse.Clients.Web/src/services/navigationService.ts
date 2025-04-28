import { NavigateFunction } from "react-router-dom";
import { INavigationClient, NavigationOptions } from "@azure/msal-browser";

/**
 * Custom navigation client for MSAL.js integration with React Router.
 * Implements INavigationClient to allow MSAL to use React Router navigation.
 */
export class NavigationService implements INavigationClient {
  private navigate: NavigateFunction;

  /**
   * Constructs a NavigationService.
   * @param navigate - The React Router navigate function.
   */
  constructor(navigate: NavigateFunction) {
    this.navigate = navigate;
  }

  /**
   * Navigates to other pages within the same web application using React Router.
   * @param url - The target URL (can be absolute or relative).
   * @param options - Navigation options (e.g., noHistory for replace).
   * @returns Promise resolving to false (MSAL expects a boolean).
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
   * Navigates to external pages outside the web application.
   * @param url - The external URL to navigate to.
   * @param _options - Unused navigation options.
   * @returns Promise resolving to true (MSAL expects a boolean).
   */
  async navigateExternal(url: string, _options: NavigationOptions) {
    window.location.assign(url);
    return true;
  }
}