import { Feature, GeoJsonProperties } from 'geojson';
import { geoJSON, LatLngBounds } from 'leaflet';
import React, { useEffect, useMemo, useState } from 'react';

import { exists } from '@/utils';

import { ComposedProperty } from '../property/ComposedProperty';
import { InventoryTabNames } from '../property/InventoryTabs';
import {
  alrLayerConfig,
  electoralLayerConfig,
  firstNationsLayerConfig,
  getDynamicFeatureConfig,
  municipalityLayerConfig,
  parcelLayerConfig,
  pimsLayerConfig,
} from './constants';
import { ContentConfig } from './LayerContent';
import { ILayerTabCollapsedViewProps } from './LayerTabCollapsedView';
import { ILayerTabViewProps } from './LayerTabView';

export interface LayerData {
  title: React.ReactNode;
  bounds?: LatLngBounds;
  feature?: Feature;
  data: GeoJsonProperties;
  config: ContentConfig;
  tab: InventoryTabNames;
  group?: string;
}

type LayerTabViewComponent =
  | React.FunctionComponent<React.PropsWithChildren<ILayerTabViewProps>>
  | React.FunctionComponent<React.PropsWithChildren<ILayerTabCollapsedViewProps>>;

interface ILayerTabContainer {
  composedProperty: ComposedProperty | null;
  activeTab: InventoryTabNames;
  View: LayerTabViewComponent;
}

// Helper to build LayerData objects
function buildLayerData({
  features,
  titlePrefix,
  tab,
  config,
  dynamicConfig = false,
  totalCount,
  group,
}: {
  features?: Feature[];
  titlePrefix: string;
  tab: InventoryTabNames;
  config: ContentConfig;
  dynamicConfig?: boolean;
  totalCount?: number;
  group?: string;
}): LayerData[] {
  if (!exists(features) || !features?.length) return [];
  return features.map((feature, index) => ({
    title: (
      <>
        {titlePrefix} ({index + 1} of {features.length})
        {totalCount ? <i> out of {totalCount} total features</i> : ''}
      </>
    ),
    bounds: feature.geometry ? geoJSON(feature.geometry).getBounds() : undefined,
    data: feature.properties,
    feature,
    config: dynamicConfig ? getDynamicFeatureConfig(feature) : config,
    tab,
    group,
  }));
}

export const LayerTabContainer: React.FC<React.PropsWithChildren<ILayerTabContainer>> = ({
  composedProperty,
  activeTab,
  View,
}) => {
  const [activePage, setActivePage] = useState<number>(0);
  const [activeGroupedPages, setActiveGroupedPages] = useState<Record<string, number>>({});

  useEffect(() => {
    setActivePage(0); // reset the page whenever the tab changes.
    setActiveGroupedPages({}); // reset grouped pages as well
  }, [activeTab]);

  const layersData = useMemo(() => {
    if (!composedProperty) return [];
    const crownFeaturesTotal =
      (composedProperty.crownTenureFeatures?.length || 0) +
      (composedProperty.crownLeaseFeatures?.length || 0) +
      (composedProperty.crownInclusionFeatures?.length || 0) +
      (composedProperty.crownInventoryFeatures?.length || 0) +
      (composedProperty.crownLicenseFeatures?.length || 0);

    return [
      ...buildLayerData({
        features: composedProperty.pimsGeoserverFeatureCollection?.features,
        titlePrefix: 'PIMS data',
        tab: InventoryTabNames.pims,
        config: pimsLayerConfig,
      }),
      ...buildLayerData({
        features: composedProperty.parcelMapFeatureCollection?.features,
        titlePrefix: 'LTSA ParcelMap data',
        tab: InventoryTabNames.pmbc,
        config: parcelLayerConfig,
      }),
      ...buildLayerData({
        features: composedProperty.crownTenureFeatures,
        titlePrefix: 'Crown Land Tenures',
        tab: InventoryTabNames.crown,
        config: {},
        dynamicConfig: true,
        totalCount: crownFeaturesTotal,
      }),
      ...buildLayerData({
        features: composedProperty.crownLicenseFeatures,
        titlePrefix: 'Crown Land Licenses',
        tab: InventoryTabNames.crown,
        config: {},
        dynamicConfig: true,
        totalCount: crownFeaturesTotal,
      }),
      ...buildLayerData({
        features: composedProperty.crownLeaseFeatures,
        titlePrefix: 'Crown Land Tenures',
        tab: InventoryTabNames.crown,
        config: {},
        dynamicConfig: true,
        totalCount: crownFeaturesTotal,
      }),
      ...buildLayerData({
        features: composedProperty.crownInventoryFeatures,
        titlePrefix: 'Crown Land Inventory',
        tab: InventoryTabNames.crown,
        config: {},
        dynamicConfig: true,
        totalCount: crownFeaturesTotal,
      }),
      ...buildLayerData({
        features: composedProperty.crownInclusionFeatures,
        titlePrefix: 'Crown Land Inclusions',
        tab: InventoryTabNames.crown,
        config: {},
        dynamicConfig: true,
        totalCount: crownFeaturesTotal,
      }),
      ...buildLayerData({
        features: composedProperty.municipalityFeatures,
        titlePrefix: 'Municipality Information',
        tab: InventoryTabNames.other,
        config: municipalityLayerConfig,
        group: 'municipality',
      }),
      ...buildLayerData({
        features: composedProperty.alrFeatures,
        titlePrefix: 'ALR Information',
        tab: InventoryTabNames.other,
        config: alrLayerConfig,
        group: 'alr',
      }),
      ...buildLayerData({
        features: composedProperty.firstNationFeatures,

        titlePrefix: 'First Nations Information',
        tab: InventoryTabNames.other,
        config: firstNationsLayerConfig,
        group: 'first_nations',
      }),
      ...buildLayerData({
        features: composedProperty.electoralFeatures,
        titlePrefix: 'Electoral District Information',
        tab: InventoryTabNames.other,
        config: electoralLayerConfig,
        group: 'electoral',
      }),
      ...buildLayerData({
        features: composedProperty.highwayFeatures,
        titlePrefix: 'Highway Research',
        tab: InventoryTabNames.highway,
        config: {},
        dynamicConfig: true,
      }),
    ];
  }, [composedProperty]);

  const activeLayersData = layersData.filter(ld => ld.tab === activeTab);

  // Always pass all props, both views will ignore unused ones
  return (
    <View
      layersData={activeLayersData}
      activePage={activePage}
      setActivePage={setActivePage}
      activeGroupedPages={activeGroupedPages}
      setActiveGroupedPages={setActiveGroupedPages}
    />
  );
};
