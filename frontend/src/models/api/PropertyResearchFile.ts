import { Api_ConcurrentVersion } from './ConcurrentVersion';
import { Api_Property } from './Property';

export interface Api_PropertyResearchFile extends Api_ConcurrentVersion {
  id?: number;
  property: Api_Property;
  propertyId: number;
  researchFileId: number;
}
