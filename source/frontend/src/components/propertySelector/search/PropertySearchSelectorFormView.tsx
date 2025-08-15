import { useMemo } from 'react';

import { SelectedFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { Section } from '@/components/common/Section/Section';
import { Table } from '@/components/Table';
import { IGeocoderResponse } from '@/hooks/pims-api/interfaces/IGeocoder';
import { featuresetToMapProperty, getPropertyName } from '@/utils/mapPropertyUtils';
import { isStrataCommonProperty } from '@/utils/propertyUtils';

import { ILayerSearchCriteria, IMapProperty } from '../models';
import LayerFilter from './LayerFilter';
import mapPropertyColumns from './mapPropertyColumns';

export interface IIdentifiedSelectedFeatureDataset extends SelectedFeatureDataset {
  id: string;
}

export interface IPropertySearchSelectorFormViewProps {
  onSelectedProperties: (properties: SelectedFeatureDataset[]) => void;
  selectedProperties: SelectedFeatureDataset[];
  onSearch: (search: ILayerSearchCriteria) => void;
  searchResults: SelectedFeatureDataset[];
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
  const selectedData = selectedProperties.map<IIdentifiedSelectedFeatureDataset>(x => {
    return { ...x, id: generatePropertyId(featuresetToMapProperty(x)) };
  });

  const identifiedSearchResults = useMemo(
    () =>
      searchResults
        .map<IIdentifiedSelectedFeatureDataset>(x => {
          return { ...x, id: generatePropertyId(featuresetToMapProperty(x)) };
        })
        .sort((a, b) => {
          const aIsStrataLot = isStrataCommonProperty(a?.parcelFeature);
          const bIsStrataLot = isStrataCommonProperty(b?.parcelFeature);
          if (aIsStrataLot === bIsStrataLot) return 0;
          if (aIsStrataLot) return -1;
          if (bIsStrataLot) return 1;
          return 0;
        }) ?? [],
    [searchResults],
  );

  function generatePropertyId(mapProperty: IMapProperty): string {
    const propertyName = getPropertyName(mapProperty);
    return `${propertyName.label}-${propertyName.value}-${mapProperty.latitude}-${mapProperty.longitude}`;
  }

  return (
    <Section header="Search for a property" data-testid="property-search-selector-section">
      <LayerFilter
        onSearch={onSearch}
        filter={search}
        addressResults={addressResults}
        onAddressChange={onAddressChange}
        onAddressSelect={onAddressSelect}
        loading={loading}
      />
      <Table<IIdentifiedSelectedFeatureDataset>
        manualPagination={false}
        name="map-properties"
        columns={mapPropertyColumns}
        data={identifiedSearchResults}
        setSelectedRows={onSelectedProperties}
        selectedRows={selectedData}
        loading={loading}
        lockPageSize={true}
        showSelectedRowCount={true}
        noRowsMessage={'No results found for your search criteria.'}
        pageSize={5}
      />
    </Section>
  );
};

export default PropertySearchSelectorFormView;
