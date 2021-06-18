import './MapView.scss';

import classNames from 'classnames';
import { FilterProvider } from 'components/maps/providers/FIlterProvider';
import * as API from 'constants/API';
import MapSideBarContainer from 'features/mapSideBar/containers/MapSideBarContainer';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import { LeafletMouseEvent } from 'leaflet';
import queryString from 'query-string';
import React, { useRef, useState } from 'react';
import { Map as LeafletMap } from 'react-leaflet';
import { useDispatch } from 'react-redux';
import { useLocation } from 'react-router-dom';
import { useAppSelector } from 'store/hooks';
import { saveClickLatLng as saveLeafletMouseEvent } from 'store/slices/leafletMouse/LeafletMouseSlice';
import { IPropertyDetail } from 'store/slices/properties';

import Map, { MapViewportChangeEvent } from '../../../components/maps/leaflet/Map';
import useParamSideBar from '../../mapSideBar/hooks/useQueryParamSideBar';

/** rough center of bc Itcha Ilgachuz Provincial Park */
const defaultLatLng = {
  lat: 52.81604319154934,
  lng: -124.67285156250001,
};

// This could also come from the API, a local file, etc -OR- replacing the <select> fields with free text inputs.
// Hard-coding it here until further requirements say otherwise.
const fetchLotSizes = () => {
  return [1, 2, 5, 10, 50, 100, 200, 300, 400, 500, 1000, 10000];
};

interface MapViewProps {
  disableMapFilterBar?: boolean;
  disabled?: boolean;
  showParcelBoundaries?: boolean;
  onMarkerPopupClosed?: (obj: IPropertyDetail) => void;
}

const MapView: React.FC<MapViewProps> = (props: MapViewProps) => {
  const lookupCodes = useLookupCodeHelpers();
  const properties = useAppSelector(state => [...state.properties.parcels]);
  const [loadedProperties, setLoadedProperties] = useState(false);
  const mapRef = useRef<LeafletMap>(null);
  const propertyDetail = useAppSelector(state => state.properties.propertyDetail);
  const agencies = lookupCodes.getByType(API.AGENCY_CODE_SET_NAME);
  const administrativeAreas = lookupCodes.getByType(API.AMINISTRATIVE_AREA_CODE_SET_NAME);

  const lotSizes = fetchLotSizes();
  const dispatch = useDispatch();

  const saveLatLng = (e: LeafletMouseEvent) => {
    if (!props.disabled) {
      dispatch(
        saveLeafletMouseEvent({
          latlng: { lat: e.latlng.lat, lng: e.latlng.lng },
          originalEvent: { timeStamp: e.originalEvent.timeStamp },
        }),
      );
    }
  };

  const { showSideBar, size } = useParamSideBar();

  const location = useLocation();
  const urlParsed = queryString.parse(location.search);
  const disableFilter = urlParsed.sidebar === 'true' ? true : false;
  return (
    <div className={classNames(showSideBar ? 'side-bar' : '', 'd-flex')}>
      <MapSideBarContainer />
      <FilterProvider>
        <Map
          sidebarSize={size}
          lat={defaultLatLng.lat}
          lng={defaultLatLng.lng}
          properties={properties}
          selectedProperty={propertyDetail}
          agencies={agencies}
          administrativeAreas={administrativeAreas}
          lotSizes={lotSizes}
          onViewportChanged={(mapFilterModel: MapViewportChangeEvent) => {
            if (!loadedProperties) {
              setLoadedProperties(true);
            }
          }}
          onMapClick={saveLatLng}
          disableMapFilterBar={disableFilter}
          interactive={!props.disabled}
          showParcelBoundaries={props.showParcelBoundaries ?? true}
          zoom={6}
          mapRef={mapRef}
        />
      </FilterProvider>
    </div>
  );
};

export default MapView;
