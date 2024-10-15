import { IconType } from 'react-icons';
import { MenuItemProps } from '@chakra-ui/react';

export interface SideBarItemProps {
  icon: IconType;
  title: string;
  to: string | (() => void);
  menuItems?: Array<MenuItemProps>;
}
