import { ApiGen_Concepts_PropertyResearchFilePurpose } from '@/models/api/generated/ApiGen_Concepts_PropertyResearchFilePurpose';
import { ApiGen_Concepts_ResearchFileProperty } from '@/models/api/generated/ApiGen_Concepts_ResearchFileProperty';
import { getEmptyBaseAudit, getEmptyResearchFile } from '@/models/defaultInitializers';
import { ILookupCode } from '@/store/slices/lookupCodes';
import { exists } from '@/utils/utils';

export class PropertyResearchFilePurposeFormModel {
  public id: number | null = null;
  public rowVersion: number | null = null;
  public propertyResearchPurposeTypeCode?: string;
  public propertyPurposeTypeDescription?: string;

  public static fromApi(
    base: ApiGen_Concepts_PropertyResearchFilePurpose,
  ): PropertyResearchFilePurposeFormModel {
    const newModel = new PropertyResearchFilePurposeFormModel();
    newModel.id = base.id;
    newModel.propertyResearchPurposeTypeCode =
      base.propertyResearchPurposeTypeCode?.id ?? undefined;
    newModel.propertyPurposeTypeDescription =
      base.propertyResearchPurposeTypeCode?.description ?? undefined;
    newModel.rowVersion = base.rowVersion ?? null;
    return newModel;
  }

  static fromLookup(base: ILookupCode): PropertyResearchFilePurposeFormModel {
    const newModel = new PropertyResearchFilePurposeFormModel();
    newModel.propertyResearchPurposeTypeCode = base.id.toString();
    newModel.propertyPurposeTypeDescription = base.name;

    return newModel;
  }

  public toApi(): ApiGen_Concepts_PropertyResearchFilePurpose {
    return {
      id: this.id ?? 0,
      propertyResearchPurposeTypeCode: {
        id: this.propertyResearchPurposeTypeCode ?? null,
        description: this.propertyPurposeTypeDescription ?? null,
        displayOrder: null,
        isDisabled: false,
      },
      rowVersion: this.rowVersion ?? null,
      ...getEmptyBaseAudit(),
    };
  }
}

export class UpdatePropertyFormModel {
  public id?: number;
  public propertyName?: string;
  public displayOrder?: number;
  public isLegalOpinionRequired?: string;
  public isLegalOpinionObtained?: string;
  public documentReference?: string;
  public researchSummary?: string;
  public propertyId?: number;

  public propertyResearchPurposeTypes?: PropertyResearchFilePurposeFormModel[];
  public rowVersion?: number;

  public researchFileRowVersion: number | null = null;
  public researchFileId: number | null = null;

  public static fromApi(base: ApiGen_Concepts_ResearchFileProperty): UpdatePropertyFormModel {
    const model = new UpdatePropertyFormModel();
    model.id = base.id;
    model.propertyName = base.propertyName ?? undefined;
    model.displayOrder = base.displayOrder ?? undefined;
    model.isLegalOpinionRequired = !exists(base.isLegalOpinionRequired)
      ? 'unknown'
      : base.isLegalOpinionRequired
      ? 'yes'
      : 'no';
    model.isLegalOpinionObtained = !exists(base.isLegalOpinionObtained)
      ? 'unknown'
      : base.isLegalOpinionObtained
      ? 'yes'
      : 'no';
    model.documentReference = base.documentReference ?? undefined;
    model.researchSummary = base.researchSummary ?? undefined;
    model.propertyId = base.property?.id;
    model.researchFileRowVersion = base?.file?.rowVersion ?? null;
    model.researchFileId = base?.fileId ?? null;

    model.propertyResearchPurposeTypes = base.propertyResearchPurposeTypes?.map(
      (x: ApiGen_Concepts_PropertyResearchFilePurpose) =>
        PropertyResearchFilePurposeFormModel.fromApi(x),
    );
    model.rowVersion = base.rowVersion ?? undefined;
    return model;
  }

  public toApi(): ApiGen_Concepts_ResearchFileProperty {
    return {
      id: this.id ?? 0,
      propertyId: this.propertyId ?? 0,
      propertyName: this.propertyName ?? null,
      displayOrder: this.displayOrder ?? null,
      isLegalOpinionRequired:
        !exists(this.isLegalOpinionRequired) || this.isLegalOpinionRequired === 'unknown'
          ? null
          : this.isLegalOpinionRequired === 'yes'
          ? true
          : false,
      isLegalOpinionObtained:
        !exists(this.isLegalOpinionObtained) || this.isLegalOpinionObtained === 'unknown'
          ? null
          : this.isLegalOpinionObtained === 'yes'
          ? true
          : false,
      documentReference: this.documentReference ?? null,
      researchSummary: this.researchSummary ?? null,
      property: null,
      location: null,
      isActive: null,
      fileId: this.researchFileId ?? 0,
      file: { ...getEmptyResearchFile(), rowVersion: this.researchFileRowVersion },
      propertyResearchPurposeTypes: this.propertyResearchPurposeTypes?.map(x => x.toApi()) ?? null,
      rowVersion: this.rowVersion ?? null,
    };
  }
}
