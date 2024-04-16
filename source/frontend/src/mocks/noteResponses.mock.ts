import { ApiGen_Concepts_EntityNote } from '@/models/api/generated/ApiGen_Concepts_EntityNote';
import { ApiGen_Concepts_Note } from '@/models/api/generated/ApiGen_Concepts_Note';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';

export const mockNotesResponse = (): ApiGen_Concepts_Note[] => {
  return [
    {
      note: 'Note 1',
      isSystemGenerated: false,
      id: 1,
      ...getEmptyBaseAudit(),
      appCreateTimestamp: '2022-07-20T20:06:10.743',
      appLastUpdateUserid: 'test user1',
    },
    {
      ...getEmptyBaseAudit(),
      note: 'Note 2',
      isSystemGenerated: false,
      appCreateTimestamp: '2022-07-21T20:06:10.743',
      appLastUpdateUserid: 'test user2',
      id: 2,
    },
    {
      ...getEmptyBaseAudit(),
      note: 'Note 3',
      isSystemGenerated: false,
      appCreateTimestamp: '2022-07-22T20:06:10.743',
      appLastUpdateUserid: 'test user4',
      id: 3,
    },
    {
      ...getEmptyBaseAudit(),
      note: 'Note 4',
      isSystemGenerated: false,
      appCreateTimestamp: '2022-07-23T20:06:10.743',
      appLastUpdateUserid: 'test user2',
      id: 4,
    },
  ];
};

export const mockEntityNote = (
  id = 0,
  parentId = 1,
  note = 'Test Note',
): ApiGen_Concepts_EntityNote => ({
  id,
  parent: {
    id: parentId,
  },
  note: {
    ...getEmptyBaseAudit(),
    id: 0,
    note,
    isSystemGenerated: false,
    appCreateTimestamp: '2022-07-20T20:06:10.743',
    appLastUpdateUserid: 'test user1',
  },
  ...getEmptyBaseAudit(),
});

export const mockNoteResponse = (
  id = 1,
  note = 'Test Note',
  rowVersion = 1,
): ApiGen_Concepts_Note => ({
  id,
  note,
  isSystemGenerated: false,
  rowVersion,
  appCreateTimestamp: '2022-07-20T20:06:10.743',
  appLastUpdateTimestamp: '2022-07-20T20:06:10.743',
  appLastUpdateUserid: 'admin',
  appCreateUserid: 'admin',
  appLastUpdateUserGuid: '14c9a273-6f4a-4859-8d59-9264d3cee53f',
  appCreateUserGuid: '14c9a273-6f4a-4859-8d59-9264d3cee53f',
});
