import { Feature, Geometry } from 'geojson';
import { useMemo } from 'react';

import { LocationFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { Section } from '@/components/common/Section/Section';
import { Table } from '@/components/Table';
import { IGeocoderResponse } from '@/hooks/pims-api/interfaces/IGeocoder';
import { PMBC_FullyAttributed_Feature_Properties } from '@/models/layers/parcelMapBC';
import { firstOrNull } from '@/utils';
import { getPropertyNameFromLocationFeatureSet } from '@/utils/mapPropertyUtils';
import { isStrataCommonProperty } from '@/utils/propertyUtils';

import { ILayerSearchCriteria } from '../models';
import LayerFilter from './LayerFilter';
import mapPropertyColumns from './mapPropertyColumns';

export interface IIdentifiedLocationFeatureDataset extends LocationFeatureDataset {
  id: string;
  firstParcelFeature: Feature<Geometry, PMBC_FullyAttributed_Feature_Properties> | null;
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
  const selectedData = useMemo(
    () =>
      selectedProperties.map<IIdentifiedLocationFeatureDataset>(x => ({
        ...x,
        id: generatePropertyId(x),
        firstParcelFeature: firstOrNull(x.parcelFeatures),
      })),
    [selectedProperties],
  );

  const searchData = useMemo(
    () =>
      searchResults.map<IIdentifiedLocationFeatureDataset>(x => ({
        ...x,
        id: generatePropertyId(x),
        firstParcelFeature: firstOrNull(x.parcelFeatures),
      })),
    [searchResults],
  );

  const identifiedSearchResults = useMemo(() => {
    return (
      searchData.sort((a, b) => {
        const aIsStrataLot = isStrataCommonProperty(a?.firstParcelFeature);
        const bIsStrataLot = isStrataCommonProperty(b?.firstParcelFeature);
        if (aIsStrataLot === bIsStrataLot) return 0;
        if (aIsStrataLot) return -1;
        if (bIsStrataLot) return 1;
        return 0;
      }) ?? []
    );
  }, [searchData]);

  function generatePropertyId(mapProperty: LocationFeatureDataset): string {
    const propertyName = getPropertyNameFromLocationFeatureSet(mapProperty);
    return `${propertyName.label}-${propertyName.value}-${mapProperty.location?.lat ?? 0}-${
      mapProperty.location?.lng ?? 0
    }`;
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
      <Table<IIdentifiedLocationFeatureDataset>
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
