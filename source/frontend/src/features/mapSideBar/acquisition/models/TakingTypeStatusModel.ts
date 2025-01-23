import { ApiGen_Concepts_AcquisitionFileTakingStatuses } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileTakingStatuses';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { ILookupCode } from '@/store/slices/lookupCodes';

export class TakingTypeStatusModel {
  acquisitionFileId: number;
  takingTypeCode: string;
  takingTypeCodeDescription: string;

  constructor(readonly id: number | null = null, readonly rowVersion: number | null = null) {
    this.id = id;
    this.rowVersion = rowVersion;
  }

  static fromApi(base: ApiGen_Concepts_AcquisitionFileTakingStatuses): TakingTypeStatusModel {
    const newModel = new TakingTypeStatusModel(base.id, base.rowVersion);

    newModel.acquisitionFileId = base.acquisitionFileId;
    newModel.takingTypeCode = base.takingStatusTypeCode.id;
    newModel.takingTypeCodeDescription = base.takingStatusTypeCode.description;

    return newModel;
  }

  static fromLookup(base: ILookupCode): TakingTypeStatusModel {
    const newModel = new TakingTypeStatusModel();
    newModel.takingTypeCode = base.id.toString();
    newModel.takingTypeCodeDescription = base.name;

    return newModel;
  }

  toApi(acquisitionFileId: number): ApiGen_Concepts_AcquisitionFileTakingStatuses {
    return {
      id: this.id ?? 0,
      acquisitionFileId: acquisitionFileId,
      takingStatusTypeCode: {
        id: this.takingTypeCode,
        description: null,
        displayOrder: null,
        isDisabled: false,
      },
      ...getEmptyBaseAudit(this.rowVersion),
    };
  }
}
