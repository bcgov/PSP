import { ApiGen_Concepts_AcquisitionFileProgressStatuses } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileProgressStatuses';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { ILookupCode } from '@/store/slices/lookupCodes';

export class ProgressStatusModel {
  acquisitionFileId: number;
  progressTypeCode: string;
  progressTypeCodeDescription: string;

  constructor(readonly id: number | null = null, readonly rowVersion: number | null = null) {
    this.id = id;
    this.rowVersion = rowVersion;
  }

  static fromApi(base: ApiGen_Concepts_AcquisitionFileProgressStatuses): ProgressStatusModel {
    const newModel = new ProgressStatusModel(base.id, base.rowVersion);

    newModel.acquisitionFileId = base.acquisitionFileId;
    newModel.progressTypeCode = base.progressStatusTypeCode.id;
    newModel.progressTypeCodeDescription = base.progressStatusTypeCode.description;

    return newModel;
  }

  static fromLookup(base: ILookupCode): ProgressStatusModel {
    const newModel = new ProgressStatusModel();
    newModel.progressTypeCode = base.id.toString();
    newModel.progressTypeCodeDescription = base.name;

    return newModel;
  }

  toApi(acquisitionFileId: number): ApiGen_Concepts_AcquisitionFileProgressStatuses {
    return {
      id: this.id ?? 0,
      acquisitionFileId: acquisitionFileId,
      progressStatusTypeCode: {
        id: this.progressTypeCode,
        description: null,
        displayOrder: null,
        isDisabled: false,
      },
      ...getEmptyBaseAudit(this.rowVersion),
    };
  }
}
