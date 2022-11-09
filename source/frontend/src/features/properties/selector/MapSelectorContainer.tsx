import { Button } from 'components/common/buttons';
import * as Styled from 'components/common/styles';
import {
  MapCursors,
  MapStateActionTypes,
  MapStateContext,
} from 'components/maps/providers/MapStateContext';
import { Section } from 'features/mapSideBar/tabs/Section';
import * as React from 'react';
import { useState } from 'react';
import { toast } from 'react-toastify';

import { PropertyForm } from '../map/shared/models';
import SelectedPropertyHeaderRow from './components/SelectedPropertyHeaderRow';
import SelectedPropertyRow from './components/SelectedPropertyRow';
import PropertyMapSelectorFormView from './map/PropertyMapSelectorFormView';
import { IMapProperty } from './models';
import { PropertySelectorTabsView, SelectorTabNames } from './PropertySelectorTabsView';
import PropertySelectorSearchContainer from './search/PropertySelectorSearchContainer';
import { getPropertyName, NameSourceType } from './utils';
export interface IMapSelectorContainerProps {
  onSelectedProperty: (property: IMapProperty) => void;
  onRemoveProperty: (index: number) => void;
  existingProperties: PropertyForm[];
}

export const MapSelectorContainer: React.FunctionComponent<IMapSelectorContainerProps> = ({
  onSelectedProperty,
  onRemoveProperty,
  existingProperties,
}) => {
  const { setState } = React.useContext(MapStateContext);
  const [selectedProperties, setSelectedProperties] = useState<IMapProperty[]>([]);
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
              existingProperties?.length === 1 && existingProperties[0].apiId === undefined
                ? existingProperties[0]
                : undefined
            }
            onSelectedProperty={(property: IMapProperty) =>
              addProperties([property], existingProperties, onSelectedProperty)
            }
          />
        }
        ListSelectorView={
          <PropertySelectorSearchContainer
            selectedProperties={selectedProperties}
            setSelectedProperties={setSelectedProperties}
          />
        }
      ></PropertySelectorTabsView>
      {activeSelectorTab === SelectorTabNames.list ? (
        <Button
          variant="secondary"
          onClick={() => addProperties(selectedProperties, existingProperties, onSelectedProperty)}
        >
          Add to selection
        </Button>
      ) : null}
      <Section header={undefined}>
        <Styled.H3>Selected properties</Styled.H3>
        <SelectedPropertyHeaderRow />
        {existingProperties.map((property, index) => (
          <SelectedPropertyRow
            key={`property.${property.latitude}-${property.longitude}-${property.pid}-${property.apiId}`}
            onRemove={() => onRemoveProperty(index)}
            nameSpace={`properties.${index}`}
            index={index}
            property={property}
          />
        ))}
        {existingProperties.length === 0 && <span>No Properties selected</span>}
      </Section>
    </>
  );
};

const addProperties = (
  newProperties: IMapProperty[],
  selectedProperties: IMapProperty[],
  addCallback: (property: IMapProperty) => void,
) => {
  newProperties.forEach((property: IMapProperty) => {
    if (!selectedProperties.some(selectedProperty => isSameProperty(selectedProperty, property))) {
      addCallback(property);
    } else {
      toast.warn(
        'A property that the user is trying to select has already been added to the selected properties list',
        { toastId: 'duplicate-property' },
      );
    }
  });
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
