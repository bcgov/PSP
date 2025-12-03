import { useContext, useMemo } from 'react';

import { TenantContext } from '@/tenants';
import { exists } from '@/utils';

import { layerDefinitions } from '../Control/LayersControl/LayerDefinitions';
import { LayerDefinition } from '../Control/LayersControl/types';

export const useConfiguredMapLayers = () => {
  const { tenant } = useContext(TenantContext);

  const layerConfiguration = tenant.layers;

  const layers = useMemo<LayerDefinition[]>(() => {
    return layerDefinitions.map<LayerDefinition>(ld => {
      const customConfig = layerConfiguration[ld.layerIdentifier];
      if (exists(customConfig)) {
        return { ...ld, ...customConfig };
      }
      return { ...ld };
    });
  }, [layerConfiguration]);

  return layers;
};
