import { ApiGen_Concepts_DispositionFileSale } from '@/models/api/generated/ApiGen_Concepts_DispositionFileSale';
import { ApiGen_Concepts_DispositionSalePurchaser } from '@/models/api/generated/ApiGen_Concepts_DispositionSalePurchaser';
import { exists, isValidIsoDateTime } from '@/utils';
import { emptyStringtoNullable, stringToBoolean } from '@/utils/formUtils';

import { DispositionSaleContactModel, WithSalePurchasers } from './DispositionSaleContactModel';

export class DispositionSaleFormModel implements WithSalePurchasers {
  finalConditionRemovalDate: string | null = null;
  saleCompletionDate: string | null = null;
  saleFiscalYear: string | null = null;
  finalSaleAmount: number | null = null;
  realtorCommissionAmount: number | null = null;
  isGstRequired = false;
  gstCollectedAmount: number | null = null;
  netBookAmount: number | null = null;
  totalCostAmount: number | null = null;
  netProceedsBeforeSppAmount: number | null = null;
  sppAmount: number | null = null;
  netProceedsAfterSppAmount: number | null = null;
  remediationAmount: number | null = null;
  dispositionPurchasers: DispositionSaleContactModel[] = [];
  dispositionPurchaserAgent: DispositionSaleContactModel | null = new DispositionSaleContactModel();
  dispositionPurchaserSolicitor: DispositionSaleContactModel | null =
    new DispositionSaleContactModel();

  constructor(
    readonly id: number | null = null,
    readonly dispositionFileId: number,
    readonly rowVersion: number | null = null,
  ) {
    this.id = id;
    this.dispositionFileId = dispositionFileId;
    this.rowVersion = rowVersion;
  }

  static fromApi(entity: ApiGen_Concepts_DispositionFileSale): DispositionSaleFormModel {
    const model = new DispositionSaleFormModel(
      entity.id,
      entity.dispositionFileId,
      entity.rowVersion,
    );

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

    model.dispositionPurchasers =
      entity.dispositionPurchasers?.map(x => DispositionSaleContactModel.fromApi(x)) || [];

    model.dispositionPurchaserAgent = entity.dispositionPurchaserAgent
      ? DispositionSaleContactModel.fromApi(entity.dispositionPurchaserAgent)
      : new DispositionSaleContactModel(null, entity.id);

    model.dispositionPurchaserSolicitor = entity.dispositionPurchaserSolicitor
      ? DispositionSaleContactModel.fromApi(entity.dispositionPurchaserSolicitor)
      : new DispositionSaleContactModel(null, entity.id);

    return model;
  }

  toApi(): ApiGen_Concepts_DispositionFileSale {
    return {
      id: this.id,
      dispositionFileId: this.dispositionFileId,
      finalConditionRemovalDate: isValidIsoDateTime(this.finalConditionRemovalDate)
        ? this.finalConditionRemovalDate
        : null,
      saleCompletionDate: isValidIsoDateTime(this.saleCompletionDate)
        ? this.finalConditionRemovalDate
        : null,
      saleFiscalYear: emptyStringtoNullable(this.saleFiscalYear),
      finalSaleAmount: this.finalSaleAmount ? parseFloat(this.finalSaleAmount.toString()) : null,
      realtorCommissionAmount: this.realtorCommissionAmount
        ? parseFloat(this.realtorCommissionAmount.toString())
        : null,
      isGstRequired: stringToBoolean(this.isGstRequired),
      gstCollectedAmount: this.gstCollectedAmount
        ? parseFloat(this.gstCollectedAmount.toString())
        : null,
      netBookAmount: this.netBookAmount ? parseFloat(this.netBookAmount.toString()) : null,
      totalCostAmount: this.totalCostAmount ? parseFloat(this.totalCostAmount.toString()) : null,
      sppAmount: this.sppAmount ? parseFloat(this.sppAmount.toString()) : null,
      remediationAmount: this.remediationAmount
        ? parseFloat(this.remediationAmount.toString())
        : null,
      dispositionPurchasers: this.dispositionPurchasers
        .filter(x => !!x.contact)
        .map(x => x.toApi())
        .filter((x): x is ApiGen_Concepts_DispositionSalePurchaser => x !== null),
      dispositionPurchaserAgent: this.dispositionPurchaserAgent?.toApi() ?? null,
      dispositionPurchaserSolicitor: this.dispositionPurchaserSolicitor?.toApi() ?? null,
      rowVersion: this.rowVersion ?? 0,
    };
  }
}

export const calculateNetProceedsBeforeSppAmount = (
  apiModel: ApiGen_Concepts_DispositionFileSale | null,
): number | null => {
  return !exists(apiModel)
    ? 0
    : (apiModel.finalSaleAmount ?? 0) -
        ((apiModel.realtorCommissionAmount ?? 0) +
          (apiModel.gstCollectedAmount ?? 0) +
          (apiModel.totalCostAmount ?? 0) +
          (apiModel.netBookAmount ?? 0));
};

export const calculateNetProceedsAfterSppAmount = (
  apiModel: ApiGen_Concepts_DispositionFileSale | null,
): number | null => {
  return !exists(apiModel)
    ? 0
    : (apiModel.finalSaleAmount ?? 0) -
        ((apiModel.realtorCommissionAmount ?? 0) +
          (apiModel.gstCollectedAmount ?? 0) +
          (apiModel.totalCostAmount ?? 0) +
          (apiModel.netBookAmount ?? 0) +
          (apiModel.sppAmount ?? 0));
};
