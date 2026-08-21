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
  const { getPropertyByPidLookupWrapper } = usePimsPropertyRepository();
  const { setModalContent, setDisplayModal } = useModalContext();

  const searchFunc = useCallback(
    async (layerSearch: ILayerSearchCriteria) => {
      if (layerSearch?.pid) {
        try {
          const result = await getPropertyByPidLookupWrapper.execute(layerSearch?.pid);
          if (result?.foundInPims && result?.property?.isRetired) {
            setModalContent({
              variant: 'error',
              okButtonText: 'Close',
              title: 'Error',
              message:
                'Only properties that are not retired in the Core Inventory can be subdivided/consolidated.',
              handleOk: () => setDisplayModal(false),
            });
            setDisplayModal(true);
          } else if (result?.property) {
            setSelectProperty(result.property);
          } else {
            setModalContent({
              variant: 'error',
              okButtonText: 'Close',
              title: 'Error',
              message: 'No property was found in Core Inventory or PMBC.',
              handleOk: () => setDisplayModal(false),
            });
            setDisplayModal(true);
          }
        } catch (e) {
          const axiosError = e as AxiosError<IApiError>;
          if (axiosError?.response?.status === 404) {
            setModalContent({
              variant: 'error',
              okButtonText: 'Close',
              title: 'Error',
              message: 'No property was found in Core Inventory or PMBC.',
              handleOk: () => setDisplayModal(false),
            });
            setDisplayModal(true);
          }
        }
      }
    },
    [getPropertyByPidLookupWrapper, setDisplayModal, setModalContent, setSelectProperty],
  );

  return (
    <PropertySelectorPidSearchView
      onSearch={searchFunc}
      loading={getPropertyByPidLookupWrapper.loading}
    />
  );
};

export default PropertySelectorPidSearchContainer;
