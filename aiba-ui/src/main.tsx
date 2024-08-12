import ReactDOM from 'react-dom/client';
import App from './App.tsx';
import './index.css';
import { ChakraProvider, ColorModeScript, extendTheme } from '@chakra-ui/react';
import { AuthProvider } from './components/AuthProvider.tsx';
import { UserSettingProvider } from './components/UserSettingProvider.tsx';

const config = {
  initialColorMode: 'dark',
  useSystemColorMode: true,
};
const theme = extendTheme({ config });

ReactDOM.createRoot(document.getElementById('root')!).render(
  <AuthProvider>
    <UserSettingProvider>
      <ChakraProvider theme={theme}>
        <ColorModeScript initialColorMode={theme.config.initialColorMode} />
        <App />
      </ChakraProvider>
    </UserSettingProvider>
  </AuthProvider>
);
