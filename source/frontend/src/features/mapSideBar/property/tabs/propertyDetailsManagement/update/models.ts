import { fromApiOrganization, fromApiPerson, IContactSearchResult } from '@/interfaces';
import { Api_PropertyContact } from '@/models/api/Property';
import { stringToNull } from '@/utils/formUtils';

export class PropertyContactFormModel {
  public id: number = 0;
  public propertyId: number = 0;
  public contact: IContactSearchResult | undefined = undefined;
  public primaryContactId: string = '';
  public purposeDescription: string = '';
  public rowVersion: number = 0;

  public toApi(): Api_PropertyContact {
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
      rowVersion: this.rowVersion,
    };
  }

  static fromApi(model: Api_PropertyContact | null): PropertyContactFormModel {
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
