import { Api_AcquisitionFile } from 'models/api/AcquisitionFile';

export const mockAcquisitionFileResponse = (
  id = 1,
  name = 'Test ACQ File',
  rowVersion = 1,
): Api_AcquisitionFile => ({
  id,
  rowVersion,
  fileNumber: '1-12345-01',
  fileName: name,
  ministryProjectNumber: '001',
  ministryProjectName: 'Hwy 14 improvements',
  assignedDate: '2022-06-27T00:00:00',
  deliveryDate: '2022-07-29T00:00:00',
  fileStatusTypeCode: {
    id: 'ACTIVE',
    description: 'Active',
    isDisabled: false,
  },
  acquisitionTypeCode: {
    id: 'CONSEN',
    description: 'Consensual Agreement',
    isDisabled: false,
  },
  regionCode: {
    id: 1,
    description: 'South Coast Region',
    isDisabled: false,
  },
  appCreateTimestamp: '2022-05-28T00:57:37.42',
  appLastUpdateTimestamp: '2022-07-28T00:57:37.42',
  appLastUpdateUserid: 'admin',
  appCreateUserid: 'admin',
  appLastUpdateUserGuid: '14c9a273-6f4a-4859-8d59-9264d3cee53f',
  appCreateUserGuid: '14c9a273-6f4a-4859-8d59-9264d3cee53f',
});
