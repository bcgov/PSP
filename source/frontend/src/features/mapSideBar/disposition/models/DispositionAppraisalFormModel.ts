import { Api_DispositionFileAppraisal } from '@/models/api/DispositionFile';
import { emptyStringtoNullable } from '@/utils/formUtils';

export class DispositionAppraisalFormModel {
  appraisedValueAmount: number | null = null;
  appraisalDate: string | null = null;
  bcaValueAmount: number | null = null;
  bcaRollYear: string | null = null;
  listPriceAmount: number | null = null;

  constructor(
    readonly id: number | null = null,
    readonly dispositionFileId: number,
    readonly rowVersion: number | null = null,
  ) {
    this.id = id;
    this.dispositionFileId = dispositionFileId;
    this.rowVersion = rowVersion;
  }

  isEmpty(): boolean {
    return (
      this.appraisedValueAmount === null &&
      (this.appraisalDate === null || this.appraisalDate === '') &&
      this.bcaValueAmount === null &&
      (this.bcaRollYear === null || this.bcaRollYear === '') &&
      this.listPriceAmount === null
    );
  }

  static fromApi(entity: Api_DispositionFileAppraisal): DispositionAppraisalFormModel {
    const model = new DispositionAppraisalFormModel(
      entity.id,
      entity.dispositionFileId,
      entity.rowVersion,
    );

    model.appraisedValueAmount = entity.appraisedAmount;
    model.appraisalDate = emptyStringtoNullable(entity.appraisalDate);
    model.bcaValueAmount = entity.bcaValueAmount;
    model.bcaRollYear = emptyStringtoNullable(entity.bcaRollYear);
    model.listPriceAmount = entity.listPriceAmount;

    return model;
  }

  toApi(): Api_DispositionFileAppraisal {
    return {
      id: this.id,
      dispositionFileId: this.dispositionFileId,
      appraisedAmount: this.appraisedValueAmount,
      appraisalDate: emptyStringtoNullable(this.appraisalDate),
      bcaValueAmount: this.bcaValueAmount,
      bcaRollYear: emptyStringtoNullable(this.bcaRollYear),
      listPriceAmount: this.listPriceAmount,
      rowVersion: this.rowVersion ?? 0,
    };
  }
}
