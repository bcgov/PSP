import {
  fromApiOrganization,
  fromApiPerson,
  IContactSearchResult,
  toOrganization,
  toPerson,
} from '@/interfaces';
import { ApiGen_Concepts_ResearchFile } from '@/models/api/generated/ApiGen_Concepts_ResearchFile';
import { ApiGen_Concepts_ResearchFilePurpose } from '@/models/api/generated/ApiGen_Concepts_ResearchFilePurpose';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { exists } from '@/utils';
import { stringToNumber, toTypeCodeNullable } from '@/utils/formUtils';

import { ResearchFileProjectFormModel } from '../../../common/models';

export class ResearchFilePurposeFormModel {
  public id?: string;
  public researchPurposeTypeCode?: string;
  public researchPurposeTypeDescription?: string;

  public static fromApi(base: ApiGen_Concepts_ResearchFilePurpose): ResearchFilePurposeFormModel {
    const newModel = new ResearchFilePurposeFormModel();
    newModel.id = base.id.toString() ?? undefined;
    newModel.researchPurposeTypeCode = base.researchPurposeTypeCode?.id ?? undefined;
    newModel.researchPurposeTypeDescription =
      base.researchPurposeTypeCode?.description ?? undefined;
    return newModel;
  }

  public toApi(): ApiGen_Concepts_ResearchFilePurpose {
    return {
      id: stringToNumber(this.id),
      researchPurposeTypeCode: {
        id: this.researchPurposeTypeCode ?? null,
        description: this.researchPurposeTypeDescription ?? null,
        displayOrder: null,
        isDisabled: false,
      },
      ...getEmptyBaseAudit(),
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

  public static fromApi(base: ApiGen_Concepts_ResearchFile): UpdateResearchSummaryFormModel {
    const model = new UpdateResearchSummaryFormModel();
    model.id = base.id;
    model.name = base.fileName ?? undefined;
    model.roadName = base.roadName ?? undefined;
    model.roadAlias = base.roadAlias ?? undefined;
    model.rfileNumber = base.fileNumber ?? undefined;
    model.statusTypeCode = base.fileStatusTypeCode?.id ?? undefined;
    model.statusTypeDescription = base.fileStatusTypeCode?.description ?? undefined;
    model.requestDate = base.requestDate ?? undefined;
    model.requestDescription = base.requestDescription ?? undefined;
    model.requestSourceDescription = base.requestSourceDescription ?? undefined;
    model.researchResult = base.researchResult ?? undefined;
    model.researchCompletionDate = base.researchCompletionDate ?? undefined;
    model.isExpropriation = base.isExpropriation ? 'true' : 'false';
    model.expropriationNotes = base.expropriationNotes ?? undefined;
    model.requestSourceTypeCode = base.requestSourceType?.id ?? undefined;
    model.requestSourceTypeDescription = base.requestSourceType?.description ?? undefined;

    if (exists(base.requestorPerson)) {
      model.requestor = fromApiPerson(base.requestorPerson);
    } else if (exists(base.requestorOrganization)) {
      model.requestor = fromApiOrganization(base.requestorOrganization);
    }
    model.researchFilePurposes = base.researchFilePurposes?.map(x =>
      ResearchFilePurposeFormModel.fromApi(x),
    );
    model.researchFileProjects =
      base.researchFileProjects?.map(x => ResearchFileProjectFormModel.fromApi(x)) || [];
    model.rowVersion = base.rowVersion ?? undefined;
    return model;
  }

  public toApi(): ApiGen_Concepts_ResearchFile {
    return {
      id: this.id ?? 0,
      fileName: this.name ?? null,
      roadName: this.roadName ?? null,
      roadAlias: this.roadAlias ?? null,
      fileNumber: this.rfileNumber ?? null,
      fileStatusTypeCode: toTypeCodeNullable(this.statusTypeCode),
      requestDate: this.requestDate ?? null,
      requestDescription: this.requestDescription ?? null,
      requestSourceDescription: this.requestSourceDescription ?? null,
      researchResult: this.researchResult ?? null,
      researchCompletionDate: this.researchCompletionDate ?? null,
      isExpropriation: !exists(this.isExpropriation)
        ? null
        : this.isExpropriation === 'true'
        ? true
        : false,
      expropriationNotes: this.expropriationNotes ?? null,
      requestSourceType: toTypeCodeNullable(this.requestSourceTypeCode),
      requestorPerson: toPerson(this.requestor) ?? null,
      requestorOrganization: toOrganization(this.requestor) ?? null,
      researchFilePurposes: this.researchFilePurposes?.map(x => x.toApi()) ?? null,
      researchFileProjects: ResearchFileProjectFormModel.toApiList(this.researchFileProjects),
      fileProperties: [],
      ...getEmptyBaseAudit(this.rowVersion),
    };
  }
}
