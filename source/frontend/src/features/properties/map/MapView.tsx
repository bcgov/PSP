import DraftSvg from 'assets/images/pins/icon-draft.svg';
import clsx from 'classnames';
import { FilterProvider } from 'components/maps/providers/FIlterProvider';
import { MapStateContext } from 'components/maps/providers/MapStateContext';
import { PropertyContextProvider } from 'components/maps/providers/PropertyContext';
import { MAP_MAX_ZOOM } from 'constants/strings';
import { IProperty } from 'interfaces';
import { Api_Property } from 'models/api/Property';
import React, { useCallback, useState } from 'react';
import { useHistory } from 'react-router-dom';
import { toast } from 'react-toastify';
import styled from 'styled-components';
import { pidParser } from 'utils';

import Map from '../../../components/maps/leaflet/Map';
import ActivityRouter from './ActivityRouter';
import { SideBarContextProvider } from './context/sidebarContext';
import MapSideBar from './MapSideBar';
import PopupRouter from './PopupRouter';

/** rough center of bc Itcha Ilgachuz Provincial Park */
const defaultLatLng = {
  lat: 52.81604319154934,
  lng: -124.67285156250001,
};

interface MapViewProps {
  showParcelBoundaries?: boolean;
  onMarkerPopupClosed?: (obj: IProperty) => void;
}

const MapView: React.FC<React.PropsWithChildren<MapViewProps>> = (props: MapViewProps) => {
  const history = useHistory();
  const [loadedProperties, setLoadedProperties] = useState(false);
  const [mapInstance, setMapInstance] = useState<L.Map | undefined>();
  const [showSideBar, setShowSideBar] = useState(false);
  const [showActionBar, setShowActionBar] = useState(false);

  const onPropertyViewClicked = (pid?: string | null, id?: number) => {
    if (id !== undefined) {
      history.push(`/mapview/sidebar/property/${id}?pid=${pid}`);
    } else if (pid !== undefined && pid !== null) {
      const parsedPid = pidParser(pid);
      history.push(`/mapview/sidebar/non-inventory-property/${parsedPid}`);
    } else {
      console.warn('Invalid marker when trying to see property information');
      toast.warn('A map parcel must have a PID in order to view detailed information');
    }
  };

  const onZoom = useCallback(
    (apiProperty?: Api_Property) =>
      apiProperty?.longitude &&
      apiProperty?.latitude &&
      mapInstance?.flyTo(
        { lat: apiProperty?.latitude, lng: apiProperty?.longitude },
        MAP_MAX_ZOOM,
        {
          animate: false,
        },
      ),
    [mapInstance],
  );

  return (
    <MapStateContext.Consumer>
      {({ cursor }) => (
        <PropertyContextProvider>
          <StyleMapView className={clsx(cursor)}>
            <SideBarContextProvider>
              <MapSideBar
                showSideBar={showSideBar}
                setShowSideBar={setShowSideBar}
                onZoom={onZoom}
              />
              <ActivityRouter setShowActionBar={setShowActionBar} />
              <PopupRouter setShowActionBar={setShowActionBar} />
            </SideBarContextProvider>
            {!showActionBar && (
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
                  onViewPropertyClick={onPropertyViewClicked}
                  showSideBar={showSideBar}
                  whenCreated={setMapInstance}
                />
              </FilterProvider>
            )}
          </StyleMapView>
        </PropertyContextProvider>
      )}
    </MapStateContext.Consumer>
  );
};

const StyleMapView = styled.div`
  display: flex;
  flex-direction: row;
  width: 100%;
  &.draft-cursor,
  &.draft-cursor .leaflet-grab,
  &.draft-cursor .leaflet-interactive {
    cursor: url(${DraftSvg}) 15 45, pointer;
  }
`;

export default MapView;
