import { dequal } from 'dequal';
import { LatLngLiteral } from 'leaflet';
import React, { useEffect, useState } from 'react';
import { toast } from 'react-toastify';
import styled from 'styled-components';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { PropertyFilter } from '@/features/properties/filter';
import {
  defaultPropertyFilter,
  IPropertyFilter,
} from '@/features/properties/filter/IPropertyFilter';
import { exists } from '@/utils';

export type MapSearchProps = object;

/**
 * Creates a component that handles searches that affect the map.
 * @param param0
 */
const MapSearch: React.FC<React.PropsWithChildren<MapSearchProps>> = () => {
  const {
    mapSearchCriteria,
    setMapSearchCriteria,
    mapClick,
    requestCenterToLocation,
    mapMarkLocation,
    mapClearLocationMark,
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
        mapMarkLocation(latLng);
        requestCenterToLocation(latLng);
        mapClick(latLng);
      }
    } else {
      mapClearLocationMark();
      setPropertySearchFilter(filter);
    }
  };

  return (
    <StyledFilterContainer className="px-0">
      <PropertyFilter
        propertyFilter={propertySearchFilter}
        useGeocoder={true}
        defaultFilter={{
          ...defaultPropertyFilter,
        }}
        onChange={handleMapFilterChange}
      />
    </StyledFilterContainer>
  );
};

export default MapSearch;

const StyledFilterContainer = styled.div`
  transition: margin 1s;

  grid-area: filter;
  background-color: #f2f2f2;
  box-shadow: 0px 4px 5px rgba(0, 0, 0, 0.2);
  z-index: 500;
  .map-filter-bar {
    align-items: center;
    justify-content: center;
    padding: 0.5rem 0;
    margin: 0;
    .vl {
      border-left: 6px solid rgba(96, 96, 96, 0.2);
      height: 4rem;
      margin-left: 1%;
      margin-right: 1%;
      border-width: 0.2rem;
    }
    .btn-primary {
      font-weight: bold;
      height: 3.5rem;
      width: 3.5rem;
      min-height: unset;
      padding: 0;
    }
    .form-control {
      font-size: 1.4rem;
    }
  }
  .form-group {
    margin-bottom: 0;
  }
`;
