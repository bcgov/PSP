import { ApiGen_Concepts_LeasePurpose } from '@/models/api/generated/ApiGen_Concepts_LeasePurpose';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { ILookupCode } from '@/store/slices/lookupCodes';

export class LeasePurposeModel {
  leaseId: number;
  purposeTypeCode: string;
  purposeTypeCodeDescription: string;
  purposeOtherDescription: string | null;

  constructor(readonly id: number | null = null, readonly rowVersion: number | null = null) {
    this.id = id;
    this.rowVersion = rowVersion;
  }

  static fromApi(base: ApiGen_Concepts_LeasePurpose): LeasePurposeModel {
    const newModel = new LeasePurposeModel(base.id, base.rowVersion);

    newModel.leaseId = base.leaseId;
    newModel.purposeTypeCode = base.leasePurposeTypeCode.id;
    newModel.purposeTypeCodeDescription = base.leasePurposeTypeCode.description;
    newModel.purposeOtherDescription = base.purposeOtherDescription;

    return newModel;
  }

  static fromLookup(base: ILookupCode): LeasePurposeModel {
    const newModel = new LeasePurposeModel();
    newModel.purposeTypeCode = base.id.toString();
    newModel.purposeTypeCodeDescription = base.name;
    newModel.purposeOtherDescription = null;

    return newModel;
  }

  toApi(leaseId: number): ApiGen_Concepts_LeasePurpose {
    return {
      id: this.id ?? 0,
      leaseId: leaseId,
      leasePurposeTypeCode: {
        id: this.purposeTypeCode,
        description: null,
        displayOrder: null,
        isDisabled: false,
      },
      purposeOtherDescription: this.purposeOtherDescription,
      ...getEmptyBaseAudit(this.rowVersion),
    };
  }
}
