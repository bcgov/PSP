import { fromApiOrganization, fromApiPerson, IContactSearchResult } from '@/interfaces';
import { ApiGen_Concepts_ManagementFileContact } from '@/models/api/generated/ApiGen_Concepts_ManagementFileContact';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { stringToNull } from '@/utils/formUtils';

export class ManagementFileContactFormModel {
  public contact: IContactSearchResult | undefined = undefined;
  public primaryContactId: number | null = null;
  public purposeDescription = '';

  constructor(
    readonly id: number = 0,
    readonly managementFileId: number,
    readonly rowVersion: number | null = null,
  ) {
    this.id = id;
    this.managementFileId = managementFileId;
    this.rowVersion = rowVersion;
  }

  public toApi(): ApiGen_Concepts_ManagementFileContact {
    return {
      id: this.id,
      managementFileId: this.managementFileId,
      organizationId: !this.contact?.personId ? this.contact?.organizationId ?? null : null,
      organization: null,
      personId: this.contact?.personId ?? null,
      person: null,
      primaryContactId: this.primaryContactId ? Number(this.primaryContactId) : null,
      primaryContact: null,
      purpose: stringToNull(this.purposeDescription),
      ...getEmptyBaseAudit(this.rowVersion),
    };
  }

  static fromApi(model: ApiGen_Concepts_ManagementFileContact): ManagementFileContactFormModel {
    const newFormModel = new ManagementFileContactFormModel(
      model.id,
      model.managementFileId,
      model.rowVersion,
    );

    if (model?.person) {
      newFormModel.contact = fromApiPerson(model.person);
    } else if (model?.organization) {
      newFormModel.contact = fromApiOrganization(model.organization);
    }
    newFormModel.primaryContactId = model.primaryContactId;

    newFormModel.purposeDescription = model.purpose || '';

    return newFormModel;
  }
}
