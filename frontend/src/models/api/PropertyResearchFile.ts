import { Api_ConcurrentVersion } from './ConcurrentVersion';
import { Api_Property } from './Property';
import { Api_ResearchFile } from './ResearchFile';
import Api_TypeCode from './TypeCode';

export interface Api_PropertyPurpose extends Api_ConcurrentVersion {
  id?: number;
  propertyPurposeType?: Api_TypeCode<string>;
  propertyResearchFileId?: number;
}

export interface Api_PropertyResearchFile extends Api_ConcurrentVersion {
  id?: number;
  propertyName?: string;
  isDisabled?: boolean;
  displayOrder?: number;
  isLegalOpinionRequired?: boolean;
  isLegalOpinionObtained?: boolean;
  documentReference?: string;
  researchSummary?: string;
  property?: Api_Property;
  researchFile?: Api_ResearchFile;
  purposeTypes?: Api_PropertyPurpose[];
}
