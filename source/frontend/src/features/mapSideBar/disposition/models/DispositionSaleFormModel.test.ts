import { mockDispositionSaleApi } from '@/mocks/dispositionFiles.mock';

import {
  calculateNetProceedsAfterSppAmount,
  calculateNetProceedsBeforeSppAmount,
  DispositionSaleFormModel,
} from './DispositionSaleFormModel';

describe('DispositionSaleFormModel tests', () => {
  const mockDispositionFileApi = mockDispositionSaleApi(10, 1);

  it('It Generates the Model from the API entity response', () => {
    const defaultValuesModel = new DispositionSaleFormModel(null, 1, null);

    expect(defaultValuesModel.id).toBe(null);
    expect(defaultValuesModel.dispositionFileId).toBe(1);
    expect(defaultValuesModel.rowVersion).toBe(null);
  });

  it('It Generates the Model from the API entity response', () => {
    const modelFromApi = DispositionSaleFormModel.fromApi(mockDispositionFileApi);

    expect(modelFromApi.id).toBe(10);
    expect(modelFromApi.dispositionFileId).toBe(1);
    expect(modelFromApi.finalConditionRemovalDate).toBe('2024-01-26');
    expect(modelFromApi.saleCompletionDate).toBe('2024-01-27');
    expect(modelFromApi.saleFiscalYear).toBe('2023');
    expect(modelFromApi.finalSaleAmount).toBe(2500000);
    expect(modelFromApi.realtorCommissionAmount).toBe(1000);
    expect(modelFromApi.isGstRequired).toBe(true);
    expect(modelFromApi.gstCollectedAmount).toBe(125000);
    expect(modelFromApi.netBookAmount).toBe(2000);
    expect(modelFromApi.totalCostAmount).toBe(3000);
    expect(modelFromApi.sppAmount).toBe(4000);
    expect(modelFromApi.remediationAmount).toBe(5000);
    expect(modelFromApi.netProceedsBeforeSppAmount).toBe(2369000);
    expect(modelFromApi.netProceedsAfterSppAmount).toBe(2365000);

    expect(modelFromApi.dispositionPurchasers).toHaveLength(3);

    expect(modelFromApi.dispositionPurchaserAgent).not.toBeNull();
    expect(modelFromApi.dispositionPurchaserAgent.id).toBe(300);
    expect(modelFromApi.dispositionPurchaserAgent.contact).not.toBeNull();
    expect(modelFromApi.dispositionPurchaserAgent.contact?.organizationId).toBe(3);
    expect(modelFromApi.dispositionPurchaserAgent.primaryContactId).toBe('3');

    expect(modelFromApi.dispositionPurchaserSolicitor).not.toBeNull();

    expect(modelFromApi.rowVersion).toBe(1);
  });

  it('It calculates the Net Proceeds amounts', () => {
    const netProceedsBeforeNull = calculateNetProceedsBeforeSppAmount(null);
    const netProceedsAfterSPPNull = calculateNetProceedsAfterSppAmount(null);

    expect(netProceedsBeforeNull).toBe(0);
    expect(netProceedsAfterSPPNull).toBe(0);

    const netProceedsBeforeSPPAmount = calculateNetProceedsBeforeSppAmount(mockDispositionFileApi);
    const netProceedsAfterSPPAmount = calculateNetProceedsAfterSppAmount(mockDispositionFileApi);

    expect(netProceedsBeforeSPPAmount).toBe(2369000);
    expect(netProceedsAfterSPPAmount).toBe(2365000);
  });

  it('It calculates the Net Proceeds amounts', () => {
    const defaultValuesModel = new DispositionSaleFormModel(null, 1, null);
    defaultValuesModel.saleFiscalYear = '2023';

    const apiModel = defaultValuesModel.toApi();

    expect(apiModel.id).toBeNull();
    expect(apiModel.dispositionFileId).toBe(1);
    expect(apiModel.saleFiscalYear).toBe('2023');
    expect(apiModel.rowVersion).toBe(0);
  });
});
