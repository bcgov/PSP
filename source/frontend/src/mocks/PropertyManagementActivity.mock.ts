import { ApiGen_Concepts_PropertyActivity } from '@/models/api/generated/ApiGen_Concepts_PropertyActivity';
import { EpochIsoDateTime } from '@/models/api/UtcIsoDateTime';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';

const emptyActivity: ApiGen_Concepts_PropertyActivity = {
  id: 0,
  activityTypeCode: {
    id: 'ACTIVITY-TYPE',
    description: 'Activity Type Description',
    isDisabled: false,
    displayOrder: 100,
  },
  activitySubtypeCode: {
    id: 'ACTIVITY-SUB-TYPE',
    description: 'Activity Type Description',
    isDisabled: false,
    displayOrder: 100,
  },
  activityStatusTypeCode: {
    id: 'ACTIVITY-STATUS-TYPE',
    description: 'Activity Type Description',
    isDisabled: false,
    displayOrder: 100,
  },
  requestAddedDateOnly: EpochIsoDateTime,
  completionDateOnly: null,
  description: '',
  requestSource: '',
  pretaxAmt: null,
  gstAmt: null,
  pstAmt: null,
  totalAmt: null,
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
export const mockGetPropertyManagementActivity = (
  id = 1,
  propertyId = 1,
): ApiGen_Concepts_PropertyActivity => ({
  ...emptyActivity,
  id: id,
  activityTypeCode: {
    id: 'ACTIVITY-TYPE',
    description: 'Activity Type Description',
    isDisabled: false,
    displayOrder: 100,
  },
  activitySubtypeCode: {
    id: 'ACTIVITYSUB-TYPE',
    description: 'Activity Sub-Type Description',
    isDisabled: false,
    displayOrder: 100,
  },
  activityStatusTypeCode: {
    id: 'ACTIVITY-STATUS',
    description: 'Activity Sub-Type Description',
    isDisabled: false,
    displayOrder: 100,
  },
  requestAddedDateOnly: '2023-10-17T00:00:00',
  rowVersion: 1,
  activityProperties: [
    {
      id: 15,
      propertyActivityId: 200,
      propertyActivity: null,
      propertyId: propertyId,
      property: null,
      ...getEmptyBaseAudit(1),
    },
  ],
});

export const mockGetPropertyManagementActivityNotStarted = (
  id = 1,
  propertyId = 1,
): ApiGen_Concepts_PropertyActivity => ({
  ...emptyActivity,
  id: id,
  activityTypeCode: {
    id: 'ACTIVITY-TYPE',
    description: 'Activity Type Description',
    isDisabled: false,
    displayOrder: 100,
  },
  activitySubtypeCode: {
    id: 'ACTIVITYSUB-TYPE',
    description: 'Activity Sub-Type Description',
    isDisabled: false,
    displayOrder: 100,
  },
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
      propertyActivityId: 200,
      propertyActivity: null,
      propertyId: propertyId,
      property: null,
      ...getEmptyBaseAudit(1),
    },
  ],
});

export const mockGetPropertyManagementActivityList = (): ApiGen_Concepts_PropertyActivity[] => [
  {
    ...emptyActivity,
    id: 1,
  },
  {
    ...emptyActivity,
    id: 2,
  },
];
