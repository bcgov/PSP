import { Feature, FeatureCollection, GeoJsonObject, GeoJsonProperties, Geometry } from 'geojson';
import { GeoJSON, geoJSON, LatLng, LatLngBounds, LatLngLiteral, Map as LeafletMap } from 'leaflet';
import { isNumber } from 'lodash';
import { useContext, useState } from 'react';
import { toast } from 'react-toastify';

import { DistrictCodes, RegionCodes } from '@/constants/index';
import { useAdminBoundaryMapLayer } from '@/hooks/repositories/mapLayer/useAdminBoundaryMapLayer';
import { useFullyAttributedParcelMapLayer } from '@/hooks/repositories/mapLayer/useFullyAttributedParcelMapLayer';
import { useLegalAdminBoundariesMapLayer } from '@/hooks/repositories/mapLayer/useLegalAdminBoundariesMapLayer';
import { useMapProperties } from '@/hooks/repositories/useMapProperties';
import useDeepCompareEffect from '@/hooks/util/useDeepCompareEffect';
import { IProperty } from '@/interfaces';

import { PopupContentConfig } from '../leaflet/LayerPopup/components/LayerPopupContent';
import {
  municipalityLayerPopupConfig,
  parcelLayerPopupConfig,
} from '../leaflet/LayerPopup/constants';
import { LayerPopupInformation } from '../leaflet/LayerPopup/LayerPopupContainer';
import { MapStateActionTypes, MapStateContext } from '../providers/MapStateContext';

interface IUseActiveParcelMapLayer {
  /** the current leaflet map reference. This hook will add layers to this map reference. */
  mapRef: React.RefObject<LeafletMap>;
  /** The currently selected property on the map */
  selectedProperty?: IProperty | null;
  /** the currently displayed layer popup information */
  layerPopup?: LayerPopupInformation;
  /** set the display of the layer popup imperatively */
  setLayerPopup: (value: React.SetStateAction<LayerPopupInformation | undefined>) => void;
  /** the most recently searched for parcel layer feature */
  parcelLayerFeature?: GeoJsonObject | null;
}

/**
 * Set the currently active feature based off of the most recent click on a map layer.
 * @param param0
 */
const useActiveFeatureLayer = ({
  selectedProperty,
  mapRef,
  setLayerPopup,
}: IUseActiveParcelMapLayer) => {
  const [activeFeatureLayer, setActiveFeatureLayer] = useState<GeoJSON>();
  const fullyAttributedService = useFullyAttributedParcelMapLayer();
  const adminBoundaryLayerService = useAdminBoundaryMapLayer();
  const adminLegalBoundaryLayerService = useLegalAdminBoundariesMapLayer();

  const {
    loadProperties: { execute: loadProperties },
  } = useMapProperties();

  const { isSelecting, setState } = useContext(MapStateContext);
  // add geojson layer to the map
  if (!!mapRef.current && !activeFeatureLayer) {
    setActiveFeatureLayer(geoJSON().addTo(mapRef.current));
  }

  const showLocationDetails = async (latLng: LatLngLiteral) => {
    try {
      activeFeatureLayer?.clearLayers();
      let properties: GeoJsonProperties | undefined = undefined;
      let center: LatLng | undefined;
      let mapBounds: LatLngBounds | undefined;
      let displayConfig: PopupContentConfig = {};
      let title = 'Location Information';
      let feature: Feature | undefined = undefined;

      // call these APIs in parallel - notice there is no "await"
      const fullyAttributedTask = fullyAttributedService.findOne(latLng);
      const regionTask = adminBoundaryLayerService.findRegion(latLng, 'GEOMETRY');
      const districtTask = adminBoundaryLayerService.findDistrict(latLng, 'GEOMETRY');
      setState({ type: MapStateActionTypes.LOADING, loading: true });

      const parcelFeature = await fullyAttributedTask;
      const regionFeature = await regionTask;
      const districtFeature = await districtTask;
      let pimsLocationProperties: FeatureCollection<Geometry, GeoJsonProperties> | undefined =
        undefined;

      if (parcelFeature !== undefined) {
        const pid = parcelFeature.properties?.PID;
        const pin = parcelFeature.properties?.PIN;
        pimsLocationProperties = await loadProperties({
          PID: pid || '',
          PIN: pin?.toString() || '',
        });
      }

      if (!isSelecting) {
        const municipalityFeature = await adminLegalBoundaryLayerService.findOneMunicipality(
          latLng,
        );

        if (municipalityFeature !== undefined) {
          title = 'Municipality Information';
          properties = municipalityFeature.properties!;
          displayConfig = municipalityLayerPopupConfig;
          feature = municipalityFeature;
          mapBounds = municipalityFeature.geometry
            ? geoJSON(municipalityFeature.geometry).getBounds()
            : undefined;
        }
      }

      if (parcelFeature !== undefined) {
        displayConfig = parcelLayerPopupConfig;
        properties = parcelFeature?.properties!;
        feature = parcelFeature;
        title = 'LTSA ParcelMap data';
        mapBounds = parcelFeature?.geometry
          ? geoJSON(parcelFeature.geometry).getBounds()
          : undefined;
      }

      if (feature === undefined) {
        feature = {
          geometry: { coordinates: [latLng.lng, latLng.lat], type: 'Point' },
          type: 'Feature',
          properties: {},
        };
      }

      if (!isSelecting) {
        if (pimsLocationProperties !== undefined && pimsLocationProperties?.features?.length > 1) {
          toast.error(
            'Unable to determine desired PIMS Property due to overlapping map boundaries. Click directly on a map marker to view that markers details.',
          );
        }
        const pimsProperty = pimsLocationProperties?.features?.length
          ? pimsLocationProperties?.features[0]
          : undefined;
        setLayerPopup({
          title,
          data: properties as any,
          config: displayConfig,
          latlng: latLng,
          center,
          bounds: mapBounds,
          feature,
          pimsProperty,
        });
      }
      const activeFeature = { ...feature };

      activeFeature.properties = {
        ...activeFeature.properties,
        IS_SELECTED: isSelecting,
        CLICK_LAT_LNG: latLng,
        REGION_NUMBER: isNumber(regionFeature?.properties.REGION_NUMBER)
          ? regionFeature?.properties.REGION_NUMBER
          : RegionCodes.Unknown,
        REGION_NAME: regionFeature?.properties.REGION_NAME ?? 'Cannot determine',
        DISTRICT_NUMBER: isNumber(districtFeature?.properties.DISTRICT_NUMBER)
          ? districtFeature?.properties.DISTRICT_NUMBER
          : DistrictCodes.Unknown,
        DISTRICT_NAME: districtFeature?.properties.DISTRICT_NAME ?? 'Cannot determine',
      };

      if (activeFeature?.geometry?.type === 'Polygon') {
        activeFeatureLayer?.addData(activeFeature);
      }
      setState({ type: MapStateActionTypes.SELECTED_FEATURE, selectedFeature: activeFeature });
    } finally {
      setState({ type: MapStateActionTypes.LOADING, loading: false });
    }
  };

  /**
   * If there is a selected property on the map, attempt to retrieve the corresponding parcel. If we find matching parcel data, use that to draw the active parcel.
   */
  useDeepCompareEffect(() => {
    if (!!activeFeatureLayer && !!selectedProperty?.latitude && !!selectedProperty?.longitude) {
      activeFeatureLayer.clearLayers();
      showLocationDetails({
        lat: selectedProperty?.latitude,
        lng: selectedProperty?.longitude,
      });
    }
  }, [selectedProperty, activeFeatureLayer]);

  return { showLocationDetails };
};

export default useActiveFeatureLayer;
