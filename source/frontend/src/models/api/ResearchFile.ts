import { Api_File } from '@/models/api/File';
import { Api_PropertyFile } from '@/models/api/PropertyFile';
import Api_TypeCode from '@/models/api/TypeCode';

import { Api_AuditFields } from './AuditFields';
import { Api_ConcurrentVersion } from './ConcurrentVersion';
import { Api_Organization } from './Organization';
import { Api_Person } from './Person';
import { Api_Project } from './Project';

export interface Api_ResearchFilePropertyPurposeType extends Api_ConcurrentVersion {
  id?: number;
  propertyPurposeType?: Api_TypeCode<string>;
  propertyResearchFileId?: number;
}

export interface Api_ResearchFileProperty extends Api_PropertyFile, Api_ConcurrentVersion {
  displayOrder?: number;
  isLegalOpinionRequired?: boolean;
  isLegalOpinionObtained?: boolean;
  documentReference?: string;
  researchSummary?: string;
  purposeTypes?: Api_ResearchFilePropertyPurposeType[];
}

export interface Api_ResearchFile extends Api_File, Api_AuditFields {
  roadName?: string;
  roadAlias?: string;
  requestDate?: string;
  requestDescription?: string;
  requestSourceDescription?: string;
  researchResult?: string;
  researchCompletionDate?: string;
  isExpropriation?: boolean;
  expropriationNotes?: string;
  requestSourceType?: Api_TypeCode<string>;
  requestorPerson?: Api_Person;
  requestorOrganization?: Api_Organization;
  researchFilePurposes?: Api_ResearchFilePurpose[];
  researchFileProjects?: Api_ResearchFileProject[];
}

export interface Api_ResearchFilePurpose extends Api_ConcurrentVersion, Api_AuditFields {
  id?: string;
  researchPurposeTypeCode?: Api_TypeCode<string>;
}

export interface Api_ResearchFileProject extends Api_ConcurrentVersion, Api_AuditFields {
  id: number | undefined;
  fileId: number | undefined;
  project: Api_Project | undefined;
  projectId: number | null;
  isDisabled: boolean | undefined;
}
