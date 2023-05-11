import * as Styled from 'components/common/styles';
import { MapStateActionTypes, MapStateContext } from 'components/maps/providers/MapStateContext';
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
  const { setState } = React.useContext(MapStateContext);

  const onClickDraftMarker = () => {
    setState({ type: MapStateActionTypes.IS_SELECTING, isSelecting: true });
  };

  React.useEffect(() => {
    return () => {
      setState({ type: MapStateActionTypes.IS_SELECTING, isSelecting: false });
    };
  }, [setState]);

  return (
    <>
      <Section header={undefined}>
        <Styled.H3>Select a property</Styled.H3>
        <PropertyMapSelectorSubForm
          onClickDraftMarker={onClickDraftMarker}
          selectedProperty={lastSelectedProperty}
        />
        <MapClickMonitor addProperty={onSelectedProperty} modifiedProperties={selectedProperties} />
      </Section>
    </>
  );
};

export default PropertyMapSelectorFormView;
