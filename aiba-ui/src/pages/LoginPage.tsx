import {
  Box,
  Button,
  Flex,
  FormControl,
  FormHelperText,
  FormLabel,
  Heading,
  Input,
  Link,
  useToast,
} from '@chakra-ui/react';
import React, { useEffect } from 'react';
import { Api } from '../services/Api.ts';
import { useLoginInfo } from '../modules/useLoginInfo.ts';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../modules/useAuth.ts';

export const LoginPage: React.FC = () => {
  const { userName, password, setUserName, setPassword } = useLoginInfo();
  const toast = useToast();
  const navigate = useNavigate();
  const { login } = useAuth();
  useEffect(() => {
    const checkLogin = async () => {
      return await Api.authorizeStatus();
    };
    checkLogin().then((response) => {
      if (response.status === 200) {
        login();
        navigate('/');
      }
    });
  }, [login, navigate]);
  const handleLogin = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    const response = await Api.login(userName, password);
    if (!response.ok) {
      const errMessage = await response.text();
      toast({
        title: 'Error ' + response.status,
        description: 'Failed to login : ' + errMessage,
        status: 'error',
        duration: 5000,
        isClosable: true,
      });
    } else {
      login();
      navigate('/');
    }
  };
  return (
    <Flex
      height="100vh"
      maxHeight={'100vh'}
      alignItems="start"
      justifyContent="center"
      overflowY="auto"
    >
      <Box p={6} rounded="md" boxShadow="lg" width="sm">
        <Heading mb={6} textAlign="center">
          Login
        </Heading>
        <form onSubmit={handleLogin}>
          <FormControl mb={4}>
            <FormLabel>User Name</FormLabel>
            <Input
              type="text"
              required
              value={userName}
              onChange={(e) => {
                setUserName(e.target.value);
              }}
            />
            <FormHelperText>please input your user name</FormHelperText>
          </FormControl>
          <FormControl mb={6}>
            <FormLabel>password</FormLabel>
            <Input
              type="password"
              required
              value={password}
              onChange={(e) => setPassword(e.target.value)}
            />
            <FormHelperText>please input your password</FormHelperText>
          </FormControl>
          <Button colorScheme="blue" type="submit" width="full">
            Login
          </Button>
        </form>
        <Box mt={4} textAlign="center">
          <Link href={'/account/register'} color="teal.500">
            Don't have an account? Register here
          </Link>
        </Box>
      </Box>
    </Flex>
  );
};
