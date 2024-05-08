import noop from 'lodash/noop';
import { FunctionComponent, useEffect, useState } from 'react';

import { useApiUsers } from '@/hooks/pims-api/useApiUsers';
import useIsMounted from '@/hooks/util/useIsMounted';

import TooltipIcon from './TooltipIcon';

export interface IUserNameTooltipProps {
  userGuid?: string | null;
  userName?: string | null;
}

/** Generic user info tooltip component that displays user name on hover */
export const UserNameTooltip: FunctionComponent<IUserNameTooltipProps> = ({
  userGuid,
  userName,
}) => {
  const isMounted = useIsMounted();
  const { getUserInfo } = useApiUsers();
  const [userNameInfo, setUserNameInfo] = useState<string>('');
  useEffect(() => {
    if (userGuid) {
      getUserInfo(userGuid)
        .then(({ data }) => {
          if (data && isMounted()) {
            const firstName = data?.person?.firstName;
            const middleNames = data?.person?.middleNames;
            const surname = data?.person?.surname;
            const nameArr: string[] = [];
            if (firstName) nameArr.push(firstName);
            if (middleNames) nameArr.push(middleNames);
            if (surname) nameArr.push(surname);
            setUserNameInfo(nameArr.join(' '));
          }
        })
        .catch(noop);
    }
  }, [userGuid, isMounted, getUserInfo]);

  return (
    <TooltipIcon
      toolTipId={'userNameTooltip'}
      innerClassName={'userNameTooltip'}
      toolTip={userNameInfo}
      customToolTipIcon={<strong>{userName ?? 'USER'}</strong>}
    ></TooltipIcon>
  );
};
