import DraftSvg from 'assets/images/pins/icon-draft.svg';
import clsx from 'classnames';
import { FilterProvider } from 'components/maps/providers/FIlterProvider';
import { PropertyContextProvider } from 'components/maps/providers/PropertyContext';
import {
  SelectedPropertyContext,
  SelectedPropertyContextProvider,
} from 'components/maps/providers/SelectedPropertyContext';
import useMapSideBarQueryParams from 'features/mapSideBar/hooks/useMapSideBarQueryParams';
import { IProperty } from 'interfaces';
import React, { useState } from 'react';
import { useHistory } from 'react-router-dom';
import { toast } from 'react-toastify';
import styled from 'styled-components';
import { pidParser } from 'utils';

import Map from '../../../components/maps/leaflet/Map';
import MapActionWindow from './MapActionWindow';
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
  const [mapInstance, setMapInstance] = useState<L.Map | undefined>();
  const {
    sidebarComponent,
    showSideBar,
    showWindow,
    actionWindowComponent,
  } = useMapSideBarQueryParams(mapInstance);

  const onMarkerClicked = (property: IProperty) => {
    history.push(`/mapview/property/${property.id}?pid=${property.pid}`);
  };

  const onPropertyViewClicked = (pid?: string | null) => {
    if (pid !== undefined && pid !== null) {
      const parsedPid = pidParser(pid);
      history.push(`/mapview/non-inventory-property/${parsedPid}`);
    } else {
      console.warn('Invalid marker when trying to see property information');
      toast.warn('A map parcel must have a PID in order to view detailed information');
    }
  };

  return (
    <SelectedPropertyContextProvider>
      <SelectedPropertyContext.Consumer>
        {({ cursor }) => (
          <PropertyContextProvider>
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
                  whenCreated={setMapInstance}
                />
              </FilterProvider>
              {showWindow && (
                <MapActionWindow showWindow={showWindow}>{actionWindowComponent}</MapActionWindow>
              )}
            </StyleMapView>
          </PropertyContextProvider>
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
    cursor: url(${DraftSvg}) 15 45, pointer;
  }
`;

export default MapView;
