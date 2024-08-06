import {
  Box,
  Button,
  Flex,
  FormControl,
  FormHelperText,
  FormLabel,
  Heading,
  Input,
  useToast,
} from '@chakra-ui/react';
import React from 'react';
import { Api } from '../services/Api.ts';
import { useLoginInfo } from '../modules/useLoginInfo.ts';
import { RegisterInfo } from '../models/RegisterInfo.ts';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../modules/useAuth.ts';

export const RegisterPage: React.FC = () => {
  const {
    userName,
    password,
    email,
    confirmPassword,
    setUserName,
    setPassword,
    setEmail,
    setConfirmPassword,
  } = useLoginInfo();
  const toast = useToast();
  const navigate = useNavigate();
  const { login } = useAuth();
  const handleLogin = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    const response = await Api.register({
      userName,
      password,
      email,
      confirmPassword,
    } as RegisterInfo);
    if (!response.ok) {
      const errMessage = await response.text();
      toast({
        title: 'Error ' + response.status,
        description: 'Failed to register : ' + errMessage,
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
      alignItems="center"
      justifyContent="center"
      bg="gray.100"
    >
      <Box bg="white" p={6} rounded="md" boxShadow="lg" width="sm">
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
          <FormControl mb={4}>
            <FormLabel>E-mail</FormLabel>
            <Input
              type="email"
              required
              value={email}
              onChange={(e) => {
                setEmail(e.target.value);
              }}
            />
            <FormHelperText>please input your E-mail</FormHelperText>
          </FormControl>
          <FormControl mb={4}>
            <FormLabel>password</FormLabel>
            <Input
              type="password"
              required
              value={password}
              onChange={(e) => setPassword(e.target.value)}
            />
            <FormHelperText>please input your password</FormHelperText>
          </FormControl>
          <FormControl mb={6}>
            <FormLabel>Confirm Password</FormLabel>
            <Input
              type="password"
              required
              value={confirmPassword}
              onChange={(e) => setConfirmPassword(e.target.value)}
            />
            <FormHelperText>enter your password again</FormHelperText>
          </FormControl>
          <Button colorScheme="blue" type="submit" width="full">
            Login
          </Button>
        </form>
      </Box>
    </Flex>
  );
};
