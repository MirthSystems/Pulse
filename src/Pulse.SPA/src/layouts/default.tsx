import { Outlet } from "react-router";
import { AuthProvider } from "@/provider";
import { Navbar } from "@/components/navbar";

/**
 * DefaultLayout provides the basic structure with AuthProvider
 * This layout wraps all public and protected routes
 */
export default function DefaultLayout() {
  return (
    <AuthProvider>
      <div className="relative flex flex-col h-screen">
        <Navbar />
        <main className="container mx-auto max-w-7xl px-6 flex-grow pt-16">
          <Outlet />
        </main>
        <footer className="w-full flex items-center justify-center py-3">
          <div className="flex items-center gap-1 text-current">
            <span className="text-default-600">© 2025</span>
            <p className="text-primary font-semibold">Pulse</p>
            <span className="text-default-600">by Mirth Systems</span>
          </div>
        </footer>
      </div>
    </AuthProvider>
  );
}
