import './MapView.scss';

import { FilterProvider } from 'components/maps/providers/FIlterProvider';
import useMapSideBarQueryParams from 'features/mapSideBar/hooks/useMapSideBarQueryParams';
import MotiInventoryContainer from 'features/mapSideBar/MotiInventoryContainer';
import { IProperty } from 'interfaces';
import React, { useState } from 'react';
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
  const { showSideBar, setShowSideBar, pid } = useMapSideBarQueryParams();

  const onMarkerClicked = (property: IProperty) => {
    setShowSideBar(true, property);
  };
  return (
    <StyleMapView>
      <MotiInventoryContainer showSideBar={showSideBar} setShowSideBar={setShowSideBar} pid={pid} />
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
          showSideBar={showSideBar}
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
