import { AxiosError } from 'axios';
import * as React from 'react';
import { useCallback } from 'react';
import { toast } from 'react-toastify';

import { usePimsPropertyRepository } from '@/hooks/repositories/usePimsPropertyRepository';
import { IApiError } from '@/interfaces/IApiError';
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
        try {
          const result = await getPropertyByPidWrapper.execute(layerSearch?.pid);
          if (result) {
            setSelectProperty(result);
          }
        } catch (e) {
          const axiosError = e as AxiosError<IApiError>;
          if (axiosError?.response?.status === 404) {
            toast.warn('The PID that you are searching for does not exist in the PIMS database.');
          }
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
