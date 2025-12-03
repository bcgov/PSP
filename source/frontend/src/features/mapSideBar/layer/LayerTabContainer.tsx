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
    if (!exists(composedProperty?.featureDataset)) {
      return [];
    }

    const featureDataset = composedProperty?.featureDataset;
    const crownFeaturesTotal =
      (featureDataset.crownLandTenuresFeatures?.length || 0) +
      (featureDataset.crownLandLeasesFeatures?.length || 0) +
      (featureDataset.crownLandInclusionsFeatures?.length || 0) +
      (featureDataset.crownLandInventoryFeatures?.length || 0) +
      (featureDataset.crownLandLicensesFeatures?.length || 0);

    return [
      ...buildLayerData({
        features: featureDataset?.pimsFeatures,
        titlePrefix: 'PIMS data',
        tab: InventoryTabNames.pims,
        config: pimsLayerConfig,
      }),
      ...buildLayerData({
        features: featureDataset?.parcelFeatures,
        titlePrefix: 'LTSA ParcelMap data',
        tab: InventoryTabNames.pmbc,
        config: parcelLayerConfig,
      }),
      ...buildLayerData({
        features: featureDataset?.crownLandTenuresFeatures,
        titlePrefix: 'Crown Land Tenures',
        tab: InventoryTabNames.crown,
        config: {},
        dynamicConfig: true,
        totalCount: crownFeaturesTotal,
      }),
      ...buildLayerData({
        features: featureDataset?.crownLandLicensesFeatures,
        titlePrefix: 'Crown Land Licenses',
        tab: InventoryTabNames.crown,
        config: {},
        dynamicConfig: true,
        totalCount: crownFeaturesTotal,
      }),
      ...buildLayerData({
        features: featureDataset?.crownLandLeasesFeatures,
        titlePrefix: 'Crown Land Tenures',
        tab: InventoryTabNames.crown,
        config: {},
        dynamicConfig: true,
        totalCount: crownFeaturesTotal,
      }),
      ...buildLayerData({
        features: featureDataset?.crownLandInventoryFeatures,
        titlePrefix: 'Crown Land Inventory',
        tab: InventoryTabNames.crown,
        config: {},
        dynamicConfig: true,
        totalCount: crownFeaturesTotal,
      }),
      ...buildLayerData({
        features: featureDataset?.crownLandInclusionsFeatures,
        titlePrefix: 'Crown Land Inclusions',
        tab: InventoryTabNames.crown,
        config: {},
        dynamicConfig: true,
        totalCount: crownFeaturesTotal,
      }),
      ...buildLayerData({
        features: featureDataset?.municipalityFeatures,
        titlePrefix: 'Municipality Information',
        tab: InventoryTabNames.other,
        config: municipalityLayerConfig,
        group: 'municipality',
      }),
      ...buildLayerData({
        features: featureDataset?.alrFeatures,
        titlePrefix: 'ALR Information',
        tab: InventoryTabNames.other,
        config: alrLayerConfig,
        group: 'alr',
      }),
      ...buildLayerData({
        features: featureDataset?.firstNationFeatures,

        titlePrefix: 'First Nations Information',
        tab: InventoryTabNames.other,
        config: firstNationsLayerConfig,
        group: 'first_nations',
      }),
      ...buildLayerData({
        features: featureDataset?.electoralFeatures,
        titlePrefix: 'Electoral District Information',
        tab: InventoryTabNames.other,
        config: electoralLayerConfig,
        group: 'electoral',
      }),
      ...buildLayerData({
        features: featureDataset?.highwayFeatures,
        titlePrefix: 'Highway Research',
        tab: InventoryTabNames.highway,
        config: {},
        dynamicConfig: true,
      }),
    ];
  }, [composedProperty]);

  const activeLayersData = useMemo(
    () => layersData.filter(ld => ld.tab === activeTab),
    [layersData, activeTab],
  );

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
