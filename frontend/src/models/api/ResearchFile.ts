import { Api_File } from 'models/api/File';
import Api_TypeCode from 'models/api/TypeCode';

import { Api_AuditFields } from './AuditFields';
import { Api_ConcurrentVersion } from './ConcurrentVersion';
import { Api_Organization } from './Organization';
import { Api_Person } from './Person';
import { Api_Property } from './Property';

export interface Api_ResearchFilePropertyPurposeType extends Api_ConcurrentVersion {
  id?: number;
  propertyPurposeType?: Api_TypeCode<string>;
  propertyResearchFileId?: number;
}

export interface Api_ResearchFileProperty extends Api_ConcurrentVersion {
  id?: number;
  isDisabled?: boolean;
  displayOrder?: number;
  property?: Api_Property;
  researchFile?: Api_ResearchFile;
  propertyName?: string;
  isLegalOpinionRequired?: boolean;
  isLegalOpinionObtained?: boolean;
  documentReference?: string;
  researchSummary?: string;
  purposeTypes?: Api_ResearchFilePropertyPurposeType[];
}

export interface Api_ResearchFile extends Api_File, Api_AuditFields {
  roadName?: string;
  roadAlias?: string;
  researchProperties?: Api_ResearchFileProperty[];
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
}

export interface Api_ResearchFilePurpose extends Api_ConcurrentVersion, Api_AuditFields {
  id?: string;
  researchPurposeTypeCode?: Api_TypeCode<string>;
}
