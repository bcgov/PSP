import { ApiGen_Concepts_ConsultationLease } from '@/models/api/generated/ApiGen_Concepts_ConsultationLease';

import { getEmptyPerson } from './contacts.mock';
import { getMockApiLease } from './lease.mock';

export const getMockApiConsultation = (): ApiGen_Concepts_ConsultationLease => ({
  id: 4,
  rowVersion: 2,
  leaseId: 1,
  personId: 1,
  lease: getMockApiLease(),
  person: getEmptyPerson(),
  organizationId: null,
  organization: null,
  primaryContactId: null,
  primaryContact: null,
  consultationTypeCode: {
    id: 'DISTRICT',
    description: 'Active',
    isDisabled: false,
    displayOrder: 10,
  },
  consultationStatusTypeCode: {
    id: 'REQCOMP',
    description: 'Required',
    isDisabled: false,
    displayOrder: 10,
  },
  consultationOutcomeTypeCode: {
    id: 'APPRDENIED',
    description: 'Approval Denied',
    isDisabled: false,
    displayOrder: 10,
  },
  otherDescription: null,
  requestedOn: '2024-01-01',
  isResponseReceived: true,
  responseReceivedDate: '2024-12-01',
  comment: 'test comment',
  appCreateTimestamp: '2024-02-06T20:56:46.47',
  appLastUpdateTimestamp: '2024-02-06T20:56:46.47',
  appLastUpdateUserid: 'dbo',
  appLastUpdateUserGuid: 'dbo',
  appCreateUserid: 'dbo',
  appCreateUserGuid: 'dbo',
});
