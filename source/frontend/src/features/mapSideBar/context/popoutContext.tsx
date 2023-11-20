import * as React from 'react';
import { useState } from 'react';

export interface IPopoutContext {
  showActionBar: boolean;
  setShowActionBar: (showActionBar: boolean) => void;
  RouterComponent: React.FunctionComponent<IMapSidebarPopoutRouterProps> | null;
  setRouterComponent: (
    routerComponent: React.FunctionComponent<IMapSidebarPopoutRouterProps>,
  ) => void;
  popoutUpdated: boolean;
  setPopoutUpdated: (popoutUpdated: boolean) => void;
}

export interface IMapSidebarPopoutRouterProps {
  setShowActionBar: (showActionBar: boolean) => void;
  onUpdate: () => void;
}

export const PopoutContext = React.createContext<IPopoutContext>({
  showActionBar: false,
  setShowActionBar: (showActionBar: boolean) => {
    throw Error('setShowActionBar function not defined');
  },
  RouterComponent: null,
  setRouterComponent: (routerComponent: React.FunctionComponent<IMapSidebarPopoutRouterProps>) => {
    throw Error('setRouterComponent function not defined');
  },
  popoutUpdated: false,
  setPopoutUpdated: (popoutUpdated: boolean) => {
    throw Error('setPopoutUpdated function not defined');
  },
});

export const PopoutContextProvider = (props: {
  children: React.ReactChild | React.ReactChild[] | React.ReactNode;
}) => {
  const [showActionBar, setShowActionBar] = useState(false);
  const [popoutUpdated, setPopoutUpdated] = useState(false);
  const [RouterComponent, setRouterComponent] =
    useState<React.FunctionComponent<IMapSidebarPopoutRouterProps> | null>(null);

  return (
    <PopoutContext.Provider
      value={{
        showActionBar: showActionBar,
        setShowActionBar: setShowActionBar,
        RouterComponent: RouterComponent,
        setRouterComponent: setRouterComponent,
        popoutUpdated: popoutUpdated,
        setPopoutUpdated: setPopoutUpdated,
      }}
    >
      {props.children}
    </PopoutContext.Provider>
  );
};
