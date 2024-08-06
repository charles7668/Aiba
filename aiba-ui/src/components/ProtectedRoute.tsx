import React, { useEffect, useState } from 'react';
import { Navigate } from 'react-router-dom';
import { useAuth } from '../modules/useAuth.ts';
import { Api } from '../services/Api.ts';

export const ProtectedRoute: React.FC<{ children: React.ReactNode }> = ({
  children,
}) => {
  const { isLoggedIn } = useAuth();
  const [loading, setLoading] = useState(true);
  const [authorized, setAuthorized] = useState(false);
  useEffect(() => {
    if (!isLoggedIn) {
      Api.authorizeStatus().then((res) => {
        if (res.status !== 200) {
          setAuthorized(false);
        } else {
          setAuthorized(true);
        }
        setLoading(false);
      });
    } else {
      setAuthorized(true);
      setLoading(false);
    }
  }, [isLoggedIn]);

  if (loading) {
    return <div>Loading...</div>;
  }
  if (!authorized) {
    return <Navigate to="/account/login" />;
  }

  return children;
};
