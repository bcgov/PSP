import {
  fromApiOrganization,
  fromApiPerson,
  IContactSearchResult,
  toOrganization,
  toPerson,
} from 'interfaces';
import { Api_ResearchFile, Api_ResearchFilePurpose } from 'models/api/ResearchFile';

export class ResearchFilePurposeFormModel {
  public id?: string;
  public researchPurposeTypeCode?: string;
  public researchPurposeTypeDescription?: string;

  public static fromApi(base: Api_ResearchFilePurpose): ResearchFilePurposeFormModel {
    var newModel = new ResearchFilePurposeFormModel();
    newModel.id = base.id;
    newModel.researchPurposeTypeCode = base.researchPurposeTypeCode?.id;
    newModel.researchPurposeTypeDescription = base.researchPurposeTypeCode?.description;
    return newModel;
  }

  public toApi(): Api_ResearchFilePurpose {
    return {
      id: this.id,
      researchPurposeTypeCode: {
        id: this.researchPurposeTypeCode,
        description: this.researchPurposeTypeDescription,
      },
    };
  }
}

export class UpdateResearchFormModel {
  public id?: number;
  public name?: string;
  public roadName?: string;
  public roadAlias?: string;
  public rfileNumber?: string;
  public statusTypeCode?: string;
  public statusTypeDescription?: string;
  //public researchProperties?: Api_ResearchFileProperty[];
  public requestDate?: string;
  public requestDescription?: string;
  public requestSourceDescription?: string;
  public researchResult?: string;
  public researchCompletionDate?: string;
  public isExpropriation?: string;
  public expropriationNotes?: string;
  public requestSourceTypeCode?: string;
  public requestSourceTypeDescription?: string;
  public requestor?: IContactSearchResult;
  public researchFilePurposes?: ResearchFilePurposeFormModel[];
  public rowVersion?: number;

  public static fromApi(base: Api_ResearchFile): UpdateResearchFormModel {
    var model = new UpdateResearchFormModel();
    model.id = base.id;
    model.name = base.name;
    model.roadName = base.roadName;
    model.roadAlias = base.roadAlias;
    model.rfileNumber = base.rfileNumber;
    model.statusTypeCode = base.researchFileStatusTypeCode?.id;
    model.statusTypeDescription = base.researchFileStatusTypeCode?.description;
    //model.researchProperties = base.researchProperties;
    model.requestDate = base.requestDate;
    model.requestDescription = base.requestDescription;
    model.requestSourceDescription = base.requestSourceDescription;
    model.researchResult = base.researchResult;
    model.researchCompletionDate = base.researchCompletionDate;
    model.isExpropriation = base.isExpropriation ? 'true' : 'false';
    model.expropriationNotes = base.expropriationNotes;
    model.requestSourceTypeCode = base.requestSourceType?.id;
    model.requestSourceTypeDescription = base.requestSourceType?.description;

    if (base.requestorPerson !== undefined) {
      model.requestor = fromApiPerson(base.requestorPerson);
    } else if (base.requestorOrganization !== undefined) {
      model.requestor = fromApiOrganization(base.requestorOrganization);
    }
    model.researchFilePurposes = base.researchFilePurposes?.map(x =>
      ResearchFilePurposeFormModel.fromApi(x),
    );
    model.rowVersion = base.rowVersion;
    return model;
  }

  public toApi(): Api_ResearchFile {
    return {
      id: this.id,
      name: this.name,
      roadName: this.roadName,
      roadAlias: this.roadAlias,
      rfileNumber: this.rfileNumber,
      researchFileStatusTypeCode: { id: this.statusTypeCode },
      //researchProperties: this.researchProperties,
      requestDate: this.requestDate,
      requestDescription: this.requestDescription,
      requestSourceDescription: this.requestSourceDescription,
      researchResult: this.researchResult,
      researchCompletionDate: this.researchCompletionDate,
      isExpropriation:
        this.isExpropriation === undefined
          ? undefined
          : this.isExpropriation === 'true'
          ? true
          : false,
      expropriationNotes: this.expropriationNotes,
      requestSourceType: { id: this.requestSourceTypeCode },
      rowVersion: this.rowVersion,
      requestorPerson: toPerson(this.requestor),
      requestorOrganization: toOrganization(this.requestor),
      //researchFilePurposes: this.researchFilePurposes?.map(x => x.toApi()),
    };
  }
}
