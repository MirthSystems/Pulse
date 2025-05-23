import { Link as RouterLink } from "react-router";
import {
  Navbar as HeroUINavbar,
  NavbarBrand,
  NavbarContent,
  NavbarItem,
} from "@heroui/navbar";
import { Logo } from "@/components/icons";
import { ThemeSwitch } from "@/components/theme-switch";
import AuthStatus from "@/components/auth/auth-status";

export const Navbar = () => {
  return (
    <HeroUINavbar maxWidth="xl" position="sticky">
      {/* Logo on left side */}
      <NavbarContent className="basis-1/5 sm:basis-full" justify="start">        <NavbarBrand className="gap-3 max-w-fit">
          <RouterLink
            className="flex justify-start items-center gap-1 text-foreground"
            to="/"
          >
            <Logo />
            <p className="font-bold text-inherit">Pulse</p>
          </RouterLink>
        </NavbarBrand>
      </NavbarContent>

      {/* Theme switch and auth on right side */}
      <NavbarContent
        className="flex sm:basis-1/5"
        justify="end"
      >
        <NavbarItem className="flex gap-2">
          <ThemeSwitch />
        </NavbarItem>

        <NavbarItem>
          <AuthStatus />
        </NavbarItem>
      </NavbarContent>
    </HeroUINavbar>
  );
};
