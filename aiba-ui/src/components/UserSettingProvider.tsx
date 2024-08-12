import React, { useState, ReactNode, useEffect } from 'react';
import { UserSettingContext } from '../modules/useUserSetting.ts';
import { UserSetting } from '../models/UserSetting.ts';
import { Api } from '../services/Api.ts';

export const UserSettingProvider: React.FC<{ children: ReactNode }> = ({
  children,
}) => {
  const [userSetting, setUserSetting] = useState<UserSetting | undefined>();
  const [isFailed, setIsFailed] = useState<boolean>(false);
  useEffect(() => {
    Api.getUserSetting().then((response) => {
      if (response.status !== 200) {
        setIsFailed(true);
        return;
      }
      response.json().then((data) => {
        setUserSetting(data);
      });
      setIsFailed(false);
    });
  }, []);

  if (isFailed)
    return <div>Failed to load user settings. Please try again later.</div>;
  return (
    <UserSettingContext.Provider value={{ userSetting, setUserSetting }}>
      {children}
    </UserSettingContext.Provider>
  );
};
