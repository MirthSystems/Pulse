import { Link } from "@heroui/link";

import { Navbar } from "@/components/navbar";
import SpecialsSearch from "@/components/specials-search";
import LoginButton from "@/components/login";

export default function DefaultLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <div className="relative flex flex-col h-screen">
      <Navbar />
      <main className="container mx-auto max-w-7xl px-6 flex-grow pt-16">
        <div className="absolute top-4 right-4">
          <LoginButton />
        </div>
        <div className="mt-8">
          <SpecialsSearch />
        </div>
        {children}
      </main>
      <footer className="w-full flex items-center justify-center py-3">
        <Link
          isExternal
          className="flex items-center gap-1 text-current"
          href="https://heroui.com"
          title="heroui.com homepage"
        >
          <span className="text-default-600">Powered by</span>
          <p className="text-primary">HeroUI</p>
        </Link>
      </footer>
    </div>
  );
}
