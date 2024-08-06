import './App.css';
import { Box } from '@chakra-ui/react';
import { MediaDetailPage } from './pages/MediaDetailPage.tsx';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import { LoginPage } from './pages/LoginPage.tsx';
import { RegisterPage } from './pages/RegisterPage.tsx';
import { HomePage } from './pages/HomePage.tsx';
import { SearchPage } from './pages/SearchPage.tsx';
import { ProtectedRoute } from './components/ProtectedRoute.tsx';

const router = createBrowserRouter([
  {
    path: '/',
    element: (
      <ProtectedRoute>
        <HomePage />
      </ProtectedRoute>
    ),
  },
  {
    path: '/detail/:providerName',
    element: (
      <ProtectedRoute>
        <MediaDetailPage />
      </ProtectedRoute>
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
    element: (
      <ProtectedRoute>
        <SearchPage />
      </ProtectedRoute>
    ),
  },
]);

function App() {
  return (
    <>
      <Box id={'app-view'} maxW={'100vw'} maxH={'100vh'} overflowY={'auto'}>
        <RouterProvider router={router} />
      </Box>
    </>
  );
}

export default App;
