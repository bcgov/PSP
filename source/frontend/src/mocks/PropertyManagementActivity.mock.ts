import { Api_PropertyActivity } from '@/models/api/PropertyActivity';

const emptyActivity: Api_PropertyActivity = {
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
  requestAddedDateTime: '',
  completionDateTime: null,
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
  rowVersion: 0,
};
export const mockGetPropertyManagementActivity = (
  id: number = 1,
  propertyId: number = 1,
): Api_PropertyActivity => ({
  ...emptyActivity,
  id: 200,
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
  requestAddedDateTime: '2023-10-17T00:00:00',
  rowVersion: 1,
  activityProperties: [
    {
      id: id,
      propertyActivityId: 200,
      propertyActivityModel: null,
      propertyId: propertyId,
      property: null,
      rowVersion: 1,
    },
  ],
});

export const mockGetPropertyManagementActivityNotStarted = (
  id: number = 1,
  propertyId: number = 1,
): Api_PropertyActivity => ({
  ...emptyActivity,
  id: 200,
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
  requestAddedDateTime: '2023-10-17T00:00:00',
  rowVersion: 1,
  activityProperties: [
    {
      id: id,
      propertyActivityId: 200,
      propertyActivityModel: null,
      propertyId: propertyId,
      property: null,
      rowVersion: 1,
    },
  ],
});

export const mockGetPropertyManagementActivityList = (): Api_PropertyActivity[] => [
  {
    ...emptyActivity,
    id: 1,
  },
  {
    ...emptyActivity,
    id: 2,
  },
];
