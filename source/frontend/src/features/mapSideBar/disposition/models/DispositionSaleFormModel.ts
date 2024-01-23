import { ApiGen_Concepts_DispositionFileSale } from '@/models/api/generated/ApiGen_Concepts_DispositionFileSale';
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

  static fromApi(entity: ApiGen_Concepts_DispositionFileSale) {
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
    model.sppAmount = entity.sppAmount;
    model.remediationAmount = entity.remediationAmount;

    model.netProceedsBeforeSppAmount = calculateNetProceedsBeforeSppAmount(entity);
    model.netProceedsAfterSppAmount = calculateNetProceedsAfterSppAmount(entity);
  }

  toApi(): ApiGen_Concepts_DispositionFileSale {
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
      sppAmount: this.sppAmount,
      remediationAmount: this.remediationAmount,
      dispositionPurchasers: [],
      dispositionPurchaserAgents: [],
      dispositionPurchaserSolicitors: [],
      netProceedsAfterSppAmount: null,
      netProceedsBeforeSppAmount: null,
      rowVersion: null,
    };
  }
}

export const calculateNetProceedsBeforeSppAmount = (
  apiModel: ApiGen_Concepts_DispositionFileSale | null,
) => {
  return apiModel == null
    ? 0
    : (apiModel.finalSaleAmount ?? 0) -
        ((apiModel.realtorCommissionAmount ?? 0) +
          (apiModel.gstCollectedAmount ?? 0) +
          (apiModel.totalCostAmount ?? 0) +
          (apiModel.netBookAmount ?? 0));
};

export const calculateNetProceedsAfterSppAmount = (
  apiModel: ApiGen_Concepts_DispositionFileSale | null,
) => {
  return apiModel == null
    ? 0
    : (apiModel.finalSaleAmount ?? 0) -
        ((apiModel.realtorCommissionAmount ?? 0) +
          (apiModel.gstCollectedAmount ?? 0) +
          (apiModel.totalCostAmount ?? 0) +
          (apiModel.netBookAmount ?? 0) +
          (apiModel.sppAmount ?? 0));
};
