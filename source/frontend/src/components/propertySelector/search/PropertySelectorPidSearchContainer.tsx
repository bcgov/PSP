import * as React from 'react';
import { useCallback } from 'react';

import { usePimsPropertyRepository } from '@/hooks/repositories/usePimsPropertyRepository';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';

import { ILayerSearchCriteria } from '../models';
import { IPropertySearchSelectorPidFormViewProps } from './PropertySelectorPidSearchView';

export interface PropertySelectorPidSearchContainerProps {
  setSelectProperty: (property: ApiGen_Concepts_Property) => void;
  PropertySelectorPidSearchView: React.FunctionComponent<
    React.PropsWithChildren<IPropertySearchSelectorPidFormViewProps>
  >;
}

export const PropertySelectorPidSearchContainer: React.FunctionComponent<
  React.PropsWithChildren<PropertySelectorPidSearchContainerProps>
> = ({ setSelectProperty, PropertySelectorPidSearchView }) => {
  const { getPropertyByPidWrapper } = usePimsPropertyRepository();

  const searchFunc = useCallback(
    async (layerSearch: ILayerSearchCriteria) => {
      if (layerSearch?.pid) {
        const result = await getPropertyByPidWrapper.execute(layerSearch?.pid);
        if (result) {
          setSelectProperty(result);
        }
      }
    },
    [getPropertyByPidWrapper, setSelectProperty],
  );

  return (
    <PropertySelectorPidSearchView
      onSearch={searchFunc}
      loading={getPropertyByPidWrapper.loading}
    />
  );
};

export default PropertySelectorPidSearchContainer;
