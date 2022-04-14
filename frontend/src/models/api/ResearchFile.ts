import Api_TypeCode from 'models/api/TypeCode';

import { Api_ConcurrentVersion } from './ConcurrentVersion';
import { Api_Property } from './Property';

export interface Api_ResearchFile extends Api_ConcurrentVersion {
  id?: number;
  name?: string;
  rfileNumber?: string;
  statusType?: Api_TypeCode<string>;
  researchProperties?: Api_ResearchFileProperty[];
  appCreateTimestamp?: string;
  updatedOn?: string;
  updatedByName?: string | null;
}

export interface Api_ResearchProperty extends Api_ConcurrentVersion {
  researchFile?: Api_ResearchFile;
  property?: Api_Property;
  description?: string;
  displayOrder?: number;
  isDisabled?: boolean;
}

export interface Api_ResearchFileProperty extends Api_ConcurrentVersion {
  id?: number;
  isDisabled?: boolean;
  displayOrder?: number;
  property?: Api_Property;
  researchFile?: Api_ResearchFile;
}
