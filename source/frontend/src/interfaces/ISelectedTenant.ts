import { Api_Contact } from '@/models/api/Contact';
import { Api_Organization } from '@/models/api/Organization';
import { Api_Person } from '@/models/api/Person';

export interface ISelectedTenant {
  id: string;
  personId?: number;
  person?: Api_Person;
  primaryContactId?: number;
  primaryContact?: Api_Person;
  organizationId?: number;
  organization?: Api_Organization;
  leaseTenantId?: number;
  isDisabled?: boolean;
  summary?: string;
  email?: string;
  mailingAddress?: string;
  municipalityName?: string;
  provinceState?: string;
  note?: string;
}

export function fromContact(baseModel: Api_Contact): ISelectedTenant {
  return {
    id: baseModel.id,
    personId: baseModel.person?.id,
    organizationId: baseModel.organization?.id,

    isDisabled: baseModel.person?.isDisabled || baseModel.organization?.isDisabled || false,
    summary:
      baseModel.organization !== undefined
        ? baseModel.organization.name || ''
        : baseModel.person?.firstName + ' ' + baseModel.person?.surname,
    email: '',
    mailingAddress: '',
    municipalityName: '',
    provinceState: '',
  };
}
