import React, { useCallback } from 'react';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';

import { IFilterContentFormProps } from './FilterContentForm';
import { PropertyFilterFormModel } from './models';

export interface IFilterContentContainerProps {
  View: React.FunctionComponent<IFilterContentFormProps>;
}

export const FilterContentContainer: React.FC<IFilterContentContainerProps> = ({ View }) => {
  const {
    setShowDisposed,
    setShowRetired,
    resetMapFilter,
    setAdvancedSearchCriteria,
    isShowingMapFilter,
    isLoading,
  } = useMapStateMachine();

  const onChange = useCallback(
    async (model: PropertyFilterFormModel) => {
      setAdvancedSearchCriteria(model);
      setShowDisposed(model.isDisposed);
      setShowRetired(model.isRetired);
    },
    [setShowDisposed, setShowRetired, setAdvancedSearchCriteria],
  );

  // Only render if the map state is filtering.
  if (isShowingMapFilter) {
    return <View onChange={onChange} onReset={resetMapFilter} isLoading={isLoading} />;
  } else {
    return <></>;
  }
};
