import { ApiGen_Concepts_ResearchFile } from '@/models/api/generated/ApiGen_Concepts_ResearchFile';
import { ApiGen_Concepts_ResearchFileProperty } from '@/models/api/generated/ApiGen_Concepts_ResearchFileProperty';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { applyDisplayOrder } from '@/utils';

import { PropertyForm } from '../../shared/models';
import { ResearchFileProjectFormModel } from '../common/models';

export class ResearchForm {
  public id?: number;
  public name: string;
  public properties: PropertyForm[];
  public researchFileProjects: ResearchFileProjectFormModel[];
  public rowVersion?: number;

  constructor() {
    this.name = '';
    this.properties = [];
    this.researchFileProjects = [];
  }

  public toApi(): ApiGen_Concepts_ResearchFile {
    const fileProperties = this.properties.map(x => this.toPropertyApi(x));
    const sortedProperties = applyDisplayOrder(fileProperties);
    return {
      id: this.id ?? 0,
      fileName: this.name,
      fileProperties: sortedProperties,
      researchFileProjects: ResearchFileProjectFormModel.toApiList(this.researchFileProjects),
      ...getEmptyBaseAudit(this.rowVersion),
      expropriationNotes: null,
      fileNumber: null,
      fileStatusTypeCode: null,
      isExpropriation: null,
      requestDate: null,
      requestDescription: null,
      requestorOrganization: null,
      requestorPerson: null,
      requestSourceDescription: null,
      requestSourceType: null,
      researchCompletionDate: null,
      researchFilePurposes: null,
      researchResult: null,
      roadAlias: null,
      roadName: null,
      totalAllowableCompensation: null,
    };
  }

  private toPropertyApi(x: PropertyForm): ApiGen_Concepts_ResearchFileProperty {
    const apiFileProperty = x.toFilePropertyApi(this.id);
    return {
      ...apiFileProperty,
      file: null,
      documentReference: null,
      isLegalOpinionObtained: null,
      isLegalOpinionRequired: null,
      propertyResearchPurposeTypes: null,
      researchSummary: null,
    };
  }

  public static fromApi(model: ApiGen_Concepts_ResearchFile): ResearchForm {
    const newForm = new ResearchForm();
    newForm.id = model.id;
    newForm.name = model.fileName || '';
    newForm.properties = model.fileProperties?.map(x => PropertyForm.fromApi(x)) || [];
    newForm.researchFileProjects =
      model.researchFileProjects?.map(x => ResearchFileProjectFormModel.fromApi(x)) || [];
    newForm.rowVersion = model.rowVersion ?? undefined;

    return newForm;
  }
}
