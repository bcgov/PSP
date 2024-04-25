import L from 'leaflet';
import { flatten } from 'lodash';
import { useEffect } from 'react';
import { useMap } from 'react-leaflet';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';

import { wmsHeaders } from '../Control/LayersControl/wmsHeaders';

const featureGroup = new L.FeatureGroup();
export const LeafletLayerListener = () => {
  const { activeLayers } = useMapStateMachine();
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
    if (mapInstance) {
      const currentLayers = Object.keys((featureGroup as any)._layers)
        .map(k => (featureGroup as any)._layers[k])
        .map(l => l.options)
        .filter(x => !!x);
      const mapLayers = flatten(activeLayers.map(l => l.nodes)).filter((x: any) => x.on);
      const layersToAdd = mapLayers.filter(
        (layer: any) => !currentLayers.find(x => x.key === layer.key),
      );
      const layersToRemove = currentLayers.filter(
        (layer: any) => !mapLayers.find((x: any) => x.key === layer.key),
      );

      layersToAdd.forEach((node: any) => {
        const layer = wmsHeaders(node.url, node);
        featureGroup.addLayer(layer);
      });

      featureGroup.eachLayer((layer: any) => {
        if (layersToRemove.find(l => l.key === layer?.options?.key)) {
          featureGroup.removeLayer(layer);
        }
      });
    }
  }, [activeLayers, mapInstance]);

  return null;
};
