import { ApiGen_CodeTypes_AcquisitionTakeStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_AcquisitionTakeStatusTypes';
import { ApiGen_Concepts_Take } from '@/models/api/generated/ApiGen_Concepts_Take';
import { toTypeCodeNullable } from '@/utils/formUtils';

export const getMockApiTakes = (): ApiGen_Concepts_Take[] => [
  {
    id: 4,
    description: '',
    newHighwayDedicationArea: 4046.8564,
    isAcquiredForInventory: null,
    isThereSurplus: true,
    isNewLicenseToConstruct: true,
    isNewHighwayDedication: true,
    isNewLandAct: true,
    isNewInterestInSrw: true,
    isLeasePayable: true,
    licenseToConstructArea: 16187.426,
    landActArea: 12140.569,
    propertyAcquisitionFile: null,
    propertyAcquisitionFileId: 1,
    statutoryRightOfWayArea: 8093.713,
    surplusArea: 20234.281,
    leasePayableArea: 20231.281,
    takeSiteContamTypeCode: toTypeCodeNullable('UNK'),
    takeTypeCode: toTypeCodeNullable('PARTIAL'),
    takeStatusTypeCode: toTypeCodeNullable(
      ApiGen_CodeTypes_AcquisitionTakeStatusTypes.INPROGRESS.toString(),
    ),
    appCreateTimestamp: '2023-03-08T04:15:56.273',
    appLastUpdateTimestamp: '2023-03-08T05:51:09.953',
    appLastUpdateUserid: 'DESMITH',
    appCreateUserid: 'DESMITH',
    appLastUpdateUserGuid: '7db28007-0d47-4ef0-bb46-c365a4b95a73',
    appCreateUserGuid: '7db28007-0d47-4ef0-bb46-c365a4b95a73',
    areaUnitTypeCode: toTypeCodeNullable('M2'),
    ltcEndDt: '2020-01-01',
    landActEndDt: '2020-01-01',
    srwEndDt: '2022-11-20',
    leasePayableEndDt: '2022-11-21',
    landActTypeCode: {
      id: 'Section 15',
      description: 'Reserve',
      displayOrder: null,
      isDisabled: false,
    },
    completionDt: '',
    rowVersion: 2,
  },
];
