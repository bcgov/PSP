import { LocationFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { Section } from '@/components/common/Section/Section';
import * as Styled from '@/components/common/styles';
import { Table } from '@/components/Table';
import { IGeocoderResponse } from '@/hooks/pims-api/interfaces/IGeocoder';
import { featuresetToMapProperty, getPropertyName } from '@/utils/mapPropertyUtils';

import { ILayerSearchCriteria, IMapProperty } from '../models';
import LayerFilter from './LayerFilter';
import mapPropertyColumns from './mapPropertyColumns';

export interface IIdentifiedLocationFeatureDataset extends LocationFeatureDataset {
  id: string;
}

export interface IPropertySearchSelectorFormViewProps {
  onSelectedProperties: (properties: LocationFeatureDataset[]) => void;
  selectedProperties: LocationFeatureDataset[];
  onSearch: (search: ILayerSearchCriteria) => void;
  searchResults: LocationFeatureDataset[];
  search?: ILayerSearchCriteria;
  loading: boolean;
  addressResults?: IGeocoderResponse[];
  onAddressChange: (searchText: string) => void;
  onAddressSelect: (selectedItem: IGeocoderResponse) => void;
}

export const PropertySearchSelectorFormView: React.FunctionComponent<
  React.PropsWithChildren<IPropertySearchSelectorFormViewProps>
> = ({
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
  const selectedData = selectedProperties.map<IIdentifiedLocationFeatureDataset>(x => {
    return { ...x, id: generatePropertyId(featuresetToMapProperty(x)) };
  });

  const identifiedSearchResults =
    searchResults?.length <= 15
      ? searchResults.map<IIdentifiedLocationFeatureDataset>(x => {
          return { ...x, id: generatePropertyId(featuresetToMapProperty(x)) };
        })
      : [];

  function generatePropertyId(mapProperty: IMapProperty): string {
    const propertyName = getPropertyName(mapProperty);
    return `${propertyName.label}-${propertyName.value}-${mapProperty.latitude}-${mapProperty.longitude}`;
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
          loading={loading}
        />
      </Section>
      <Section header={undefined}>
        <Table<IIdentifiedLocationFeatureDataset>
          manualPagination={false}
          name="map-properties"
          columns={mapPropertyColumns}
          data={identifiedSearchResults}
          setSelectedRows={searchResults?.length <= 15 ? onSelectedProperties : undefined}
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
