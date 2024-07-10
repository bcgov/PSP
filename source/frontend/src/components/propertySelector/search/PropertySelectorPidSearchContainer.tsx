import { AxiosError } from 'axios';
import { useCallback } from 'react';

import { usePimsPropertyRepository } from '@/hooks/repositories/usePimsPropertyRepository';
import { useModalContext } from '@/hooks/useModalContext';
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
  const { setModalContent, setDisplayModal } = useModalContext();

  const searchFunc = useCallback(
    async (layerSearch: ILayerSearchCriteria) => {
      if (layerSearch?.pid) {
        try {
          const result = await getPropertyByPidWrapper.execute(layerSearch?.pid);
          if (result) {
            if (!result.isOwned || result.isRetired) {
              setModalContent({
                variant: 'error',
                okButtonText: 'Close',
                title: 'Error',
                message:
                  'Only properties that are part of the Core Inventory (owned) can be subdivided/consolidated. This property is not in core inventory within PIMS.',
                handleOk: () => setDisplayModal(false),
              });
              setDisplayModal(true);
            } else {
              setSelectProperty(result);
            }
          }
        } catch (e) {
          const axiosError = e as AxiosError<IApiError>;
          if (axiosError?.response?.status === 404) {
            setModalContent({
              variant: 'error',
              okButtonText: 'Close',
              title: 'Error',
              message:
                'Only properties that are part of the Core Inventory (owned) can be subdivided/consolidated. This property is not in core inventory within PIMS.',
              handleOk: () => setDisplayModal(false),
            });
            setDisplayModal(true);
          }
        }
      }
    },
    [getPropertyByPidWrapper, setDisplayModal, setModalContent, setSelectProperty],
  );

  return (
    <PropertySelectorPidSearchView
      onSearch={searchFunc}
      loading={getPropertyByPidWrapper.loading}
    />
  );
};

export default PropertySelectorPidSearchContainer;
