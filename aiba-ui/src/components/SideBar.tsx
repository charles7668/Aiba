import React from 'react';
import { Box, Icon, Text, VStack } from '@chakra-ui/react';
import { useNavigate } from 'react-router-dom';
import { IconType } from 'react-icons';

const SidebarItem: React.FC<{
  icon: React.ElementType;
  title: string;
  to: string | (() => void);
}> = ({ icon, title, to }) => {
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
    </Box>
  );
};

export const SideBar: React.FC<{
  items: Array<{ icon: IconType; title: string; to: string | (() => void) }>;
}> = ({ items }) => {
  return (
    <VStack align="start" spacing={4} minW={'10em'}>
      {items.map((item) => (
        <SidebarItem
          icon={item.icon}
          title={item.title}
          to={item.to}
        ></SidebarItem>
      ))}
    </VStack>
  );
};
