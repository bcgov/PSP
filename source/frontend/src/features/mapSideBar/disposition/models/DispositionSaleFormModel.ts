import { Api_DispositionFileSale } from '@/models/api/DispositionFile';
import { emptyStringtoNullable } from '@/utils/formUtils';

import { DispositionSaleContactModel, WithSalePurchasers } from './DispositionSaleContactModel';

export class DispositionSaleFormModel implements WithSalePurchasers {
  finalConditionRemovalDate: string | null = null;
  saleCompletionDate: string | null = null;
  saleFiscalYear: string | null = null;
  finalSaleAmount: number | null = null;
  realtorCommissionAmount: number | null = null;
  isGstRequired: boolean = false;
  gstCollectedAmount: number | null = null;
  netBookAmount: number | null = null;
  totalCostAmount: number | null = null;
  netProceedsBeforeSppAmount: number | null = null;
  sppAmount: number | null = null;
  netProceedsAfterSppAmount: number | null = null;
  remediationAmount: number | null = null;
  dispositionPurchasers: DispositionSaleContactModel[] = [];
  dispositionPurchaserAgent: DispositionSaleContactModel = new DispositionSaleContactModel();
  dispositionPurchaserSolicitor: DispositionSaleContactModel = new DispositionSaleContactModel();

  constructor(
    readonly id: number | null = null,
    readonly dispositionFileId: number,
    readonly rowVersion: number | null = null,
  ) {
    this.id = id;
    this.dispositionFileId = dispositionFileId;
    this.rowVersion = rowVersion;
  }

  static fromApi(entity: Api_DispositionFileSale): DispositionSaleFormModel {
    const model = new DispositionSaleFormModel(entity.id, entity.dispositionFileId);

    model.finalConditionRemovalDate = entity.finalConditionRemovalDate;
    model.saleCompletionDate = entity.saleCompletionDate;
    model.saleFiscalYear = entity.saleFiscalYear;
    model.finalSaleAmount = entity.finalSaleAmount;
    model.realtorCommissionAmount = entity.realtorCommissionAmount;
    model.isGstRequired = entity.isGstRequired ?? false;
    model.gstCollectedAmount = entity.gstCollectedAmount;
    model.netBookAmount = entity.netBookAmount;
    model.totalCostAmount = entity.totalCostAmount;
    model.sppAmount = entity.sppAmount;
    model.remediationAmount = entity.remediationAmount;

    model.netProceedsBeforeSppAmount = calculateNetProceedsBeforeSppAmount(entity);
    model.netProceedsAfterSppAmount = calculateNetProceedsAfterSppAmount(entity);

    return model;
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
      sppAmount: this.sppAmount,
      remediationAmount: this.remediationAmount,
      dispositionPurchasers: [],
      dispositionPurchaserAgent: null,
      dispositionPurchaserSolicitor: null,
    };
  }
}

export const calculateNetProceedsBeforeSppAmount = (apiModel: Api_DispositionFileSale | null) => {
  return apiModel == null
    ? 0
    : (apiModel.finalSaleAmount ?? 0) -
        ((apiModel.realtorCommissionAmount ?? 0) +
          (apiModel.gstCollectedAmount ?? 0) +
          (apiModel.totalCostAmount ?? 0) +
          (apiModel.netBookAmount ?? 0));
};

export const calculateNetProceedsAfterSppAmount = (apiModel: Api_DispositionFileSale | null) => {
  return apiModel == null
    ? 0
    : (apiModel.finalSaleAmount ?? 0) -
        ((apiModel.realtorCommissionAmount ?? 0) +
          (apiModel.gstCollectedAmount ?? 0) +
          (apiModel.totalCostAmount ?? 0) +
          (apiModel.netBookAmount ?? 0) +
          (apiModel.sppAmount ?? 0));
};
