import { useApiUsers } from 'hooks/pims-api/useApiUsers';
import useIsMounted from 'hooks/useIsMounted';
import * as React from 'react';
import styled from 'styled-components';

import TooltipIcon from './TooltipIcon';

export interface IUserNameTooltipProps {
  userGuid?: string;
  userName?: string;
}

/** Generic user info tooltip component that displays user name on hover */
export const UserNameTooltip: React.FunctionComponent<
  React.PropsWithChildren<IUserNameTooltipProps>
> = ({ userGuid, userName }) => {
  const isMounted = useIsMounted();
  const { getUserInfo } = useApiUsers();
  const [userNameInfo, setUserNameInfo] = React.useState<string>('');
  React.useEffect(() => {
    if (userGuid) {
      getUserInfo(userGuid).then(({ data }) => {
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
      });
    }
  }, [userGuid, isMounted, getUserInfo]);

  return (
    <TooltipIcon
      toolTipId={'userNameTooltip'}
      innerClassName={'userNameTooltip'}
      toolTip={userNameInfo}
      customToolTipIcon={<StyledUserLabel>{userName ?? 'USER'}</StyledUserLabel>}
    ></TooltipIcon>
  );
};

const StyledUserLabel = styled.span`
  font-weight: bold;
  color: ${({ theme }) => theme.css.secondaryVariantColor};
`;
