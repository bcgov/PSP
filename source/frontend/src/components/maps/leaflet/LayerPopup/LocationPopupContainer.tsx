import { Feature, Geometry } from 'geojson';
import { LatLngLiteral, Popup as LeafletPopup } from 'leaflet';
import React, { useCallback, useEffect, useMemo, useState } from 'react';
import { Popup } from 'react-leaflet/Popup';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import {
  TANTALIS_CrownLandInclusions_Feature_Properties,
  TANTALIS_CrownLandInventory_Feature_Properties,
  TANTALIS_CrownLandLeases_Feature_Properties,
  TANTALIS_CrownLandLicenses_Feature_Properties,
  TANTALIS_CrownLandTenures_Feature_Properties,
} from '@/models/layers/crownLand';
import { MOT_DistrictBoundary_Feature_Properties } from '@/models/layers/motDistrictBoundary';
import { MOT_RegionalBoundary_Feature_Properties } from '@/models/layers/motRegionalBoundary';
import { WHSE_Municipalities_Feature_Properties } from '@/models/layers/municipalities';
import { PMBC_FullyAttributed_Feature_Properties } from '@/models/layers/parcelMapBC';
import { ISS_ProvincialPublicHighway } from '@/models/layers/pimsHighwayLayer';
import { PIMS_Property_Location_View } from '@/models/layers/pimsPropertyLocationView';
import { firstOrNull } from '@/utils';

import { LayerPopupContainer } from './LayerPopupContainer';
import { MultiplePropertyPopupView } from './MultiplePropertyPopupView';

export interface SinglePropertyFeatureDataSet {
  selectingComponentId: string | null;
  location: LatLngLiteral;
  fileLocation: LatLngLiteral | null;
  parcelFeature: Feature<Geometry, PMBC_FullyAttributed_Feature_Properties> | null;
  pimsFeature: Feature<Geometry, PIMS_Property_Location_View> | null;
  regionFeature: Feature<Geometry, MOT_RegionalBoundary_Feature_Properties> | null;
  districtFeature: Feature<Geometry, MOT_DistrictBoundary_Feature_Properties> | null;

  municipalityFeatures: Feature<Geometry, WHSE_Municipalities_Feature_Properties>[] | null;
  highwayFeatures: Feature<Geometry, ISS_ProvincialPublicHighway>[] | null;
  crownLandLeasesFeatures: Feature<Geometry, TANTALIS_CrownLandLeases_Feature_Properties>[] | null;
  crownLandLicensesFeatures:
    | Feature<Geometry, TANTALIS_CrownLandLicenses_Feature_Properties>[]
    | null;
  crownLandTenuresFeatures:
    | Feature<Geometry, TANTALIS_CrownLandTenures_Feature_Properties>[]
    | null;
  crownLandInventoryFeatures:
    | Feature<Geometry, TANTALIS_CrownLandInventory_Feature_Properties>[]
    | null;
  crownLandInclusionsFeatures:
    | Feature<Geometry, TANTALIS_CrownLandInclusions_Feature_Properties>[]
    | null;
}

export const LocationPopupContainer = React.forwardRef<
  LeafletPopup,
  React.PropsWithChildren<unknown>
>((_, ref) => {
  const mapMachine = useMapStateMachine();

  const [singleFeatureDataset, setSingleFeatureDataset] =
    useState<SinglePropertyFeatureDataSet | null>(null);

  const hasMultipleProperties = useMemo(() => {
    return (
      singleFeatureDataset === null &&
      mapMachine.mapLocationFeatureDataset.parcelFeatures?.length > 1
    );
  }, [mapMachine.mapLocationFeatureDataset.parcelFeatures?.length, singleFeatureDataset]);

  const onSelectProperty = useCallback(
    (feature: Feature<Geometry, PMBC_FullyAttributed_Feature_Properties> | null) => {
      const singleFeature: SinglePropertyFeatureDataSet = {
        ...mapMachine.mapLocationFeatureDataset,
        parcelFeature: feature,
        pimsFeature: firstOrNull(mapMachine.mapLocationFeatureDataset.pimsFeatures),
      };
      setSingleFeatureDataset(singleFeature);
    },
    [mapMachine.mapLocationFeatureDataset],
  );

  useEffect(() => {
    if (mapMachine.mapLocationFeatureDataset.parcelFeatures?.length <= 1) {
      onSelectProperty(mapMachine.mapLocationFeatureDataset.parcelFeatures[0]);
    }
  }, [
    mapMachine.mapLocationFeatureDataset.parcelFeatures,
    hasMultipleProperties,
    onSelectProperty,
  ]);

  return (
    <Popup
      ref={ref}
      position={mapMachine.mapLocationFeatureDataset?.location}
      offset={[0, -25]}
      closeButton={false}
      autoPan={false}
      autoClose={false}
      closeOnClick={false}
      closeOnEscapeKey={false}
    >
      {hasMultipleProperties && (
        <MultiplePropertyPopupView
          featureDataset={mapMachine.mapLocationFeatureDataset}
          onSelectProperty={onSelectProperty}
        />
      )}
      {!hasMultipleProperties && <LayerPopupContainer featureDataset={singleFeatureDataset} />}
    </Popup>
  );
});
