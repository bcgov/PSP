import { LatLngLiteral } from 'leaflet';
import { FunctionComponent, useCallback, useState } from 'react';
import { toast } from 'react-toastify';

import { Button } from '@/components/common/buttons';
import { useMapProperties } from '@/hooks/repositories/useMapProperties';
import { isValidId } from '@/utils';
import {
  featuresetToMapProperty,
  getPropertyName,
  NameSourceType,
  pidFromFeatureSet,
  pinFromFeatureSet,
} from '@/utils/mapPropertyUtils';

import { SelectedFeatureDataset } from '../common/mapFSM/useLocationFeatureLoader';
import PropertyMapSelectorFormView from './map/PropertyMapSelectorFormView';
import { PropertySelectorTabsView, SelectorTabNames } from './PropertySelectorTabsView';
import PropertySelectorSearchContainer from './search/PropertySelectorSearchContainer';

export interface IMapSelectorContainerProps {
  addSelectedProperties: (properties: SelectedFeatureDataset[]) => void;
  repositionSelectedProperty: (
    property: SelectedFeatureDataset,
    latLng: LatLngLiteral,
    propertyIndex: number | null,
  ) => void;
  modifiedProperties: SelectedFeatureDataset[]; // TODO: Figure out if this component really needs the entire LocationFeatureDataset. It could be that only the lat long are needed.
  selectedComponentId?: string;
}

export const MapSelectorContainer: FunctionComponent<IMapSelectorContainerProps> = ({
  addSelectedProperties,
  repositionSelectedProperty,
  modifiedProperties,
  selectedComponentId,
}) => {
  const [searchSelectedProperties, setSearchSelectedProperties] = useState<
    SelectedFeatureDataset[]
  >([]);
  const [activeSelectorTab, setActiveSelectorTab] = useState<SelectorTabNames>(
    SelectorTabNames.map,
  );
  const modifiedMapProperties = modifiedProperties.map(mp => mp);
  const [lastSelectedProperty, setLastSelectedProperty] = useState<
    SelectedFeatureDataset | undefined
  >(
    modifiedProperties?.length === 1 &&
      (modifiedProperties[0]?.pimsFeature || modifiedProperties[0]?.parcelFeature) // why? Because create from map needs to show the info differently
      ? modifiedMapProperties[0]
      : undefined,
  );
  const {
    loadProperties: { execute: loadProperties },
  } = useMapProperties();

  const addWithPimsFeature = useCallback(
    async (properties: SelectedFeatureDataset[]) => {
      const updatedPropertiesPromises = properties.map(async property => {
        if (property.pimsFeature?.properties?.PROPERTY_ID) {
          return property;
        }
        const pid = pidFromFeatureSet(property);
        const pin = pinFromFeatureSet(property);
        if (!pid && !pin) {
          return property;
        }
        const queryObject = {};
        if (isValidId(+pid)) {
          queryObject['PID'] = pid;
        }
        if (isValidId(+pin)) {
          queryObject['PIN'] = pin;
        }
        const pimsProperty = await loadProperties(queryObject);
        if (pimsProperty.features.length > 0) {
          // TODO: Might need updates to work with multiple properties
          property.pimsFeature = pimsProperty.features[0];
        }
        return property;
      });
      const updatedProperties = await Promise.all(updatedPropertiesPromises);
      addSelectedProperties(updatedProperties);
    },
    [addSelectedProperties, loadProperties],
  );
  return (
    <>
      <PropertySelectorTabsView
        activeTab={activeSelectorTab}
        setActiveTab={setActiveSelectorTab}
        MapSelectorView={
          <PropertyMapSelectorFormView
            onSelectedProperty={async (property: SelectedFeatureDataset) => {
              setLastSelectedProperty(property);
              await addProperties([property], modifiedMapProperties, addWithPimsFeature);
            }}
            onRepositionedProperty={(
              property: SelectedFeatureDataset,
              latLng: LatLngLiteral,
              propertyIndex: number | null,
            ) => {
              setLastSelectedProperty(property);
              repositionSelectedProperty(property, latLng, propertyIndex);
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
      />
      {activeSelectorTab === SelectorTabNames.list ? (
        <Button
          variant="secondary"
          onClick={async () => {
            await addProperties(
              searchSelectedProperties,
              modifiedMapProperties,
              addWithPimsFeature,
            );
            setSearchSelectedProperties([]);
          }}
        >
          Add to selection
        </Button>
      ) : null}
    </>
  );
};

const addProperties = async (
  newProperties: SelectedFeatureDataset[],
  selectedProperties: SelectedFeatureDataset[],
  addCallback: (properties: SelectedFeatureDataset[]) => Promise<void>,
) => {
  const propertiesToAdd: SelectedFeatureDataset[] = [];
  newProperties.forEach((property: SelectedFeatureDataset) => {
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
    await addCallback(propertiesToAdd);
  }
};

const isSameProperty = (lhs: SelectedFeatureDataset, rhs: SelectedFeatureDataset) => {
  const lhsName = getPropertyName(featuresetToMapProperty(lhs));
  const rhsName = getPropertyName(featuresetToMapProperty(rhs));
  if (
    (lhsName.label === rhsName.label &&
      lhsName.label !== NameSourceType.NONE &&
      lhsName.label !== NameSourceType.PLAN) ||
    (lhsName.label === NameSourceType.PLAN &&
      lhs.location.lat === rhs.location.lat &&
      lhs.location.lng === rhs.location.lng)
  ) {
    return lhsName.value === rhsName.value;
  }
  return false;
};

export default MapSelectorContainer;
