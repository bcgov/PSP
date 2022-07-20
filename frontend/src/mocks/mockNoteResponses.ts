import { Api_Note } from 'models/api/Note';

export const mockNotesResponse = (): Api_Note[] => {
  return [
    { note: 'Note 1', appCreateTimestamp: '10-Jan-2022', appLastUpdateUserid: 'test user1', id: 1 },
    { note: 'Note 2', appCreateTimestamp: '10-Jan-2022', appLastUpdateUserid: 'test user2', id: 2 },
    { note: 'Note 3', appCreateTimestamp: '10-Jan-2022', appLastUpdateUserid: 'test user4', id: 3 },
    { note: 'Note 4', appCreateTimestamp: '20-Jan-2022', appLastUpdateUserid: 'test user2', id: 4 },
  ];
};
