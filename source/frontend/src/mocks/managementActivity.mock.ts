import { ApiGen_Concepts_ManagementActivity } from '@/models/api/generated/ApiGen_Concepts_ManagementActivity';
import { EpochIsoDateTime } from '@/models/api/UtcIsoDateTime';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';

const emptyActivity: ApiGen_Concepts_ManagementActivity = {
  id: 0,
  managementFileId: null,
  managementFile: null,
  activityTypeCode: {
    id: 'ACTIVITY-TYPE',
    description: 'Activity Type Description',
    isDisabled: false,
    displayOrder: 100,
  },
  activitySubTypeCodes: [],
  activityStatusTypeCode: {
    id: 'ACTIVITY-STATUS-TYPE',
    description: 'Activity Type Description',
    isDisabled: false,
    displayOrder: 100,
  },
  requestAddedDateOnly: EpochIsoDateTime,
  requestorPersonId: null,
  requestorOrganizationId: null,
  requestorPrimaryContactId: null,
  completionDateOnly: null,
  description: '',
  requestSource: '',
  isDisabled: false,
  serviceProviderOrgId: null,
  serviceProviderOrg: null,
  serviceProviderPersonId: null,
  serviceProviderPerson: null,
  involvedParties: [],
  ministryContacts: [],
  activityProperties: [],
  invoices: [],
  ...getEmptyBaseAudit(0),
};

export const getMockPropertyManagementActivity = (
  id = 1,
  propertyId = 1,
): ApiGen_Concepts_ManagementActivity => ({
  ...emptyActivity,
  id: id,
  activityTypeCode: {
    id: 'APPLICPERMIT',
    description: 'Applications/Permits',
    isDisabled: false,
    displayOrder: 1,
  },
  activitySubTypeCodes: [
    {
      id: 100,
      managementActivityId: 1,
      managementActivitySubtypeCode: {
        id: 'ACCESS',
        description: 'Access',
        isDisabled: false,
        displayOrder: 1,
      },
      ...getEmptyBaseAudit(1),
    },
  ],
  activityStatusTypeCode: {
    id: 'NOTSTARTED',
    description: 'Not started',
    isDisabled: false,
    displayOrder: 100,
  },
  requestAddedDateOnly: '2023-10-17T00:00:00',
  completionDateOnly: null,
  rowVersion: 1,
  activityProperties: [
    {
      id: 15,
      managementActivityId: 1,
      managementActivity: null,
      propertyId: propertyId,
      property: null,
      ...getEmptyBaseAudit(1),
    },
  ],
});

export const mockGetPropertyManagementActivityNotStarted = (
  id = 1,
  propertyId = 1,
): ApiGen_Concepts_ManagementActivity => ({
  ...emptyActivity,
  id: id,
  activityTypeCode: {
    id: 'APPLICPERMIT',
    description: 'Applications/Permits',
    isDisabled: false,
    displayOrder: 1,
  },
  activitySubTypeCodes: [],
  activityStatusTypeCode: {
    id: 'NOTSTARTED',
    description: 'Not started',
    isDisabled: false,
    displayOrder: 100,
  },
  requestAddedDateOnly: '2023-10-17T00:00:00',
  rowVersion: 1,
  activityProperties: [
    {
      id: 73,
      managementActivityId: 200,
      managementActivity: null,
      propertyId: propertyId,
      property: null,
      ...getEmptyBaseAudit(1),
    },
  ],
});

export const mockGetManagementActivityList = (): ApiGen_Concepts_ManagementActivity[] => [
  {
    ...emptyActivity,
    id: 1,
  },
  {
    ...emptyActivity,
    id: 2,
  },
];
