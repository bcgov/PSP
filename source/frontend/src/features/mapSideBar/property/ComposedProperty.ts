import { Feature, FeatureCollection, GeoJsonProperties, Geometry } from 'geojson';

import { LtsaOrders, SpcpOrder } from '@/interfaces/ltsaModels';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { ApiGen_Concepts_PropertyAssociations } from '@/models/api/generated/ApiGen_Concepts_PropertyAssociations';
import { ADM_IndianReserveBands_Feature_Properties } from '@/models/layers/admIndianReserveBands';
import { WHSE_AgriculturalLandReservePoly_Feature_Properties } from '@/models/layers/alcAgriculturalReserve';
import { IBcAssessmentSummary } from '@/models/layers/bcAssesment';
import {
  TANTALIS_CrownLandInclusions_Feature_Properties,
  TANTALIS_CrownLandInventory_Feature_Properties,
  TANTALIS_CrownLandLeases_Feature_Properties,
  TANTALIS_CrownLandLicenses_Feature_Properties,
  TANTALIS_CrownLandTenures_Feature_Properties,
} from '@/models/layers/crownLand';
import { EBC_ELECTORAL_DISTS_BS10_SVW_Feature_Properties } from '@/models/layers/electoralBoundaries';
import { WHSE_Municipalities_Feature_Properties } from '@/models/layers/municipalities';
import { PMBC_FullyAttributed_Feature_Properties } from '@/models/layers/parcelMapBC';
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
  parcelMapFeatureCollection:
    | FeatureCollection<Geometry, PMBC_FullyAttributed_Feature_Properties>
    | undefined;
  pimsGeoserverFeatureCollection: FeatureCollection<Geometry, GeoJsonProperties> | undefined; // TODO: These need to be strongly typed
  bcAssessmentSummary: IBcAssessmentSummary | undefined;
  crownTenureFeatures: Feature<Geometry, TANTALIS_CrownLandTenures_Feature_Properties>[];
  crownLeaseFeatures: Feature<Geometry, TANTALIS_CrownLandLeases_Feature_Properties>[];
  crownLicenseFeatures: Feature<Geometry, TANTALIS_CrownLandLicenses_Feature_Properties>[];
  crownInclusionFeatures: Feature<Geometry, TANTALIS_CrownLandInclusions_Feature_Properties>[];
  crownInventoryFeatures: Feature<Geometry, TANTALIS_CrownLandInventory_Feature_Properties>[];
  highwayFeatures: Feature<Geometry, ISS_ProvincialPublicHighway>[];
  municipalityFeatures: Feature<Geometry, WHSE_Municipalities_Feature_Properties>[];
  firstNationFeatures: Feature<Geometry, ADM_IndianReserveBands_Feature_Properties>[];
  alrFeatures: Feature<Geometry, WHSE_AgriculturalLandReservePoly_Feature_Properties>[];
  electoralFeatures: Feature<Geometry, EBC_ELECTORAL_DISTS_BS10_SVW_Feature_Properties>[];
}
