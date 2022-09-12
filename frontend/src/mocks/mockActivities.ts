import { Api_Activity } from 'models/api/Activity';

export const mockActivitiesResponse = (): Api_Activity[] => [
  {
    activityTemplate: {
      activityTemplateTypeCode: {
        id: 'SURVEY',
        description: 'Survey',
      },
    },
    id: 1,
    activityStatusTypeCode: {
      id: 'DRAFT',
      description: 'Draft',
    },
    description: 'Survey Activity',
    activityDataJson: '',
  },
  {
    activityTemplate: {
      activityTemplateTypeCode: {
        id: 'GENERAL',
        description: 'General',
      },
    },
    id: 2,
    activityStatusTypeCode: {
      id: 'DRAFT',
      description: 'Draft',
    },
    description: 'General Activity',
    activityDataJson: '',
  },
  {
    activityTemplate: {
      activityTemplateTypeCode: {
        id: 'SITEVIS',
        description: 'Site Visit',
      },
    },
    id: 3,
    activityStatusTypeCode: {
      id: 'DRAFT',
      description: 'Draft',
    },
    description: 'Site Visit Activity',
    activityDataJson: '',
  },
];

export const getMockActivityResponse: () => Api_Activity = () => ({
  id: 2,
  activityTemplateId: 1,
  activityTemplateTypeCode: {
    id: 'GENERAL',
    description: 'General',
    isDisabled: false,
  },
  activityStatusTypeCode: {
    id: 'NOSTART',
    description: 'Not Started',
    isDisabled: false,
  },
  activityTemplate: {
    id: 1,
    activityTemplateTypeCode: {
      id: 'GENERAL',
      description: 'General',
      isDisabled: false,
    },
    appCreateTimestamp: '2022-08-13T23:42:04.923',
    appLastUpdateTimestamp: '2022-08-13T23:42:04.923',
    appLastUpdateUserid: 'dbo',
    appCreateUserid: 'dbo',
    rowVersion: 1,
  },
  appCreateTimestamp: '0001-01-01T00:00:00',
  appLastUpdateTimestamp: '0001-02-01T00:00:00',
  rowVersion: 0,
  description: 'test description',
  activityDataJson: '{}',
});
