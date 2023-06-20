import { Api_PropertyImprovement } from '@/models/api/PropertyImprovement';

export class ILeaseImprovementsForm {
  improvements: ILeaseImprovementForm[] = [];
}

export class ILeaseImprovementForm {
  id?: number;
  leaseId?: number;
  propertyImprovementTypeId: string = '';
  propertyImprovementType: string = '';
  description: string = '';
  structureSize: string = '';
  address: string = '';
  rowVersion?: number;

  public static fromApi(improvement: Api_PropertyImprovement) {
    const form = new ILeaseImprovementForm();
    form.id = improvement.id ?? undefined;
    form.leaseId = improvement.leaseId ?? undefined;
    form.propertyImprovementTypeId = improvement.propertyImprovementTypeCode?.id ?? '';
    form.propertyImprovementType = improvement.propertyImprovementTypeCode?.description ?? '';
    form.description = improvement.improvementDescription ?? '';
    form.structureSize = improvement.structureSize ?? '';
    form.address = improvement.address ?? '';
    form.rowVersion = improvement.rowVersion ?? undefined;
    return form;
  }

  public static toApi(form: ILeaseImprovementForm) {
    const improvement: Api_PropertyImprovement = {
      id: form.id ?? null,
      leaseId: form.leaseId ?? null,
      lease: null,
      propertyImprovementTypeCode: { id: form.propertyImprovementTypeId },
      improvementDescription: form.description,
      structureSize: form.structureSize,
      address: form.address,
      rowVersion: form.rowVersion,
    };
    return improvement;
  }
}
