import { MAP_MAX_ZOOM } from 'constants/strings';
import AddAcquisitionContainer from 'features/properties/map/acquisition/add/AddAcquisitionContainer';
import AddResearchContainer from 'features/properties/map/research/add/AddResearchContainer';
import ResearchContainer from 'features/properties/map/research/ResearchContainer';
import { IPropertyApiModel } from 'interfaces/IPropertyApiModel';
import { isNumber } from 'lodash';
import queryString from 'query-string';
import React, { useCallback, useState } from 'react';
import { useHistory, useLocation } from 'react-router-dom';

import MotiInventoryContainer from '../MotiInventoryContainer';

interface IMapSideBar {
  sidebarComponent: React.ReactNode;
  showSideBar: boolean;
  setShowSideBar: (show: boolean) => void;
}

export enum MapSidebarContextType {
  INFO = 'info',
  SELECTOR = 'selector',
}

export enum MapViewState {
  MAP_ONLY = 'map_only',
  RESEARCH_ADD = 'research_add',
  RESEARCH_VIEW = 'research_view',
  PROPERTY_INFORMATION = 'property_information',
  ACQUISITION_ADD = 'acquisition_add',
}

/** control the state of the side bar via the route. */
export const useMapSideBarQueryParams = (map?: L.Map): IMapSideBar => {
  const history = useHistory();
  const location = useLocation();

  const [sidebarComponent, setSidebarComponent] = useState<React.ReactNode>();
  const [showSideBar, setShowSideBar] = useState(false);

  const onZoom = useCallback(
    (apiProperty?: IPropertyApiModel) =>
      apiProperty?.longitude &&
      apiProperty?.latitude &&
      map?.flyTo({ lat: apiProperty?.latitude, lng: apiProperty?.longitude }, MAP_MAX_ZOOM, {
        animate: false,
      }),
    [map],
  );

  React.useEffect(() => {
    const handleClose = () => {
      history.push('/mapview');
    };

    var parts = location.pathname.split('/');
    var currentState: MapViewState = MapViewState.MAP_ONLY;
    var researchId: number = 0;
    var propertyId: number | undefined = 0;
    var pid = '';
    if (parts.length === 2) {
      currentState = MapViewState.MAP_ONLY;
    } else if (parts.length > 2) {
      if (parts[2] === 'research') {
        if (parts.length === 4 && parts[3] === 'new') {
          currentState = MapViewState.RESEARCH_ADD;
        } else if (parts.length >= 4 && isNumber(Number(parts[3]))) {
          researchId = Number(parts[3]);
          currentState = MapViewState.RESEARCH_VIEW;
        } else {
          currentState = MapViewState.MAP_ONLY;
        }
      } else if (parts.length > 3 && parts[2] === 'property') {
        pid = queryString.parse(location.search).pid?.toString() ?? '';
        propertyId = !!parts[3] ? +propertyId : undefined;
        currentState = MapViewState.PROPERTY_INFORMATION;
      } else if (parts.length > 3 && parts[2] === 'non-inventory-property') {
        pid = parts[3];
        currentState = MapViewState.PROPERTY_INFORMATION;
      } else if (parts[2] === 'acquisition') {
        if (parts.length === 4 && parts[3] === 'new') {
          currentState = MapViewState.ACQUISITION_ADD;
        }
      } else {
        currentState = MapViewState.MAP_ONLY;
      }
    } else {
      currentState = MapViewState.MAP_ONLY;
    }

    switch (currentState as MapViewState) {
      case MapViewState.MAP_ONLY:
        setSidebarComponent(<></>);
        setShowSideBar(false);
        break;
      case MapViewState.RESEARCH_ADD:
        setSidebarComponent(<AddResearchContainer onClose={handleClose} />);
        setShowSideBar(true);
        break;
      case MapViewState.RESEARCH_VIEW:
        setSidebarComponent(
          <ResearchContainer researchFileId={researchId} onClose={handleClose} />,
        );
        setShowSideBar(true);
        break;
      case MapViewState.PROPERTY_INFORMATION:
        setSidebarComponent(
          <MotiInventoryContainer
            onClose={handleClose}
            id={propertyId}
            pid={pid}
            onZoom={onZoom}
          />,
        );
        setShowSideBar(true);
        break;
      case MapViewState.ACQUISITION_ADD:
        setSidebarComponent(<AddAcquisitionContainer />);
        setShowSideBar(true);
        break;
      default:
        setSidebarComponent(<></>);
        setShowSideBar(false);
    }
  }, [history, location, setSidebarComponent, setShowSideBar, onZoom]);

  return {
    sidebarComponent,
    showSideBar,
    setShowSideBar,
  };
};

export default useMapSideBarQueryParams;
