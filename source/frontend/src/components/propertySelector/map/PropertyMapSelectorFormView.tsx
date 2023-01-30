import * as Styled from 'components/common/styles';
import {
  MapCursors,
  MapStateActionTypes,
  MapStateContext,
} from 'components/maps/providers/MapStateContext';
import { Section } from 'features/mapSideBar/tabs/Section';
import * as React from 'react';

import MapClickMonitor from '../MapClickMonitor';
import { IMapProperty } from '../models';
import PropertyMapSelectorSubForm from './PropertyMapSelectorSubForm';

export interface IPropertyMapSelectorFormViewProps {
  onSelectedProperty: (property: IMapProperty) => void;
  lastSelectedProperty?: IMapProperty;
  selectedProperties: IMapProperty[];
}

const PropertyMapSelectorFormView: React.FunctionComponent<
  React.PropsWithChildren<IPropertyMapSelectorFormViewProps>
> = ({ onSelectedProperty, lastSelectedProperty, selectedProperties }) => {
  const { setState, cursor } = React.useContext(MapStateContext);

  const onClickDraftMarker = () => {
    setState({ type: MapStateActionTypes.IS_SELECTING, isSelecting: true });
  };

  const onClickAway = () => {
    // Prevent this handler from getting triggered immediately.
    // We need a timeout here to give the map time to process its own click event while selecting properties on the map.
    setTimeout(() => {
      if (cursor !== MapCursors.DEFAULT) {
        setState({ type: MapStateActionTypes.IS_SELECTING, isSelecting: false });
      }
    }, 100);
  };

  return (
    <>
      <Section header={undefined}>
        <Styled.H3>Select a property</Styled.H3>
        <PropertyMapSelectorSubForm
          onClickDraftMarker={onClickDraftMarker}
          onClickAway={onClickAway}
          selectedProperty={lastSelectedProperty}
        />
        <MapClickMonitor addProperty={onSelectedProperty} modifiedProperties={selectedProperties} />
      </Section>
    </>
  );
};

export default PropertyMapSelectorFormView;
