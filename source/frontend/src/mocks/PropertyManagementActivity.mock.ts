import { Api_PropertyManagementActivity } from '@/models/api/Property';

export const mockGetPropertyManagementActivity = (
  id: number = 1,
  propertyId: number = 1,
): Api_PropertyManagementActivity => ({
  id,
  propertyId,
  propertyActivityId: 200,
  activity: {
    id: 200,
    activityTypeCode: 'ACTIVITY-TYPE',
    activityType: {
      id: 'ACTIVITY-TYPE',
      description: 'Activity Type Description',
      isDisabled: false,
      displayOrder: 100,
    },
    activitySubTypeCode: 'ACTIVITYSUB-TYPE',
    activitySubType: {
      id: 'ACTIVITYSUB-TYPE',
      description: 'Activity Sub-Type Description',
      isDisabled: false,
      displayOrder: 100,
    },
    activityStatusTypeCode: 'ACTIVITY-STATUS',
    activityStatusType: {
      id: 'ACTIVITY-STATUS',
      description: 'Activity Sub-Type Description',
      isDisabled: false,
      displayOrder: 100,
    },
    requestedAddedDate: '2023-10-17T00:00:00',
    rowVersion: 1,
  },
  rowVersion: 1,
});

export const mockGetPropertyManagementActivityList = (): Api_PropertyManagementActivity[] => [
  {
    id: 1,
    propertyId: 1,
    propertyActivityId: 200,
    activity: {
      id: 200,
      activityTypeCode: 'ACTIVITY-TYPE',
      activityType: {
        id: 'ACTIVITY-TYPE',
        description: 'Activity Type Description',
        isDisabled: false,
        displayOrder: 100,
      },
      activitySubTypeCode: 'ACTIVITYSUB-TYPE',
      activitySubType: {
        id: 'ACTIVITYSUB-TYPE',
        description: 'Activity Sub-Type Description',
        isDisabled: false,
        displayOrder: 100,
      },
      activityStatusTypeCode: 'ACTIVITY-STATUS',
      activityStatusType: {
        id: 'ACTIVITY-STATUS',
        description: 'Activity Sub-Type Description',
        isDisabled: false,
        displayOrder: 100,
      },
      requestedAddedDate: '2023-10-17T00:00:00',
      rowVersion: 1,
    },
    rowVersion: 1,
  },
  {
    id: 2,
    propertyId: 1,
    propertyActivityId: 201,
    activity: {
      id: 201,
      activityTypeCode: 'ACTIVITY-TYPE',
      activityType: {
        id: 'ACTIVITY-TYPE',
        description: 'Activity Type Description',
        isDisabled: false,
        displayOrder: 100,
      },
      activitySubTypeCode: 'ACTIVITYSUB-TYPE',
      activitySubType: {
        id: 'ACTIVITYSUB-TYPE',
        description: 'Activity Sub-Type Description',
        isDisabled: false,
        displayOrder: 100,
      },
      activityStatusTypeCode: 'ACTIVITY-STATUS',
      activityStatusType: {
        id: 'ACTIVITY-STATUS',
        description: 'Activity Sub-Type Description',
        isDisabled: false,
        displayOrder: 100,
      },
      requestedAddedDate: '2023-10-18T00:00:00',
      rowVersion: 1,
    },
    rowVersion: 1,
  },
];
