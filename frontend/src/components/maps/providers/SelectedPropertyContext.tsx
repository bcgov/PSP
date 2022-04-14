import { IProperty } from 'interfaces';
import { noop } from 'lodash';
import React from 'react';

export enum MapCursors {
  DRAFT = 'draft-cursor',
}
export interface ISelectedPropertyContext {
  propertyInfo: IProperty | null;
  setPropertyInfo: (propertyInfo: IProperty | null) => void;
  loading: boolean;
  setLoading: (loading: boolean) => void;
  cursor?: MapCursors;
  setCursor: (cursor?: MapCursors) => void;
  isSelecting: boolean;
}

export const SelectedPropertyContext = React.createContext<ISelectedPropertyContext>({
  propertyInfo: null,
  setPropertyInfo: noop,
  loading: false,
  setLoading: noop,
  setCursor: noop,
  isSelecting: false,
});

interface ISelectedPropertyContextComponent {
  values?: Partial<ISelectedPropertyContext>;
}

/**
 * Allows for the property information to be sent to the map
 * when the user clicks on a marker
 */
export const SelectedPropertyContextProvider: React.FC<ISelectedPropertyContextComponent> = ({
  children,
  values,
}) => {
  const [propertyInfo, setPropertyInfo] = React.useState<IProperty | null>(
    values?.propertyInfo ?? null,
  );
  const [loading, setLoading] = React.useState<boolean>(values?.loading ?? false);
  const [cursor, setCursor] = React.useState<MapCursors | undefined>(undefined);

  return (
    <SelectedPropertyContext.Provider
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
    </SelectedPropertyContext.Provider>
  );
};
