import { NavigateFunction } from "react-router-dom";
import { INavigationClient, NavigationOptions } from "@azure/msal-browser";

/**
 * This is an example for overriding the default function MSAL uses to navigate to other urls in your webpage
 */
export class CustomNavigationClient implements INavigationClient {
    private navigate: NavigateFunction;

    constructor(navigate: NavigateFunction) {
        this.navigate = navigate;
    }

    /**
     * Navigates to other pages within the same web application
     * You can use the useNavigate hook provided by react-router-dom to take advantage of client-side routing
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
     * Navigates to external pages outside your web application
     * @param url
     * @param options
     */
    async navigateExternal(url: string, options: NavigationOptions) {
        window.location.assign(url);
        return true;
    }
}