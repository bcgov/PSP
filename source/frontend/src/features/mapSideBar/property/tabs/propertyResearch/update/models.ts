import { ApiGen_Concepts_PropertyPurpose } from '@/models/api/generated/ApiGen_Concepts_PropertyPurpose';
import { ApiGen_Concepts_ResearchFile } from '@/models/api/generated/ApiGen_Concepts_ResearchFile';
import { ApiGen_Concepts_ResearchFileProperty } from '@/models/api/generated/ApiGen_Concepts_ResearchFileProperty';
import { exists } from '@/utils/utils';

export class PropertyResearchFilePurposeFormModel {
  public id?: number;
  public propertyPurposeTypeCode?: string;
  public propertyPurposeTypeDescription?: string;
  public version?: number;

  public static fromApi(
    base: ApiGen_Concepts_PropertyPurpose,
  ): PropertyResearchFilePurposeFormModel {
    const newModel = new PropertyResearchFilePurposeFormModel();
    newModel.id = base.id;
    newModel.propertyPurposeTypeCode = base.propertyPurposeType?.id ?? undefined;
    newModel.propertyPurposeTypeDescription = base.propertyPurposeType?.description ?? undefined;
    newModel.version = base.rowVersion ?? undefined;
    return newModel;
  }

  public toApi(): ApiGen_Concepts_PropertyPurpose {
    return {
      id: this.id ?? 0,
      propertyPurposeType: {
        id: this.propertyPurposeTypeCode ?? null,
        description: this.propertyPurposeTypeDescription ?? null,
        displayOrder: null,
        isDisabled: false,
      },
      rowVersion: this.version ?? null,
      propertyResearchFileId: 0,
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

  public purposeTypes?: PropertyResearchFilePurposeFormModel[];
  public rowVersion?: number;

  public researchFile: ApiGen_Concepts_ResearchFile | null = null;

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
    model.researchFile = base.file;

    model.purposeTypes = base.purposeTypes?.map((x: ApiGen_Concepts_PropertyPurpose) =>
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
      fileId: this.researchFile?.id ?? 0,
      file: this.researchFile,
      purposeTypes: this.purposeTypes?.map(x => x.toApi()) ?? null,
      rowVersion: this.rowVersion ?? null,
    };
  }
}
