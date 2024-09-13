import React, { useState, ReactNode, useEffect } from 'react';
import { UserSettingContext } from '../modules/useUserSetting.ts';
import { UserSetting } from '../models/UserSetting.ts';
import { Api } from '../services/Api.ts';

export const UserSettingProvider: React.FC<{ children: ReactNode }> = ({
  children,
}) => {
  const [userSetting, setUserSetting] = useState<UserSetting | undefined>();
  useEffect(() => {
    Api.getUserSetting().then((response) => {
      if (response.status !== 200) {
        setUserSetting(undefined);
        return;
      }
      response.json().then((data) => {
        setUserSetting(data);
      });
    });
  }, []);

  return (
    <UserSettingContext.Provider value={{ userSetting, setUserSetting }}>
      {children}
    </UserSettingContext.Provider>
  );
};
