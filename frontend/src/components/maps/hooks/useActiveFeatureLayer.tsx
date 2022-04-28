import { Feature, GeoJsonObject, GeoJsonProperties } from 'geojson';
import useDeepCompareEffect from 'hooks/useDeepCompareEffect';
import { IProperty } from 'interfaces';
import { GeoJSON, geoJSON, LatLng, LatLngBounds, Map as LeafletMap } from 'leaflet';
import { isEmpty } from 'lodash';
import { useContext, useState } from 'react';

import {
  LayerPopupInformation,
  MUNICIPALITY_LAYER_URL,
  municipalityLayerPopupConfig,
  parcelLayerPopupConfig,
  PARCELS_LAYER_URL,
  useLayerQuery,
} from '../leaflet/LayerPopup';
import { SelectedPropertyContext } from '../providers/SelectedPropertyContext';

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
  const parcelsService = useLayerQuery(PARCELS_LAYER_URL);
  const municipalitiesService = useLayerQuery(MUNICIPALITY_LAYER_URL);
  const { isSelecting, setSelectedFeature } = useContext(SelectedPropertyContext);
  // add geojson layer to the map
  if (!!mapRef.current && !activeFeatureLayer) {
    setActiveFeatureLayer(geoJSON().addTo(mapRef.current));
  }

  const showLocationDetails = async (latLng: LatLng) => {
    activeFeatureLayer?.clearLayers();
    let properties: GeoJsonProperties | undefined = undefined;
    let center: LatLng | undefined;
    let mapBounds: LatLngBounds | undefined;
    let displayConfig = {};
    let title = 'Municipality Information';
    let feature: Feature | undefined = undefined;

    const parcel = await parcelsService.findOneWhereContains(latLng);

    if (!isSelecting) {
      const municipality = await municipalitiesService.findOneWhereContains(latLng);

      if (municipality?.features?.length === 1) {
        properties = municipality.features[0].properties!;
        displayConfig = municipalityLayerPopupConfig;
        feature = municipality.features[0];
        mapBounds = municipality.features[0]?.geometry
          ? geoJSON(municipality.features[0].geometry).getBounds()
          : undefined;
      }
    }

    if (parcel?.features?.length === 1) {
      displayConfig = parcelLayerPopupConfig;
      properties = parcel.features[0].properties!;
      feature = parcel.features[0];
      title = 'LTSA ParcelMap data';
      mapBounds = parcel.features[0]?.geometry
        ? geoJSON(parcel.features[0].geometry).getBounds()
        : undefined;
    } else if (parcel?.features?.length === 0 && isSelecting) {
      feature = {
        geometry: { coordinates: [latLng.lng, latLng.lat], type: 'Point' },
        type: 'Feature',
        properties: [],
      };
    }

    if ((!isEmpty(properties) || isSelecting) && feature) {
      if (!isSelecting) {
        setLayerPopup({
          title,
          data: properties as any,
          config: displayConfig as any,
          latlng: latLng,
          center,
          bounds: mapBounds,
          feature,
        } as any);
      }
      feature.properties = {
        ...feature.properties,
        IS_SELECTED: isSelecting,
        CLICK_LAT_LNG: latLng,
      };
      activeFeatureLayer?.addData(feature);
      setSelectedFeature(feature);
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
      } as LatLng);
    }
  }, [selectedProperty, activeFeatureLayer]);

  return { showLocationDetails };
};

export default useActiveFeatureLayer;
