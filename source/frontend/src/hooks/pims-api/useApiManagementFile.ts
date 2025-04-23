import React from 'react';

import { Api_LastUpdatedBy } from '@/models/api/File';
import { ApiGen_Concepts_ManagementFile } from '@/models/api/generated/ApiGen_Concepts_ManagementFile';
import { ApiGen_Concepts_ManagementFileProperty } from '@/models/api/generated/ApiGen_Concepts_ManagementFileProperty';
import { ApiGen_Concepts_ManagementFileTeam } from '@/models/api/generated/ApiGen_Concepts_ManagementFileTeam';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';

import useAxiosApi from './useApi';

/**
 * PIMS API wrapper to centralize all AJAX requests to the disposition file endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiManagementFile = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      putManagementFileProperties: (
        dspFile: ApiGen_Concepts_ManagementFile,
        userOverrideCodes: UserOverrideCode[] = [],
      ) =>
        api.put<ApiGen_Concepts_ManagementFile>(
          `/managementfiles/${dspFile?.id}/properties?${userOverrideCodes
            .map(o => `userOverrideCodes=${o}`)
            .join('&')}`,
          dspFile,
        ),
      postManagementFileApi: (
        managementFile: ApiGen_Concepts_ManagementFile,
        userOverrideCodes: UserOverrideCode[] = [],
      ) =>
        api.post<ApiGen_Concepts_ManagementFile>(
          `/managementfiles?${userOverrideCodes.map(o => `userOverrideCodes=${o}`).join('&')}`,
          managementFile,
        ),
      getManagementFile: (managementFileId: number) =>
        api.get<ApiGen_Concepts_ManagementFile>(`/managementfiles/${managementFileId}`),
      putManagementFileApi: (
        managementFileId: number,
        managementFile: ApiGen_Concepts_ManagementFile,
        userOverrideCodes: UserOverrideCode[] = [],
      ) =>
        api.put<ApiGen_Concepts_ManagementFile>(
          `/managementfiles/${managementFileId}?${userOverrideCodes
            .map(o => `userOverrideCodes=${o}`)
            .join('&')}`,
          managementFile,
        ),

      getLastUpdatedByApi: (managementFileId: number) =>
        api.get<Api_LastUpdatedBy>(`/managementfiles/${managementFileId}/updateInfo`),

      getManagementFileProperties: (managementFileId: number) =>
        api.get<ApiGen_Concepts_ManagementFileProperty[]>(
          `/managementfiles/${managementFileId}/properties`,
        ),
      getAllManagementFileTeamMembers: () =>
        api.get<ApiGen_Concepts_ManagementFileTeam[]>(`/managementfiles/team-members`),
    }),
    [api],
  );
};
