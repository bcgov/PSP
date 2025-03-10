import { Popup as LeafletPopup } from 'leaflet';
import React, { useMemo } from 'react';
import { Popup } from 'react-leaflet/Popup';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';

import { LayerPopupContainer } from './LayerPopupContainer';
import { MultiplePropertyPopupView } from './MultiplePropertyPopupView';
import { ComposedProperty } from '@/features/mapSideBar/property/ComposedProperty';

export const LocationPopupContainer = React.forwardRef<
  LeafletPopup,
  React.PropsWithChildren<unknown>
>((props, ref) => {
  const mapMachine = useMapStateMachine();

  const multipleProperties = useMemo(() => {
    return mapMachine.mapLocationFeatureDataset.parcelFeatures !== null;
  }, [mapMachine.mapLocationFeatureDataset.parcelFeatures]);

  const composedProperty: ComposedProperty = {};

  return (
    <Popup
      ref={ref}
      position={mapMachine.mapLocationSelected}
      offset={[0, -25]}
      closeButton={false}
      autoPan={false}
      autoClose={false}
      closeOnClick={false}
      closeOnEscapeKey={false}
    >
      {multipleProperties && (
        <MultiplePropertyPopupView featureDataset={mapMachine.mapLocationFeatureDataset} />
      )}
      {!multipleProperties && (
        <LayerPopupContainer featureDataset={mapMachine.mapLocationFeatureDataset} />
      )}
    </Popup>
  );
});
