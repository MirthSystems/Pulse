import { useAuth0 } from "@auth0/auth0-react";
import { Outlet, data } from "react-router";
import LoadingPage from "@/pages/loading";

interface ProtectedRouteProps {
  children?: React.ReactNode;
}

export const ProtectedRoute: React.FC<ProtectedRouteProps> = ({ children }) => {
  const { isAuthenticated, isLoading } = useAuth0();
  if (isLoading) {
    return <LoadingPage />;
  }
  
  if (!isAuthenticated) {
    throw data({
      message: "You need to log in to access this page.",
      isAuthError: true
    }, { status: 401 });
  }

  return children ? children : <Outlet />;
};

export default ProtectedRoute;
