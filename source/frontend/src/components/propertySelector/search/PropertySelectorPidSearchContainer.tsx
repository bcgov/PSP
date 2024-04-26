import { AxiosError } from 'axios';
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
            if (!result.isOwned || result.isRetired) {
              toast.warn(
                'Only properties that are part of the Core Inventory (owned) can be subdivided/consolidated. This property is not in core inventory within PIMS.',
              );
            } else {
              setSelectProperty(result);
            }
          }
        } catch (e) {
          const axiosError = e as AxiosError<IApiError>;
          if (axiosError?.response?.status === 404) {
            toast.warn(
              'Only properties that are part of the Core Inventory (owned) can be subdivided/consolidated. This property is not in core inventory within PIMS.',
            );
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
