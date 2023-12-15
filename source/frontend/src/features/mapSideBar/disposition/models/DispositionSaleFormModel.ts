import { Api_DispositionFileSale } from '@/models/api/DispositionFile';
import { emptyStringtoNullable } from '@/utils/formUtils';

export class DispositionSaleFormModel {
  finalConditionRemovalDate: string | null = null;
  saleCompletionDate: string | null = null;
  saleFiscalYear: string | null = null;
  finalSaleAmount: number | null = null;
  realtorCommissionAmount: number | null = null;
  isGstRequired: boolean | null = null;
  gstCollectedAmount: number | null = null;
  netBookAmount: number | null = null;
  totalCostAmount: number | null = null;
  netProceedsBeforeSppAmount: number | null = null;
  sppAmount: number | null = null;
  netProceedsAfterSppAmount: number | null = null;
  remediationAmount: number | null = null;

  constructor(readonly id: number | null = null, readonly dispositionFileId: number) {
    this.id = id;
    this.dispositionFileId = dispositionFileId;
  }

  static fromApi(entity: Api_DispositionFileSale) {
    const model = new DispositionSaleFormModel(entity.id, entity.dispositionFileId);

    model.finalConditionRemovalDate = entity.finalConditionRemovalDate;
    model.saleCompletionDate = entity.saleCompletionDate;
    model.saleFiscalYear = entity.saleFiscalYear;
    model.finalSaleAmount = entity.finalSaleAmount;
    model.realtorCommissionAmount = entity.realtorCommissionAmount;
    model.isGstRequired = entity.isGstRequired;
    model.gstCollectedAmount = entity.gstCollectedAmount;
    model.netBookAmount = entity.netBookAmount;
    model.totalCostAmount = entity.totalCostAmount;
    model.netProceedsBeforeSppAmount = entity.netProceedsBeforeSppAmount;
    model.sppAmount = entity.sppAmount;
    model.netProceedsAfterSppAmount = entity.netProceedsAfterSppAmount;
    model.remediationAmount = entity.remediationAmount;
  }

  toApi(): Api_DispositionFileSale {
    return {
      id: this.id,
      dispositionFileId: this.dispositionFileId,
      finalConditionRemovalDate: emptyStringtoNullable(this.finalConditionRemovalDate),
      saleCompletionDate: emptyStringtoNullable(this.saleCompletionDate),
      saleFiscalYear: emptyStringtoNullable(this.saleFiscalYear),
      finalSaleAmount: this.finalSaleAmount,
      realtorCommissionAmount: this.realtorCommissionAmount,
      isGstRequired: this.isGstRequired,
      gstCollectedAmount: this.gstCollectedAmount,
      netBookAmount: this.netBookAmount,
      totalCostAmount: this.totalCostAmount,
      netProceedsBeforeSppAmount: this.netProceedsBeforeSppAmount,
      sppAmount: this.sppAmount,
      netProceedsAfterSppAmount: this.netProceedsAfterSppAmount,
      remediationAmount: this.remediationAmount,
    };
  }
}
