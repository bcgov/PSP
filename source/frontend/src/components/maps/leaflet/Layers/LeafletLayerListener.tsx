import L from 'leaflet';
import { useEffect, useRef } from 'react';
import { useMap } from 'react-leaflet';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import useDeepCompareEffect from '@/hooks/util/useDeepCompareEffect';
import { exists } from '@/utils';

import { wmsHeaders } from '../Control/LayersControl/wmsHeaders';
import { useConfiguredMapLayers } from './useConfiguredMapLayers';

export const LeafletLayerListener = ({ pane }: { pane: string }) => {
  const { activeLayers, mapLayersToRefresh, setMapLayersToRefresh } = useMapStateMachine();
  const mapInstance = useMap();

  const mapLayers = useConfiguredMapLayers();

  const featureGroupRef = useRef(new L.FeatureGroup<L.TileLayer.WMS>([], { pane }));
  const instance = featureGroupRef.current;

  useEffect(() => {
    if (exists(mapInstance) && exists(instance)) {
      instance.addTo(mapInstance);
    }

    return () => {
      if (exists(mapInstance) && exists(instance)) {
        mapInstance?.removeLayer(instance);
      }
    };
  }, [instance, mapInstance]);

  useDeepCompareEffect(() => {
    if (mapLayersToRefresh?.size > 0) {
      const currentLayers = instance.getLayers().filter(exists);

      mapLayersToRefresh.forEach(configLayer => {
        const currentLayer = currentLayers.find(
          l => (l as any).options.layerIdentifier === configLayer,
        );

        if (exists(currentLayer) && exists(instance)) {
          instance.removeLayer(currentLayer);
          instance.addLayer(currentLayer);
        }
      });
      setMapLayersToRefresh(new Set());
    }
  }, [instance, mapInstance, mapLayersToRefresh, setMapLayersToRefresh]);

  useEffect(() => {
    if (exists(mapInstance) && exists(instance)) {
      const currentLayers = instance.getLayers().filter(exists);

      const currentIdentifierSet = new Set(
        currentLayers.map(l => (l as any).options.layerIdentifier),
      );

      activeLayers.forEach(x => {
        if (!currentIdentifierSet.has(x)) {
          const layerDefinition = mapLayers.find(ld => ld.layerIdentifier === x);
          const newLayer = wmsHeaders(layerDefinition.url, { ...layerDefinition, pane });
          instance.addLayer(newLayer);
        }
      });

      currentIdentifierSet.forEach(x => {
        if (!activeLayers.has(x)) {
          const currentLayer = currentLayers.find(cl => (cl as any).options.layerIdentifier === x);
          instance.removeLayer(currentLayer);
        }
      });
    }
  }, [activeLayers, instance, mapInstance, mapLayers, pane]);

  return null;
};
