import {
  Box,
  Divider,
  Flex,
  Heading,
  Icon,
  IconButton,
  Menu,
  MenuButton,
  MenuItem,
  MenuList,
  Spacer,
  useColorModeValue,
} from '@chakra-ui/react';
import React, { useEffect } from 'react';
import { MdAccountCircle } from 'react-icons/md';
import { Api } from '../services/Api.ts';
import { useAuth, AuthContextType } from '../modules/useAuth.ts';
import { useNavigate, Link, NavigateFunction } from 'react-router-dom';

const getUserIconMenuItems = (
  authContext: AuthContextType,
  navigate: NavigateFunction
) => {
  if (authContext.isLoggedIn) {
    return (
      <>
        <MenuItem onClick={() => navigate('/settings')}>Settings</MenuItem>
        <Divider />
        <MenuItem
          onClick={async () => {
            await Api.logout();
            authContext.logout();
            navigate('/account/login');
          }}
        >
          Logout
        </MenuItem>
      </>
    );
  }
  return (
    <>
      <MenuItem>
        <Link to={'/account/login'}>Logout</Link>
      </MenuItem>
    </>
  );
};

export const TopToolBar: React.FC<{ id?: string }> = ({ id = null }) => {
  const bg = useColorModeValue('white', 'black');
  const color = useColorModeValue('black', 'white');
  const { isLoggedIn, login, logout } = useAuth();
  const [loginStatus, setLoginStatus] = React.useState(false);
  const navigate = useNavigate();
  useEffect(() => {
    if (isLoggedIn) {
      setLoginStatus(true);
    } else {
      setLoginStatus(false);
    }
  }, [isLoggedIn]);

  return (
    <Box
      {...(id && { id })}
      bg={bg}
      px={4}
      py={2}
      pos={'sticky'}
      left={0}
      top={0}
      zIndex={1000}
    >
      <Flex align="center" p={0}>
        <Link to={'/'}>
          <Box px={3}>
            <Heading size="md" color={color}>
              Aiba
            </Heading>
          </Box>
        </Link>
        <Link to={'/search'}>
          <Box px={3}>
            <Heading size="md" color={color}>
              Search
            </Heading>
          </Box>
        </Link>
        <Spacer />
        <Menu>
          <MenuButton
            as={IconButton}
            isRound={true}
            icon={<Icon as={MdAccountCircle} />}
            fontSize="40px"
            aria-label={'account'}
          />
          <MenuList>
            {getUserIconMenuItems(
              { isLoggedIn: loginStatus, login, logout },
              navigate
            )}
          </MenuList>
        </Menu>
      </Flex>
    </Box>
  );
};
