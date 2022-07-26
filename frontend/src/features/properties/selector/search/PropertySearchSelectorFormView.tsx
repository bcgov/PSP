import * as Styled from 'components/common/styles';
import { Table } from 'components/Table';
import { Section } from 'features/mapSideBar/tabs/Section';
import { IGeocoderResponse } from 'hooks/useApi';
import * as React from 'react';

import { ILayerSearchCriteria, IMapProperty } from '../models';
import { LayerFilter } from './LayerFilter';
import mapPropertyColumns from './mapPropertyColumns';

export interface IPropertySearchSelectorFormViewProps {
  onSelectedProperties: (properties: IMapProperty[]) => void;
  selectedProperties: IMapProperty[];
  onSearch: (search: ILayerSearchCriteria) => void;
  searchResults: IMapProperty[];
  search?: ILayerSearchCriteria;
  loading: boolean;
  addressResults?: IGeocoderResponse[];
  onAddressChange: (searchText: string) => void;
  onAddressSelect: (selectedItem: IGeocoderResponse) => void;
}

export const PropertySearchSelectorFormView: React.FunctionComponent<IPropertySearchSelectorFormViewProps> = ({
  onSelectedProperties,
  selectedProperties,
  onSearch,
  searchResults,
  search,
  loading,
  addressResults,
  onAddressChange,
  onAddressSelect,
}) => {
  return (
    <>
      <Section header={undefined}>
        <Styled.H3>Search for a property</Styled.H3>
        <LayerFilter
          setFilter={onSearch}
          filter={search}
          addressResults={addressResults}
          onAddressChange={onAddressChange}
          onAddressSelect={onAddressSelect}
        />
      </Section>
      <Section header={undefined}>
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
      </Section>
    </>
  );
};

export default PropertySearchSelectorFormView;
