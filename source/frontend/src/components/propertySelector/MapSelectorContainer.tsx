import { Button } from 'components/common/buttons';
import {
  MapCursors,
  MapStateActionTypes,
  MapStateContext,
} from 'components/maps/providers/MapStateContext';
import { IMapProperty } from 'components/propertySelector/models';
import { PropertyForm } from 'features/properties/map/shared/models';
import * as React from 'react';
import { useState } from 'react';
import { toast } from 'react-toastify';
import { getPropertyName, NameSourceType } from 'utils/mapPropertyUtils';

import PropertyMapSelectorFormView from './map/PropertyMapSelectorFormView';
import { PropertySelectorTabsView, SelectorTabNames } from './PropertySelectorTabsView';
import PropertySelectorSearchContainer from './search/PropertySelectorSearchContainer';

export interface IMapSelectorContainerProps {
  addSelectedProperties: (properties: IMapProperty[]) => void;
  modifiedProperties: PropertyForm[];
}

export const MapSelectorContainer: React.FunctionComponent<
  React.PropsWithChildren<IMapSelectorContainerProps>
> = ({ addSelectedProperties, modifiedProperties }) => {
  const { setState } = React.useContext(MapStateContext);
  const [searchSelectedProperties, setSearchSelectedProperties] = useState<IMapProperty[]>([]);
  const [activeSelectorTab, setActiveSelectorTab] = useState<SelectorTabNames>(
    SelectorTabNames.map,
  );
  React.useEffect(() => {
    return () => setState({ type: MapStateActionTypes.CURSOR, cursor: MapCursors.DEFAULT });
  }, [setState]);

  return (
    <>
      <PropertySelectorTabsView
        activeTab={activeSelectorTab}
        setActiveTab={setActiveSelectorTab}
        MapSelectorView={
          <PropertyMapSelectorFormView
            initialSelectedProperty={
              modifiedProperties?.length === 1 && modifiedProperties[0].apiId === undefined // why? Because create from map needs to show the info differently
                ? modifiedProperties[0]
                : undefined
            }
            onSelectedProperty={(property: IMapProperty) =>
              addProperties([property], modifiedProperties, addSelectedProperties)
            }
            selectedProperties={modifiedProperties}
          />
        }
        ListSelectorView={
          <PropertySelectorSearchContainer
            selectedProperties={searchSelectedProperties}
            setSelectedProperties={setSearchSelectedProperties}
          />
        }
      ></PropertySelectorTabsView>
      {activeSelectorTab === SelectorTabNames.list ? (
        <Button
          variant="secondary"
          onClick={() =>
            addProperties(searchSelectedProperties, modifiedProperties, addSelectedProperties)
          }
        >
          Add to selection
        </Button>
      ) : null}
    </>
  );
};

const addProperties = (
  newProperties: IMapProperty[],
  selectedProperties: IMapProperty[],
  addCallback: (properties: IMapProperty[]) => void,
) => {
  const propertiesToAdd: IMapProperty[] = [];
  newProperties.forEach((property: IMapProperty) => {
    if (!selectedProperties.some(selectedProperty => isSameProperty(selectedProperty, property))) {
      propertiesToAdd.push(property);
    } else {
      toast.warn(
        'A property that the user is trying to select has already been added to the selected properties list',
        { toastId: 'duplicate-property' },
      );
    }
  });

  if (propertiesToAdd.length > 0) {
    addCallback(propertiesToAdd);
  }
};

const isSameProperty = (lhs: IMapProperty, rhs: IMapProperty) => {
  const lhsName = getPropertyName(lhs);
  const rhsName = getPropertyName(rhs);
  if (lhsName.label === rhsName.label && lhsName.label !== NameSourceType.NONE) {
    return lhsName.value === rhsName.value;
  }
  return false;
};

export default MapSelectorContainer;
