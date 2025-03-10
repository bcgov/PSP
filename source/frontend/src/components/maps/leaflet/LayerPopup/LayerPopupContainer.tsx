import { Feature, GeoJsonProperties } from 'geojson';
import { geoJSON, LatLngBounds, LatLngLiteral } from 'leaflet';
import React, { useEffect, useState } from 'react';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { LocationFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { exists } from '@/utils';

import { PopupContentConfig } from './components/LayerPopupContent';
import {
  getDynamicFeatureConfig,
  highwayLayerPopupConfig,
  municipalityLayerPopupConfig,
  parcelLayerPopupConfig,
} from './constants';
import { LayerPopupView } from './LayerPopupView';

export interface LayerData {
  title: string;
  bounds?: LatLngBounds;
  feature?: Feature;
  /**
   * Data coming from the GeoJSON feature.properties
   * @property
   * @example
   * feature: {
   *  properties: {
   *    'ADMIN_AREA_ID': 1,
   *    'ADMIN_AERA_NAME: 'West Saanich'
   *  }
   * }
   */
  data: GeoJsonProperties;
  /**
   * A configuration used to display the properties fields in the popup content
   * @property
   * @example
   * {ADMIN_AREA_SQFT: (data: any) => `${data.ADMIN_AREA_SQFT} ft^2`}
   */
  config: PopupContentConfig;
}

export type LayerPopupInformation = {
  latlng: LatLngLiteral;
  layers: LayerData[];
};

const emptyLayerPopupInformation: LayerPopupInformation = {
  latlng: { lat: 0, lng: 0 },
  layers: [],
};

interface ILayerPopupContainer {
  featureDataset: LocationFeatureDataset | null;
}

export const LayerPopupContainer: React.FC<
  React.PropsWithChildren<ILayerPopupContainer>
> = props => {
  //const mapMachine = useMapStateMachine();
  const composedProperty = props.composedProperty;

  const [layerPopup, setLayerPopup] = useState<LayerPopupInformation>(emptyLayerPopupInformation);

  useEffect(() => {
    if (mapMachine.mapLocationFeatureDataset) {
      const featureSet = mapMachine.mapLocationFeatureDataset;

      const layersData: LayerData[] = [];

      if (exists(featureSet.parcelFeature)) {
        const parcelData: LayerData = { title: 'LTSA ParcelMap data', data: null, config: {} };

        parcelData.bounds = featureSet.parcelFeature.geometry
          ? geoJSON(featureSet.parcelFeature.geometry).getBounds()
          : undefined;
        parcelData.config = parcelLayerPopupConfig;
        parcelData.data = featureSet.parcelFeature.properties;
        parcelData.feature = featureSet.parcelFeature;
        layersData.push(parcelData);
      }
      if (exists(featureSet.crownLandLeasesFeatures)) {
        const parcelData: LayerData = {
          title: 'Crown Land Leases',
          data: null,
          config: {},
        };

        parcelData.bounds = featureSet.crownLandLeasesFeatures.geometry
          ? geoJSON(featureSet.crownLandLeasesFeatures.geometry).getBounds()
          : undefined;
        parcelData.config = getDynamicFeatureConfig(featureSet.crownLandLeasesFeatures);
        parcelData.data = featureSet.crownLandLeasesFeatures.properties;
        parcelData.feature = featureSet.crownLandLeasesFeatures;
        layersData.push(parcelData);
      }
      if (exists(featureSet.crownLandLicensesFeatures)) {
        const parcelData: LayerData = {
          title: 'Crown Land Licenses',
          data: null,
          config: {},
        };

        parcelData.bounds = featureSet.crownLandLicensesFeatures.geometry
          ? geoJSON(featureSet.crownLandLicensesFeatures.geometry).getBounds()
          : undefined;
        parcelData.config = getDynamicFeatureConfig(featureSet.crownLandLicensesFeatures);
        parcelData.data = featureSet.crownLandLicensesFeatures.properties;
        parcelData.feature = featureSet.crownLandLicensesFeatures;
        layersData.push(parcelData);
      }
      if (exists(featureSet.crownLandTenuresFeatures)) {
        const parcelData: LayerData = {
          title: 'Crown Land Tenures',
          data: null,
          config: {},
        };

        parcelData.bounds = featureSet.crownLandTenuresFeatures.geometry
          ? geoJSON(featureSet.crownLandTenuresFeatures.geometry).getBounds()
          : undefined;
        parcelData.config = getDynamicFeatureConfig(featureSet.crownLandTenuresFeatures);
        parcelData.data = featureSet.crownLandTenuresFeatures.properties;
        parcelData.feature = featureSet.crownLandTenuresFeatures;
        layersData.push(parcelData);
      }
      if (exists(featureSet.crownLandInventoryFeatures)) {
        const parcelData: LayerData = {
          title: 'Crown Land Inventory',
          data: null,
          config: {},
        };

        parcelData.bounds = featureSet.crownLandInventoryFeatures.geometry
          ? geoJSON(featureSet.crownLandInventoryFeatures.geometry).getBounds()
          : undefined;
        parcelData.config = getDynamicFeatureConfig(featureSet.crownLandInventoryFeatures);
        parcelData.data = featureSet.crownLandInventoryFeatures.properties;
        parcelData.feature = featureSet.crownLandInventoryFeatures;
        layersData.push(parcelData);
      }
      if (exists(featureSet.crownLandInclusionsFeatures)) {
        const parcelData: LayerData = {
          title: 'Crown Land Inclusions',
          data: null,
          config: {},
        };

        parcelData.bounds = featureSet.crownLandInclusionsFeatures.geometry
          ? geoJSON(featureSet.crownLandInclusionsFeatures.geometry).getBounds()
          : undefined;
        parcelData.config = getDynamicFeatureConfig(featureSet.crownLandInclusionsFeatures);
        parcelData.data = featureSet.crownLandInclusionsFeatures.properties;
        parcelData.feature = featureSet.crownLandInclusionsFeatures;
        layersData.push(parcelData);
      }
      if (exists(featureSet.municipalityFeatures)) {
        const parcelData: LayerData = {
          title: 'Municipality Information',
          data: null,
          config: {},
        };

        parcelData.bounds = featureSet.municipalityFeatures.geometry
          ? geoJSON(featureSet.municipalityFeatures.geometry).getBounds()
          : undefined;
        parcelData.config = municipalityLayerPopupConfig;
        parcelData.data = featureSet.municipalityFeatures.properties;
        parcelData.feature = featureSet.municipalityFeatures;
        layersData.push(parcelData);
      }
      if (exists(featureSet.highwayFeatures) && featureSet.highwayFeatures.length > 0) {
        featureSet.highwayFeatures.forEach((highwayFeature, index) => {
          const parcelData: LayerData = {
            title: `Highway Research (${index + 1} of ${featureSet.highwayFeatures.length})`,
            data: null,
            config: {},
          };
          parcelData.bounds = highwayFeature.geometry
            ? geoJSON(highwayFeature.geometry).getBounds()
            : undefined;
          parcelData.config = highwayLayerPopupConfig;
          parcelData.data = highwayFeature.properties;
          parcelData.feature = highwayFeature;
          layersData.push(parcelData);
        });
      }

      setLayerPopup({
        latlng: mapMachine.mapLocationFeatureDataset.location,
        layers: layersData,
      });
    }
  }, [mapMachine]);

  return (
    <LayerPopupView layerPopup={layerPopup} featureDataset={mapMachine.mapLocationFeatureDataset} />
  );
};
