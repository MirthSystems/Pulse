import { Label } from 'src/components/label';
import { SvgColor } from 'src/components/svg-color';

// ----------------------------------------------------------------------

const icon = (name: string) => <SvgColor src={`/assets/icons/navbar/${name}.svg`} />;

export type NavItem = {
  title: string;
  path: string;
  icon: React.ReactNode;
  info?: React.ReactNode;
};

export const navData = [
  {
    title: 'Analytics',
    path: '/my-account',
    icon: icon('ic-analytics'),
  },
  {
    title: 'User',
    path: '/my-account/user',
    icon: icon('ic-user'),
  },
  {
    title: 'Product',
    path: '/my-account/products',
    icon: icon('ic-cart'),
    info: (
      <Label color="error" variant="inverted">
        +3
      </Label>
    ),
  },
  {
    title: 'Blog',
    path: '/my-account/blog',
    icon: icon('ic-blog'),
  }
];
