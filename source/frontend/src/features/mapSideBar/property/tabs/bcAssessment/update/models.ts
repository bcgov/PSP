import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { EpochIsoDateTime } from '@/models/api/UtcIsoDateTime';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { exists } from '@/utils';

export class PropertyNetBookFormModel {
  propertyId = 0;
  rowVersion = 0;
  netBookAmount: number | null = null;
  netBookNote: string | null = null;

  static fromApi(apiModel: ApiGen_Concepts_Property | null): PropertyNetBookFormModel {
    const model = new PropertyNetBookFormModel();
    if (exists(apiModel)) {
      model.propertyId = apiModel.id;
      model.rowVersion = apiModel.rowVersion;
      model.netBookAmount = apiModel.netBookAmount;
      model.netBookNote = apiModel.netBookNote;
    }
    return model;
  }

  toApi(): ApiGen_Concepts_Property {
    return {
      id: this.propertyId,
      netBookAmount: this.netBookAmount ?? 0,
      netBookNote: this.netBookNote ?? null,
      propertyType: null,
      anomalies: null,
      tenures: null,
      roadTypes: null,
      status: null,
      dataSource: null,
      region: null,
      district: null,
      dataSourceEffectiveDateOnly: EpochIsoDateTime,
      latitude: null,
      longitude: null,
      isRetired: false,
      pphStatusUpdateUserid: null,
      pphStatusUpdateTimestamp: null,
      pphStatusUpdateUserGuid: null,
      isRwyBeltDomPatent: null,
      pphStatusTypeCode: null,
      address: null,
      pid: null,
      pin: null,
      planNumber: null,
      isOwned: false,
      areaUnit: null,
      landArea: null,
      isVolumetricParcel: null,
      volumetricMeasurement: null,
      volumetricUnit: null,
      volumetricType: null,
      landLegalDescription: null,
      municipalZoning: null,
      location: null,
      boundary: null,
      generalLocation: null,
      surplusDeclarationType: null,
      surplusDeclarationComment: null,
      surplusDeclarationDate: EpochIsoDateTime,
      historicalFileNumbers: null,
      tenureCleanups: null,
      ...getEmptyBaseAudit(this.rowVersion),
    };
  }
}
