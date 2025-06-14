import { useContext, useMemo } from 'react';

import { TenantContext } from '@/tenants';
import { exists } from '@/utils';

import { layerDefinitions } from '../Control/LayersControl/LayerDefinitions';
import { LayerDefinition } from '../Control/LayersControl/types';

export const useConfiguredMapLayers = () => {
  const {
    tenant: { layers: layerConfiguration },
  } = useContext(TenantContext);

  const layers = useMemo<LayerDefinition[]>(() => {
    return layerDefinitions.map(ld => {
      const customConfig = layerConfiguration[ld.layerIdentifier];
      if (exists(customConfig)) {
        return { ...ld, a: customConfig };
      }
      return { ...ld };
    });
  }, [layerConfiguration]);

  return layers;
};
