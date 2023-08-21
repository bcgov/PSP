import { InterestHolderType } from '@/constants/interestHolderTypes';
import { PayeeOption } from '@/features/mapSideBar/acquisition/models/PayeeOptionModel';
import { PayeeType } from '@/features/mapSideBar/acquisition/models/PayeeTypeModel';
import { fromApiOrganization } from '@/interfaces';
import {
  Api_ExpropriationPayment,
  Api_ExpropriationPaymentItem,
} from '@/models/api/ExpropriationPayment';
import { booleanToString, stringToBoolean, toTypeCode } from '@/utils/formUtils';
import { isNullOrWhitespace } from '@/utils/utils';

import { ExpropriationAuthorityFormModel } from '../../models';

export class Form8FormModel {
  acquisitionFileId: number;
  acquisitionOwnerId: number | null = null;
  interestHolderId: number | null = null;
  expropriatingAuthorityId: number | null = null;
  expropriationAuthority: ExpropriationAuthorityFormModel | null =
    new ExpropriationAuthorityFormModel();
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
    const expropriationPaymentApi = {
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
        .map<Api_ExpropriationPaymentItem>(x => x.toApi()),
    };

    if (isNullOrWhitespace(this.payeeKey)) {
      return expropriationPaymentApi;
    }

    const payeeOption = payeeOptions.find(x => x.value === this.payeeKey);

    if (payeeOption === undefined) {
      return expropriationPaymentApi;
    }

    switch (payeeOption.payeeType) {
      case PayeeType.Owner:
        expropriationPaymentApi.acquisitionOwnerId = payeeOption.api_id;
        expropriationPaymentApi.interestHolderId = null;
        break;
      case PayeeType.OwnerRepresentative:
      case PayeeType.OwnerSolicitor:
      case PayeeType.InterestHolder:
        expropriationPaymentApi.interestHolderId = payeeOption.api_id;
        expropriationPaymentApi.acquisitionOwnerId = null;
        break;
    }

    return expropriationPaymentApi;
  }

  static fromApi(model: Api_ExpropriationPayment): Form8FormModel {
    const newForm = new Form8FormModel(model.id, model.acquisitionFileId);

    newForm.acquisitionOwnerId = model.acquisitionOwnerId;
    newForm.interestHolderId = model.interestHolderId;
    newForm.expropriatingAuthorityId = model.expropriatingAuthorityId;
    newForm.rowVersion = model.rowVersion;
    newForm.isDisabled = model.isDisabled;
    newForm.description = model.description;
    newForm.paymentItems = model.paymentItems?.map(x => Form8PaymentItemModel.fromApi(x)) ?? [];
    newForm.payeeKey = getPayeeKey(model);
    newForm.expropriationAuthority = {
      organizationId: model.expropriatingAuthorityId,
      organization: model.expropriatingAuthority,
      contact: model.expropriatingAuthority
        ? fromApiOrganization(model.expropriatingAuthority)
        : null,
    };

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

  static fromApi(model: Api_ExpropriationPaymentItem): Form8PaymentItemModel {
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

  toApi(): Api_ExpropriationPaymentItem {
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

const getPayeeKey = (form8Api: Api_ExpropriationPayment): string => {
  if (form8Api.acquisitionOwnerId) {
    return PayeeOption.generateKey(form8Api.acquisitionOwnerId, PayeeType.Owner);
  }

  if (form8Api.interestHolderId) {
    if (
      form8Api.interestHolder?.interestHolderType?.id === InterestHolderType.OWNER_REPRESENTATIVE
    ) {
      return PayeeOption.generateKey(form8Api.interestHolderId, PayeeType.OwnerRepresentative);
    } else if (
      form8Api.interestHolder?.interestHolderType?.id === InterestHolderType.OWNER_SOLICITOR
    ) {
      return PayeeOption.generateKey(form8Api.interestHolderId, PayeeType.OwnerSolicitor);
    } else {
      return PayeeOption.generateKey(form8Api.interestHolderId, PayeeType.InterestHolder);
    }
  }

  return '';
};
