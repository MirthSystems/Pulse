import React from "react";
import ReactDOM from "react-dom/client";
import { 
  createBrowserRouter, 
  RouterProvider
} from "react-router";

import { Provider, ErrorProvider } from "./provider.tsx";
import "@/styles/globals.css";

import DefaultLayout from "@/layouts/default";
import BackofficeLayout from "@/layouts/backoffice";

import IndexPage from "@/pages/index";
import SearchPage from "@/pages/search";
import ErrorPage from "@/pages/error";
import LoadingPage from "@/pages/loading";

const BackofficeIndex = React.lazy(() => import("@/pages/backoffice/index"));

const router = createBrowserRouter([{
    path: "/",
    element: <DefaultLayout />,
    errorElement: (
      <ErrorProvider>
        <ErrorPage />
      </ErrorProvider>
    ),
    children: [
      // Public routes
      { index: true, element: <IndexPage /> },
      { path: "search", element: <SearchPage /> },
      { 
        path: "backoffice",
        element: <BackofficeLayout />,
        children: [
          { 
            index: true, 
            element: (
              <React.Suspense fallback={<LoadingPage />}>
                <BackofficeIndex />
              </React.Suspense>
            )
          },
          // Add additional backoffice routes here as needed
        ]
      },
    ]
  }
]);

ReactDOM.createRoot(document.getElementById("root")!).render(
  <React.StrictMode>
    <Provider>
      <RouterProvider router={router} />
    </Provider>
  </React.StrictMode>,
);
