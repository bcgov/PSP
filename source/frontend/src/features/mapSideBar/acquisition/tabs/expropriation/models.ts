import { IContactSearchResult } from '@/interfaces';
import { Api_AcquisitionFileProperty } from '@/models/api/AcquisitionFile';
import { Api_Organization } from '@/models/api/Organization';

export class ExpropriationAuthorityFormModel {
  organizationId: number | null = null;
  organization: Api_Organization | null = null;
  contact: IContactSearchResult | null = null;
}

class ExpropriationBaseModel {
  expropriationAuthority = new ExpropriationAuthorityFormModel();
}

export class ExpropriationForm1Model extends ExpropriationBaseModel {
  impactedProperties: Api_AcquisitionFileProperty[] = [];
  landInterest: string = '';
  purpose: string = '';
}

export class ExpropriationForm5Model extends ExpropriationBaseModel {
  impactedProperties: Api_AcquisitionFileProperty[] = [];
}

export class ExpropriationForm9Model extends ExpropriationBaseModel {
  impactedProperties: Api_AcquisitionFileProperty[] = [];
  registeredPlanNumbers: string = '';
}
