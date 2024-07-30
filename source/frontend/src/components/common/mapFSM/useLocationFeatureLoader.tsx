import { Feature, FeatureCollection, Geometry } from 'geojson';
import { LatLngLiteral } from 'leaflet';
import { useCallback } from 'react';

import { useAdminBoundaryMapLayer } from '@/hooks/repositories/mapLayer/useAdminBoundaryMapLayer';
import { useFullyAttributedParcelMapLayer } from '@/hooks/repositories/mapLayer/useFullyAttributedParcelMapLayer';
import { usePimsHighwayLayer } from '@/hooks/repositories/mapLayer/useHighwayLayer';
import { useLegalAdminBoundariesMapLayer } from '@/hooks/repositories/mapLayer/useLegalAdminBoundariesMapLayer';
import { usePimsPropertyLayer } from '@/hooks/repositories/mapLayer/usePimsPropertyLayer';
import { useMapProperties } from '@/hooks/repositories/useMapProperties';
import { MOT_DistrictBoundary_Feature_Properties } from '@/models/layers/motDistrictBoundary';
import { MOT_RegionalBoundary_Feature_Properties } from '@/models/layers/motRegionalBoundary';
import { WHSE_Municipalities_Feature_Properties } from '@/models/layers/municipalities';
import { PMBC_FullyAttributed_Feature_Properties } from '@/models/layers/parcelMapBC';
import { ISS_ProvincialPublicHighway } from '@/models/layers/pimsHighwayLayer';
import { PIMS_Property_Location_View } from '@/models/layers/pimsPropertyLocationView';

export interface LocationFeatureDataset {
  selectingComponentId: string | null;
  location: LatLngLiteral;
  fileLocation: LatLngLiteral | null;
  pimsFeature: Feature<Geometry, PIMS_Property_Location_View> | null;
  parcelFeature: Feature<Geometry, PMBC_FullyAttributed_Feature_Properties> | null;
  regionFeature: Feature<Geometry, MOT_RegionalBoundary_Feature_Properties> | null;
  districtFeature: Feature<Geometry, MOT_DistrictBoundary_Feature_Properties> | null;
  municipalityFeature: Feature<Geometry, WHSE_Municipalities_Feature_Properties> | null;
  highwayFeature: Feature<Geometry, ISS_ProvincialPublicHighway> | null;
}

const useLocationFeatureLoader = () => {
  const fullyAttributedService = useFullyAttributedParcelMapLayer();
  const adminBoundaryLayerService = useAdminBoundaryMapLayer();
  const adminLegalBoundaryLayerService = useLegalAdminBoundariesMapLayer();
  const highwayLayerService = usePimsHighwayLayer();

  const {
    loadProperties: { execute: loadProperties },
  } = useMapProperties();
  const { findOneByBoundary } = usePimsPropertyLayer();

  const fullyAttributedServiceFindOne = fullyAttributedService.findOne;
  const adminBoundaryLayerServiceFindRegion = adminBoundaryLayerService.findRegion;
  const adminBoundaryLayerServiceFindDistrict = adminBoundaryLayerService.findDistrict;
  const adminLegalBoundaryLayerServiceFindOneMunicipality =
    adminLegalBoundaryLayerService.findOneMunicipality;
  const highwayLayerServiceFindOne = highwayLayerService.findOne;

  const loadLocationDetails = useCallback(
    async (latLng: LatLngLiteral): Promise<LocationFeatureDataset> => {
      const result: LocationFeatureDataset = {
        selectingComponentId: null,
        location: latLng,
        fileLocation: latLng,
        pimsFeature: null,
        parcelFeature: null,
        regionFeature: null,
        districtFeature: null,
        municipalityFeature: null,
        highwayFeature: null,
      };

      // call these APIs in parallel - notice there is no "await"
      const fullyAttributedTask = fullyAttributedServiceFindOne(latLng);
      const regionTask = adminBoundaryLayerServiceFindRegion(latLng, 'SHAPE');
      const districtTask = adminBoundaryLayerServiceFindDistrict(latLng, 'SHAPE');
      const highwayTask = highwayLayerServiceFindOne(latLng, 'GEOMETRY');

      const [parcelFeature, regionFeature, districtFeature, highwayFeature] = await Promise.all([
        fullyAttributedTask,
        regionTask,
        districtTask,
        highwayTask,
      ]);

      let pimsLocationProperties:
        | FeatureCollection<Geometry, PIMS_Property_Location_View>
        | undefined = undefined;

      // Load PimsProperties
      if (latLng) {
        const latLngFeature = await findOneByBoundary(latLng, 'GEOMETRY', 4326);
        if (latLngFeature !== undefined) {
          pimsLocationProperties = { features: [latLngFeature], type: 'FeatureCollection' };
        }
      } else if (parcelFeature !== undefined) {
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
      result.highwayFeature = highwayFeature ?? null;

      return result;
    },
    [
      fullyAttributedServiceFindOne,
      adminBoundaryLayerServiceFindRegion,
      adminBoundaryLayerServiceFindDistrict,
      highwayLayerServiceFindOne,
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
