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
