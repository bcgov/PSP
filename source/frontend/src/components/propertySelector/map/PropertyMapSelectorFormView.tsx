import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { Section } from '@/components/common/Section/Section';
import * as Styled from '@/components/common/styles';

import MapClickMonitor from '../MapClickMonitor';
import { IMapProperty } from '../models';
import PropertyMapSelectorSubForm from './PropertyMapSelectorSubForm';

export interface IPropertyMapSelectorFormViewProps {
  onSelectedProperty: (property: IMapProperty) => void;
  lastSelectedProperty?: IMapProperty;
  selectedProperties: IMapProperty[];
  selectedComponentId?: string | null;
}

const PropertyMapSelectorFormView: React.FunctionComponent<
  React.PropsWithChildren<IPropertyMapSelectorFormViewProps>
> = ({ onSelectedProperty, lastSelectedProperty, selectedProperties, selectedComponentId }) => {
  const mapMachine = useMapStateMachine();

  const onClickDraftMarker = () => {
    mapMachine.startSelection(selectedComponentId ?? undefined);
  };

  return (
    <Section header={undefined}>
      <Styled.H3>Select a property</Styled.H3>
      <PropertyMapSelectorSubForm
        onClickDraftMarker={onClickDraftMarker}
        selectedProperty={lastSelectedProperty}
      />

      <MapClickMonitor
        addProperty={onSelectedProperty}
        modifiedProperties={selectedProperties}
        selectedComponentId={selectedComponentId ?? null}
      />
    </Section>
  );
};

export default PropertyMapSelectorFormView;
