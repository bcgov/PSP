import { ApiGen_Concepts_PropertyImprovement } from '@/models/api/generated/ApiGen_Concepts_PropertyImprovement';

export const getMockPropertyImprovementApi = (
  propertyId = 1,
): ApiGen_Concepts_PropertyImprovement => ({
  id: 1000,
  propertyId: propertyId,
  property: null,
  improvementDescription: 'TEST DESCRIPTION',
  propertyImprovementTypeCode: {
    id: 'COMMBLDG',
    description: 'Commercial Building',
    isDisabled: false,
    displayOrder: 1,
  },
  appCreateTimestamp: '2026-01-09T19:48:26.643',
  appLastUpdateTimestamp: '2026-01-09T19:48:26.643',
  appLastUpdateUserid: 'EHERRERA',
  appCreateUserid: 'EHERRERA',
  appLastUpdateUserGuid: '939a27d0-76cd-49b0-b474-53166adb73da',
  appCreateUserGuid: '939a27d0-76cd-49b0-b474-53166adb73da',
  rowVersion: 1,
});

export const getMockPropertyImprovementsApi = (
  propertyId = 1,
): ApiGen_Concepts_PropertyImprovement[] => {
  return [
    {
      id: 1000,
      propertyId: propertyId,
      property: null,
      improvementDescription: 'TEST DESCRIPTION',
      propertyImprovementTypeCode: {
        id: 'COMMBLDG',
        description: 'Commercial Building',
        isDisabled: false,
        displayOrder: 1,
      },
      appCreateTimestamp: '2026-01-09T19:48:26.643',
      appLastUpdateTimestamp: '2026-01-09T19:48:26.643',
      appLastUpdateUserid: 'EHERRERA',
      appCreateUserid: 'EHERRERA',
      appLastUpdateUserGuid: '939a27d0-76cd-49b0-b474-53166adb73da',
      appCreateUserGuid: '939a27d0-76cd-49b0-b474-53166adb73da',
      rowVersion: 1,
    },
  ];
};
