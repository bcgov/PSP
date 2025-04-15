import { IContactSearchResult } from '@/interfaces';
import { ApiGen_Concepts_AcquisitionFileProperty } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileProperty';
import { ApiGen_Concepts_Organization } from '@/models/api/generated/ApiGen_Concepts_Organization';

export class ExpropriationAuthorityFormModel {
  organizationId: number | null = null;
  organization: ApiGen_Concepts_Organization | null = null;
  contact: IContactSearchResult | null = null;
}

class ExpropriationBaseModel {
  expropriationAuthority = new ExpropriationAuthorityFormModel();
}

export class ExpropriationForm1Model extends ExpropriationBaseModel {
  impactedProperties: ApiGen_Concepts_AcquisitionFileProperty[] = [];
  landInterest = '';
  purpose = '';
}

export class ExpropriationForm5Model extends ExpropriationBaseModel {
  impactedProperties: ApiGen_Concepts_AcquisitionFileProperty[] = [];
}

export class ExpropriationForm9Model extends ExpropriationBaseModel {
  impactedProperties: ApiGen_Concepts_AcquisitionFileProperty[] = [];
  registeredPlanNumbers = '';
}
