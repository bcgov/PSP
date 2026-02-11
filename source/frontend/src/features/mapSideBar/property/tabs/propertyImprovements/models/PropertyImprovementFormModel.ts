import { ApiGen_Concepts_PropertyImprovement } from '@/models/api/generated/ApiGen_Concepts_PropertyImprovement';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { fromTypeCode, toTypeCodeNullable } from '@/utils/formUtils';
import { isValidIsoDateTime } from '@/utils/utils';

export class PropertyImprovementFormModel {
  name: string | null = '';
  description: string | null = '';
  improvementTypeCode: string | null = '';
  improvementStatusCode: string | null = '';
  improvementDate: string | null = '';

  constructor(
    readonly id: number | null = 0,
    readonly propertyId: number,
    readonly rowVersion = 0,
  ) {
    this.id = id;
    this.propertyId = propertyId;
    this.rowVersion = rowVersion;
  }

  public toApi(): ApiGen_Concepts_PropertyImprovement {
    return {
      id: this.id,
      propertyId: this.propertyId,
      property: null,
      improvementName: this.name,
      improvementDescription: this.description,
      improvementDate: isValidIsoDateTime(this.improvementDate) ? this.improvementDate : null,
      improvementTypeCode: toTypeCodeNullable(this.improvementTypeCode),
      improvementStatusCode: toTypeCodeNullable(this.improvementStatusCode),
      ...getEmptyBaseAudit(this.rowVersion),
    };
  }

  static fromApi(base: ApiGen_Concepts_PropertyImprovement): PropertyImprovementFormModel {
    const form = new PropertyImprovementFormModel(base.id, base.propertyId, base.rowVersion);

    form.name = base.improvementName ?? '';
    form.description = base.improvementDescription ?? '';
    form.improvementDate = base.improvementDate ?? '';
    form.improvementTypeCode = fromTypeCode(base.improvementTypeCode) ?? '';
    form.improvementStatusCode = fromTypeCode(base.improvementStatusCode) ?? '';

    return form;
  }
}
