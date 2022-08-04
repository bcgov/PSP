import { Api_Activity } from 'models/api/Activity';

export const mockActivitiesResponse = (): Api_Activity[] => [
  {
    activityTemplateTypeCode: {
      id: 'SURVEY',
      description: 'Survey',
    },
    id: 1,
    activityStatusTypeCode: {
      id: 'DRAFT',
      description: 'Draft',
    },
    description: 'Survey Activity',
  },
  {
    activityTemplateTypeCode: {
      id: 'GENERAL',
      description: 'General',
    },
    id: 2,
    activityStatusTypeCode: {
      id: 'DRAFT',
      description: 'Draft',
    },
    description: 'General Activity',
  },
  {
    activityTemplateTypeCode: {
      id: 'SITEVIS',
      description: 'Site Visit',
    },
    id: 3,
    activityStatusTypeCode: {
      id: 'DRAFT',
      description: 'Draft',
    },
    description: 'Site Visit Activity',
  },
];
