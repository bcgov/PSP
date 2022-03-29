import './MapView.scss';

import clsx from 'classnames';
import { FilterProvider } from 'components/maps/providers/FIlterProvider';
import {
  PropertyPopUpContext,
  PropertyPopUpContextProvider,
} from 'components/maps/providers/PropertyPopUpProvider';
import useMapSideBarQueryParams from 'features/mapSideBar/hooks/useMapSideBarQueryParams';
import MapSideBarContainer from 'features/mapSideBar/MapSideBarContainer';
import { IProperty } from 'interfaces';
import { LeafletMouseEvent } from 'leaflet';
import React, { useState } from 'react';
import { useDispatch } from 'react-redux';
import { saveClickLatLng as saveLeafletMouseEvent } from 'store/slices/leafletMouse/LeafletMouseSlice';
import styled from 'styled-components';

import Map from '../../../components/maps/leaflet/Map';

/** rough center of bc Itcha Ilgachuz Provincial Park */
const defaultLatLng = {
  lat: 52.81604319154934,
  lng: -124.67285156250001,
};

interface MapViewProps {
  showParcelBoundaries?: boolean;
  onMarkerPopupClosed?: (obj: IProperty) => void;
}

const MapView: React.FC<MapViewProps> = (props: MapViewProps) => {
  const [loadedProperties, setLoadedProperties] = useState(false);
  const dispatch = useDispatch();
  const { showSideBar } = useMapSideBarQueryParams();

  const saveLatLng = (e: LeafletMouseEvent) => {
    dispatch(
      saveLeafletMouseEvent({
        latlng: { lat: e.latlng.lat, lng: e.latlng.lng },
        originalEvent: { timeStamp: e.originalEvent.timeStamp },
      }),
    );
  };
  return (
    <PropertyPopUpContextProvider>
      <PropertyPopUpContext.Consumer>
        {({ cursor }) => (
          <StyleMapView
            className={clsx(
              {
                sidebar: showSideBar,
                mapview: true,
              },
              cursor,
            )}
          >
            <MapSideBarContainer />
            <FilterProvider>
              <Map
                lat={defaultLatLng.lat}
                lng={defaultLatLng.lng}
                onViewportChanged={() => {
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
        )}
      </PropertyPopUpContext.Consumer>
    </PropertyPopUpContextProvider>
  );
};

const StyleMapView = styled.div`
  display: flex;
  width: 100vw;
`;

export default MapView;
