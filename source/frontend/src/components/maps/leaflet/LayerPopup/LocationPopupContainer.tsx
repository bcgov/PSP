import { Feature, Geometry } from 'geojson';
import { Popup as LeafletPopup } from 'leaflet';
import React, { useCallback, useMemo } from 'react';
import { Popup } from 'react-leaflet/Popup';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { LocationFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { PMBC_FullyAttributed_Feature_Properties } from '@/models/layers/parcelMapBC';

import { MultiplePropertyPopupView } from './MultiplePropertyPopupView';

export const LocationPopupContainer = React.forwardRef<
  LeafletPopup,
  React.PropsWithChildren<unknown>
>((_, ref) => {
  const mapMachine = useMapStateMachine();

  // not absolutely necessary, but it does provide an extra safety by not rendering if there is more than one property
  const hasMultipleProperties = useMemo(() => {
    return mapMachine.mapLocationFeatureDataset?.parcelFeatures?.length > 1;
  }, [mapMachine.mapLocationFeatureDataset?.parcelFeatures?.length]);

  const onSelectProperty = useCallback(
    (feature: Feature<Geometry, PMBC_FullyAttributed_Feature_Properties> | null) => {
      const locationDataSet: LocationFeatureDataset = {
        ...mapMachine.mapLocationFeatureDataset,
        parcelFeatures: [feature],
      };
      mapMachine.setSelectedLocation(locationDataSet);
    },
    [mapMachine],
  );

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
    </Popup>
  );
});
