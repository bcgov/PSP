import { Feature, FeatureCollection, GeoJsonProperties, Geometry } from 'geojson';

import { LtsaOrders, SpcpOrder } from '@/interfaces/ltsaModels';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { ApiGen_Concepts_PropertyAssociations } from '@/models/api/generated/ApiGen_Concepts_PropertyAssociations';
import { IBcAssessmentSummary } from '@/models/layers/bcAssesment';
import {
  TANTALIS_CrownLandInclusions_Feature_Properties,
  TANTALIS_CrownLandInventory_Feature_Properties,
  TANTALIS_CrownLandLeases_Feature_Properties,
  TANTALIS_CrownLandLicenses_Feature_Properties,
  TANTALIS_CrownLandTenures_Feature_Properties,
} from '@/models/layers/crownLand';
import { WHSE_Municipalities_Feature_Properties } from '@/models/layers/municipalities';
import { ISS_ProvincialPublicHighway } from '@/models/layers/pimsHighwayLayer';

export interface ComposedProperty {
  pid: string | undefined;
  pin: string | undefined;
  planNumber: string | undefined;
  id: number | undefined;
  ltsaOrders: LtsaOrders | undefined;
  spcpOrder: SpcpOrder | undefined;
  pimsProperty: ApiGen_Concepts_Property | undefined;
  propertyAssociations: ApiGen_Concepts_PropertyAssociations | undefined;
  parcelMapFeatureCollection: FeatureCollection<Geometry, GeoJsonProperties> | undefined; // TODO: These need to be strongly typed
  pimsGeoserverFeatureCollection: FeatureCollection<Geometry, GeoJsonProperties> | undefined; // TODO: These need to be strongly typed
  bcAssessmentSummary: IBcAssessmentSummary | undefined;
  crownTenureFeatures: Feature<Geometry, TANTALIS_CrownLandTenures_Feature_Properties>[];
  crownLeaseFeatures: Feature<Geometry, TANTALIS_CrownLandLeases_Feature_Properties>[];
  crownLicenseFeatures: Feature<Geometry, TANTALIS_CrownLandLicenses_Feature_Properties>[];
  crownInclusionFeatures: Feature<Geometry, TANTALIS_CrownLandInclusions_Feature_Properties>[];
  crownInventoryFeatures: Feature<Geometry, TANTALIS_CrownLandInventory_Feature_Properties>[];
  highwayFeatures: Feature<Geometry, ISS_ProvincialPublicHighway>[];
  municipalityFeatures: Feature<Geometry, WHSE_Municipalities_Feature_Properties>[];
}
