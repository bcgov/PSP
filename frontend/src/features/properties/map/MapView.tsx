import './MapView.scss';

import classNames from 'classnames';
import { FilterProvider } from 'components/maps/providers/FIlterProvider';
import * as API from 'constants/API';
import { MotiInventoryContainer } from 'features/mapSideBar';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import { LeafletMouseEvent } from 'leaflet';
import queryString from 'query-string';
import React, { useState } from 'react';
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

interface MapViewProps {
  disableMapFilterBar?: boolean;
  showParcelBoundaries?: boolean;
  onMarkerPopupClosed?: (obj: IPropertyDetail) => void;
}

const MapView: React.FC<MapViewProps> = (props: MapViewProps) => {
  const lookupCodes = useLookupCodeHelpers();
  const properties = useAppSelector(state => [...state.properties.properties]);
  const [loadedProperties, setLoadedProperties] = useState(false);
  const propertyDetail = useAppSelector(state => state.properties.propertyDetail);
  const organizations = lookupCodes.getByType(API.ORGANIZATION_CODE_SET_NAME);
  const administrativeAreas = lookupCodes.getByType(API.ADMINISTRATIVE_AREA_CODE_SET_NAME);

  const dispatch = useDispatch();

  const saveLatLng = (e: LeafletMouseEvent) => {
    dispatch(
      saveLeafletMouseEvent({
        latlng: { lat: e.latlng.lat, lng: e.latlng.lng },
        originalEvent: { timeStamp: e.originalEvent.timeStamp },
      }),
    );
  };

  const { showSideBar, size } = useParamSideBar();

  const location = useLocation();
  const urlParsed = queryString.parse(location.search);
  const disableFilter = urlParsed.sidebar === 'true' ? true : false;
  return (
    <div className={classNames(showSideBar ? 'side-bar' : '', 'd-flex')}>
      <MotiInventoryContainer />
      <FilterProvider>
        <Map
          sidebarSize={size}
          lat={defaultLatLng.lat}
          lng={defaultLatLng.lng}
          properties={properties}
          selectedProperty={propertyDetail}
          organizations={organizations}
          administrativeAreas={administrativeAreas}
          onViewportChanged={(mapFilterModel: MapViewportChangeEvent) => {
            if (!loadedProperties) {
              setLoadedProperties(true);
            }
          }}
          onMapClick={saveLatLng}
          disableMapFilterBar={disableFilter}
          showParcelBoundaries={props.showParcelBoundaries ?? true}
          zoom={6}
        />
      </FilterProvider>
    </div>
  );
};

export default MapView;
