import { Api_ConcurrentVersion } from './ConcurrentVersion';

export interface Api_ResearchFile extends Api_ConcurrentVersion {
  name?: string;
  rfileNumber?: string;
  properties?: Api_ResearchProperty[];
}

export interface Api_ResearchProperty extends Api_ConcurrentVersion {
  researchFile?: Api_ResearchFile;
  property?: Api_Property;
  displayOrder?: number;
  isDisabled?: boolean;
}

export interface Api_Property extends Api_ConcurrentVersion {
  id?: string;
  pid?: string;
}
