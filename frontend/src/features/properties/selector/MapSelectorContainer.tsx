import { Button } from 'components/common/buttons';
import * as Styled from 'components/common/styles';
import { SelectedPropertyContext } from 'components/maps/providers/SelectedPropertyContext';
import { StyledFormSection } from 'features/mapSideBar/tabs/SectionStyles';
import * as React from 'react';
import { useState } from 'react';
import { toast } from 'react-toastify';

import SelectedPropertyHeaderRow from '../map/research/add/SelectedPropertyHeaderRow';
import SelectedPropertyRow from '../map/research/add/SelectedPropertyRow';
import PropertyMapSelectorFormView from './map/PropertyMapSelectorFormView';
import { IMapProperty } from './models';
import { PropertySelectorTabsView, SelectorTabNames } from './PropertySelectorTabsView';
import PropertySelectorSearchContainer from './search/PropertySelectorSearchContainer';
import { getPropertyIdentifier } from './utils';
export interface IMapSelectorContainerProps {
  onSelectedProperty: (property: IMapProperty) => void;
  onRemoveProperty: (index: number) => void;
  existingProperties: IMapProperty[];
}

export const MapSelectorContainer: React.FunctionComponent<IMapSelectorContainerProps> = ({
  onSelectedProperty,
  onRemoveProperty,
  existingProperties,
}) => {
  const { setCursor } = React.useContext(SelectedPropertyContext);
  const [selectedProperties, setSelectedProperties] = useState<IMapProperty[]>([]);
  const [activeSelectorTab, setActiveSelectorTab] = useState<SelectorTabNames>(
    SelectorTabNames.map,
  );
  const existingPropertiesWithIds = existingProperties.map(property => ({
    ...property,
    id: getPropertyIdentifier(property),
  }));
  React.useEffect(() => {
    return () => setCursor(undefined);
  }, [setCursor]);

  return (
    <>
      <PropertySelectorTabsView
        activeTab={activeSelectorTab}
        setActiveTab={setActiveSelectorTab}
        MapSelectorView={<PropertyMapSelectorFormView onSelectedProperty={onSelectedProperty} />}
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
          onClick={() =>
            addProperties(selectedProperties, existingPropertiesWithIds, onSelectedProperty)
          }
        >
          Add to selection
        </Button>
      ) : null}
      <StyledFormSection>
        <Styled.H3>Selected properties</Styled.H3>
        <SelectedPropertyHeaderRow />
        {existingProperties.map((property, index) => (
          <SelectedPropertyRow
            key={`property.${property.latitude}-${property.longitude}-${property.pid}`}
            onRemove={() => onRemoveProperty(index)}
            nameSpace={`properties.${index}`}
            index={index}
            property={property}
          />
        ))}

        {existingProperties.length === 0 && <span>No Properties selected</span>}
      </StyledFormSection>
    </>
  );
};

const addProperties = (
  newProperties: IMapProperty[],
  selectedProperties: IMapProperty[],
  addCallback: (property: IMapProperty) => void,
) => {
  newProperties.forEach((property: IMapProperty) => {
    if (!selectedProperties.some(selectedProperty => selectedProperty.id === property.id)) {
      addCallback(property);
    } else {
      toast.warn(
        'A property that the user is trying to select has already been added to the selected properties list',
        { toastId: 'duplicate-property' },
      );
    }
  });
};

export default MapSelectorContainer;
