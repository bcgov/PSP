import * as React from 'react';

import { useBctfaOwnershipRepository } from '@/hooks/repositories/useBctfaOwnershipRepository';

import { IUpdateBcftaOwnershipViewProps } from './UpdateBctfaOwnershipView';

interface IUpdateBctfaOwnershipContainerProps {
  View: React.FunctionComponent<IUpdateBcftaOwnershipViewProps>;
}

export const UpdateBctfaOwnershipContainer: React.FunctionComponent<
  IUpdateBctfaOwnershipContainerProps
> = ({ View }) => {
  const { updateBctfaOwnershipApi } = useBctfaOwnershipRepository();
  return (
    <View
      onSubmit={async (file: File) => {
        await updateBctfaOwnershipApi.execute(file);
      }}
    />
  );
};

export default UpdateBctfaOwnershipContainer;
