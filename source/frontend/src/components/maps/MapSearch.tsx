import { Map as LeafletMap } from 'leaflet';
import { isEqualWith } from 'lodash';
import isEqual from 'lodash/isEqual';
import React, { useContext, useRef, useState } from 'react';
import { Container } from 'react-bootstrap';
import styled from 'styled-components';

import { IGeoSearchParams } from '@/constants/API';
import { PropertyFilter } from '@/features/properties/filter';
import { IPropertyFilter } from '@/features/properties/filter/IPropertyFilter';

import { useFilterContext } from './providers/FIlterProvider';
//import * as Styled from './leaflet/styles';
import { MapStateActionTypes, MapStateContext } from './providers/MapStateContext';

export type MapSearchProps = {};

const defaultFilterValues: IPropertyFilter = {
  searchBy: 'pinOrPid',
  pinOrPid: '',
  address: '',
  latitude: '',
  longitude: '',
};

// TODO: Check that this makes sense as the filter types should match IPropertyFilter
const whitelistedFilterKeys = [
  'PID',
  'PIN',
  'STREET_ADDRESS_1',
  'LOCATION',
  'latitude',
  'longitude',
];

/**
 * Converts the map filter to a geo search filter.
 * @param filter The map filter.
 */
const getQueryParams = (filter: IPropertyFilter): IGeoSearchParams => {
  // The map will search for either identifier.
  const pinOrPidValue = filter.pinOrPid ? filter.pinOrPid?.replace(/-/g, '') : undefined;
  return {
    PID: pinOrPidValue,
    PIN: pinOrPidValue,
    STREET_ADDRESS_1: filter.address,
    latitude: filter.latitude,
    longitude: filter.longitude,
    forceExactMatch: true,
  };
};

/**
 * Creates a Leaflet map and by default includes a number of preconfigured layers.
 * @param param0
 */
const MapSearch: React.FC<React.PropsWithChildren<MapSearchProps>> = () => {
  const { setState, selectedInventoryProperty } = useContext(MapStateContext);

  const mapRef = useRef<LeafletMap | null>(null);

  if (mapRef.current && !selectedInventoryProperty) {
    const center = mapRef.current.getCenter();
    /*lat = center.lat;
    lng = center.lng;*/
  }

  //const something = useMap();

  /*useEffect(() => {
    dispatch(setMapRef(mapRef));
  }, [mapRef]);*/

  const [geoFilter, setGeoFilter] = useState<IGeoSearchParams>({
    ...defaultFilterValues,
  });

  const [triggerFilterChanged, setTriggerFilterChanged] = useState(true);

  const { setChanged } = useFilterContext();

  const handleMapFilterChange = async (filter: IPropertyFilter) => {
    const compareValues = (objValue: IPropertyFilter, othValue: IPropertyFilter) => {
      type filterKey = keyof IPropertyFilter;
      return whitelistedFilterKeys.reduce((acc, key) => {
        return (
          (isEqual(objValue[key as filterKey], othValue[key as filterKey]) ||
            (!objValue[key as filterKey] && !othValue[key as filterKey])) &&
          acc
        );
      }, true);
    };
    // Search button will always trigger filter changed (triggerFilterChanged is set to true when search button is clicked)
    if (!isEqualWith(geoFilter, getQueryParams(filter), compareValues) || triggerFilterChanged) {
      setState({
        type: MapStateActionTypes.SELECTED_INVENTORY_PROPERTY,
        selectedInventoryProperty: null,
      });
      setGeoFilter(getQueryParams(filter));
      setChanged(true);
      setTriggerFilterChanged(false);
    }
  };

  return (
    <StyledFilterContainer fluid className="px-0">
      <PropertyFilter
        useGeocoder={true}
        defaultFilter={{
          ...defaultFilterValues,
        }}
        onChange={handleMapFilterChange}
        setTriggerFilterChanged={setTriggerFilterChanged}
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
