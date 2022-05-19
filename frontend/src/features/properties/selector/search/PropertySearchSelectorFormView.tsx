import { Button } from 'components/common/buttons';
import * as Styled from 'components/common/styles';
import { Table } from 'components/Table';
import { StyledFormSection } from 'features/mapSideBar/tabs/SectionStyles';
import * as React from 'react';

import { ILayerSearchCriteria, IMapProperty } from '../models';
import { LayerFilter } from './LayerFilter';
import mapPropertyColumns from './mapPropertyColumns';

export interface IPropertySearchSelectorFormViewProps {
  onSelectedProperties: (properties: IMapProperty[]) => void;
  onAddProperties: (properties: IMapProperty[]) => void;
  selectedProperties: IMapProperty[];
  onSearch: (search: ILayerSearchCriteria) => void;
  searchResults: IMapProperty[];
  search?: ILayerSearchCriteria;
  loading: boolean;
}

export const PropertySearchSelectorFormView: React.FunctionComponent<IPropertySearchSelectorFormViewProps> = ({
  onSelectedProperties,
  onAddProperties,
  selectedProperties,
  onSearch,
  searchResults,
  search,
  loading,
}) => {
  return (
    <>
      <StyledFormSection>
        <Styled.H3>Search for a property</Styled.H3>
        <LayerFilter setFilter={onSearch} filter={search} />
      </StyledFormSection>
      <StyledFormSection>
        <Table<IMapProperty>
          manualPagination={false}
          name="map-properties"
          columns={mapPropertyColumns}
          data={searchResults?.length <= 15 ? searchResults : []}
          setSelectedRows={onSelectedProperties}
          selectedRows={selectedProperties}
          loading={loading}
          lockPageSize={true}
          showSelectedRowCount={searchResults?.length <= 15}
          noRowsMessage={
            searchResults?.length <= 15
              ? 'No results found for your search criteria.'
              : 'Too many results (more than 15) match this criteria. Please refine your search.'
          }
          pageSize={5}
        />
        <Button variant="secondary" onClick={() => onAddProperties(selectedProperties)}>
          Add to selection
        </Button>
      </StyledFormSection>
    </>
  );
};

export default PropertySearchSelectorFormView;
