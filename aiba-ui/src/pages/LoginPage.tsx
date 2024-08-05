import { Box, Button, Flex, FormControl, FormHelperText, FormLabel, Heading, Input } from '@chakra-ui/react';
import React, { useState } from 'react';
import { Api } from '../services/Api.ts';

export const LoginPage: React.FC = () => {
  const [userName, setUserName] = useState('');
  const [password, setPassword] = useState('');
  const handleLogin = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    try{
      const response = await Api.login(userName, password);
      console.log(response);
    }catch (error) {
      console.log(error as string);
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
      <Box
        bg="white"
        p={6}
        rounded="md"
        boxShadow="lg"
        width="sm"
      >
        <Heading mb={6} textAlign="center">Login</Heading>
        <form onSubmit={handleLogin}>
          <FormControl mb={4}>
            <FormLabel>User Name</FormLabel>
            <Input type="text" required value={userName} onChange={(e) => {
              setUserName(e.target.value);
            }} />
            <FormHelperText>please input your user name</FormHelperText>
          </FormControl>
          <FormControl mb={6}>
            <FormLabel>password</FormLabel>
            <Input type="password" required value={password} onChange={e => setPassword(e.target.value)} />
            <FormHelperText>please input your password</FormHelperText>
          </FormControl>
          <Button colorScheme="blue" type="submit" width="full">Login</Button>
        </form>
      </Box>
    </Flex>
  );
};