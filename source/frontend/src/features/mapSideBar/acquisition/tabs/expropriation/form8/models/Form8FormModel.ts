import { PayeeOption } from '@/features/mapSideBar/acquisition/models/PayeeOption';
import { PayeeType } from '@/features/mapSideBar/acquisition/models/PayeeTypeModel';
import {
  Api_ExpropiationPaymentItem,
  Api_ExpropriationPayment,
} from '@/models/api/ExpropriationPayment';
import { booleanToString, stringToBoolean, toTypeCode } from '@/utils/formUtils';
import { isNullOrWhitespace } from '@/utils/utils';

import { ExpropriationAuthorityFormModel } from '../../models';

export class Form8FormModel {
  acquisitionFileId: number;
  acquisitionOwnerId: number | null = null;
  interestHolderId: number | null = null;
  expropriatingAuthorityId: number | null = null;
  expropriationAuthority: ExpropriationAuthorityFormModel | null = null;
  description: string | null = '';
  paymentItems: Form8PaymentItemModel[] = [];
  isDisabled: boolean | null = false;
  rowVersion: number | null = null;
  payeeKey: string = '';

  constructor(readonly id: number | null = null, acquisitionFileId: number) {
    this.id = id;
    this.acquisitionFileId = acquisitionFileId;
  }

  toApi(payeeOptions: PayeeOption[]): Api_ExpropriationPayment {
    let expropiationPaymentApi = {
      id: this.id,
      acquisitionFileId: this.acquisitionFileId,
      acquisitionOwnerId: this.acquisitionOwnerId,
      acquisitionOwner: null,
      interestHolderId: this.interestHolderId,
      interestHolder: null,
      expropriatingAuthorityId: this.expropriationAuthority?.contact?.organizationId!,
      expropriatingAuthority: null,
      description: this.description,
      isDisabled: this.isDisabled,
      rowVersion: this.rowVersion,
      paymentItems: this.paymentItems
        .filter(x => !x.isEmpty())
        .map<Api_ExpropiationPaymentItem>(x => x.toApi()),
    };

    if (isNullOrWhitespace(this.payeeKey)) {
      return expropiationPaymentApi;
    }

    const payeeOption = payeeOptions.find(x => x.value === this.payeeKey);

    if (payeeOption === undefined) {
      return expropiationPaymentApi;
    }

    switch (payeeOption.payeeType) {
      case PayeeType.Owner:
        expropiationPaymentApi.acquisitionOwnerId = payeeOption.api_id;
        expropiationPaymentApi.interestHolderId = null;

        break;
      case PayeeType.OwnerRepresentative:
      case PayeeType.OwnerSolicitor:
      case PayeeType.InterestHolder:
        expropiationPaymentApi.interestHolderId = payeeOption.api_id;
        expropiationPaymentApi.acquisitionOwnerId = null;
        break;
    }

    return expropiationPaymentApi;
  }

  static fromApi(model: Api_ExpropriationPayment): Form8FormModel {
    const newForm = new Form8FormModel(model.id, model.acquisitionFileId);

    newForm.acquisitionOwnerId = model.acquisitionOwnerId;
    newForm.interestHolderId = model.interestHolderId;
    newForm.expropriatingAuthorityId = model.expropriatingAuthorityId;
    newForm.rowVersion = model.rowVersion;
    newForm.isDisabled = model.isDisabled;
    newForm.paymentItems = model.paymentItems?.map(x => Form8PaymentItemModel.fromApi(x)) ?? [];

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
  isDisabled: boolean | null = null;

  constructor(
    private id: number | null = null,
    private expropriationPaymentId: number | null = null,
  ) {}

  isEmpty(): boolean {
    return this.paymentItemTypeCode === '' && this.pretaxAmount === 0;
  }

  static fromApi(model: Api_ExpropiationPaymentItem): Form8PaymentItemModel {
    const newPaymentItem = new Form8PaymentItemModel(
      model.id ?? null,
      model.expropriationPaymentId,
    );

    newPaymentItem.paymentItemTypeCode = model.paymentItemTypeCode ?? '';
    newPaymentItem.isGstRequired = booleanToString(model.isGstRequired);
    newPaymentItem.pretaxAmount = model.pretaxAmount ?? 0;
    newPaymentItem.taxAmount = model.taxAmount ?? 0;
    newPaymentItem.totalAmount = model.totalAmount ?? 0;
    newPaymentItem.rowVersion = model.rowVersion;
    newPaymentItem.isDisabled = model.isDisabled;

    return newPaymentItem;
  }

  toApi(): Api_ExpropiationPaymentItem {
    return {
      id: this.id,
      expropriationPaymentId: this.expropriationPaymentId,
      paymentItemTypeCode: this.paymentItemTypeCode ?? null,
      paymentItemType: toTypeCode(this.paymentItemTypeCode) ?? null,
      isGstRequired: stringToBoolean(this.isGstRequired),
      pretaxAmount: this.pretaxAmount,
      taxAmount: this.taxAmount,
      totalAmount: this.totalAmount,
      rowVersion: this.rowVersion ?? null,
      isDisabled: this.isDisabled,
    };
  }
}
