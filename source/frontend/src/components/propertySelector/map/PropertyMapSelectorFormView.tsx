import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { LocationFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { Section } from '@/components/common/Section/Section';
import * as Styled from '@/components/common/styles';

import MapClickMonitor from '../MapClickMonitor';
import PropertyMapSelectorSubForm from './PropertyMapSelectorSubForm';

export interface IPropertyMapSelectorFormViewProps {
  onNewLocation: (property: LocationFeatureDataset, hasMultipleProperties: boolean) => void;
  lastSelectedProperty?: LocationFeatureDataset;
}

const PropertyMapSelectorFormView: React.FunctionComponent<IPropertyMapSelectorFormViewProps> = ({
  onNewLocation,
  lastSelectedProperty,
}) => {
  const mapMachine = useMapStateMachine();

  const onClickDraftMarker = () => {
    mapMachine.startSelection();
  };

  return (
    <Section header={undefined}>
      <Styled.H3>Select a property</Styled.H3>
      <PropertyMapSelectorSubForm
        onClickDraftMarker={onClickDraftMarker}
        selectedProperty={lastSelectedProperty}
      />

      <MapClickMonitor
        onNewLocation={(
          locationDataset: LocationFeatureDataset,
          hasMultipleProperties: boolean,
        ) => {
          if (mapMachine.isSelecting) {
            onNewLocation(locationDataset, hasMultipleProperties);
          }
        }}
      />
    </Section>
  );
};

export default PropertyMapSelectorFormView;
