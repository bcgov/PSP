import { Api_Project } from 'models/api/Project';

export const mockProjects = (): Api_Project[] => [
  {
    id: 1,
    businessFunctionCode: 13,
    costTypeCode: 13,
    workActivityCode: 11,
    regionCode: 1,
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
    regionCode: 3,
    code: 777,
    description: 'test DESCRIPTION 2',
    note: 'test NOTE 2',
    rowVersion: 1,
  },
];
