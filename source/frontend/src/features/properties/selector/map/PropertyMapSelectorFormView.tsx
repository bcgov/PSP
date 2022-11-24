import * as Styled from 'components/common/styles';
import {
  MapCursors,
  MapStateActionTypes,
  MapStateContext,
} from 'components/maps/providers/MapStateContext';
import { Section } from 'features/mapSideBar/tabs/Section';
import * as React from 'react';
import { useEffect } from 'react';

import MapClickMonitor from '../components/MapClickMonitor';
import { IMapProperty } from '../models';
import PropertyMapSelectorSubForm from './PropertyMapSelectorSubForm';

export interface IPropertyMapSelectorFormViewProps {
  onSelectedProperty: (property: IMapProperty) => void;
  initialSelectedProperty?: IMapProperty;
}

const PropertyMapSelectorFormView: React.FunctionComponent<IPropertyMapSelectorFormViewProps> = ({
  onSelectedProperty,
  initialSelectedProperty,
}) => {
  const { setState, cursor } = React.useContext(MapStateContext);

  const [selectedProperty, setSelectedProperty] = React.useState<IMapProperty | undefined>(
    initialSelectedProperty,
  );

  useEffect(() => {
    if (initialSelectedProperty !== undefined) {
      setSelectedProperty(initialSelectedProperty);
    }
  }, [initialSelectedProperty]);

  const onClickDraftMarker = () => {
    setState({ type: MapStateActionTypes.IS_SELECTING, isSelecting: true });
  };

  const onClickAway = () => {
    if (cursor !== MapCursors.DEFAULT) {
      setState({ type: MapStateActionTypes.IS_SELECTING, isSelecting: false });
    }
  };

  const addProperty = (property: IMapProperty) => {
    setSelectedProperty(property);
    onSelectedProperty(property);
  };
  return (
    <>
      <Section header={undefined}>
        <Styled.H3>Select a property</Styled.H3>
        <PropertyMapSelectorSubForm
          onClickDraftMarker={onClickDraftMarker}
          onClickAway={onClickAway}
          selectedProperty={selectedProperty}
        />
        <MapClickMonitor addProperty={addProperty} />
      </Section>
    </>
  );
};

export default PropertyMapSelectorFormView;
