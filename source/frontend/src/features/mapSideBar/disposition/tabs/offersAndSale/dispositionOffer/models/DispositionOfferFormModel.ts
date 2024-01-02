import { Api_DispositionFileOffer } from '@/models/api/DispositionFile';
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

  static fromApi(entity: Api_DispositionFileOffer): DispositionOfferFormModel {
    const model = new DispositionOfferFormModel(entity.id, entity.dispositionFileId);

    model.dispositionOfferStatusTypeCode = entity.dispositionOfferStatusTypeCode;
    model.offerName = entity.offerName;
    model.offerDate = entity.offerDate;
    model.offerExpiryDate = entity.offerExpiryDate;
    model.offerAmount = entity.offerAmount;
    model.offerNote = entity.offerNote;
    model.rowVersion = entity.rowVersion ?? null;

    return model;
  }

  toApi(): Api_DispositionFileOffer {
    return {
      id: this.id,
      dispositionFileId: this.dispositionFileId,
      dispositionOfferStatusTypeCode: emptyStringtoNullable(this.dispositionOfferStatusTypeCode),
      dispositionOfferStatusType: toTypeCodeNullable(this.dispositionOfferStatusTypeCode),
      offerName: emptyStringtoNullable(this.offerName),
      offerDate: emptyStringtoNullable(this.offerDate),
      offerExpiryDate: emptyStringtoNullable(this.offerExpiryDate),
      offerAmount: this.offerAmount,
      offerNote: emptyStringtoNullable(this.offerNote),
      rowVersion: this.rowVersion ?? 0,
    };
  }
}
