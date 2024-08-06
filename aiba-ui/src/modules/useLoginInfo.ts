import { useState } from 'react';

export const useLoginInfo = () => {
  const [userName, setUserName] = useState('');
  const [password, setPassword] = useState('');
  const [email, setEmail] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  return {
    userName,
    password,
    email,
    confirmPassword,
    setUserName,
    setPassword,
    setEmail,
    setConfirmPassword,
  };
};
