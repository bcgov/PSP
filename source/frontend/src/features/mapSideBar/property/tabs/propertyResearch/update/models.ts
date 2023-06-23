import {
  Api_ResearchFileProperty,
  Api_ResearchFilePropertyPurposeType,
} from '@/models/api/ResearchFile';

export class PropertyResearchFilePurposeFormModel {
  public id?: number;
  public propertyPurposeTypeCode?: string;
  public propertyPurposeTypeDescription?: string;
  public version?: number;

  public static fromApi(
    base: Api_ResearchFilePropertyPurposeType,
  ): PropertyResearchFilePurposeFormModel {
    var newModel = new PropertyResearchFilePurposeFormModel();
    newModel.id = base.id;
    newModel.propertyPurposeTypeCode = base.propertyPurposeType?.id;
    newModel.propertyPurposeTypeDescription = base.propertyPurposeType?.description;
    newModel.version = base.rowVersion;
    return newModel;
  }

  public toApi(): Api_ResearchFilePropertyPurposeType {
    return {
      id: this.id,
      propertyPurposeType: {
        id: this.propertyPurposeTypeCode,
        description: this.propertyPurposeTypeDescription,
      },
      rowVersion: this.version,
    };
  }
}

export class UpdatePropertyFormModel {
  public id?: number;
  public propertyName?: string;
  public isDisabled?: boolean;
  public displayOrder?: number;
  public isLegalOpinionRequired?: string;
  public isLegalOpinionObtained?: string;
  public documentReference?: string;
  public researchSummary?: string;
  public propertyId?: number;
  public researchFileId?: number;
  public researchFileVersion?: number;
  public purposeTypes?: PropertyResearchFilePurposeFormModel[];
  public rowVersion?: number;

  public static fromApi(base: Api_ResearchFileProperty): UpdatePropertyFormModel {
    var model = new UpdatePropertyFormModel();
    model.id = base.id;
    model.propertyName = base.propertyName;
    model.isDisabled = base.isDisabled;
    model.displayOrder = base.displayOrder;
    model.isLegalOpinionRequired =
      base.isLegalOpinionRequired === undefined
        ? 'unknown'
        : base.isLegalOpinionRequired
        ? 'yes'
        : 'no';
    model.isLegalOpinionObtained =
      base.isLegalOpinionObtained === undefined
        ? 'unknown'
        : base.isLegalOpinionObtained
        ? 'yes'
        : 'no';
    model.documentReference = base.documentReference;
    model.researchSummary = base.researchSummary;
    model.propertyId = base.property?.id;
    model.researchFileId = base.file?.id;
    model.researchFileVersion = base.file?.rowVersion;

    model.purposeTypes = base.purposeTypes?.map((x: Api_ResearchFilePropertyPurposeType) =>
      PropertyResearchFilePurposeFormModel.fromApi(x),
    );
    model.rowVersion = base.rowVersion;
    return model;
  }

  public toApi(): Api_ResearchFileProperty {
    return {
      id: this.id,
      propertyId: this.propertyId,
      propertyName: this.propertyName,
      isDisabled: this.isDisabled,
      displayOrder: this.displayOrder,
      isLegalOpinionRequired:
        this.isLegalOpinionRequired === undefined || this.isLegalOpinionRequired === 'unknown'
          ? undefined
          : this.isLegalOpinionRequired === 'yes'
          ? true
          : false,
      isLegalOpinionObtained:
        this.isLegalOpinionObtained === undefined || this.isLegalOpinionObtained === 'unknown'
          ? undefined
          : this.isLegalOpinionObtained === 'yes'
          ? true
          : false,
      documentReference: this.documentReference,
      researchSummary: this.researchSummary,
      property: { id: this.propertyId },
      file: { id: this.researchFileId, rowVersion: this.researchFileVersion },
      purposeTypes: this.purposeTypes?.map(x => x.toApi()),
      rowVersion: this.rowVersion,
    };
  }
}
