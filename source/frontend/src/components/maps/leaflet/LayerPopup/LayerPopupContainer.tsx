import { Feature, GeoJsonProperties } from 'geojson';
import { geoJSON, LatLngBounds, LatLngLiteral, Popup as LeafletPopup } from 'leaflet';
import React, { useEffect, useState } from 'react';
import { Popup } from 'react-leaflet/Popup';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
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

export const LayerPopupContainer = React.forwardRef<LeafletPopup, React.PropsWithChildren<unknown>>(
  (props, ref) => {
    const mapMachine = useMapStateMachine();

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
        if (exists(featureSet.crownLandLeasesFeature)) {
          const parcelData: LayerData = {
            title: 'Crown Land Leases',
            data: null,
            config: {},
          };

          parcelData.bounds = featureSet.crownLandLeasesFeature.geometry
            ? geoJSON(featureSet.crownLandLeasesFeature.geometry).getBounds()
            : undefined;
          parcelData.config = getDynamicFeatureConfig(featureSet.crownLandLeasesFeature);
          parcelData.data = featureSet.crownLandLeasesFeature.properties;
          parcelData.feature = featureSet.crownLandLeasesFeature;
          layersData.push(parcelData);
        }
        if (exists(featureSet.crownLandLicensesFeature)) {
          const parcelData: LayerData = {
            title: 'Crown Land Licenses',
            data: null,
            config: {},
          };

          parcelData.bounds = featureSet.crownLandLicensesFeature.geometry
            ? geoJSON(featureSet.crownLandLicensesFeature.geometry).getBounds()
            : undefined;
          parcelData.config = getDynamicFeatureConfig(featureSet.crownLandLicensesFeature);
          parcelData.data = featureSet.crownLandLicensesFeature.properties;
          parcelData.feature = featureSet.crownLandLicensesFeature;
          layersData.push(parcelData);
        }
        if (exists(featureSet.crownLandTenuresFeature)) {
          const parcelData: LayerData = {
            title: 'Crown Land Tenures',
            data: null,
            config: {},
          };

          parcelData.bounds = featureSet.crownLandTenuresFeature.geometry
            ? geoJSON(featureSet.crownLandTenuresFeature.geometry).getBounds()
            : undefined;
          parcelData.config = getDynamicFeatureConfig(featureSet.crownLandTenuresFeature);
          parcelData.data = featureSet.crownLandTenuresFeature.properties;
          parcelData.feature = featureSet.crownLandTenuresFeature;
          layersData.push(parcelData);
        }
        if (exists(featureSet.crownLandInventoryFeature)) {
          const parcelData: LayerData = {
            title: 'Crown Land Inventory',
            data: null,
            config: {},
          };

          parcelData.bounds = featureSet.crownLandInventoryFeature.geometry
            ? geoJSON(featureSet.crownLandInventoryFeature.geometry).getBounds()
            : undefined;
          parcelData.config = getDynamicFeatureConfig(featureSet.crownLandInventoryFeature);
          parcelData.data = featureSet.crownLandInventoryFeature.properties;
          parcelData.feature = featureSet.crownLandInventoryFeature;
          layersData.push(parcelData);
        }
        if (exists(featureSet.crownLandInclusionsFeature)) {
          const parcelData: LayerData = {
            title: 'Crown Land Inclusions',
            data: null,
            config: {},
          };

          parcelData.bounds = featureSet.crownLandInclusionsFeature.geometry
            ? geoJSON(featureSet.crownLandInclusionsFeature.geometry).getBounds()
            : undefined;
          parcelData.config = getDynamicFeatureConfig(featureSet.crownLandInclusionsFeature);
          parcelData.data = featureSet.crownLandInclusionsFeature.properties;
          parcelData.feature = featureSet.crownLandInclusionsFeature;
          layersData.push(parcelData);
        }
        if (exists(featureSet.municipalityFeature)) {
          const parcelData: LayerData = {
            title: 'Municipality Information',
            data: null,
            config: {},
          };

          parcelData.bounds = featureSet.municipalityFeature.geometry
            ? geoJSON(featureSet.municipalityFeature.geometry).getBounds()
            : undefined;
          parcelData.config = municipalityLayerPopupConfig;
          parcelData.data = featureSet.municipalityFeature.properties;
          parcelData.feature = featureSet.municipalityFeature;
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
      <Popup
        ref={ref}
        position={layerPopup.latlng}
        offset={[0, -25]}
        closeButton={false}
        autoPan={false}
        autoClose={false}
        closeOnClick={false}
        closeOnEscapeKey={false}
      >
        <LayerPopupView
          {...props}
          layerPopup={layerPopup}
          featureDataset={mapMachine.mapLocationFeatureDataset}
        />
      </Popup>
    );
  },
);
