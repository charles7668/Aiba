import ReactDOM from 'react-dom/client';
import App from './App.tsx';
import './index.css';
import { ChakraProvider } from '@chakra-ui/react';
import { AuthProvider } from './components/AuthProvider.tsx';

ReactDOM.createRoot(document.getElementById('root')!).render(
  <AuthProvider>
    <ChakraProvider>
      <App />
    </ChakraProvider>
  </AuthProvider>
);
