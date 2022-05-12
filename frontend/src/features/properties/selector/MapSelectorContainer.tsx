import { SelectedPropertyContext } from 'components/maps/providers/SelectedPropertyContext';
import * as React from 'react';

import { PropertySelectorTabsView } from '../../mapSideBar/tabs/PropertySelectorTabsView';
import { IMapProperty } from './models';
import PropertySelectorFormView from './PropertySelectorFormView';

export interface IMapSelectorContainerProps {
  onSelectedProperty: (property: IMapProperty) => void;
}

export const MapSelectorContainer: React.FunctionComponent<IMapSelectorContainerProps> = ({
  onSelectedProperty,
}) => {
  const { setCursor } = React.useContext(SelectedPropertyContext);
  React.useEffect(() => {
    return () => setCursor(undefined);
  }, [setCursor]);
  return (
    <>
      <PropertySelectorTabsView
        MapSelectorView={<PropertySelectorFormView onSelectedProperty={onSelectedProperty} />}
        ListSelectorView={<></>}
      ></PropertySelectorTabsView>
    </>
  );
};

export default MapSelectorContainer;
