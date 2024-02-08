import { isNumber } from 'lodash';

import {
  fromApiOrganization,
  fromApiPerson,
  IContactSearchResult,
} from '@/interfaces/IContactSearchResult';
import { ApiGen_Concepts_DispositionSalePurchaser } from '@/models/api/generated/ApiGen_Concepts_DispositionSalePurchaser';

export interface WithSalePurchasers {
  dispositionPurchasers: DispositionSalePurchaserContactModel[];
}

export class DispositionSalePurchaserContactModel {
  contact: IContactSearchResult | null = null;
  primaryContactId: string = '';

  constructor(
    readonly id: number | null = null,
    readonly saleId: number | null = null,
    readonly rowVersion: number = 0,
    contact: IContactSearchResult | null = null,
  ) {
    this.id = id;
    this.saleId = saleId;
    this.contact = contact;
    this.rowVersion = rowVersion;
  }

  toApi(): ApiGen_Concepts_DispositionSalePurchaser | null {
    const personId = this.contact?.personId ?? null;
    const organizationId = !personId ? this.contact?.organizationId ?? null : null;

    if (personId === null && organizationId === null) {
      return null;
    }

    return {
      id: this.id ?? 0,
      dispositionSaleId: this.saleId ?? 0,
      personId: personId,
      person: null,
      organizationId: organizationId,
      organization: null,
      primaryContactId:
        !!this.primaryContactId && isNumber(+this.primaryContactId) && organizationId !== null
          ? Number(this.primaryContactId)
          : null,
      primaryContact: null,
      rowVersion: this.rowVersion ?? 0,
    };
  }

  static fromApi(
    model: ApiGen_Concepts_DispositionSalePurchaser | null,
  ): DispositionSalePurchaserContactModel {
    const contact: IContactSearchResult | null = model?.person
      ? fromApiPerson(model?.person)
      : model?.organization
      ? fromApiOrganization(model.organization)
      : null;

    const newForm = new DispositionSalePurchaserContactModel(
      model?.id,
      model?.dispositionSaleId,
      model?.rowVersion ?? 0,
      contact,
    );

    if (model?.primaryContactId) {
      newForm.primaryContactId = model.primaryContactId.toString();
    }

    return newForm;
  }
}
