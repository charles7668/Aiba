import React from 'react';
import { Navigate } from 'react-router-dom';
import { useAuth } from '../modules/useAuth.ts';
import { Api } from '../services/Api.ts';

export const ProtectedRoute: React.FC<{ children: React.ReactNode }> = ({
  children,
}) => {
  const { isLoggedIn } = useAuth();

  if (!isLoggedIn) {
    Api.authorizeStatus().then((res) => {
      if (res.status !== 200) {
        return <Navigate to="/account/login" />;
      } else {
        return children;
      }
    });
  }
  return children;
};
