import clsx from 'classnames';
import React, { useCallback, useState } from 'react';
import styled from 'styled-components';

import DraftSvg from '@/assets/images/pins/icon-draft.svg';
import MapView from '@/components/maps/MapView';
import { FilterProvider } from '@/components/maps/providers/FIlterProvider';
import { MapStateContext } from '@/components/maps/providers/MapStateContext';
import { PropertyContextProvider } from '@/components/maps/providers/PropertyContext';
import { MAP_MAX_ZOOM } from '@/constants/strings';
import { SideBarContextProvider } from '@/features/mapSideBar/context/sidebarContext';
import MapSideBar from '@/features/mapSideBar/MapSideBar';
import ActivityRouter from '@/features/mapSideBar/router/ActivityRouter';
import CompensationRequisitionRouter from '@/features/mapSideBar/router/CompensationRequisitionRouter';
import PopupRouter from '@/features/mapSideBar/router/PopupRouter';
import { IProperty } from '@/interfaces';
import { Api_Property } from '@/models/api/Property';

/** rough center of bc Itcha Ilgachuz Provincial Park */
const defaultLatLng = {
  lat: 52.81604319154934,
  lng: -124.67285156250001,
};

interface MapContainerProps {
  showParcelBoundaries?: boolean;
  onMarkerPopupClosed?: (obj: IProperty) => void;
}

const MapContainer: React.FC<React.PropsWithChildren<MapContainerProps>> = (
  props: MapContainerProps,
) => {
  const [mapInstance, setMapInstance] = useState<L.Map | undefined>();
  const [showSideBar, setShowSideBar] = useState(false);
  const [showActionBar, setShowActionBar] = useState(false);

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
              <CompensationRequisitionRouter setShowActionBar={setShowActionBar} />
            </SideBarContextProvider>
            {!showActionBar && (
              <FilterProvider>
                <MapView
                  lat={defaultLatLng.lat}
                  lng={defaultLatLng.lng}
                  showParcelBoundaries={props.showParcelBoundaries ?? true}
                  zoom={6}
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

export default MapContainer;
