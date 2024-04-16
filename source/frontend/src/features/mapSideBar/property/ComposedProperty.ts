import { FeatureCollection, GeoJsonProperties, Geometry } from 'geojson';

import { LtsaOrders } from '@/interfaces/ltsaModels';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { ApiGen_Concepts_PropertyAssociations } from '@/models/api/generated/ApiGen_Concepts_PropertyAssociations';
import { IBcAssessmentSummary } from '@/models/layers/bcAssesment';

export interface ComposedProperty {
  pid: string | undefined;
  pin: string | undefined;
  id: number | undefined;
  ltsaOrders: LtsaOrders | undefined;
  pimsProperty: ApiGen_Concepts_Property | undefined;
  propertyAssociations: ApiGen_Concepts_PropertyAssociations | undefined;
  parcelMapFeatureCollection: FeatureCollection<Geometry, GeoJsonProperties> | undefined; // TODO: These need to be strongly typed
  geoserverFeatureCollection: FeatureCollection<Geometry, GeoJsonProperties> | undefined; // TODO: These need to be strongly typed
  bcAssessmentSummary: IBcAssessmentSummary | undefined;
}
