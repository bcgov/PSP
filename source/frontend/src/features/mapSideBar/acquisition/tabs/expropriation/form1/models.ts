import { IContactSearchResult } from '@/interfaces';
import { Api_AcquisitionFileProperty } from '@/models/api/AcquisitionFile';
import { Api_Organization } from '@/models/api/Organization';

export class ExpropriationForm1Model {
  organizationId: number | null = null;
  organization: Api_Organization | null = null;
  expropriationAuthorityContact: IContactSearchResult | null = null;
  impactedProperties: Api_AcquisitionFileProperty[] = [];
  landInterest: string = '';
  purpose: string = '';
}
