import { IContactSearchResult } from '@/interfaces';
import { Api_AcquisitionFileProperty } from '@/models/api/AcquisitionFile';
import { Api_Organization } from '@/models/api/Organization';

export class Form1Model {}

export class ExpropriationForm1Model {
  expropriationAuthority = new ExpropriationAuthorityModel();
  impactedProperties: Api_AcquisitionFileProperty[] = [];
  landInterest: string = '';
  purpose: string = '';
}

export class ExpropriationAuthorityModel {
  organizationId: number | null = null;
  organization: Api_Organization | null = null;
  contact: IContactSearchResult | null = null;
}
