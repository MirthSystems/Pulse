import { Outlet } from "react-router";
import ProtectedRoute from "@/components/auth/protected-route";

/**
 * BackofficeLayout wraps routes that require authentication
 * This layout should be used for backoffice and other protected routes
 */
export default function BackofficeLayout() {
  return (
    <ProtectedRoute>
      <Outlet />
    </ProtectedRoute>
  );
}
