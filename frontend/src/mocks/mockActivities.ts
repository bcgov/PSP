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
  activityDataJson: '{"version": "1.0"}',
  actInstPropAcqFiles: [
    {
      id: 23,
      activityId: 2,
      propertyFileId: 1,
      appCreateTimestamp: '2022-09-10T00:09:43.37',
      appLastUpdateTimestamp: '2022-09-10T00:09:43.37',
      appLastUpdateUserid: 'admin',
      appCreateUserid: 'admin',
      appLastUpdateUserGuid: '14c9a273-6f4a-4859-8d59-9264d3cee53f',
      appCreateUserGuid: '14c9a273-6f4a-4859-8d59-9264d3cee53f',
      rowVersion: 1,
    },
    {
      id: 24,
      activityId: 2,
      propertyFileId: 2,
      appCreateTimestamp: '2022-09-10T00:09:43.37',
      appLastUpdateTimestamp: '2022-09-10T00:09:43.37',
      appLastUpdateUserid: 'admin',
      appCreateUserid: 'admin',
      appLastUpdateUserGuid: '14c9a273-6f4a-4859-8d59-9264d3cee53f',
      appCreateUserGuid: '14c9a273-6f4a-4859-8d59-9264d3cee53f',
      rowVersion: 1,
    },
  ],
  actInstPropRsrchFiles: [
    {
      id: 23,
      activityId: 2,
      propertyFileId: 2,
      appCreateTimestamp: '2022-09-10T00:09:43.37',
      appLastUpdateTimestamp: '2022-09-10T00:09:43.37',
      appLastUpdateUserid: 'admin',
      appCreateUserid: 'admin',
      appLastUpdateUserGuid: '14c9a273-6f4a-4859-8d59-9264d3cee53f',
      appCreateUserGuid: '14c9a273-6f4a-4859-8d59-9264d3cee53f',
      rowVersion: 1,
    },
    {
      id: 24,
      activityId: 2,
      propertyFileId: 1,
      appCreateTimestamp: '2022-09-10T00:09:43.37',
      appLastUpdateTimestamp: '2022-09-10T00:09:43.37',
      appLastUpdateUserid: 'admin',
      appCreateUserid: 'admin',
      appLastUpdateUserGuid: '14c9a273-6f4a-4859-8d59-9264d3cee53f',
      appCreateUserGuid: '14c9a273-6f4a-4859-8d59-9264d3cee53f',
      rowVersion: 1,
    },
  ],
});
