import { Api_DispositionFileAppraisal } from '@/models/api/DispositionFile';

export class DispositionAppraisalFormModel {
  appraisedValueAmount: number | null = null;
  appraisalDate: string | null = null;
  bcaValueAmount: number | null = null;
  bcaRollYear: string | null = null;
  listPriceAmount: number | null = null;

  constructor(readonly id: number | null = null, readonly dispositionFileId: number) {
    this.id = id;
    this.dispositionFileId = dispositionFileId;
  }

  toApi(): Api_DispositionFileAppraisal {
    return {
      appraisedAmount: this.appraisedValueAmount,
      appraisalDate: this.appraisalDate,
      bcaValueAmount: this.bcaValueAmount,
      bcaRollYear: this.bcaRollYear,
      listPriceAmount: this.listPriceAmount,
    };
  }
}
