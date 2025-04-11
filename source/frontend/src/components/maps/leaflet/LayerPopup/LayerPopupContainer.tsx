import { Feature, GeoJsonProperties } from 'geojson';
import { geoJSON, LatLngBounds, LatLngLiteral } from 'leaflet';
import React, { useEffect, useState } from 'react';

import { exists } from '@/utils';

import { PopupContentConfig } from './components/LayerPopupContent';
import {
  getDynamicFeatureConfig,
  highwayLayerPopupConfig,
  municipalityLayerPopupConfig,
  parcelLayerPopupConfig,
} from './constants';
import { LayerPopupView } from './LayerPopupView';
import { SinglePropertyFeatureDataSet } from './LocationPopupContainer';

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
  featureDataset: SinglePropertyFeatureDataSet | null;
}

export const LayerPopupContainer: React.FC<
  React.PropsWithChildren<ILayerPopupContainer>
> = props => {
  const featureSet = props.featureDataset;

  const [layerPopup, setLayerPopup] = useState<LayerPopupInformation>(emptyLayerPopupInformation);

  useEffect(() => {
    if (featureSet) {
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

      if (
        exists(featureSet.crownLandLeasesFeatures) &&
        featureSet.crownLandLeasesFeatures.length > 0
      ) {
        featureSet.crownLandLeasesFeatures.forEach((crownLandLeasesFeature, index) => {
          const parcelData: LayerData = {
            title: `Crown Land Leases (${index + 1} of ${featureSet.municipalityFeatures.length})`,
            data: null,
            config: {},
          };

          parcelData.bounds = crownLandLeasesFeature.geometry
            ? geoJSON(crownLandLeasesFeature.geometry).getBounds()
            : undefined;
          parcelData.config = getDynamicFeatureConfig(crownLandLeasesFeature);
          parcelData.data = crownLandLeasesFeature.properties;
          parcelData.feature = crownLandLeasesFeature;
          layersData.push(parcelData);
        });
      }

      if (
        exists(featureSet.crownLandLicensesFeatures) &&
        featureSet.crownLandLicensesFeatures.length > 0
      ) {
        featureSet.crownLandLicensesFeatures.forEach((crownLandLicensesFeature, index) => {
          const parcelData: LayerData = {
            title: `Crown Land Licenses (${index + 1} of ${
              featureSet.crownLandLicensesFeatures.length
            })`,
            data: null,
            config: {},
          };

          parcelData.bounds = crownLandLicensesFeature.geometry
            ? geoJSON(crownLandLicensesFeature.geometry).getBounds()
            : undefined;
          parcelData.config = getDynamicFeatureConfig(crownLandLicensesFeature);
          parcelData.data = crownLandLicensesFeature.properties;
          parcelData.feature = crownLandLicensesFeature;
          layersData.push(parcelData);
        });
      }

      if (
        exists(featureSet.crownLandTenuresFeatures) &&
        featureSet.crownLandTenuresFeatures.length > 0
      ) {
        featureSet.crownLandTenuresFeatures.forEach((crownLandTenuresFeature, index) => {
          const parcelData: LayerData = {
            title: `Crown Land Tenures (${index + 1} of ${
              featureSet.crownLandTenuresFeatures.length
            })`,
            data: null,
            config: {},
          };

          parcelData.bounds = crownLandTenuresFeature.geometry
            ? geoJSON(crownLandTenuresFeature.geometry).getBounds()
            : undefined;
          parcelData.config = getDynamicFeatureConfig(crownLandTenuresFeature);
          parcelData.data = crownLandTenuresFeature.properties;
          parcelData.feature = crownLandTenuresFeature;
          layersData.push(parcelData);
        });
      }

      if (
        exists(featureSet.crownLandInventoryFeatures) &&
        featureSet.crownLandInventoryFeatures.length > 0
      ) {
        featureSet.crownLandInventoryFeatures.forEach((crownLandInventoryFeature, index) => {
          const parcelData: LayerData = {
            title: `Crown Land Inventory (${index + 1} of ${
              featureSet.crownLandInventoryFeatures.length
            })`,
            data: null,
            config: {},
          };

          parcelData.bounds = crownLandInventoryFeature.geometry
            ? geoJSON(crownLandInventoryFeature.geometry).getBounds()
            : undefined;
          parcelData.config = getDynamicFeatureConfig(crownLandInventoryFeature);
          parcelData.data = crownLandInventoryFeature.properties;
          parcelData.feature = crownLandInventoryFeature;
          layersData.push(parcelData);
        });
      }

      if (
        exists(featureSet.crownLandInclusionsFeatures) &&
        featureSet.crownLandInclusionsFeatures.length > 0
      ) {
        featureSet.crownLandInclusionsFeatures.forEach((crownLandInclusionsFeature, index) => {
          const parcelData: LayerData = {
            title: `Crown Land Inclusions (${index + 1} of ${
              featureSet.crownLandInclusionsFeatures.length
            })`,
            data: null,
            config: {},
          };

          parcelData.bounds = crownLandInclusionsFeature.geometry
            ? geoJSON(crownLandInclusionsFeature.geometry).getBounds()
            : undefined;
          parcelData.config = getDynamicFeatureConfig(crownLandInclusionsFeature);
          parcelData.data = crownLandInclusionsFeature.properties;
          parcelData.feature = crownLandInclusionsFeature;
          layersData.push(parcelData);
        });
      }

      if (exists(featureSet.municipalityFeatures) && featureSet.municipalityFeatures.length > 0) {
        featureSet.municipalityFeatures.forEach((municipalityFeature, index) => {
          const parcelData: LayerData = {
            title: `Municipality Information (${index + 1} of ${
              featureSet.municipalityFeatures.length
            })`,
            data: null,
            config: {},
          };

          parcelData.bounds = municipalityFeature.geometry
            ? geoJSON(municipalityFeature.geometry).getBounds()
            : undefined;
          parcelData.config = municipalityLayerPopupConfig;
          parcelData.data = municipalityFeature.properties;
          parcelData.feature = municipalityFeature;
          layersData.push(parcelData);
        });
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
        latlng: featureSet.location,
        layers: layersData,
      });
    }
  }, [featureSet]);

  return <LayerPopupView layerPopup={layerPopup} featureDataset={featureSet} />;
};
