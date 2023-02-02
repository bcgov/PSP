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
  id: 20,
  projectStatusTypeCode: {
    id: 'AC',
    description: 'Active (AC)',
    isDisabled: false,
  },
  regionCode: {
    id: 0,
    code: '1',
    description: 'South Coast Region',
  },
  code: '771',
  description: 'Project Cool A',
  note: 'Summary of the Project Cool A',
  products: [
    {
      id: 48,
      acquisitionFiles: [],
      code: '70',
      description: 'Product A',
      startDate: '2023-02-01T00:00:00',
      costEstimate: 60.0,
      costEstimateDate: '2023-02-02T00:00:00',
      objective: 'Objective of Product A',
      scope: 'Scope of Product A',
      rowVersion: 1,
    },
    {
      id: 49,
      acquisitionFiles: [],
      code: '71',
      description: 'Product B',
      startDate: '2023-02-03T00:00:00',
      costEstimate: 61.0,
      costEstimateDate: '2023-02-04T00:00:00',
      objective: 'Objective of Product B',
      scope: 'Scope of Product B',
      rowVersion: 1,
    },
  ],
  appCreateTimestamp: '2023-02-01T00:48:16.987',
  appLastUpdateTimestamp: '2023-02-01T00:48:16.987',
  appLastUpdateUserid: 'USER_A',
  appCreateUserid: 'USER_B',
  appLastUpdateUserGuid: '77777777-7777-7777-7777-777777777777',
  appCreateUserGuid: '77777777-7777-7777-7777-777777777777',
  rowVersion: 1,
});
