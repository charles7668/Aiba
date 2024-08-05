import './App.css';
import { TopToolBar } from './components/TopToolBar.tsx';
import { MainContent } from './components/MainContent.tsx';
import { Box } from '@chakra-ui/react';
import { MediaDetailPage } from './pages/MediaDetailPage.tsx';
import {
  createBrowserRouter,
  RouterProvider,
} from 'react-router-dom';
import { LoginPage } from './pages/LoginPage.tsx';

const router = createBrowserRouter([
  {
    path: '/',
    element: <MainContent />,
  },
  {
    path: '/detail/:providerName',
    element: <MediaDetailPage />,
  },
  {
    path: '/account/login',
    element: <LoginPage />,
  },
]);

function App() {
  return (
    <>
      <Box id={'app-view'} maxW={'100vw'} maxH={'100vh'} overflowY={'hidden'}>
        <TopToolBar id={'top-tool-bar'} />
        <RouterProvider router={router} />
      </Box>
    </>
  );
}

export default App;
