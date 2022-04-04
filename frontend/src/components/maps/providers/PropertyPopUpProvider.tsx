import { IProperty } from 'interfaces';
import { noop } from 'lodash';
import React from 'react';

export enum MapCursors {
  DRAFT = 'draft-cursor',
}
export interface IPopUpContext {
  propertyInfo: IProperty | null;
  setPropertyInfo: (propertyInfo: IProperty | null) => void;
  loading: boolean;
  setLoading: (loading: boolean) => void;
  cursor?: MapCursors;
  setCursor: (cursor?: MapCursors) => void;
  isSelecting: boolean;
}

export const PropertyPopUpContext = React.createContext<IPopUpContext>({
  propertyInfo: null,
  setPropertyInfo: noop,
  loading: false,
  setLoading: noop,
  setCursor: noop,
  isSelecting: false,
});

interface IPopUpContextComponent {
  values?: Partial<IPopUpContext>;
}

/**
 * Allows for the property information to be sent to the map
 * when the user clicks on a marker
 */
export const PropertyPopUpContextProvider: React.FC<IPopUpContextComponent> = ({
  children,
  values,
}) => {
  const [propertyInfo, setPropertyInfo] = React.useState<IProperty | null>(
    values?.propertyInfo ?? null,
  );
  const [loading, setLoading] = React.useState<boolean>(values?.loading ?? false);
  const [cursor, setCursor] = React.useState<MapCursors | undefined>(undefined);
  return (
    <PropertyPopUpContext.Provider
      value={{
        propertyInfo,
        setPropertyInfo,
        loading,
        setLoading,
        cursor,
        setCursor,
        isSelecting: cursor === MapCursors.DRAFT,
      }}
    >
      {children}
    </PropertyPopUpContext.Provider>
  );
};
