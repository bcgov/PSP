import * as Styled from 'components/common/styles';
import { SelectedPropertyContext } from 'components/maps/providers/SelectedPropertyContext';
import { StyledFormSection } from 'features/mapSideBar/tabs/SectionStyles';
import * as React from 'react';
import { toast } from 'react-toastify';

import SelectedPropertyHeaderRow from '../map/research/add/SelectedPropertyHeaderRow';
import SelectedPropertyRow from '../map/research/add/SelectedPropertyRow';
import PropertyMapSelectorFormView from './map/PropertyMapSelectorFormView';
import { IMapProperty } from './models';
import { PropertySelectorTabsView } from './PropertySelectorTabsView';
import PropertySelectorSearchContainer from './search/PropertySelectorSearchContainer';
import { getPropertyIdentifier } from './utils';
export interface IMapSelectorContainerProps {
  onSelectedProperty: (property: IMapProperty) => void;
  onRemoveProperty: (index: number) => void;
  selectedProperties: IMapProperty[];
}

export const MapSelectorContainer: React.FunctionComponent<IMapSelectorContainerProps> = ({
  onSelectedProperty,
  onRemoveProperty,
  selectedProperties,
}) => {
  const { setCursor } = React.useContext(SelectedPropertyContext);
  const selectedPropertiesWithIds = selectedProperties.map(property => ({
    ...property,
    id: getPropertyIdentifier(property),
  }));
  React.useEffect(() => {
    return () => setCursor(undefined);
  }, [setCursor]);

  return (
    <>
      <PropertySelectorTabsView
        MapSelectorView={<PropertyMapSelectorFormView onSelectedProperty={onSelectedProperty} />}
        ListSelectorView={
          <PropertySelectorSearchContainer
            onSelectedProperty={(property: IMapProperty) => {
              console.log(selectedPropertiesWithIds, property);
              if (
                !selectedPropertiesWithIds.some(
                  selectedProperty => selectedProperty.id === property.id,
                )
              ) {
                onSelectedProperty(property);
              } else {
                toast.warn(
                  'A property that the user is trying to select has already been added to the selected properties list',
                  { toastId: 'duplicate-property' },
                );
              }
            }}
            selectedProperties={selectedPropertiesWithIds}
          />
        }
      ></PropertySelectorTabsView>
      <StyledFormSection>
        <Styled.H3>Selected properties</Styled.H3>
        <SelectedPropertyHeaderRow />
        {selectedProperties.map((property, index) => (
          <SelectedPropertyRow
            key={`property.${property.latitude}-${property.longitude}-${property.pid}`}
            onRemove={() => onRemoveProperty(index)}
            nameSpace={`properties.${index}`}
            index={index}
            property={property}
          />
        ))}

        {selectedProperties.length === 0 && <span>No Properties selected</span>}
      </StyledFormSection>
    </>
  );
};

export default MapSelectorContainer;
