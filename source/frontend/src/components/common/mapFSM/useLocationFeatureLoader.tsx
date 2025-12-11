import { Feature, FeatureCollection, Geometry } from 'geojson';
import { LatLngLiteral } from 'leaflet';
import { useCallback } from 'react';

import { useAdminBoundaryMapLayer } from '@/hooks/repositories/mapLayer/useAdminBoundaryMapLayer';
import { useFullyAttributedParcelMapLayer } from '@/hooks/repositories/mapLayer/useFullyAttributedParcelMapLayer';
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

export interface FeatureDataset {
  selectingComponentId: string | null;
  location: LatLngLiteral;
  fileLocation: LatLngLiteral | null;
}

export interface LocationFeatureDataset extends FeatureDataset {
  parcelFeatures: Feature<Geometry, PMBC_FullyAttributed_Feature_Properties>[] | null;
  pimsFeatures: Feature<Geometry, PIMS_Property_Location_View>[] | null;
  regionFeature: Feature<Geometry, MOT_RegionalBoundary_Feature_Properties> | null;
  districtFeature: Feature<Geometry, MOT_DistrictBoundary_Feature_Properties> | null;
  municipalityFeatures: Feature<Geometry, WHSE_Municipalities_Feature_Properties>[] | null;
  highwayFeatures: Feature<Geometry, ISS_ProvincialPublicHighway>[] | null;
  crownLandLeasesFeatures: Feature<Geometry, TANTALIS_CrownLandLeases_Feature_Properties>[] | null;
  crownLandLicensesFeatures:
    | Feature<Geometry, TANTALIS_CrownLandLicenses_Feature_Properties>[]
    | null;
  crownLandTenuresFeatures:
    | Feature<Geometry, TANTALIS_CrownLandTenures_Feature_Properties>[]
    | null;
  crownLandInventoryFeatures:
    | Feature<Geometry, TANTALIS_CrownLandInventory_Feature_Properties>[]
    | null;
  crownLandInclusionsFeatures:
    | Feature<Geometry, TANTALIS_CrownLandInclusions_Feature_Properties>[]
    | null;
}

export interface SelectedFeatureDataset extends FeatureDataset {
  id?: string;
  location: LatLngLiteral;
  parcelFeature: Feature<Geometry, PMBC_FullyAttributed_Feature_Properties> | null;
  pimsFeature: Feature<Geometry, PIMS_Property_Location_View> | null;
  regionFeature: Feature<Geometry, MOT_RegionalBoundary_Feature_Properties> | null;
  districtFeature: Feature<Geometry, MOT_DistrictBoundary_Feature_Properties> | null;
  municipalityFeature: Feature<Geometry, WHSE_Municipalities_Feature_Properties> | null;
  isActive?: boolean;
  displayOrder?: number;
}

export interface WorklistLocationFeatureDataset
  extends Omit<FeatureDataset, 'selectingComponentId' | 'fileLocation'> {
  fullyAttributedFeatures: FeatureCollection<
    Geometry,
    PMBC_FullyAttributed_Feature_Properties
  > | null;
  pimsFeature: Feature<Geometry, PIMS_Property_Location_View> | null;
  regionFeature: Feature<Geometry, MOT_RegionalBoundary_Feature_Properties> | null;
  districtFeature: Feature<Geometry, MOT_DistrictBoundary_Feature_Properties> | null;
}

const useLocationFeatureLoader = () => {
  const fullyAttributedService = useFullyAttributedParcelMapLayer();
  const adminBoundaryLayerService = useAdminBoundaryMapLayer();
  const adminLegalBoundaryLayerService = useLegalAdminBoundariesMapLayer();

  const {
    loadProperties: { execute: loadProperties },
  } = useMapProperties();
  const { findAllByBoundary } = usePimsPropertyLayer();

  const fullyAttributedServiceFindAll = fullyAttributedService.findMany;

  // Single result
  const adminBoundaryLayerServiceFindRegion = adminBoundaryLayerService.findRegion;
  const adminBoundaryLayerServiceFindDistrict = adminBoundaryLayerService.findDistrict;

  // Multiple results
  const adminLegalBoundaryLayerServiceFindOneMunicipality =
    adminLegalBoundaryLayerService.findMultipleMunicipality;

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
        pimsFeatures: null,
        parcelFeatures: null,
        regionFeature: null,
        districtFeature: null,
        municipalityFeatures: null,
        highwayFeatures: null,
        crownLandLeasesFeatures: null,
        crownLandLicensesFeatures: null,
        crownLandTenuresFeatures: null,
        crownLandInventoryFeatures: null,
        crownLandInclusionsFeatures: null,
      };

      // call these APIs in parallel - notice there is no "await"
      // Could return multiple results
      const fullyAttributedTask = fullyAttributedServiceFindAll(latLng);

      // single results expected
      const regionTask = adminBoundaryLayerServiceFindRegion(latLng, 'SHAPE');
      const districtTask = adminBoundaryLayerServiceFindDistrict(latLng, 'SHAPE');
      const municipalityFeatureTask = adminLegalBoundaryLayerServiceFindOneMunicipality(latLng);

      const [parcelFeatureCollection, regionFeature, districtFeature, municipalityFeatures] =
        await Promise.all([fullyAttributedTask, regionTask, districtTask, municipalityFeatureTask]);

      let pimsLocationProperties: Feature<Geometry, PIMS_Property_Location_View>[] | undefined =
        undefined;

      // Load PimsProperties
      // - first attempt to find it by our internal PIMS id
      // - then try to find it on our boundary layer.
      // - if not found by boundary attempt to match it by PID / PIN coming from parcel-map
      const isInPims = isValidId(Number(pimsPropertyId));
      if (isInPims) {
        pimsLocationProperties = (await loadProperties({ PROPERTY_ID: Number(pimsPropertyId) }))
          .features;
      } else {
        const boundaryPimsFeatures = await findAllByBoundary(latLng, 'GEOMETRY', 4326);
        if (exists(boundaryPimsFeatures)) {
          pimsLocationProperties = boundaryPimsFeatures;
        } else if (exists(parcelFeatureCollection?.features)) {
          const parcelFeatures = parcelFeatureCollection.features;
          pimsLocationProperties = [];

          const pimsFeatureTasks: Promise<
            FeatureCollection<Geometry, PIMS_Property_Location_View>
          >[] = [];

          for (let i = 0; i < parcelFeatures.length; i++) {
            const parcelFeature = parcelFeatures[i];
            if (exists(parcelFeature.properties?.PID) || exists(parcelFeature.properties?.PIN)) {
              const pimsFeaturesTask = loadProperties({
                PID: parcelFeature.properties?.PID || '',
                PIN: parcelFeature.properties?.PIN?.toString() || '',
              });

              pimsFeatureTasks.push(pimsFeaturesTask);
            }
          }

          const pimsFeatures = await Promise.all(pimsFeatureTasks);

          pimsFeatures
            .filter(exists)
            .flatMap(x => x.features)
            .filter(exists)
            .forEach(y => pimsLocationProperties.push(y));
        }
      }

      result.pimsFeatures = pimsLocationProperties ?? null;
      result.parcelFeatures = parcelFeatureCollection?.features ?? null;
      result.regionFeature = regionFeature ?? null;
      result.districtFeature = districtFeature ?? null;
      result.municipalityFeatures = municipalityFeatures ?? null;

      return result;
    },
    [
      adminBoundaryLayerServiceFindDistrict,
      adminBoundaryLayerServiceFindRegion,
      adminLegalBoundaryLayerServiceFindOneMunicipality,
      findAllByBoundary,
      fullyAttributedServiceFindAll,
      loadProperties,
    ],
  );

  const loadWorklistLocationDetails = useCallback(
    async ({ latLng }: { latLng: LatLngLiteral }): Promise<WorklistLocationFeatureDataset> => {
      const result: WorklistLocationFeatureDataset = {
        location: latLng,
        fullyAttributedFeatures: null,
        pimsFeature: null,
        regionFeature: null,
        districtFeature: null,
      };

      // call these APIs in parallel - notice there is no "await"
      const fullyAttributedTask = fullyAttributedServiceFindAll(latLng);
      const regionTask = adminBoundaryLayerServiceFindRegion(latLng, 'SHAPE');
      const districtTask = adminBoundaryLayerServiceFindDistrict(latLng, 'SHAPE');

      const [pmbcFeatures, regionFeature, districtFeature] = await Promise.all([
        fullyAttributedTask,
        regionTask,
        districtTask,
      ]);

      result.fullyAttributedFeatures = pmbcFeatures ?? null;
      result.regionFeature = regionFeature ?? null;
      result.districtFeature = districtFeature ?? null;

      return result;
    },
    [
      adminBoundaryLayerServiceFindDistrict,
      adminBoundaryLayerServiceFindRegion,
      fullyAttributedServiceFindAll,
    ],
  );

  return {
    loadLocationDetails,
    loadWorklistLocationDetails,
  };
};

export default useLocationFeatureLoader;
