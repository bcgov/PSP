import { ApiGen_Concepts_PropertyImprovement } from '@/models/api/generated/ApiGen_Concepts_PropertyImprovement';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { fromTypeCode, toTypeCodeNullable } from '@/utils/formUtils';

export class PropertyImprovementFormModel {
  propertyImprovementTypeCode: string | null = '';
  description = '';

  constructor(
    readonly id: number | null = null,
    readonly propertyId: number,
    readonly rowVersion = 0,
  ) {
    this.id = id;
    this.propertyId = propertyId;
  }

  public static fromApi(base: ApiGen_Concepts_PropertyImprovement): PropertyImprovementFormModel {
    const form = new PropertyImprovementFormModel(base.id, base.propertyId, base.rowVersion);
    form.propertyImprovementTypeCode = fromTypeCode(base.propertyImprovementTypeCode) ?? '';
    form.description = base.improvementDescription;

    return form;
  }

  public toApi() {
    return {
      id: this.id,
      propertyId: this.propertyId,
      property: null,
      propertyImprovementTypeCode: toTypeCodeNullable(this.propertyImprovementTypeCode),
      improvementDescription: this.description,
      ...getEmptyBaseAudit(this.rowVersion),
    };
  }
}
