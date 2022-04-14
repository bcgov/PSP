import clsx from 'classnames';
import { FilterProvider } from 'components/maps/providers/FIlterProvider';
import {
  SelectedPropertyContext,
  SelectedPropertyContextProvider,
} from 'components/maps/providers/SelectedPropertyContext';
import useMapSideBarQueryParams from 'features/mapSideBar/hooks/useMapSideBarQueryParams';
import { IProperty } from 'interfaces';
import React, { useState } from 'react';
import { useHistory } from 'react-router-dom';
import styled from 'styled-components';
import { pidParser } from 'utils';

import Map from '../../../components/maps/leaflet/Map';
import MapSideBar from './MapSideBar';

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
  const history = useHistory();
  const [loadedProperties, setLoadedProperties] = useState(false);

  const { sidebarComponent, showSideBar } = useMapSideBarQueryParams();

  const onMarkerClicked = (property: IProperty) => {
    const parsedPid = pidParser(property.pid);
    history.push(`/mapview/property/${parsedPid}`);
  };

  const onPropertyViewClicked = (pid?: string | null) => {
    if (pid !== undefined && pid !== null) {
      const parsedPid = pidParser(pid);
      history.push(`/mapview/property/${parsedPid}`);
    } else {
      console.warn('Invalid pin when trying to see property information');
    }
  };

  return (
    <SelectedPropertyContextProvider>
      <SelectedPropertyContext.Consumer>
        {({ cursor }) => (
          <StyleMapView className={clsx(cursor)}>
            <MapSideBar showSideBar={showSideBar}>{sidebarComponent}</MapSideBar>
            <FilterProvider>
              <Map
                lat={defaultLatLng.lat}
                lng={defaultLatLng.lng}
                onViewportChanged={() => {
                  if (!loadedProperties) {
                    setLoadedProperties(true);
                  }
                }}
                showParcelBoundaries={props.showParcelBoundaries ?? true}
                zoom={6}
                onPropertyMarkerClick={onMarkerClicked}
                onViewPropertyClick={onPropertyViewClicked}
                showSideBar={showSideBar}
              />
            </FilterProvider>
          </StyleMapView>
        )}
      </SelectedPropertyContext.Consumer>
    </SelectedPropertyContextProvider>
  );
};

const StyleMapView = styled.div`
  display: flex;
  width: 100vw;

  &.draft-cursor,
  &.draft-cursor .leaflet-grab,
  &.draft-cursor .leaflet-interactive {
    cursor: url('data:image/svg+xml,%3Csvg version="1.2" xmlns="http://www.w3.org/2000/svg" xmlnsXlink="http://www.w3.org/1999/xlink" overflow="visible" preserveAspectRatio="none" viewBox="0 0 24 24" width="97" height="97"%3E%3Cg%3E%3Cpath xmlnsDefault="http://www.w3.org/2000/svg" id="map-marker" d="M13.88,11.22c-1.04,1.04-2.72,1.04-3.76,0.01c0,0-0.01-0.01-0.01-0.01C9.06,10.18,9.06,8.5,10.1,7.46 c0,0,0.01-0.01,0.01-0.01c1.04-1.04,2.72-1.04,3.76-0.01c0,0,0.01,0.01,0.01,0.01C14.86,8.48,14.8,10.14,13.88,11.22z M17.33,9.34 c0.02-1.42-0.54-2.78-1.56-3.77C14.78,4.55,13.42,3.98,12,4c-1.42-0.02-2.78,0.55-3.77,1.57C7.21,6.56,6.65,7.92,6.67,9.33 C6.64,9.97,6.75,10.61,7,11.2l3.8,8.06c0.1,0.23,0.27,0.41,0.48,0.54c0.43,0.26,0.98,0.26,1.41,0c0.21-0.13,0.38-0.32,0.49-0.54 L17,11.2c0.25-0.59,0.36-1.22,0.34-1.86l0,0H17.33z" style="fill: rgb(66, 139, 202);" vectorEffect="non-scaling-stroke"/%3E%3C/g%3E%3C/svg%3E')
        48.5 81,
      pointer;
  }
`;

export default MapView;
