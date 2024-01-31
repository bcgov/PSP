import { ApiGen_Concepts_DispositionFileAppraisal } from '@/models/api/generated/ApiGen_Concepts_DispositionFileAppraisal';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { isValidIsoDateTime } from '@/utils';
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

  static fromApi(entity: ApiGen_Concepts_DispositionFileAppraisal): DispositionAppraisalFormModel {
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

  toApi(): ApiGen_Concepts_DispositionFileAppraisal {
    return {
      id: this.id,
      dispositionFileId: this.dispositionFileId,
      appraisedAmount: this.appraisedValueAmount
        ? parseFloat(this.appraisedValueAmount.toString())
        : null,
      appraisalDate: isValidIsoDateTime(this.appraisalDate) ? this.appraisalDate : null,
      bcaValueAmount: this.bcaValueAmount ? parseFloat(this.bcaValueAmount.toString()) : null,
      bcaRollYear: emptyStringtoNullable(this.bcaRollYear),
      listPriceAmount: this.listPriceAmount ? parseFloat(this.listPriceAmount.toString()) : null,
      ...getEmptyBaseAudit(this.rowVersion),
    };
  }
}
