import { Api_Project } from 'models/api/Project';

export const mockProjects = (): Api_Project[] => [
  {
    id: 1,
    businessFunctionCode: 13,
    costTypeCode: 13,
    workActivityCode: 11,
    regionCode: {
      id: 1,
      description: 'South Coast Region',
    },
    code: 776,
    description: 'test DESCRIPTION 1',
    note: 'test NOTE 1',
    rowVersion: 1,
  },
  {
    id: 2,
    businessFunctionCode: 12,
    costTypeCode: 13,
    workActivityCode: 11,
    regionCode: {
      id: 2,
      description: 'Southern Interior Region',
    },
    code: 777,
    description: 'test DESCRIPTION 2',
    note: 'test NOTE 2',
    rowVersion: 1,
  },
];

export const mockProjectPostResponse = (
  id: number = 1,
  rowVersion = 1,
  description: string = 'TRANS-CANADA HWY - 10',
  regionCodeId: number = 1,
): Api_Project => ({
  id,
  rowVersion,
  code: undefined,
  description,
  regionCode: {
    id: regionCodeId,
    description: 'REGON',
  },
  projectStatusTypeCode: undefined,
  businessFunctionCode: null,
  workActivityCode: null,
  note: '',
  appCreateTimestamp: '2022-05-28T00:57:37.42',
  appLastUpdateTimestamp: '2022-07-28T00:57:37.42',
  appLastUpdateUserid: 'admin',
  appCreateUserid: 'admin',
  appLastUpdateUserGuid: '14c9a273-6f4a-4859-8d59-9264d3cee53f',
  appCreateUserGuid: '14c9a273-6f4a-4859-8d59-9264d3cee53f',
});
