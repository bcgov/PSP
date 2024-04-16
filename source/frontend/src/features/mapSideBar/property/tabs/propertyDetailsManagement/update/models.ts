import { fromApiOrganization, fromApiPerson, IContactSearchResult } from '@/interfaces';
import { ApiGen_Concepts_PropertyContact } from '@/models/api/generated/ApiGen_Concepts_PropertyContact';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { stringToNull } from '@/utils/formUtils';

export class PropertyContactFormModel {
  public id = 0;
  public propertyId = 0;
  public contact: IContactSearchResult | undefined = undefined;
  public primaryContactId = '';
  public purposeDescription = '';
  public rowVersion = 0;

  public toApi(): ApiGen_Concepts_PropertyContact {
    return {
      id: this.id,
      propertyId: this.propertyId,
      organizationId: !this.contact?.personId ? this.contact?.organizationId ?? null : null,
      organization: null,
      personId: this.contact?.personId ?? null,
      person: null,
      primaryContactId: this.primaryContactId !== '' ? Number(this.primaryContactId) : null,
      primaryContact: null,
      purpose: stringToNull(this.purposeDescription),
      ...getEmptyBaseAudit(this.rowVersion),
    };
  }

  static fromApi(model: ApiGen_Concepts_PropertyContact | null): PropertyContactFormModel {
    const newFormModel = new PropertyContactFormModel();
    newFormModel.id = model?.id || 0;
    newFormModel.propertyId = model?.propertyId || 0;
    newFormModel.rowVersion = model?.rowVersion || 0;
    if (model?.person) {
      newFormModel.contact = fromApiPerson(model.person);
    } else if (model?.organization) {
      newFormModel.contact = fromApiOrganization(model.organization);
    }

    if (model?.primaryContactId) {
      newFormModel.primaryContactId = model.primaryContactId.toString();
    }

    newFormModel.purposeDescription = model?.purpose || '';
    return newFormModel;
  }
}
