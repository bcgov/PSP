import * as Styled from 'components/common/styles';
import { Table } from 'components/Table';
import { Section } from 'features/mapSideBar/tabs/Section';
import { IGeocoderResponse } from 'hooks/useApi';
import * as React from 'react';

import { ILayerSearchCriteria, IMapProperty } from '../models';
import { getPropertyName } from '../utils';
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

interface SelectableProperty extends IMapProperty {
  id: string;
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
  const tableData =
    searchResults?.length <= 15
      ? searchResults.map<SelectableProperty>(x => {
          return { ...x, id: generatePropertyId(x) };
        })
      : [];

  const selectedData = selectedProperties.map<SelectableProperty>(x => {
    return { ...x, id: generatePropertyId(x) };
  });

  function generatePropertyId(mapProperty: IMapProperty): string {
    const propertyName = getPropertyName(mapProperty);
    return `${propertyName.label}-${propertyName.value}`;
  }

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
        <Table<SelectableProperty>
          manualPagination={false}
          name="map-properties"
          columns={mapPropertyColumns}
          data={tableData}
          setSelectedRows={onSelectedProperties}
          selectedRows={selectedData}
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
