import { isNumber } from 'lodash';

import {
  fromApiOrganization,
  fromApiPerson,
  IContactSearchResult,
} from '@/interfaces/IContactSearchResult';
import { Api_DispositionSaleContact } from '@/models/api/DispositionFile';

export interface WithSalePurchasers {
  dispositionPurchasers: DispositionSaleContactModel[];
}

export class DispositionSaleContactModel {
  contact: IContactSearchResult | null = null;
  primaryContactId: string = '';

  constructor(
    readonly id: number | null = null,
    readonly dispositionSaleId: number | null = null,
    readonly rowVersion: number = 0,
    contact: IContactSearchResult | null = null,
  ) {
    this.id = id;
    this.dispositionSaleId = dispositionSaleId;
    this.contact = contact;
    this.rowVersion = rowVersion;
  }

  toApi(): Api_DispositionSaleContact | null {
    const personId = this.contact?.personId ?? null;
    const organizationId = !personId ? this.contact?.organizationId ?? null : null;

    if (personId === null && organizationId === null) {
      return null;
    }

    return {
      id: this.id ?? 0,
      dispositionSaleId: this.dispositionSaleId,
      personId: personId,
      person: null,
      organizationId: organizationId,
      organization: null,
      primaryContactId:
        !!this.primaryContactId && isNumber(+this.primaryContactId)
          ? Number(this.primaryContactId)
          : null,
      primaryContact: null,
      rowVersion: this.rowVersion ?? 0,
    };
  }

  static fromApi(model: Api_DispositionSaleContact | null): DispositionSaleContactModel {
    const contact: IContactSearchResult | null = model?.person
      ? fromApiPerson(model?.person)
      : model?.organization
      ? fromApiOrganization(model.organization)
      : null;

    const newForm = new DispositionSaleContactModel(
      model?.id,
      model?.dispositionSaleId,
      model?.rowVersion,
      contact,
    );

    if (model?.primaryContactId) {
      newForm.primaryContactId = model.primaryContactId.toString();
    }

    return newForm;
  }
}
