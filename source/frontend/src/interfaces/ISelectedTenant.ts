import { ApiGen_Concepts_Contact } from '@/models/api/generated/ApiGen_Concepts_Contact';
import { ApiGen_Concepts_Organization } from '@/models/api/generated/ApiGen_Concepts_Organization';
import { ApiGen_Concepts_Person } from '@/models/api/generated/ApiGen_Concepts_Person';
import { exists } from '@/utils/utils';

export interface ISelectedTenant {
  id: string;
  personId?: number;
  person?: ApiGen_Concepts_Person;
  primaryContactId?: number;
  primaryContact?: ApiGen_Concepts_Person;
  organizationId?: number;
  organization?: ApiGen_Concepts_Organization;
  leaseTenantId?: number;
  isDisabled?: boolean;
  summary?: string;
  email?: string;
  mailingAddress?: string;
  municipalityName?: string;
  provinceState?: string;
  note?: string;
}

export function fromContact(baseModel: ApiGen_Concepts_Contact): ISelectedTenant {
  return {
    id: baseModel.id ?? '',
    personId: baseModel.person?.id,
    organizationId: baseModel.organization?.id,

    isDisabled: baseModel.person?.isDisabled || baseModel.organization?.isDisabled || false,
    summary: exists(baseModel.organization)
      ? baseModel.organization.name || ''
      : baseModel.person?.firstName + ' ' + baseModel.person?.surname,
    email: '',
    mailingAddress: '',
    municipalityName: '',
    provinceState: '',
  };
}
