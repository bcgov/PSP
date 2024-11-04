import L from 'leaflet';
import { flatten } from 'lodash';
import { useEffect } from 'react';
import { useMap } from 'react-leaflet';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { exists } from '@/utils';

import { wmsHeaders } from '../Control/LayersControl/wmsHeaders';

const featureGroup = new L.FeatureGroup<L.TileLayer.WMS>();
export const LeafletLayerListener = () => {
  const { activeLayers, mapLayersToRefresh, setMapLayersToRefresh } = useMapStateMachine();
  const mapInstance = useMap();

  useEffect(() => {
    if (mapInstance) {
      featureGroup.addTo(mapInstance);
    }

    return () => {
      mapInstance?.removeLayer(featureGroup);
    };
  }, [mapInstance]);

  useEffect(() => {
    if (mapLayersToRefresh?.length) {
      const currentLayers = featureGroup.getLayers().filter(exists);
      mapLayersToRefresh.forEach(configLayer => {
        const currentLayer = currentLayers.find(l => (l as any).options.key === configLayer.key);

        if (currentLayer) {
          featureGroup.removeLayer(currentLayer);
          featureGroup.addLayer(currentLayer);
        }
      });
      setMapLayersToRefresh([]);
    }
  }, [mapInstance, mapLayersToRefresh, setMapLayersToRefresh]);

  useEffect(() => {
    console.log(featureGroup.getLayers());
    if (mapInstance) {
      const currentLayers = featureGroup.getLayers().filter(exists);
      const mapLayers = flatten(activeLayers.map(l => l.nodes));

      mapLayers.forEach(configLayer => {
        const currentLayer = currentLayers.find(l => (l as any).options.key === configLayer.key);
        if (configLayer.on === true) {
          if (!currentLayer) {
            const newLayer = wmsHeaders(configLayer.url, configLayer);
            featureGroup.addLayer(newLayer);
          }
        } else {
          if (currentLayer) {
            featureGroup.removeLayer(currentLayer);
          }
        }
      });
    }
  }, [activeLayers, mapInstance]);

  return null;
};
