import { LatLngLiteral } from 'leaflet';
import { toast } from 'react-toastify';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import useDraftMarkerSynchronizer from '@/hooks/useDraftMarkerSynchronizer';
import { usePrevious } from '@/hooks/usePrevious';
import useDeepCompareEffect from '@/hooks/util/useDeepCompareEffect';
import { exists, firstOrNull, isValidId } from '@/utils';
import { featuresetToLocationBoundaryDataset } from '@/utils/mapPropertyUtils';

import { SelectedFeatureDataset } from '../common/mapFSM/useLocationFeatureLoader';

interface IMapClickMonitorProps {
  addProperty: (property: SelectedFeatureDataset) => void;
  repositionProperty: (
    property: SelectedFeatureDataset,
    latLng: LatLngLiteral,
    propertyIndex: number | null,
  ) => void;
  modifiedProperties: SelectedFeatureDataset[]; // TODO: this should be just a list of lat longs
  selectedComponentId: string | null;
}

export const MapClickMonitor: React.FunctionComponent<IMapClickMonitorProps> = ({
  addProperty,
  repositionProperty,
  modifiedProperties,
  selectedComponentId,
}) => {
  const mapMachine = useMapStateMachine();

  const previous = usePrevious(mapMachine.mapLocationFeatureDataset);
  const modifiedMapProperties = modifiedProperties.map(mp =>
    featuresetToLocationBoundaryDataset(mp),
  );
  useDraftMarkerSynchronizer(selectedComponentId ? [] : modifiedMapProperties); // disable the draft marker synchronizer if the selecting component is set - the parent will need to control the draft markers.

  useDeepCompareEffect(() => {
    if (
      mapMachine.isSelecting &&
      mapMachine.mapLocationFeatureDataset &&
      previous !== mapMachine.mapLocationFeatureDataset &&
      previous !== undefined &&
      (!selectedComponentId ||
        selectedComponentId === mapMachine.mapLocationFeatureDataset.selectingComponentId)
    ) {
      const selectedFeature: SelectedFeatureDataset = {
        location: mapMachine.mapLocationFeatureDataset.location,
        parcelFeature: firstOrNull(mapMachine.mapLocationFeatureDataset.parcelFeatures),
        pimsFeature: firstOrNull(mapMachine.mapLocationFeatureDataset.pimsFeatures),
        regionFeature: mapMachine.mapLocationFeatureDataset.regionFeature,
        districtFeature: mapMachine.mapLocationFeatureDataset.districtFeature,
        municipalityFeature: firstOrNull(mapMachine.mapLocationFeatureDataset.municipalityFeatures),
        selectingComponentId: mapMachine.mapLocationFeatureDataset.selectingComponentId,
        fileLocation: mapMachine.mapLocationFeatureDataset.fileLocation,
      };
      const parcelFeaturesNotInPims =
        mapMachine.mapLocationFeatureDataset.parcelFeatures?.filter(pf => {
          const matchingProperty = mapMachine.mapLocationFeatureDataset.pimsFeatures?.find(
            plp =>
              (isValidId(pf.properties.PID_NUMBER) &&
                plp.properties.PID === pf.properties.PID_NUMBER) ||
              (isValidId(pf.properties.PIN) && plp.properties.PIN === pf.properties.PIN),
          );
          return !exists(matchingProperty);
        }) ?? [];

      // psp-10193, we cannot support adding a property via click when there are multiple parcel map of multiple pims references already at this location.
      if (
        parcelFeaturesNotInPims.length +
          (mapMachine.mapLocationFeatureDataset.pimsFeatures?.length ?? 0) >
        1
      ) {
        toast.error(
          'There are multiple properties at the clicked location. Use the "Search" functionality instead of "Locate on Map" to add one of the properties at this location instead.',
          { autoClose: false },
        );
        return;
      }

      addProperty(selectedFeature);
    }

    if (
      mapMachine.isRepositioning &&
      mapMachine.repositioningFeatureDataset &&
      mapMachine.mapLocationFeatureDataset &&
      previous !== mapMachine.mapLocationFeatureDataset &&
      previous !== undefined &&
      (!selectedComponentId ||
        selectedComponentId === mapMachine.mapLocationFeatureDataset.selectingComponentId)
    ) {
      repositionProperty(
        mapMachine.repositioningFeatureDataset,
        mapMachine.mapLocationFeatureDataset.location,
        mapMachine.repositioningPropertyIndex,
      );
    }
  }, [
    addProperty,
    mapMachine.isSelecting,
    mapMachine.isRepositioning,
    mapMachine.mapLocationFeatureDataset,
    mapMachine.repositioningFeatureDataset,
    previous,
  ]);
  return <></>;
};

export default MapClickMonitor;
