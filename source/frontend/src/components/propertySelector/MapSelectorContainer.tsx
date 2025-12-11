import { FunctionComponent, useCallback, useState } from 'react';
import { toast } from 'react-toastify';

import { Button } from '@/components/common/buttons';
import { useMapProperties } from '@/hooks/repositories/useMapProperties';
import { firstOrNull, isValidId } from '@/utils';
import {
  areLocationFeatureDatasetsEqual,
  getPropertyNameFromLocationFeatureSet,
  pidFromFeatureSet,
  pinFromFeatureSet,
} from '@/utils/mapPropertyUtils';

import { LocationFeatureDataset } from '../common/mapFSM/useLocationFeatureLoader';
import PropertyMapSelectorFormView from './map/PropertyMapSelectorFormView';
import { PropertySelectorTabsView, SelectorTabNames } from './PropertySelectorTabsView';
import PropertySelectorSearchContainer from './search/PropertySelectorSearchContainer';

export interface IMapSelectorContainerProps {
  addSelectedProperties: (properties: LocationFeatureDataset[]) => void;
  modifiedProperties: LocationFeatureDataset[]; // TODO: Figure out if this component really needs the entire LocationFeatureDataset. It could be that only the lat long are needed.
}

export const MapSelectorContainer: FunctionComponent<IMapSelectorContainerProps> = ({
  addSelectedProperties,
  modifiedProperties,
}) => {
  const [searchSelectedProperties, setSearchSelectedProperties] = useState<
    LocationFeatureDataset[]
  >([]);
  const [activeSelectorTab, setActiveSelectorTab] = useState<SelectorTabNames>(
    SelectorTabNames.map,
  );

  const [lastSelectedProperty, setLastSelectedProperty] = useState<
    LocationFeatureDataset | undefined
  >(
    modifiedProperties?.length === 1 &&
      (modifiedProperties[0]?.pimsFeatures || modifiedProperties[0]?.parcelFeatures) // why? Because create from map needs to show the info differently
      ? modifiedProperties[0]
      : undefined,
  );
  const {
    loadProperties: { execute: loadProperties },
  } = useMapProperties();

  const addWithPimsFeature = useCallback(
    async (properties: LocationFeatureDataset[]) => {
      const updatedPropertiesPromises = properties.map(async property => {
        // TODO: Might need an update to work with multiple properties
        if (firstOrNull(property.pimsFeatures)?.properties?.PROPERTY_ID) {
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
          property.pimsFeatures = [pimsProperty.features[0]];
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
            onNewLocation={async (property: LocationFeatureDataset) => {
              setLastSelectedProperty(property);
              await addProperties([property], modifiedProperties, addWithPimsFeature);
            }}
            lastSelectedProperty={
              lastSelectedProperty
                ? modifiedProperties.find(
                    p =>
                      getPropertyNameFromLocationFeatureSet(p).value ===
                      getPropertyNameFromLocationFeatureSet(lastSelectedProperty).value,
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
            await addProperties(searchSelectedProperties, modifiedProperties, addWithPimsFeature);
            setSearchSelectedProperties([]);
          }}
          data-testid="add-selected-properties-button"
        >
          Add to selection
        </Button>
      ) : null}
    </>
  );
};

const addProperties = async (
  newProperties: LocationFeatureDataset[],
  selectedProperties: LocationFeatureDataset[],
  addCallback: (properties: LocationFeatureDataset[]) => Promise<void>,
) => {
  const propertiesToAdd: LocationFeatureDataset[] = [];
  newProperties.forEach((property: LocationFeatureDataset) => {
    if (
      !selectedProperties.some(selectedProperty =>
        areLocationFeatureDatasetsEqual(selectedProperty, property),
      )
    ) {
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

export default MapSelectorContainer;
