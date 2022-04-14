import AddResearchContainer from 'features/properties/map/research/add/AddResearchContainer';
import React, { useState } from 'react';
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
  PROPERTY_INFORMATION = 'property_information',
  PROPERTY_SEARCH = 'property_search',
}

/** control the state of the side bar via the route. */
export const useMapSideBarQueryParams = (): IMapSideBar => {
  const history = useHistory();
  const location = useLocation();

  const [sidebarComponent, setSidebarComponent] = useState<React.ReactNode>();
  const [showSideBar, setShowSideBar] = useState(false);

  React.useEffect(() => {
    const handleClose = () => {
      history.push('/mapview');
    };

    var parts = location.pathname.split('/');
    var currentState: MapViewState = MapViewState.MAP_ONLY;
    if (parts.length === 2) {
      currentState = MapViewState.MAP_ONLY;
    } else if (parts[2] === 'research') {
      if (parts[3] === 'new') {
        currentState = MapViewState.RESEARCH_ADD;
      } else {
        currentState = MapViewState.MAP_ONLY;
      }
    } else if (parts[2] === 'property') {
      if (parts[3] === 'search') {
        currentState = MapViewState.PROPERTY_SEARCH;
      } else {
        currentState = MapViewState.PROPERTY_INFORMATION;
      }
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
      case MapViewState.PROPERTY_SEARCH:
        setSidebarComponent(<MotiInventoryContainer onClose={handleClose} pid={parts[3]} />);
        setShowSideBar(false);
        break;
      case MapViewState.PROPERTY_INFORMATION:
        setSidebarComponent(<MotiInventoryContainer onClose={handleClose} pid={parts[3]} />);
        setShowSideBar(true);
        break;
      default:
        setSidebarComponent(<></>);
        setShowSideBar(false);
    }
  }, [history, location, setSidebarComponent, setShowSideBar]);

  return {
    sidebarComponent,
    showSideBar,
    setShowSideBar,
  };
};

export default useMapSideBarQueryParams;
