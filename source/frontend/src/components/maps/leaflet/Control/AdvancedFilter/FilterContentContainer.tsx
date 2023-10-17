//import './Legend.scss';

import React, { useCallback } from 'react';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { usePimsPropertyRepository } from '@/hooks/repositories/usePimsPropertyRepository';
import { Api_PropertyFilterCriteria } from '@/models/api/ProjectFilterCriteria';

import { IFilterContentFormProps } from './FilterContentForm';
import { PropertyFilterFormModel } from './models';

interface IFilterContentContainerProps {
  View: React.FunctionComponent<IFilterContentFormProps>;
}

export const FilterContentContainer: React.FC<
  React.PropsWithChildren<IFilterContentContainerProps>
> = ({ View }) => {
  const mapMachine = useMapStateMachine();

  const setVisiblePimsProperties = mapMachine.setVisiblePimsProperties;

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
    (model: PropertyFilterFormModel) => {
      filterProperties(model.toApi());
    },
    [filterProperties],
  );

  return <View onChange={onChange} isLoading={getMatchingProperties.loading} />;
};
