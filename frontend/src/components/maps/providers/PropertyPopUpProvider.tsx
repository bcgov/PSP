import { IProperty } from 'interfaces';
import { IPropertyApiModel } from 'interfaces/IPropertyApiModel';
import { noop } from 'lodash';
import React from 'react';

export interface IPopUpContext {
  propertyInfo: IPropertyApiModel | null;
  setPropertyInfo: (propertyInfo: IPropertyApiModel | null) => void;
  loading: boolean;
  setLoading: (loading: boolean) => void;
}

export const PropertyPopUpContext = React.createContext<IPopUpContext>({
  propertyInfo: null,
  setPropertyInfo: noop,
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
  const [propertyInfo, setPropertyInfo] = React.useState<IProperty | null>(
    values?.propertyInfo ?? null,
  );
  const [loading, setLoading] = React.useState<boolean>(values?.loading ?? false);
  return (
    <PropertyPopUpContext.Provider
      value={{
        propertyInfo,
        setPropertyInfo,
        loading,
        setLoading,
      }}
    >
      {children}
    </PropertyPopUpContext.Provider>
  );
};
