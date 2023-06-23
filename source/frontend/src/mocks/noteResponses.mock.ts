import { Api_EntityNote, Api_Note } from '@/models/api/Note';

export const mockNotesResponse = (): Api_Note[] => {
  return [
    {
      note: 'Note 1',
      appCreateTimestamp: '2022-07-20T20:06:10.743',
      appLastUpdateUserid: 'test user1',
      id: 1,
    },
    {
      note: 'Note 2',
      appCreateTimestamp: '2022-07-21T20:06:10.743',
      appLastUpdateUserid: 'test user2',
      id: 2,
    },
    {
      note: 'Note 3',
      appCreateTimestamp: '2022-07-22T20:06:10.743',
      appLastUpdateUserid: 'test user4',
      id: 3,
    },
    {
      note: 'Note 4',
      appCreateTimestamp: '2022-07-23T20:06:10.743',
      appLastUpdateUserid: 'test user2',
      id: 4,
    },
  ];
};

export const mockEntityNote = (
  id: number | undefined = undefined,
  parentId = 1,
  note = 'Test Note',
): Api_EntityNote => ({
  id,
  parent: {
    id: parentId,
  },
  note: {
    note,
    appCreateTimestamp: '2022-07-20T20:06:10.743',
    appLastUpdateUserid: 'test user1',
  },
});

export const mockNoteResponse = (id = 1, note = 'Test Note', rowVersion = 1): Api_Note => ({
  id,
  note,
  rowVersion,
  appCreateTimestamp: '2022-07-20T20:06:10.743',
  appLastUpdateTimestamp: '2022-07-20T20:06:10.743',
  appLastUpdateUserid: 'admin',
  appCreateUserid: 'admin',
  appLastUpdateUserGuid: '14c9a273-6f4a-4859-8d59-9264d3cee53f',
  appCreateUserGuid: '14c9a273-6f4a-4859-8d59-9264d3cee53f',
});
