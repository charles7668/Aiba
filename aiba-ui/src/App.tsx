import './App.css';
import { Box } from '@chakra-ui/react';
import { MediaDetailPage } from './pages/MediaDetailPage.tsx';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import { LoginPage } from './pages/LoginPage.tsx';
import { RegisterPage } from './pages/RegisterPage.tsx';
import { HomePage } from './pages/HomePage.tsx';
import { SearchPage } from './pages/SearchPage.tsx';
import { ProtectedRoute } from './components/ProtectedRoute.tsx';
import { ProfileSetting, SettingsPage } from './pages/SettingsPage.tsx';
import { useAuth } from './modules/useAuth.ts';
import React, { useEffect } from 'react';
import { Api } from './services/Api.ts';
import { TopToolBar } from './components/TopToolBar.tsx';
import { LibrarySetting } from './components/LibrarySetting.tsx';
import { CollectionPage } from './pages/CollectionPage.tsx';

export const ProtectedComponent: React.FC<{ child: React.ReactNode }> = ({
  child,
}) => {
  return (
    <>
      <TopToolBar />
      <Box id={'app-view'} maxW={'100vw'} maxH={'100vh'} overflowY={'auto'}>
        <ProtectedRoute>{child}</ProtectedRoute>
      </Box>
    </>
  );
};

const router = createBrowserRouter([
  {
    path: '/',
    element: <ProtectedComponent child={<HomePage />}></ProtectedComponent>,
  },
  {
    path: '/detail/:providerName',
    element: (
      <ProtectedComponent child={<MediaDetailPage />}></ProtectedComponent>
    ),
  },
  {
    path: '/account/login',
    element: <LoginPage />,
  },
  {
    path: '/account/register',
    element: <RegisterPage />,
  },
  {
    path: '/search',
    element: <ProtectedComponent child={<SearchPage />}></ProtectedComponent>,
  },
  {
    path: '/collection',
    element: (
      <ProtectedComponent child={<CollectionPage />}></ProtectedComponent>
    ),
  },
  {
    path: '/settings',
    element: <ProtectedComponent child={<SettingsPage />}></ProtectedComponent>,
    children: [
      {
        path: 'profile',
        element: <ProfileSetting />,
      },
      {
        path: 'library',
        element: <LibrarySetting />,
      },
    ],
  },
]);

function App() {
  const { login, logout } = useAuth();
  useEffect(() => {
    Api.authorizeStatus().then((res) => {
      if (res.status !== 200) {
        logout();
      } else {
        login();
      }
    });
  }, [login, logout]);
  return (
    <>
      <RouterProvider router={router} />
    </>
  );
}

export default App;
