import {
  Box,
  Flex,
  Heading,
  Icon,
  IconButton,
  Spacer,
  useColorModeValue,
} from '@chakra-ui/react';
import React from 'react';
import './TopToolBar.css';
import { MdAccountCircle } from 'react-icons/md';

export const TopToolBar: React.FC<{ id?: string }> = ({ id = null }) => {
  const bg = useColorModeValue('white', 'black');
  const color = useColorModeValue('black', 'white');

  return (
    <Box {...(id && { id })} bg={bg} px={4} py={2}>
      <Flex align="center">
        <Heading size="md" color={color}>
          Aiba
        </Heading>
        <Spacer />
        <IconButton
          isRound={true}
          icon={<Icon as={MdAccountCircle} />}
          fontSize="40px"
          aria-label={'account'}
        />
      </Flex>
    </Box>
  );
};
