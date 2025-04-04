import { Feature, FeatureCollection, Geometry } from 'geojson';
import { LatLngLiteral } from 'leaflet';
import { useCallback } from 'react';

import { useAdminBoundaryMapLayer } from '@/hooks/repositories/mapLayer/useAdminBoundaryMapLayer';
import { useCrownLandLayer } from '@/hooks/repositories/mapLayer/useCrownLandLayer';
import { useFullyAttributedParcelMapLayer } from '@/hooks/repositories/mapLayer/useFullyAttributedParcelMapLayer';
import { usePimsHighwayLayer } from '@/hooks/repositories/mapLayer/useHighwayLayer';
import { useLegalAdminBoundariesMapLayer } from '@/hooks/repositories/mapLayer/useLegalAdminBoundariesMapLayer';
import { usePimsPropertyLayer } from '@/hooks/repositories/mapLayer/usePimsPropertyLayer';
import { useMapProperties } from '@/hooks/repositories/useMapProperties';
import {
  TANTALIS_CrownLandInclusions_Feature_Properties,
  TANTALIS_CrownLandInventory_Feature_Properties,
  TANTALIS_CrownLandLeases_Feature_Properties,
  TANTALIS_CrownLandLicenses_Feature_Properties,
  TANTALIS_CrownLandTenures_Feature_Properties,
} from '@/models/layers/crownLand';
import { MOT_DistrictBoundary_Feature_Properties } from '@/models/layers/motDistrictBoundary';
import { MOT_RegionalBoundary_Feature_Properties } from '@/models/layers/motRegionalBoundary';
import { WHSE_Municipalities_Feature_Properties } from '@/models/layers/municipalities';
import { PMBC_FullyAttributed_Feature_Properties } from '@/models/layers/parcelMapBC';
import { ISS_ProvincialPublicHighway } from '@/models/layers/pimsHighwayLayer';
import { PIMS_Property_Location_View } from '@/models/layers/pimsPropertyLocationView';
import { exists, isValidId } from '@/utils';

export interface LocationFeatureDataset {
  selectingComponentId: string | null;
  location: LatLngLiteral;
  fileLocation: LatLngLiteral | null;
  pimsFeature: Feature<Geometry, PIMS_Property_Location_View> | null;
  parcelFeature: Feature<Geometry, PMBC_FullyAttributed_Feature_Properties> | null;
  regionFeature: Feature<Geometry, MOT_RegionalBoundary_Feature_Properties> | null;
  districtFeature: Feature<Geometry, MOT_DistrictBoundary_Feature_Properties> | null;
  municipalityFeature: Feature<Geometry, WHSE_Municipalities_Feature_Properties> | null;
  highwayFeatures: Feature<Geometry, ISS_ProvincialPublicHighway>[] | null;
  crownLandLeasesFeature: Feature<Geometry, TANTALIS_CrownLandLeases_Feature_Properties> | null;
  crownLandLicensesFeature: Feature<Geometry, TANTALIS_CrownLandLicenses_Feature_Properties> | null;
  crownLandTenuresFeature: Feature<Geometry, TANTALIS_CrownLandTenures_Feature_Properties> | null;
  crownLandInventoryFeature: Feature<
    Geometry,
    TANTALIS_CrownLandInventory_Feature_Properties
  > | null;
  crownLandInclusionsFeature: Feature<
    Geometry,
    TANTALIS_CrownLandInclusions_Feature_Properties
  > | null;
}

const useLocationFeatureLoader = () => {
  const fullyAttributedService = useFullyAttributedParcelMapLayer();
  const adminBoundaryLayerService = useAdminBoundaryMapLayer();
  const adminLegalBoundaryLayerService = useLegalAdminBoundariesMapLayer();
  const highwayLayerService = usePimsHighwayLayer();
  const crownLandLayerService = useCrownLandLayer();

  const {
    loadProperties: { execute: loadProperties },
  } = useMapProperties();
  const { findOneByBoundary } = usePimsPropertyLayer();

  const fullyAttributedServiceFindOne = fullyAttributedService.findOne;
  const adminBoundaryLayerServiceFindRegion = adminBoundaryLayerService.findRegion;
  const adminBoundaryLayerServiceFindDistrict = adminBoundaryLayerService.findDistrict;
  const adminLegalBoundaryLayerServiceFindOneMunicipality =
    adminLegalBoundaryLayerService.findOneMunicipality;
  const highwayLayerServiceFindMultiple = highwayLayerService.findMultiple;

  const crownLandLayerServiceFindOneLicense = crownLandLayerService.findOneCrownLandLicense;
  const crownLandLayerServiceFindOneTenure = crownLandLayerService.findOneCrownLandTenure;
  const crownLandLayerServiceFindOneLease = crownLandLayerService.findOneCrownLandLease;
  const crownLandLayerServiceFindOneInclusion = crownLandLayerService.findOneCrownLandInclusion;
  const crownLandLayerServiceFindOneInventory = crownLandLayerService.findOneCrownLandInventory;

  const loadLocationDetails = useCallback(
    async ({
      latLng,
      pimsPropertyId,
    }: {
      latLng: LatLngLiteral;
      pimsPropertyId?: number | null;
    }): Promise<LocationFeatureDataset> => {
      const result: LocationFeatureDataset = {
        selectingComponentId: null,
        location: latLng,
        fileLocation: latLng,
        pimsFeature: null,
        parcelFeature: null,
        regionFeature: null,
        districtFeature: null,
        municipalityFeature: null,
        highwayFeatures: null,
        crownLandLeasesFeature: null,
        crownLandLicensesFeature: null,
        crownLandTenuresFeature: null,
        crownLandInventoryFeature: null,
        crownLandInclusionsFeature: null,
      };

      // call these APIs in parallel - notice there is no "await"
      const fullyAttributedTask = fullyAttributedServiceFindOne(latLng);
      const regionTask = adminBoundaryLayerServiceFindRegion(latLng, 'SHAPE');
      const districtTask = adminBoundaryLayerServiceFindDistrict(latLng, 'SHAPE');
      const highwayTask = highwayLayerServiceFindMultiple(latLng, 'GEOMETRY');
      const crownLandLeaseTask = crownLandLayerServiceFindOneLease(latLng);
      const crownLandLicensesTask = crownLandLayerServiceFindOneLicense(latLng);
      const crownLandTenuresTask = crownLandLayerServiceFindOneTenure(latLng);
      const crownLandInventoryTask = crownLandLayerServiceFindOneInventory(latLng);
      const crownLandInclusionsTask = crownLandLayerServiceFindOneInclusion(latLng);
      const municipalityFeatureTask = adminLegalBoundaryLayerServiceFindOneMunicipality(latLng);

      const [
        parcelFeature,
        regionFeature,
        districtFeature,
        highwayFeatures,
        crownLandLeaseFeature,
        crownLandLicensesFeature,
        crownLandTenuresFeature,
        crownLandInventoryFeature,
        crownLandInclusionsFeature,
        municipalityFeature,
      ] = await Promise.all([
        fullyAttributedTask,
        regionTask,
        districtTask,
        highwayTask,
        crownLandLeaseTask,
        crownLandLicensesTask,
        crownLandTenuresTask,
        crownLandInventoryTask,
        crownLandInclusionsTask,
        municipalityFeatureTask,
      ]);

      let pimsLocationProperties:
        | FeatureCollection<Geometry, PIMS_Property_Location_View>
        | undefined = undefined;

      // Load PimsProperties
      // - first attempt to find it by our internal PIMS id
      // - then try to find it on our boundary layer.
      // - if not found by boundary attempt to match it by PID / PIN coming from parcel-map
      const isInPims = isValidId(Number(pimsPropertyId));
      if (isInPims) {
        pimsLocationProperties = await loadProperties({ PROPERTY_ID: Number(pimsPropertyId) });
      } else {
        const boundaryFeature = await findOneByBoundary(latLng, 'GEOMETRY', 4326);
        if (exists(boundaryFeature)) {
          pimsLocationProperties = { features: [boundaryFeature], type: 'FeatureCollection' };
        } else if (
          exists(parcelFeature) &&
          (exists(parcelFeature.properties?.PID) || exists(parcelFeature.properties?.PIN))
        ) {
          pimsLocationProperties = await loadProperties({
            PID: parcelFeature.properties?.PID || '',
            PIN: parcelFeature.properties?.PIN?.toString() || '',
          });
        }
      }

      if (exists(pimsLocationProperties?.features) && pimsLocationProperties.features.length > 0) {
        result.pimsFeature = pimsLocationProperties.features[0] ?? null;
      }

      result.parcelFeature = parcelFeature ?? null;
      result.regionFeature = regionFeature ?? null;
      result.districtFeature = districtFeature ?? null;
      result.municipalityFeature = municipalityFeature ?? null;
      result.highwayFeatures = highwayFeatures ?? null;
      result.crownLandLeasesFeature = crownLandLeaseFeature ?? null;
      result.crownLandLicensesFeature = crownLandLicensesFeature ?? null;
      result.crownLandTenuresFeature = crownLandTenuresFeature ?? null;
      result.crownLandInventoryFeature = crownLandInventoryFeature ?? null;
      result.crownLandInclusionsFeature = crownLandInclusionsFeature ?? null;

      return result;
    },
    [
      fullyAttributedServiceFindOne,
      adminBoundaryLayerServiceFindRegion,
      adminBoundaryLayerServiceFindDistrict,
      highwayLayerServiceFindMultiple,
      crownLandLayerServiceFindOneLease,
      crownLandLayerServiceFindOneLicense,
      crownLandLayerServiceFindOneTenure,
      crownLandLayerServiceFindOneInventory,
      crownLandLayerServiceFindOneInclusion,
      adminLegalBoundaryLayerServiceFindOneMunicipality,
      findOneByBoundary,
      loadProperties,
    ],
  );

  return {
    loadLocationDetails,
  };
};

export default useLocationFeatureLoader;
