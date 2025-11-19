import { ApiGen_Concepts_PropertyImprovement } from '@/models/api/generated/ApiGen_Concepts_PropertyImprovement';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { toTypeCodeNullable } from '@/utils/formUtils';

export class ILeaseImprovementsForm {
  improvements: ILeaseImprovementForm[] = [];
}

export class ILeaseImprovementForm {
  id?: number;
  propertyId?: number;
  propertyImprovementTypeId = '';
  propertyImprovementType = '';
  description = '';
  structureSize = '';
  address = '';
  rowVersion?: number;

  public static fromApi(improvement: ApiGen_Concepts_PropertyImprovement) {
    const form = new ILeaseImprovementForm();
    form.id = improvement.id ?? undefined;
    form.propertyId = improvement.propertyId ?? undefined;
    form.propertyImprovementTypeId = improvement.propertyImprovementTypeCode?.id ?? '';
    form.propertyImprovementType = improvement.propertyImprovementTypeCode?.description ?? '';
    form.description = improvement.improvementDescription ?? '';
    form.structureSize = improvement.structureSize ?? '';
    form.address = improvement.address ?? '';
    form.rowVersion = improvement.rowVersion ?? undefined;
    return form;
  }

  public static toApi(form: ILeaseImprovementForm) {
    const improvement: ApiGen_Concepts_PropertyImprovement = {
      id: form.id ?? null,
      propertyId: form.propertyId ?? null,
      property: null,
      propertyImprovementTypeCode: toTypeCodeNullable(form.propertyImprovementTypeId),
      improvementDescription: form.description,
      structureSize: form.structureSize,
      address: form.address,
      ...getEmptyBaseAudit(form.rowVersion),
    };
    return improvement;
  }
}
