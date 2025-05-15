import { InterestHolderType } from '@/constants/interestHolderTypes';
import { PayeeOption } from '@/features/mapSideBar/acquisition/models/PayeeOptionModel';
import { PayeeType } from '@/features/mapSideBar/acquisition/models/PayeeTypeModel';
import { ApiGen_Concepts_ExpropriationEvent } from '@/models/api/generated/ApiGen_Concepts_ExpropriationEvent';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { exists, isValidString } from '@/utils';
import {
  formatAcquisitionOwnerName,
  formatInterestHolderName,
  fromTypeCodeNullable,
  stringToNull,
  toTypeCodeNullable,
} from '@/utils/formUtils';

export class ExpropriationEventFormModel {
  id: number | null = null;
  readonly acquisitionFileId: number;
  acquisitionOwnerId: number | null = null;
  interestHolderId: number | null = null;
  eventTypeCode: string | null = null;
  eventDate: string | null = null;
  payeeKey = '';
  rowVersion = 0;

  constructor(acquisitionFileId: number) {
    this.acquisitionFileId = acquisitionFileId;
  }

  toApi(payeeOptions: PayeeOption[] = []): ApiGen_Concepts_ExpropriationEvent {
    const apiModel: ApiGen_Concepts_ExpropriationEvent = {
      id: this.id ?? 0,
      acquisitionFileId: this.acquisitionFileId,
      acquisitionOwnerId: this.acquisitionOwnerId,
      interestHolderId: this.interestHolderId,
      eventType: toTypeCodeNullable(this.eventTypeCode),
      eventDate: stringToNull(this.eventDate),
      acquisitionOwner: null,
      interestHolder: null,
      ...getEmptyBaseAudit(this.rowVersion),
    };

    const payeeOption = isValidString(this.payeeKey)
      ? payeeOptions.find(x => x.value === this.payeeKey)
      : undefined;

    if (exists(payeeOption)) {
      switch (payeeOption.payeeType) {
        case PayeeType.Owner:
          apiModel.acquisitionOwnerId = payeeOption.api_id;
          apiModel.interestHolderId = null;
          break;
        case PayeeType.OwnerRepresentative:
        case PayeeType.OwnerSolicitor:
        case PayeeType.InterestHolder:
          apiModel.interestHolderId = payeeOption.api_id;
          apiModel.acquisitionOwnerId = null;
          break;
      }
    }

    return apiModel;
  }

  static createEmpty(acquisitionFileId: number) {
    const newForm = new ExpropriationEventFormModel(acquisitionFileId);
    return newForm;
  }

  static fromApi(apiModel: ApiGen_Concepts_ExpropriationEvent): ExpropriationEventFormModel {
    const newForm = new ExpropriationEventFormModel(apiModel.acquisitionFileId);
    newForm.id = apiModel.id;
    newForm.acquisitionOwnerId = apiModel.acquisitionOwnerId;
    newForm.interestHolderId = apiModel.interestHolderId;
    newForm.eventTypeCode = fromTypeCodeNullable(apiModel.eventType);
    newForm.eventDate = apiModel.eventDate || '';
    newForm.payeeKey = getPayeeKey(apiModel);
    newForm.rowVersion = apiModel.rowVersion;

    return newForm;
  }
}

export class ExpropriationEventRow {
  id: number | null = null;
  readonly acquisitionFileId: number;
  eventDescription: string | null = null;
  eventDate: string | null = null;
  ownerOrInterestHolder: string | null = null;

  constructor(acquisitionFileId: number) {
    this.acquisitionFileId = acquisitionFileId;
  }

  static fromApi(apiModel: ApiGen_Concepts_ExpropriationEvent): ExpropriationEventRow {
    const newForm = new ExpropriationEventRow(apiModel.acquisitionFileId);
    newForm.id = apiModel.id;
    newForm.eventDescription = apiModel.eventType?.description;
    newForm.eventDate = apiModel.eventDate || '';

    if (exists(apiModel.acquisitionOwner)) {
      newForm.ownerOrInterestHolder = formatAcquisitionOwnerName(apiModel.acquisitionOwner) ?? '';
    } else if (exists(apiModel.interestHolder)) {
      newForm.ownerOrInterestHolder = formatInterestHolderName(apiModel.interestHolder) ?? '';
    }

    return newForm;
  }
}

const getPayeeKey = (apiModel: ApiGen_Concepts_ExpropriationEvent): string => {
  if (apiModel.acquisitionOwnerId) {
    return PayeeOption.generateKey(apiModel.acquisitionOwnerId, PayeeType.Owner);
  }

  if (apiModel.interestHolderId) {
    if (
      apiModel.interestHolder?.interestHolderType?.id === InterestHolderType.OWNER_REPRESENTATIVE
    ) {
      return PayeeOption.generateKey(apiModel.interestHolderId, PayeeType.OwnerRepresentative);
    } else if (
      apiModel.interestHolder?.interestHolderType?.id === InterestHolderType.OWNER_SOLICITOR
    ) {
      return PayeeOption.generateKey(apiModel.interestHolderId, PayeeType.OwnerSolicitor);
    } else {
      return PayeeOption.generateKey(apiModel.interestHolderId, PayeeType.InterestHolder);
    }
  }

  return '';
};
