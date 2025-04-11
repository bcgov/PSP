import { ApiGen_Concepts_ExpropriationEvent } from '@/models/api/generated/ApiGen_Concepts_ExpropriationEvent';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { fromTypeCodeNullable, stringToNull, toTypeCodeNullable } from '@/utils/formUtils';

export class ExpropriationEventFormModel {
  id: number | null = null;
  readonly acquisitionFileId: number;
  acquisitionOwnerId: number | null = null;
  interestHolderId: number | null = null;
  eventTypeCode: string | null = null;
  eventDate: string | null = null;
  rowVersion = 0;

  constructor(acquisitionFileId: number) {
    this.acquisitionFileId = acquisitionFileId;
  }

  toApi(): ApiGen_Concepts_ExpropriationEvent {
    return {
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
  }

  static fromApi(apiModel: ApiGen_Concepts_ExpropriationEvent): ExpropriationEventFormModel {
    const newForm = new ExpropriationEventFormModel(apiModel.acquisitionFileId);
    newForm.id = apiModel.id;
    newForm.acquisitionOwnerId = apiModel.acquisitionOwnerId;
    newForm.interestHolderId = apiModel.interestHolderId;
    newForm.eventTypeCode = fromTypeCodeNullable(apiModel.eventType);
    newForm.eventDate = apiModel.eventDate || '';
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
    newForm.ownerOrInterestHolder = 'Change me (Test Value)';
    newForm.eventDescription = apiModel.eventType?.description;
    newForm.eventDate = apiModel.eventDate || '';

    return newForm;
  }
}
