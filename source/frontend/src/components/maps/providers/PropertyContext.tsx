import { noop } from 'lodash';
import React from 'react';

import { PointFeature } from '../types';

export interface IPropertyContext {
  properties: PointFeature[];
  setProperties: (properties: PointFeature[]) => void;
  propertiesLoading: boolean;
  setPropertiesLoading: (loading: boolean) => void;
}

export const PropertyContext = React.createContext<IPropertyContext>({
  properties: [],
  setProperties: noop,
  propertiesLoading: false,
  setPropertiesLoading: noop,
});

interface IPropertyContextComponent {
  values?: Partial<IPropertyContext>;
}

export const PropertyContextProvider: React.FC<
  React.PropsWithChildren<IPropertyContextComponent>
> = ({ children, values }) => {
  const [properties, setProperties] = React.useState<PointFeature[]>(values?.properties ?? []);
  const [propertiesLoading, setPropertiesLoading] = React.useState<boolean>(
    values?.propertiesLoading ?? false,
  );
  return (
    <PropertyContext.Provider
      value={{
        properties,
        setProperties: values?.setProperties ?? setProperties,
        propertiesLoading,
        setPropertiesLoading,
      }}
    >
      {children}
    </PropertyContext.Provider>
  );
};
