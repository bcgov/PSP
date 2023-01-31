import { Api_Project } from 'models/api/Project';

export const mockProjects = (): Api_Project[] => [
  {
    id: 1,
    businessFunctionCode: { id: 1, code: '13' },
    costTypeCode: { id: 1, code: '13' },
    workActivityCode: { id: 1, code: '11' },
    regionCode: {
      id: 1,
      description: 'South Coast Region',
    },
    code: '776',
    description: 'test DESCRIPTION 1',
    note: 'test NOTE 1',
    rowVersion: 1,
  },
  {
    id: 2,
    businessFunctionCode: { id: 1, code: '12' },
    costTypeCode: { id: 1, code: '13' },
    workActivityCode: { id: 1, code: '11' },
    regionCode: {
      id: 2,
      description: 'Southern Interior Region',
    },
    code: '777',
    description: 'test DESCRIPTION 2',
    note: 'test NOTE 2',
    rowVersion: 1,
  },
];

export const mockProjectPostResponse = (
  rowVersion: number = 1,
  description: string = 'TRANS-CANADA HWY - 10',
  code: string,
  regionCodeId: number = 1,
  statusCode: string = 'AC',
  summary: string = 'NEW PROJECT',
): Api_Project => ({
  id: 1,
  rowVersion: rowVersion,
  code: code,
  description: description,
  regionCode: {
    id: regionCodeId,
    description: 'REGION',
  },
  projectStatusTypeCode: {
    id: statusCode,
    description: 'ACTIVE (AC)',
  },
  businessFunctionCode: undefined,
  workActivityCode: undefined,
  note: summary,
  appCreateTimestamp: '2022-05-28T00:57:37.42',
  appLastUpdateTimestamp: '2022-07-28T00:57:37.42',
  appLastUpdateUserid: 'admin',
  appCreateUserid: 'admin',
  appLastUpdateUserGuid: '14c9a273-6f4a-4859-8d59-9264d3cee53f',
  appCreateUserGuid: '14c9a273-6f4a-4859-8d59-9264d3cee53f',
});

export const mockProjectGetResponse = (): Api_Project => ({
  id: 1,
  rowVersion: 1,
  code: '9999',
  description: 'Project Description',
  regionCode: {
    id: 2,
    description: 'Southern Interior Region',
  },
  projectStatusTypeCode: {
    description: 'ACTIVE',
    id: 'AC',
  },
  businessFunctionCode: undefined,
  workActivityCode: undefined,
  note: 'NOTE VALUE',
  appCreateTimestamp: '2022-05-28T00:57:37.42',
  appLastUpdateTimestamp: '2022-07-28T00:57:37.42',
  appLastUpdateUserid: 'admin',
  appCreateUserid: 'admin',
  appLastUpdateUserGuid: '14c9a273-6f4a-4859-8d59-9264d3cee53f',
  appCreateUserGuid: '14c9a273-6f4a-4859-8d59-9264d3cee53f',
});
