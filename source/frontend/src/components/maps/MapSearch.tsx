import { dequal } from 'dequal';
import React, { useEffect, useState } from 'react';
import { Container } from 'react-bootstrap';
import styled from 'styled-components';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { PropertyFilter } from '@/features/properties/filter';
import {
  defaultPropertyFilter,
  IPropertyFilter,
} from '@/features/properties/filter/IPropertyFilter';

export type MapSearchProps = {};

/**
 * Creates a Leaflet map and by default includes a number of preconfigured layers.
 * @param param0
 */
const MapSearch: React.FC<React.PropsWithChildren<MapSearchProps>> = () => {
  const { mapFilter: mapMachineMapFilter, setMapFilter: mapMachineSetMapFilter } =
    useMapStateMachine();

  const [mapFilter, setMapFilter] = useState<IPropertyFilter | null>(null);

  useEffect(() => {
    if (mapFilter !== null && !dequal(mapFilter, mapMachineMapFilter)) {
      mapMachineSetMapFilter(mapFilter);
    }
  }, [mapFilter, mapMachineSetMapFilter, mapMachineMapFilter]);

  const handleMapFilterChange = (filter: IPropertyFilter) => {
    console.log('handleMapFilterChange', filter);
    setMapFilter(filter);
  };

  const searchButtonClicked = () => {};

  return (
    <StyledFilterContainer fluid className="px-0">
      <PropertyFilter
        useGeocoder={true}
        defaultFilter={{
          ...defaultPropertyFilter,
        }}
        onChange={handleMapFilterChange}
        searchButtonClicked={searchButtonClicked}
      />
    </StyledFilterContainer>
  );
};

export default MapSearch;

const StyledFilterContainer = styled(Container)`
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
      color: white;
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
