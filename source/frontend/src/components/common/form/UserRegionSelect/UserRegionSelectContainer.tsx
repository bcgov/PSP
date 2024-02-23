import * as React from 'react';
import { useEffect } from 'react';

import * as API from '@/constants/API';
import { useUserInfoRepository } from '@/hooks/repositories/useUserInfoRepository';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { exists } from '@/utils';

import { Select, SelectProps } from '../Select';

export interface IUserRegionSelectContainerProps {
  field: string;
  includeAll?: boolean;
}

/** display a list of all regions filtered by the current user's regions. */
export const UserRegionSelectContainer: React.FunctionComponent<
  IUserRegionSelectContainerProps & Partial<SelectProps>
> = ({ field, ...rest }) => {
  const { getOptionsByType } = useLookupCodeHelpers();
  const { obj } = useKeycloakWrapper();
  const { idir_user_guid } = obj.userInfo;
  const formattedGuid = idir_user_guid?.replace(
    /(.{8})(.{4})(.{4})(.{4})(.{12})/,
    '$1-$2-$3-$4-$5',
  );
  const { retrieveUserInfo, retrieveUserInfoResponse } = useUserInfoRepository();
  const regionTypes = getOptionsByType(API.REGION_TYPES);
  const userRegionCodes =
    retrieveUserInfoResponse?.userRegions?.map(ur => ur.regionCode?.toString()).filter(exists) ??
    [];
  const userRegionTypes = regionTypes.filter(r => userRegionCodes?.includes(r.code ?? ''));

  useEffect(() => {
    formattedGuid && retrieveUserInfo(formattedGuid);
  }, [formattedGuid, retrieveUserInfo]);

  if (!regionTypes?.length) {
    return (
      <Select
        {...rest}
        tooltip={`Unable to load regions, contact your administrator if this error persists.`}
        options={[]}
        field={field}
      ></Select>
    );
  }
  if (rest.includeAll) {
    userRegionTypes.unshift({ label: 'All Regions', value: '' });
  }

  return (
    <Select
      {...rest}
      options={userRegionTypes}
      field={field}
      className="d-flex"
      tooltip={
        !userRegionTypes?.length
          ? `You aren't associated to any regions! ask an administrator to add you to one or more regions.`
          : ''
      }
    ></Select>
  );
};
