import { ApiGen_Concepts_CompensationRequisitionProperty } from '@/models/api/generated/ApiGen_Concepts_CompensationRequisitionProperty';

export class CompensationRequisitionPropertyForm {
  constructor(
    readonly id: number | null = null,
    readonly compensationRequisitionId: number,
    readonly propertyAcquisitionFileId: number,
    readonly rowVersion: number = 0,
  ) {
    this.id = id;
    this.compensationRequisitionId = compensationRequisitionId;
    this.rowVersion = rowVersion;
    this.propertyAcquisitionFileId = propertyAcquisitionFileId;
  }

  static fromApi(
    apiModel: ApiGen_Concepts_CompensationRequisitionProperty,
  ): CompensationRequisitionPropertyForm {
    const formModel = new CompensationRequisitionPropertyForm(
      apiModel.compensationRequisitionPropertyId,
      apiModel.compensationRequisitionId,
      apiModel.propertyAcquisitionFileId,
      apiModel.rowVersion,
    );

    return formModel;
  }

  toApi(): ApiGen_Concepts_CompensationRequisitionProperty {
    return {
      compensationRequisitionPropertyId: this.id,
      compensationRequisitionId: this.compensationRequisitionId,
      propertyAcquisitionFileId: this.propertyAcquisitionFileId,
      rowVersion: this.rowVersion,
    } as ApiGen_Concepts_CompensationRequisitionProperty;
  }
}
