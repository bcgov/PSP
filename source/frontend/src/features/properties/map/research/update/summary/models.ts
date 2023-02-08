import {
  fromApiOrganization,
  fromApiPerson,
  IAutocompletePrediction,
  IContactSearchResult,
  toOrganization,
  toPerson,
} from 'interfaces';
import {
  Api_ResearchFile,
  Api_ResearchFileProject,
  Api_ResearchFilePurpose,
} from 'models/api/ResearchFile';

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

export class ResearchFileProjectFormModel {
  public id?: number;
  public fileId?: number;
  public project?: IAutocompletePrediction;

  public static fromApi(base: Api_ResearchFileProject): ResearchFileProjectFormModel {
    const newModel = new ResearchFileProjectFormModel();
    newModel.id = base.id;
    newModel.fileId = base.fileId;
    newModel.project =
      base.project !== undefined
        ? { id: base.project.id!, text: base.project.description || '' }
        : undefined;
    return newModel;
  }

  public toApi(): Api_ResearchFileProject {
    return {
      id: this.id,
      fileId: this.fileId,
      project:
        this.project?.id !== undefined && this.project?.id !== 0
          ? { id: this.project?.id }
          : undefined,
    };
  }
}

export class UpdateResearchSummaryFormModel {
  public id?: number;
  public name?: string;
  public roadName?: string;
  public roadAlias?: string;
  public rfileNumber?: string;
  public statusTypeCode?: string;
  public statusTypeDescription?: string;
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
  public researchFileProjects: ResearchFileProjectFormModel[] = [];
  public rowVersion?: number;

  public static fromApi(base: Api_ResearchFile): UpdateResearchSummaryFormModel {
    var model = new UpdateResearchSummaryFormModel();
    model.id = base.id;
    model.name = base.fileName;
    model.roadName = base.roadName;
    model.roadAlias = base.roadAlias;
    model.rfileNumber = base.fileNumber;
    model.statusTypeCode = base.fileStatusTypeCode?.id;
    model.statusTypeDescription = base.fileStatusTypeCode?.description;
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
    model.researchFileProjects =
      base.researchFileProjects?.map(x => ResearchFileProjectFormModel.fromApi(x)) || [];
    model.rowVersion = base.rowVersion;
    return model;
  }

  public toApi(): Api_ResearchFile {
    return {
      id: this.id,
      fileName: this.name,
      roadName: this.roadName,
      roadAlias: this.roadAlias,
      fileNumber: this.rfileNumber,
      fileStatusTypeCode: { id: this.statusTypeCode },
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
      researchFilePurposes: this.researchFilePurposes?.map(x => x.toApi()),
      researchFileProjects: this.researchFileProjects
        .map(x => x.toApi())
        .filter(rp => rp?.project !== undefined),
    };
  }
}
