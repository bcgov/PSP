import { featureCollection } from '@turf/turf';
import { Feature, Geometry } from 'geojson';
import { Popup as LeafletPopup } from 'leaflet';
import React, { useCallback, useMemo } from 'react';
import { Popup } from 'react-leaflet/Popup';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import {
  LocationFeatureDataset,
  WorklistLocationFeatureDataset,
} from '@/components/common/mapFSM/useLocationFeatureLoader';
import { PMBC_FullyAttributed_Feature_Properties } from '@/models/layers/parcelMapBC';
import { exists, firstOrNull } from '@/utils';

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

  const onAddPropertyToWorklist = useCallback(
    (
      feature: Feature<Geometry, PMBC_FullyAttributed_Feature_Properties>,
      featureDataset: LocationFeatureDataset,
    ) => {
      const worklistDataSet: WorklistLocationFeatureDataset = {
        fullyAttributedFeatures: exists(feature) ? featureCollection([feature]) : null,
        pimsFeature: null,
        regionFeature: featureDataset?.regionFeature ?? null,
        districtFeature: featureDataset?.districtFeature ?? null,
        location: featureDataset?.location ?? null,
      };
      mapMachine.worklistAdd(worklistDataSet);
    },
    [mapMachine],
  );

  const onAddAllToWorklist = useCallback(
    (featureDataset: LocationFeatureDataset) => {
      const worklistDataSet: WorklistLocationFeatureDataset = {
        fullyAttributedFeatures: exists(featureDataset?.parcelFeatures)
          ? featureCollection(featureDataset.parcelFeatures)
          : null,
        pimsFeature: firstOrNull(featureDataset?.pimsFeatures),
        regionFeature: featureDataset?.regionFeature ?? null,
        districtFeature: featureDataset?.districtFeature ?? null,
        location: featureDataset?.location ?? null,
      };
      mapMachine.worklistAdd(worklistDataSet);
    },
    [mapMachine],
  );

  const onCloseButtonPressed = (event: React.MouseEvent<HTMLElement, MouseEvent>) => {
    event.stopPropagation();
    mapMachine.closePopup();
  };

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
          onAddPropertyToWorklist={onAddPropertyToWorklist}
          onAddAllToWorklist={onAddAllToWorklist}
          onClose={onCloseButtonPressed}
        />
      )}
    </Popup>
  );
});
