import { Api_ConcurrentVersion_Null } from './ConcurrentVersion';
import { DateOnly } from './DateOnly';
import { Api_Organization } from './Organization';
import { Api_Person } from './Person';
import { Api_Property } from './Property';
import Api_TypeCode from './TypeCode';

export interface Api_PropertyActivity extends Api_ConcurrentVersion_Null {
  id: number;
  activityTypeCode: Api_TypeCode<string>;
  activitySubtypeCode: Api_TypeCode<string>;
  activityStatusTypeCode: Api_TypeCode<string>;
  requestAddedDateOnly: DateOnly;
  completionDateOnly: DateOnly | null;
  description: string;
  requestSource: string;
  pretaxAmt: number | null;
  gstAmt: number | null;
  pstAmt: number | null;
  totalAmt: number | null;
  isDisabled: boolean | null;
  serviceProviderOrgId: number | null;
  serviceProviderOrg: Api_Organization | null;
  serviceProviderPersonId: number | null;
  serviceProviderPerson: Api_Person | null;
  involvedParties: Api_PropertyActivityInvolvedParty[];
  ministryContacts: Api_PropertyMinistryContact[];
  activityProperties: Api_PropertyActivityProperty[];
  invoices: Api_PropertyActivityInvoice[];
}

export interface Api_PropertyActivityInvolvedParty extends Api_ConcurrentVersion_Null {
  id: number;
  organizationId: number | null;
  organization: Api_Organization | null;
  personId: number | null;
  person: Api_Person | null;
  propertyActivityId: number;
  propertyActivity: Api_PropertyActivity | null;
}
export interface Api_PropertyMinistryContact extends Api_ConcurrentVersion_Null {
  id: number;
  personId: number;
  person: Api_Person | null;
  propertyActivityId: number;
  pimsPropertyActivity: Api_PropertyActivity | null;
}

export interface Api_PropertyActivityProperty extends Api_ConcurrentVersion_Null {
  id: number;
  propertyActivityId: number;
  propertyActivityModel: Api_PropertyActivity | null;
  propertyId: number;
  property: Api_Property | null;
}

export interface Api_PropertyActivityInvoice extends Api_ConcurrentVersion_Null {
  id: number;
  invoiceDateTime: string;
  invoiceNum: string;
  description: string;
  pretaxAmount: number;
  gstAmount: number | null;
  pstAmount: number | null;
  totalAmount: number | null;
  isPstRequired: boolean | null;
  isDisabled: boolean | null;
  propertyActivityId: number;
  propertyActivity: Api_PropertyActivity | null;
}

export interface Api_PropertyActivitySubtype {
  typeCode: string;
  parentTypeCode: string;
  description: string;
  isDisabled: boolean | null;
}
