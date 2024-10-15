import React, { ReactNode } from 'react';
import {
  Box,
  Icon,
  IconButton,
  Menu,
  MenuButton,
  MenuItem,
  MenuItemProps,
  MenuList,
  Text,
  VStack,
} from '@chakra-ui/react';
import { useNavigate } from 'react-router-dom';
import { VerticalDotsIcon } from '../icons/VerticalDotsIcon.ts';
import { SideBarItemProps } from '../models/SideBarItemProps.ts';

const SidebarItem: React.FC<{
  icon: React.ElementType;
  title: string;
  to: string | (() => void);
  MenuProp?: ReactNode;
}> = ({ icon, title, to, MenuProp }) => {
  const navigate = useNavigate();
  return (
    <Box
      onClick={() => {
        if (typeof to === 'string') {
          navigate(to);
          return;
        }
        to();
      }}
      minW={'100%'}
      w={'100%'}
      display="flex"
      alignItems="center"
      justifyContent="space-between"
      cursor="pointer"
      p={3}
      _hover={{ bg: 'gray.700' }}
    >
      <Box display="flex" alignItems="center">
        <Icon as={icon} mr={3} boxSize={6} />
        <Text>{title}</Text>
      </Box>
      {MenuProp && MenuProp}
    </Box>
  );
};

interface SideBarItemSubMenuProps {
  items?: Array<MenuItemProps>;
}

const SideBarItemSubMenu: React.FC<SideBarItemSubMenuProps> = (props) => {
  return (
    props.items && (
      <Menu>
        <MenuButton
          as={IconButton}
          aria-label="Options"
          icon={<VerticalDotsIcon />}
          variant={'unstyled'}
          color={'white'}
          size={'xs'}
        />
        <MenuList>
          {props.items.map((item, index) => (
            <MenuItem key={index} {...(item as MenuItemProps)}></MenuItem>
          ))}
        </MenuList>
      </Menu>
    )
  );
};

export const SideBar: React.FC<{
  items: Array<SideBarItemProps>;
}> = ({ items }) => {
  return (
    <VStack align="start" spacing={4} minW={'10em'}>
      {items.map((item, index) => (
        <SidebarItem
          key={index}
          icon={item.icon}
          title={item.title}
          to={item.to}
          MenuProp={<SideBarItemSubMenu items={item.menuItems} />}
        ></SidebarItem>
      ))}
    </VStack>
  );
};
