import { WMSOptions } from 'leaflet';

export interface LayerDefinition extends WMSOptions {
  layerIdentifier: string;
  authenticated?: boolean;
  url: string;
  cql_filter?: string;
  zIndexAbsolute?: boolean;
  legendUrl?: string;
}

interface BaseLayerItem {
  label: string;
  key: string;
}

export interface LayerMenuItem extends BaseLayerItem {
  layerDefinitionId?: string;
  color?: string;
  nodes?: never;
}

export interface LayerMenuGroup extends BaseLayerItem {
  nodes: LayerMenuEntry[];
  layerDefinitionId?: never;
  color?: never;
}

export type LayerMenuEntry = LayerMenuGroup | LayerMenuItem;

export function isLayerItem(layerEntry?: LayerMenuEntry): layerEntry is LayerMenuItem {
  return layerEntry?.layerDefinitionId !== undefined;
}

export function isLayerGroup(layerEntry?: LayerMenuEntry): layerEntry is LayerMenuGroup {
  return layerEntry?.nodes !== undefined;
}

export function getChildrenIds(rootEntry: LayerMenuGroup): string[] {
  let keys: string[] = [];
  rootEntry.nodes.forEach(n => {
    if (isLayerItem(n)) {
      keys.push(n.layerDefinitionId);
    } else {
      keys = keys.concat(getChildrenIds(n));
    }
  });

  return keys;
}
