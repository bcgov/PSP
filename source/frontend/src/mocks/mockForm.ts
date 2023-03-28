import { Api_FileForm, Api_Form } from 'models/api/Form';
export const getMockApiForm = (): Api_Form => ({ name: 'H120' });
export const getMockApiFileForms = (): Api_FileForm[] => [
  {
    id: 2,
    fileId: 1,
    formTypeCode: {
      id: 'H179T',
      name: 'Offer agreement - Total (H179 T)',
      isDisabled: false,
      displayOrder: 0,
    },
    rowVersion: 1,
  },
  {
    id: 3,
    fileId: 1,
    formTypeCode: {
      id: 'H179A',
      name: 'Offer agreement - Section 3 (H179 A)',
      isDisabled: false,
      displayOrder: 0,
    },
    rowVersion: 1,
  },
  {
    id: 4,
    fileId: 1,
    formTypeCode: {
      id: 'H120',
      name: 'Payment requisition (H120)',
      isDisabled: false,
      displayOrder: 0,
    },
    rowVersion: 1,
  },
];
