import Api_TypeCode from 'models/api/TypeCode';

import { Api_ConcurrentVersion } from './ConcurrentVersion';
import { Api_Organization } from './Organization';
import { Api_Person } from './Person';
import { Api_Property } from './Property';

export interface Api_ResearchFileProperty extends Api_ConcurrentVersion {
  id?: number;
  isDisabled?: boolean;
  displayOrder?: number;
  property?: Api_Property;
  researchFile?: Api_ResearchFile;
}

export interface Api_ResearchFile extends Api_ConcurrentVersion {
  id?: number;
  name?: string;
  roadName?: string;
  roadAlias?: string;
  rfileNumber?: string;
  researchFileStatusTypeCode?: Api_TypeCode<string>;
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
  appCreateTimestamp?: string;
  appLastUpdateTimestamp?: string;
  appLastUpdateUserid?: string;
  appCreateUserid?: string;
}

export interface Api_ResearchFilePurpose extends Api_ConcurrentVersion {
  id?: string;
  researchPurposeTypeCode?: Api_TypeCode<string>;
  appCreateTimestamp?: string;
  appLastUpdateTimestamp?: string;
  appLastUpdateUserid?: string;
  appCreateUserid?: string;
}
