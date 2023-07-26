import { IContactSearchResult } from '@/interfaces/IContactSearchResult';
import { Api_Form8, Api_PaymentItem } from '@/models/api/Form8';
import { Api_Organization } from '@/models/api/Organization';
import { booleanToString, toTypeCode } from '@/utils/formUtils';

export class Form8FormModel {
  acquisitionFileId: number;
  acquisitionOwnerId: number | null = null;
  interestHolderId: number | null = null;
  expropriatingAuthorityId: number | null = null;
  expropriationAuthority: ExpropriationAuthorityFormModel | null = null;
  description: string | null = '';
  paymentItems: Form8PaymentItemModel[] = [];
  isDisabled: boolean = false;
  rowVersion: number | null = null;
  payeeKey: string = '';

  constructor(readonly id: number | null = null, acquisitionFileId: number) {
    this.id = id;
    this.acquisitionFileId = acquisitionFileId;
  }

  toApi(): Api_Form8 {
    return {
      id: this.id,
      acquisitionFileId: this.acquisitionFileId,
      acquisitionOwnerId: this.acquisitionOwnerId,
      interestHolderId: this.interestHolderId,
      expropriatingAuthorityId: this.expropriatingAuthorityId,
      description: this.description,
      paymentItemTypeCode: toTypeCode('ADVNCTAKNGTTL') ?? null,
      isGstRequired: false,
      pretaxAmount: 0,
      taxAmount: 0,
      totalAmount: 0,
      isDisabled: this.isDisabled,
      rowVersion: this.rowVersion,
    };
  }

  static fromApi(model: Api_Form8): Form8FormModel {
    const newForm = new Form8FormModel(model.id, model.acquisitionFileId);
    newForm.acquisitionOwnerId = model.acquisitionOwnerId;
    newForm.interestHolderId = model.interestHolderId;
    newForm.expropriatingAuthorityId = model.expropriatingAuthorityId;
    newForm.rowVersion = model.rowVersion;

    return newForm;
  }
}

export class Form8PaymentItemModel {
  paymentItemTypeCode: string = '';
  isGstRequired: string = 'false';
  pretaxAmount: number = 0;
  taxAmount: number = 0;
  totalAmount: number = 0;
  rowVersion: number | null = null;

  constructor(private id: number | null = 0, private form8Id: number) {
    this.id = id;
    this.form8Id = form8Id;
  }

  static fromApi(model: Api_PaymentItem): Form8PaymentItemModel {
    const newPaymentItem = new Form8PaymentItemModel(model.id ?? null, model.form8Id);
    newPaymentItem.paymentItemTypeCode = model.paymentItemTypeCode?.id ?? '';
    newPaymentItem.isGstRequired = booleanToString(model.isGstRequired);
    newPaymentItem.pretaxAmount = model.pretaxAmount ?? 0;
    newPaymentItem.taxAmount = model.taxAmount ?? 0;
    newPaymentItem.totalAmount = model.totalAmount ?? 0;

    return newPaymentItem;
  }
}

export class ExpropriationAuthorityFormModel {
  organizationId: number | null = null;
  organization: Api_Organization | null = null;
  contact: IContactSearchResult | null = null;
}
