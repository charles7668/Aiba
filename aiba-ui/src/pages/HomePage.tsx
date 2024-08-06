import React from 'react';
import { TopToolBar } from '../components/TopToolBar.tsx';

export const HomePage: React.FC = () => {
  return (
    <>
      <TopToolBar id={'top-tool-bar'} />
      <div>
        <h1>Home Page</h1>
      </div>
    </>
  );
};
