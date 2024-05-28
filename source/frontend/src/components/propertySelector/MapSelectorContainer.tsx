import { FunctionComponent, useState } from 'react';
import { toast } from 'react-toastify';

import { Button } from '@/components/common/buttons';
import { isValidId } from '@/utils';
import { featuresetToMapProperty, getPropertyName, NameSourceType } from '@/utils/mapPropertyUtils';

import { LocationFeatureDataset } from '../common/mapFSM/useLocationFeatureLoader';
import PropertyMapSelectorFormView from './map/PropertyMapSelectorFormView';
import { PropertySelectorTabsView, SelectorTabNames } from './PropertySelectorTabsView';
import PropertySelectorSearchContainer from './search/PropertySelectorSearchContainer';

export interface IMapSelectorContainerProps {
  addSelectedProperties: (properties: LocationFeatureDataset[]) => void; // TODO: This component should be providing the featureDataset instead of the IMapProperty.
  modifiedProperties: LocationFeatureDataset[]; // TODO: Figure out if this component really needs the entire propertyForm. It could be that only the lat long are needed.
  selectedComponentId?: string;
}

export const MapSelectorContainer: FunctionComponent<IMapSelectorContainerProps> = ({
  addSelectedProperties,
  modifiedProperties,
  selectedComponentId,
}) => {
  const [searchSelectedProperties, setSearchSelectedProperties] = useState<
    LocationFeatureDataset[]
  >([]);
  const [activeSelectorTab, setActiveSelectorTab] = useState<SelectorTabNames>(
    SelectorTabNames.map,
  );
  const modifiedMapProperties = modifiedProperties.map(mp => mp);
  const [lastSelectedProperty, setLastSelectedProperty] = useState<
    LocationFeatureDataset | undefined
  >(
    modifiedProperties?.length === 1 && isValidId(+modifiedProperties[0]?.pimsFeature?.id) // why? Because create from map needs to show the info differently
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
            onSelectedProperty={(property: LocationFeatureDataset) => {
              setLastSelectedProperty(property);
              addProperties([property], modifiedMapProperties, addSelectedProperties);
            }}
            selectedProperties={modifiedMapProperties}
            selectedComponentId={selectedComponentId}
            lastSelectedProperty={
              lastSelectedProperty
                ? modifiedMapProperties.find(
                    p =>
                      getPropertyName(featuresetToMapProperty(p)).value ===
                      getPropertyName(featuresetToMapProperty(lastSelectedProperty)).value,
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
  newProperties: LocationFeatureDataset[],
  selectedProperties: LocationFeatureDataset[],
  addCallback: (properties: LocationFeatureDataset[]) => void,
) => {
  const propertiesToAdd: LocationFeatureDataset[] = [];
  newProperties.forEach((property: LocationFeatureDataset) => {
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

const isSameProperty = (lhs: LocationFeatureDataset, rhs: LocationFeatureDataset) => {
  const lhsName = getPropertyName(featuresetToMapProperty(lhs));
  const rhsName = getPropertyName(featuresetToMapProperty(rhs));
  if (lhsName.label === rhsName.label && lhsName.label !== NameSourceType.NONE) {
    return lhsName.value === rhsName.value;
  }
  return false;
};

export default MapSelectorContainer;
