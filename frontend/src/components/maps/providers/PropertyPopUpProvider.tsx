import { IBuilding, IParcel } from 'interfaces';
import { noop } from 'lodash';
import React from 'react';

export interface IPopUpContext {
  propertyInfo: IParcel | IBuilding | null;
  setPropertyInfo: (propertyInfo: IParcel | IBuilding | null) => void;
  propertyTypeId: number | null;
  setPropertyTypeId: (propertyTypeId: number) => void;
  loading: boolean;
  setLoading: (loading: boolean) => void;
}

export const PropertyPopUpContext = React.createContext<IPopUpContext>({
  propertyInfo: null,
  setPropertyInfo: noop,
  propertyTypeId: null,
  setPropertyTypeId: noop,
  loading: false,
  setLoading: noop,
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
  const [propertyInfo, setPropertyInfo] = React.useState<IParcel | IBuilding | null>(
    values?.propertyInfo ?? null,
  );
  const [propertyTypeId, setPropertyTypeId] = React.useState<number | null>(
    values?.propertyTypeId ?? null,
  );
  const [loading, setLoading] = React.useState<boolean>(values?.loading ?? false);
  return (
    <PropertyPopUpContext.Provider
      value={{
        propertyInfo,
        setPropertyInfo,
        propertyTypeId,
        setPropertyTypeId,
        loading,
        setLoading,
      }}
    >
      {children}
    </PropertyPopUpContext.Provider>
  );
};
