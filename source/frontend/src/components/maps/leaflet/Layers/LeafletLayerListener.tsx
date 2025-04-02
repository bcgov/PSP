import L from 'leaflet';
import { flatten } from 'lodash';
import { useEffect, useRef } from 'react';
import { useMap } from 'react-leaflet';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import useDeepCompareEffect from '@/hooks/util/useDeepCompareEffect';
import { exists } from '@/utils';

import { wmsHeaders } from '../Control/LayersControl/wmsHeaders';

export const LeafletLayerListener = ({ pane }: { pane: string }) => {
  const { activeLayers, mapLayersToRefresh, setMapLayersToRefresh } = useMapStateMachine();
  const mapInstance = useMap();

  const featureGroupRef = useRef(new L.FeatureGroup<L.TileLayer.WMS>([], { pane }));
  const instance = featureGroupRef.current;

  useEffect(() => {
    if (mapInstance) {
      instance.addTo(mapInstance);
    }

    return () => {
      mapInstance?.removeLayer(instance);
    };
  }, [instance, mapInstance]);

  useDeepCompareEffect(() => {
    if (mapLayersToRefresh?.length) {
      const currentLayers = instance.getLayers().filter(exists);
      mapLayersToRefresh.forEach(configLayer => {
        const currentLayer = currentLayers.find(l => (l as any).options.key === configLayer.key);

        if (currentLayer) {
          instance.removeLayer(currentLayer);
          instance.addLayer(currentLayer);
        }
      });
      setMapLayersToRefresh([]);
    }
  }, [instance, mapInstance, mapLayersToRefresh, setMapLayersToRefresh]);

  useEffect(() => {
    if (mapInstance) {
      const currentLayers = instance.getLayers().filter(exists);
      const mapLayers = flatten(activeLayers.map(l => l.nodes));

      mapLayers.forEach(configLayer => {
        const currentLayer = currentLayers.find(l => (l as any).options.key === configLayer.key);
        if (configLayer.on === true) {
          if (!currentLayer) {
            const newLayer = wmsHeaders(configLayer.url, { ...configLayer, pane });
            instance.addLayer(newLayer);
          }
        } else {
          if (currentLayer) {
            instance.removeLayer(currentLayer);
          }
        }
      });
    }
  }, [activeLayers, instance, mapInstance, pane]);

  return null;
};
