import React from "react";
import { useRouteError, isRouteErrorResponse, Link } from "react-router";
import { Button, Card, CardBody } from "@heroui/react";

/**
 * ErrorPage - Error handling component
 * 
 * This component handles various error types with custom messages:
 * - 401: Unauthorized access (login required)
 * - 403: Forbidden access (insufficient permissions)
 * - 404: Page not found
 * - 500: Server error
 * - Other errors: Generic error handling
 * 
 * The component shows a friendly UI with actionable buttons to help users recover.
 */

const ErrorPage: React.FC = () => {
  const error = useRouteError();
  console.log("Error details:", error);
    let title = "Error";
  let message = "An unexpected error has occurred.";
  let actionText = "Go Home";
  let actionLink = "/";
  let errorIcon = "😵";
  
  const isProtectedRoute = window.location.pathname.startsWith('/backoffice');

  if (isRouteErrorResponse(error)) {
    switch (error.status) {
      case 401:
        title = "Unauthorized";
        message = error.data || "You need to log in to access this page.";
        actionText = "Go to Home";
        errorIcon = "🔒";
        break;
      case 403:
        title = "Access Denied";
        message = error.data || "You don't have permission to access this resource.";
        errorIcon = "🚫";
        break;
      case 404:
        title = "Page Not Found";
        message = error.data || "Sorry, we couldn't find the page you're looking for.";
        errorIcon = "🔍";
        break;
      case 500:
        title = "Server Error";
        message = "Something went wrong on our end. Please try again later.";
        errorIcon = "⚠️";        
        break;
      default:
        title = `Error ${error.status}`;
        message = error.statusText || "Unknown Error";
        errorIcon = "❌";
    }
  } else if (error instanceof Error) {
    title = "Application Error";
    message = error.message;
    errorIcon = "⚠️";
  }
  
  if (isProtectedRoute) {
    title = "Authentication Required";
    message = "You need to log in to access this page.";
    errorIcon = "🔒";
  }

  const handleGoBack = () => {
    if (window.history.length > 1) {
      window.history.back();
    } else {
      window.location.href = "/";
    }
  };

  return (
    <div className="flex flex-col items-center justify-center min-h-[80vh] px-4">
      <Card className="max-w-md w-full">
        <CardBody className="text-center p-8">
          <div className="text-6xl mb-4">{errorIcon}</div>
          <h1 className="text-3xl font-bold mb-4 text-foreground">{title}</h1>
          <p className="text-lg text-default-600 mb-8">{message}</p>          <div className="flex flex-row gap-3">
            <Button 
              onClick={handleGoBack}
              variant="light"
              size="lg"
              className="flex-1"
            >
              Go Back
            </Button>
            
            <Button 
              as={Link}
              to={actionLink}
              color="primary"
              variant="solid"
              size="lg"
              className="flex-1"
            >
              {actionText}
            </Button>
          </div>
        </CardBody>
      </Card>
    </div>
  );
};

export default ErrorPage;
