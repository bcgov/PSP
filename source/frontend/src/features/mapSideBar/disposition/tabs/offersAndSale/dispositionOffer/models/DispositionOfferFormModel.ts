import { ApiGen_Concepts_DispositionFileOffer } from '@/models/api/generated/ApiGen_Concepts_DispositionFileOffer';
import { EpochIsoDateTime } from '@/models/api/UtcIsoDateTime';
import { isValidIsoDateTime } from '@/utils';
import { emptyStringtoNullable, toTypeCodeNullable } from '@/utils/formUtils';

export class DispositionOfferFormModel {
  dispositionOfferStatusTypeCode: string | null = null;
  offerName: string | null = null;
  offerDate: string | null = null;
  offerExpiryDate: string | null = null;
  offerAmount: number | null = null;
  offerNote: string | null = null;
  rowVersion: number | null = null;

  constructor(readonly id: number | null = null, readonly dispositionFileId: number) {
    this.id = id;
    this.dispositionFileId = dispositionFileId;
  }

  static fromApi(entity: ApiGen_Concepts_DispositionFileOffer): DispositionOfferFormModel {
    const model = new DispositionOfferFormModel(entity.id, entity.dispositionFileId);

    model.dispositionOfferStatusTypeCode = entity.dispositionOfferStatusTypeCode;
    model.offerName = entity.offerName;
    model.offerDate = isValidIsoDateTime(entity.offerDate) ? entity.offerDate : null;
    model.offerExpiryDate = entity.offerExpiryDate;
    model.offerAmount = entity.offerAmount;
    model.offerNote = entity.offerNote;
    model.rowVersion = entity.rowVersion ?? null;

    return model;
  }

  toApi(): ApiGen_Concepts_DispositionFileOffer {
    return {
      id: this.id,
      dispositionFileId: this.dispositionFileId,
      dispositionOfferStatusTypeCode: emptyStringtoNullable(this.dispositionOfferStatusTypeCode),
      dispositionOfferStatusType: toTypeCodeNullable(this.dispositionOfferStatusTypeCode),
      offerName: emptyStringtoNullable(this.offerName),
      offerDate: isValidIsoDateTime(this.offerDate) ? this.offerDate : EpochIsoDateTime,
      offerExpiryDate: isValidIsoDateTime(this.offerExpiryDate) ? this.offerExpiryDate : null,
      offerAmount: this.offerAmount ?? 0,
      offerNote: emptyStringtoNullable(this.offerNote),
      rowVersion: this.rowVersion ?? 0,
    };
  }
}
