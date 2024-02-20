import { isNumber } from 'lodash';

import {
  fromApiOrganization,
  fromApiPerson,
  IContactSearchResult,
} from '@/interfaces/IContactSearchResult';
import { ApiGen_Concepts_DispositionSalePurchaser } from '@/models/api/generated/ApiGen_Concepts_DispositionSalePurchaser';
import { ApiGen_Concepts_DispositionSalePurchaserAgent } from '@/models/api/generated/ApiGen_Concepts_DispositionSalePurchaserAgent';
import { ApiGen_Concepts_DispositionSalePurchaserSolicitor } from '@/models/api/generated/ApiGen_Concepts_DispositionSalePurchaserSolicitor';

export interface WithSalePurchasers {
  dispositionPurchasers: DispositionSaleContactModel[];
}

export class DispositionSaleContactModel {
  contact: IContactSearchResult | null = null;
  primaryContactId = '';

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

  toApi():
    | ApiGen_Concepts_DispositionSalePurchaser
    | ApiGen_Concepts_DispositionSalePurchaserAgent
    | ApiGen_Concepts_DispositionSalePurchaserSolicitor
    | null {
    const personId = this.contact?.personId ?? null;
    const organizationId = !personId ? this.contact?.organizationId ?? null : null;

    if (personId === null && organizationId === null) {
      return null;
    }

    return {
      id: this.id ?? 0,
      dispositionSaleId: this.dispositionSaleId ?? 0,
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

  static fromApi(
    model:
      | ApiGen_Concepts_DispositionSalePurchaser
      | ApiGen_Concepts_DispositionSalePurchaserAgent
      | ApiGen_Concepts_DispositionSalePurchaserSolicitor
      | null,
  ): DispositionSaleContactModel {
    const contact: IContactSearchResult | null = model?.person
      ? fromApiPerson(model?.person)
      : model?.organization
      ? fromApiOrganization(model.organization)
      : null;

    const newForm = new DispositionSaleContactModel(
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
