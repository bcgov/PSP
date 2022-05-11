import { Api_PropertyPurpose, Api_PropertyResearchFile } from 'models/api/PropertyResearchFile';

export class PropertyResearchFilePurposeFormModel {
  public id?: number;
  public propertyPurposeTypeCode?: string;
  public propertyPurposeTypeDescription?: string;
  public version?: number;

  public static fromApi(base: Api_PropertyPurpose): PropertyResearchFilePurposeFormModel {
    var newModel = new PropertyResearchFilePurposeFormModel();
    newModel.id = base.id;
    newModel.propertyPurposeTypeCode = base.propertyPurposeType?.id;
    newModel.propertyPurposeTypeDescription = base.propertyPurposeType?.description;
    newModel.version = base.rowVersion;
    return newModel;
  }

  public toApi(): Api_PropertyPurpose {
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
  public reaserchFileVersion?: number;
  public purposeTypes?: PropertyResearchFilePurposeFormModel[];
  public rowVersion?: number;

  public static fromApi(base: Api_PropertyResearchFile): UpdatePropertyFormModel {
    var model = new UpdatePropertyFormModel();
    model.id = base.id;
    model.propertyName = base.propertyName;
    model.isDisabled = base.isDisabled;
    model.displayOrder = base.displayOrder;
    model.isLegalOpinionRequired =
      base.isLegalOpinionRequired === undefined
        ? 'unknown'
        : base.isLegalOpinionRequired
        ? 'true'
        : 'false';
    model.isLegalOpinionObtained =
      base.isLegalOpinionObtained === undefined
        ? 'unknown'
        : base.isLegalOpinionObtained
        ? 'true'
        : 'false';
    model.documentReference = base.documentReference;
    model.researchSummary = base.researchSummary;
    model.propertyId = base.property?.id;
    model.researchFileId = base.researchFile?.id;
    model.reaserchFileVersion = base.researchFile?.rowVersion;

    model.purposeTypes = base.purposeTypes?.map(x =>
      PropertyResearchFilePurposeFormModel.fromApi(x),
    );
    model.rowVersion = base.rowVersion;
    return model;
  }

  public toApi(): Api_PropertyResearchFile {
    return {
      id: this.id,
      propertyName: this.propertyName,
      isDisabled: this.isDisabled,
      displayOrder: this.displayOrder,
      isLegalOpinionRequired:
        this.isLegalOpinionRequired === undefined || this.isLegalOpinionRequired === 'unknown'
          ? undefined
          : this.isLegalOpinionRequired === 'true'
          ? true
          : false,
      isLegalOpinionObtained:
        this.isLegalOpinionObtained === undefined || this.isLegalOpinionObtained === 'unknown'
          ? undefined
          : this.isLegalOpinionObtained === 'true'
          ? true
          : false,
      documentReference: this.documentReference,
      researchSummary: this.researchSummary,
      property: { id: this.propertyId },
      researchFile: { id: this.researchFileId, rowVersion: this.reaserchFileVersion },
      purposeTypes: this.purposeTypes?.map(x => x.toApi()),
      rowVersion: this.rowVersion,
    };
  }
}
