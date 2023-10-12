import { Api_LastUpdatedBy } from '@/models/api/File';

export const mockLastUpdatedBy: (parentId: number) => Api_LastUpdatedBy = (parentId: number) => ({
  parentId,
  appLastUpdateUserid: 'MARODRIG',
  appLastUpdateUserGuid: '123123-14f0-477c-a7fb-sdfwerwea',
  appLastUpdateTimestamp: '2023-10-06T22:48:17.06',
});
