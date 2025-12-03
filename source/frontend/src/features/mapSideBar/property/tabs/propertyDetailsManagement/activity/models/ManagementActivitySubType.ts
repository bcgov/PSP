import { ApiGen_Base_CodeType } from '@/models/api/generated/ApiGen_Base_CodeType';
import { ApiGen_Concepts_ManagementActivitySubType } from '@/models/api/generated/ApiGen_Concepts_ManagementActivitySubType';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { ILookupCode } from '@/store/slices/lookupCodes';

export class ManagementActivitySubTypeModel {
  subTypeCode: string;
  subTypeCodeDescription: string;

  constructor(
    readonly managementActivityId: number | null = null,
    readonly id: number | null = null,
    readonly rowVersion: number | null = null,
  ) {
    this.managementActivityId = managementActivityId;
    this.id = id;
    this.rowVersion = rowVersion;
  }

  static fromApi(base: ApiGen_Concepts_ManagementActivitySubType): ManagementActivitySubTypeModel {
    const newModel = new ManagementActivitySubTypeModel(
      base.managementActivityId,
      base.id,
      base.rowVersion,
    );

    newModel.subTypeCode = base.managementActivitySubtypeCode.id;
    newModel.subTypeCodeDescription = base.managementActivitySubtypeCode.description;

    return newModel;
  }

  static fromLookup(
    managementActivityId: number | null = null,
    base: ILookupCode,
  ): ManagementActivitySubTypeModel {
    const newModel = new ManagementActivitySubTypeModel(managementActivityId);
    newModel.subTypeCode = base.id.toString();
    newModel.subTypeCodeDescription = base.name;

    return newModel;
  }

  toApi(managementActivityId: number | null = null): ApiGen_Concepts_ManagementActivitySubType {
    return {
      id: this.id ?? 0,
      managementActivityId: managementActivityId ?? 0,
      managementActivitySubtypeCode: {
        id: this.subTypeCode,
      } as ApiGen_Base_CodeType<string>,
      ...getEmptyBaseAudit(this.rowVersion),
    } as ApiGen_Concepts_ManagementActivitySubType;
  }
}
