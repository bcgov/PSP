import { useContext } from 'react';

import { TenantContext } from '@/tenants';

import { layersTree } from '../Control/LayersControl/data';
import { ILayerItem } from '../Control/LayersControl/types';

export const useConfiguredMapLayers = () => {
  const {
    tenant: { layers: confLayers },
  } = useContext(TenantContext);
  const layers = layersTree.map((parent, parentIndex) => {
    //add any layers defined in the configuration.
    const layer = confLayers?.find(cl => cl.key === parent.key);

    const layerNodes = [...(layer?.nodes ?? [])];
    const parentNodes =
      parent?.nodes?.filter(node => !layerNodes.find(layerNode => layerNode.id === node.id)) ?? [];
    const allNodes = [...parentNodes, ...layerNodes];

    return {
      ...parent,
      nodes: allNodes?.map((node: ILayerItem, index) => ({
        ...node,
        zIndex: node.zIndexAbsolute === true ? node.zIndex : (parentIndex + 1) * index,
        opacity: node?.opacity !== undefined ? Number(node?.opacity) : 0.8,
      })),
    };
  });
  return layers;
};
