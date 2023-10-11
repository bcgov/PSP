import { Api_File } from '@/models/api/File';

import { Api_ConcurrentVersion } from './ConcurrentVersion';
import { Api_Property } from './Property';

export interface Api_PropertyFile extends Api_ConcurrentVersion {
  id?: number;
  file?: Api_File;
  fileId?: number;
  propertyName?: string;
  isDisabled?: boolean;
  displayOrder?: number;
  isLegalOpinionRequired?: boolean;
  isLegalOpinionObtained?: boolean;
  documentReference?: string;
  researchSummary?: string;
  property?: Api_Property;
  propertyId?: number;
}
