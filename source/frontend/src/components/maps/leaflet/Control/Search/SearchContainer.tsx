import { dequal } from 'dequal';
import { LatLngLiteral } from 'leaflet';
import React, { useEffect, useState } from 'react';
import { toast } from 'react-toastify';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { IPropertyFilter } from '@/features/properties/filter/IPropertyFilter';
import { exists } from '@/utils';

import { ISearchViewProps } from './SearchView';

export interface ISearchContainerProps {
  View: React.FunctionComponent<ISearchViewProps>;
}

export const SearchContainer: React.FC<ISearchContainerProps> = ({ View }) => {
  const {
    mapSearchCriteria,
    setMapSearchCriteria,
    mapClick,
    requestCenterToLocation,
    mapFeatureData,
  } = useMapStateMachine();

  const [propertySearchFilter, setPropertySearchFilter] = useState<IPropertyFilter | null>(null);

  useEffect(() => {
    if (propertySearchFilter !== null && !dequal(mapSearchCriteria, propertySearchFilter)) {
      setMapSearchCriteria(propertySearchFilter);
    }
  }, [propertySearchFilter, mapSearchCriteria, setMapSearchCriteria]);

  const handleMapFilterChange = (filter: IPropertyFilter) => {
    if (['coordinates', 'name', 'address'].includes(filter.searchBy)) {
      let latLng: LatLngLiteral = undefined;
      switch (filter.searchBy) {
        case 'name':
        case 'address':
          if (exists(filter.latitude)) {
            latLng = { lat: +filter.latitude, lng: +filter.longitude };
          } else {
            toast.warn('No valid location found for address - unable to zoom to location.');
          }
          break;
        case 'coordinates':
          latLng = filter.coordinates?.toLatLng();
      }
      if (latLng) {
        mapClick(latLng);
        requestCenterToLocation(latLng);
      }
    } else {
      setPropertySearchFilter(filter);
    }
  };

  return (
    <View
      propertyFilter={propertySearchFilter}
      onFilterChange={handleMapFilterChange}
      searchResult={mapFeatureData}
    />
  );
};
