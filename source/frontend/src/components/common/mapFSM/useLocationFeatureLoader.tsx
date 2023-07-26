import { Feature, FeatureCollection, Geometry } from 'geojson';
import { LatLngLiteral } from 'leaflet';
import { useCallback } from 'react';

import { useAdminBoundaryMapLayer } from '@/hooks/repositories/mapLayer/useAdminBoundaryMapLayer';
import { useFullyAttributedParcelMapLayer } from '@/hooks/repositories/mapLayer/useFullyAttributedParcelMapLayer';
import { useLegalAdminBoundariesMapLayer } from '@/hooks/repositories/mapLayer/useLegalAdminBoundariesMapLayer';
import { useMapProperties } from '@/hooks/repositories/useMapProperties';
import { MOT_DistrictBoundary_Feature_Properties } from '@/models/layers/motDistrictBoundary';
import { MOT_RegionalBoundary_Feature_Properties } from '@/models/layers/motRegionalBoundary';
import { WHSE_Municipalities_Feature_Properties } from '@/models/layers/municipalities';
import { PMBC_FullyAttributed_Feature_Properties } from '@/models/layers/parcelMapBC';
import { PIMS_Property_Location_View } from '@/models/layers/pimsPropertyLocationView';

export interface LocationFeatureDataset {
  location: LatLngLiteral;
  pimsFeature: Feature<Geometry, PIMS_Property_Location_View> | null;
  parcelFeature: Feature<Geometry, PMBC_FullyAttributed_Feature_Properties> | null;
  regionFeature: Feature<Geometry, MOT_RegionalBoundary_Feature_Properties> | null;
  districtFeature: Feature<Geometry, MOT_DistrictBoundary_Feature_Properties> | null;
  municipalityFeature: Feature<Geometry, WHSE_Municipalities_Feature_Properties> | null;
}

const useLocationFeatureLoader = () => {
  const fullyAttributedService = useFullyAttributedParcelMapLayer();
  const adminBoundaryLayerService = useAdminBoundaryMapLayer();
  const adminLegalBoundaryLayerService = useLegalAdminBoundariesMapLayer();

  const {
    loadProperties: { execute: loadProperties },
  } = useMapProperties();

  const fullyAttributedServiceFindOne = fullyAttributedService.findOne;
  const adminBoundaryLayerServiceFindRegion = adminBoundaryLayerService.findRegion;
  const adminBoundaryLayerServiceFindDistrict = adminBoundaryLayerService.findDistrict;
  const adminLegalBoundaryLayerServiceFindOneMunicipality =
    adminLegalBoundaryLayerService.findOneMunicipality;

  const loadLocationDetails = useCallback(
    async (latLng: LatLngLiteral): Promise<LocationFeatureDataset> => {
      const result: LocationFeatureDataset = {
        location: latLng,
        pimsFeature: null,
        parcelFeature: null,
        regionFeature: null,
        districtFeature: null,
        municipalityFeature: null,
      };
      try {
        // call these APIs in parallel - notice there is no "await"
        const fullyAttributedTask = fullyAttributedServiceFindOne(latLng);
        const regionTask = adminBoundaryLayerServiceFindRegion(latLng, 'GEOMETRY');
        const districtTask = adminBoundaryLayerServiceFindDistrict(latLng, 'GEOMETRY');

        const [parcelFeature, regionFeature, districtFeature] = await Promise.all([
          fullyAttributedTask,
          regionTask,
          districtTask,
        ]);

        let pimsLocationProperties:
          | FeatureCollection<Geometry, PIMS_Property_Location_View>
          | undefined = undefined;

        // Load PimsProperties
        if (parcelFeature !== undefined) {
          pimsLocationProperties = await loadProperties({
            PID: parcelFeature.properties?.PID || '',
            PIN: parcelFeature.properties?.PIN?.toString() || '',
          });
        }

        const municipalityFeature = await adminLegalBoundaryLayerServiceFindOneMunicipality(latLng);

        if (
          pimsLocationProperties?.features !== undefined &&
          pimsLocationProperties.features.length > 0
        ) {
          result.pimsFeature = pimsLocationProperties.features[0] ?? null;
        }

        result.parcelFeature = parcelFeature ?? null;
        result.regionFeature = regionFeature ?? null;
        result.districtFeature = districtFeature ?? null;
        result.municipalityFeature = municipalityFeature ?? null;
      } finally {
        // TODO: Remove once the try above is deemed no longer necessary.
      }

      return result;
    },
    [
      loadProperties,
      fullyAttributedServiceFindOne,
      adminBoundaryLayerServiceFindRegion,
      adminBoundaryLayerServiceFindDistrict,
      adminLegalBoundaryLayerServiceFindOneMunicipality,
    ],
  );

  return {
    loadLocationDetails,
  };
};

export default useLocationFeatureLoader;
