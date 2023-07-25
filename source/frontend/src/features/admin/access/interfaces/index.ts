import { AccessRequestStatus } from '@/constants/accessStatus';

export interface IAccessRequestModel {
  id: number;
  userId: number;
  businessIdentifierValue: string;
  firstName: string;
  surname: string;
  email: string;
  position: string;
  status: AccessRequestStatus;
  role: string;
  note: string;
}
