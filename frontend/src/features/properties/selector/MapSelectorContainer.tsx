import {
  MapCursors,
  SelectedPropertyContext,
} from 'components/maps/providers/SelectedPropertyContext';
import { IProperty } from 'interfaces';
import * as React from 'react';

import { PropertySelectorTabsView } from '../../mapSideBar/tabs/PropertySelectorTabsView';
import PropertySelectorFormView from './PropertySelectorFormView';

export interface IMapSelectorContainerProps {
  properties?: IProperty[];
}

export const MapSelectorContainer: React.FunctionComponent<IMapSelectorContainerProps> = ({
  properties,
}) => {
  const { setCursor, cursor } = React.useContext(SelectedPropertyContext);
  return (
    <>
      <PropertySelectorTabsView
        MapSelectorView={
          <PropertySelectorFormView
            onClickDraftMarker={() => {
              console.log('Cursor set!');
              setCursor(MapCursors.DRAFT);
            }}
            onClickAway={() => {
              setCursor(undefined);
            }}
            selecting={cursor === MapCursors.DRAFT}
            properties={properties}
          />
        }
        ListSelectorView={<></>}
      ></PropertySelectorTabsView>
    </>
  );
};

export default MapSelectorContainer;
