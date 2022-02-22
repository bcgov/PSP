import './MapView.scss';

import { FilterProvider } from 'components/maps/providers/FIlterProvider';
import MapSideBarContainer from 'features/mapSideBar/MapSideBarContainer';
import { LeafletMouseEvent } from 'leaflet';
import queryString from 'query-string';
import React, { useState } from 'react';
import { useDispatch } from 'react-redux';
import { useLocation } from 'react-router-dom';
import { useAppSelector } from 'store/hooks';
import { saveClickLatLng as saveLeafletMouseEvent } from 'store/slices/leafletMouse/LeafletMouseSlice';
import { IPropertyDetail } from 'store/slices/properties';
import styled from 'styled-components';

import Map, { MapViewportChangeEvent } from '../../../components/maps/leaflet/Map';

/** rough center of bc Itcha Ilgachuz Provincial Park */
const defaultLatLng = {
  lat: 52.81604319154934,
  lng: -124.67285156250001,
};

interface MapViewProps {
  showParcelBoundaries?: boolean;
  onMarkerPopupClosed?: (obj: IPropertyDetail) => void;
}

const MapView: React.FC<MapViewProps> = (props: MapViewProps) => {
  const [loadedProperties, setLoadedProperties] = useState(false);
  const propertyDetail = useAppSelector(state => state.properties.propertyDetail);

  const dispatch = useDispatch();

  const saveLatLng = (e: LeafletMouseEvent) => {
    dispatch(
      saveLeafletMouseEvent({
        latlng: { lat: e.latlng.lat, lng: e.latlng.lng },
        originalEvent: { timeStamp: e.originalEvent.timeStamp },
      }),
    );
  };

  const location = useLocation();
  const urlParsed = queryString.parse(location.search);
  const showSideBar = urlParsed.sidebar === 'true';
  return (
    <StyleMapView className={showSideBar ? 'side-bar' : ''}>
      <MapSideBarContainer />
      <FilterProvider>
        <Map
          lat={defaultLatLng.lat}
          lng={defaultLatLng.lng}
          selectedProperty={propertyDetail}
          onViewportChanged={(mapFilterModel: MapViewportChangeEvent) => {
            if (!loadedProperties) {
              setLoadedProperties(true);
            }
          }}
          onMapClick={saveLatLng}
          showSideBar={showSideBar}
          showParcelBoundaries={props.showParcelBoundaries ?? true}
          zoom={6}
        />
      </FilterProvider>
    </StyleMapView>
  );
};

const StyleMapView = styled.div`
  display: flex;
  width: 100vw;
`;

export default MapView;
