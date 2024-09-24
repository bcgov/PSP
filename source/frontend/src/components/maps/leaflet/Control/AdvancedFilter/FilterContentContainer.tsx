import React, { useCallback } from 'react';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { usePimsPropertyRepository } from '@/hooks/repositories/usePimsPropertyRepository';
import { Api_PropertyFilterCriteria } from '@/models/api/ProjectFilterCriteria';

import { IFilterContentFormProps } from './FilterContentForm';
import { PropertyFilterFormModel } from './models';

export interface IFilterContentContainerProps {
  View: React.FunctionComponent<IFilterContentFormProps>;
}

export const FilterContentContainer: React.FC<IFilterContentContainerProps> = ({ View }) => {
  const { isFiltering, setVisiblePimsProperties, setShowDisposed, setShowRetired, resetMapFilter } =
    useMapStateMachine();

  const { getMatchingProperties } = usePimsPropertyRepository();

  const matchProperties = getMatchingProperties.execute;

  const filterProperties = useCallback(
    async (filter: Api_PropertyFilterCriteria) => {
      const retrievedProperties = await matchProperties(filter);

      if (retrievedProperties !== undefined) {
        setVisiblePimsProperties(retrievedProperties);
      }
    },
    [matchProperties, setVisiblePimsProperties],
  );

  const onChange = useCallback(
    async (model: PropertyFilterFormModel) => {
      await filterProperties(model.toApi());
      setShowDisposed(model.isDisposed);
      setShowRetired(model.isRetired);
    },
    [filterProperties, setShowDisposed, setShowRetired],
  );

  // Only render if the map state is filtering.
  if (isFiltering) {
    return (
      <View
        onChange={onChange}
        onReset={resetMapFilter}
        isLoading={getMatchingProperties.loading}
      />
    );
  } else {
    return <></>;
  }
};
