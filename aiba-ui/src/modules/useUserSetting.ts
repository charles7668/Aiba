import { createContext, useContext } from 'react';
import { UserSetting } from '../models/UserSetting.ts';

export interface UserSettingContextType {
  userSetting: UserSetting | undefined;
  setUserSetting: (userSetting: UserSetting) => void;
}

export const UserSettingContext = createContext<
  UserSettingContextType | undefined
>(undefined);

export const useUserSetting = () => {
  const context = useContext(UserSettingContext);
  if (context === undefined) {
    throw new Error(
      'useUserSetting must be used within an UserSettingProvider'
    );
  }
  return context;
};
