import { FeatureCollection, GeoJsonProperties, Geometry } from 'geojson';

import { LtsaOrders } from '@/interfaces/ltsaModels';
import { Api_Property, Api_PropertyAssociations } from '@/models/api/Property';
import { IBcAssessmentSummary } from '@/models/layers/bcAssesment';

export interface ComposedProperty {
  pid: string | undefined;
  pin: string | undefined;
  id: number | undefined;
  ltsaOrders: LtsaOrders | undefined;
  pimsProperty: Api_Property | undefined;
  propertyAssociations: Api_PropertyAssociations | undefined;
  parcelMapFeatureCollection: FeatureCollection<Geometry, GeoJsonProperties> | undefined; // TODO: These need to be strongly typed
  geoserverFeatureCollection: FeatureCollection<Geometry, GeoJsonProperties> | undefined; // TODO: These need to be strongly typed
  bcAssessmentSummary: IBcAssessmentSummary | undefined;
}
