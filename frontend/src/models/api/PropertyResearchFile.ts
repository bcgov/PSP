import { Api_ConcurrentVersion } from './ConcurrentVersion';
import { Api_PropertyFile } from './PropertyFile';
import { Api_ResearchFile } from './ResearchFile';
import Api_TypeCode from './TypeCode';

export interface Api_PropertyPurpose extends Api_ConcurrentVersion {
  id?: number;
  propertyPurposeType?: Api_TypeCode<string>;
  propertyResearchFileId?: number;
}

export interface Api_PropertyResearchFile extends Api_PropertyFile {
  researchFile?: Api_ResearchFile;
  purposeTypes?: Api_PropertyPurpose[];
}
