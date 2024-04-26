import { FunctionComponent, useState } from 'react';
import { toast } from 'react-toastify';

import { Button } from '@/components/common/buttons';
import { IMapProperty } from '@/components/propertySelector/models';
import { PropertyForm } from '@/features/mapSideBar/shared/models';
import { isValidId } from '@/utils';
import { getPropertyName, NameSourceType } from '@/utils/mapPropertyUtils';

import PropertyMapSelectorFormView from './map/PropertyMapSelectorFormView';
import { PropertySelectorTabsView, SelectorTabNames } from './PropertySelectorTabsView';
import PropertySelectorSearchContainer from './search/PropertySelectorSearchContainer';

export interface IMapSelectorContainerProps {
  addSelectedProperties: (properties: IMapProperty[]) => void; // TODO: This component should be providing the featureDataset instead of the IMapProperty.
  modifiedProperties: PropertyForm[]; // TODO: Figure out if this component really needs the entire propertyForm. It could be that only the lat long are needed.
  selectedComponentId?: string;
}

export const MapSelectorContainer: FunctionComponent<IMapSelectorContainerProps> = ({
  addSelectedProperties,
  modifiedProperties,
  selectedComponentId,
}) => {
  const [searchSelectedProperties, setSearchSelectedProperties] = useState<IMapProperty[]>([]);
  const [activeSelectorTab, setActiveSelectorTab] = useState<SelectorTabNames>(
    SelectorTabNames.map,
  );
  const modifiedMapProperties = modifiedProperties.map(mp => mp.toMapProperty());
  const [lastSelectedProperty, setLastSelectedProperty] = useState<IMapProperty | undefined>(
    modifiedProperties?.length === 1 && isValidId(modifiedProperties[0].apiId) // why? Because create from map needs to show the info differently
      ? modifiedMapProperties[0]
      : undefined,
  );

  return (
    <>
      <PropertySelectorTabsView
        activeTab={activeSelectorTab}
        setActiveTab={setActiveSelectorTab}
        MapSelectorView={
          <PropertyMapSelectorFormView
            onSelectedProperty={(property: IMapProperty) => {
              setLastSelectedProperty(property);
              addProperties([property], modifiedMapProperties, addSelectedProperties);
            }}
            selectedProperties={modifiedMapProperties}
            selectedComponentId={selectedComponentId}
            lastSelectedProperty={
              lastSelectedProperty
                ? modifiedMapProperties.find(
                    p => getPropertyName(p).value === getPropertyName(lastSelectedProperty).value,
                  )
                : undefined // use the property from the modified properties list from the parent, for consistency.
            }
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
          onClick={() => {
            addProperties(searchSelectedProperties, modifiedMapProperties, addSelectedProperties);
            setSearchSelectedProperties([]);
          }}
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
