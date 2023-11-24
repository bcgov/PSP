import * as React from 'react';
import { useEffect } from 'react';

import { useInterestHolderRepository } from '@/hooks/repositories/useInterestHolderRepository';
import { Api_AcquisitionFile } from '@/models/api/AcquisitionFile';

import { IStakeHolderViewProps } from './StakeHolderView';

export interface IStakeHolderContainerProps {
  View: React.FC<IStakeHolderViewProps>;
  onEdit: () => void;
  acquisitionFile: Api_AcquisitionFile;
}

export const StakeHolderContainer: React.FunctionComponent<IStakeHolderContainerProps> = ({
  View,
  onEdit,
  acquisitionFile,
}) => {
  const {
    getAcquisitionInterestHolders: {
      execute: getInterestHolders,
      response: apiInterestHolders,
      loading,
    },
  } = useInterestHolderRepository();

  useEffect(() => {
    if (acquisitionFile?.id) {
      getInterestHolders(acquisitionFile.id);
    }
  }, [acquisitionFile.id, getInterestHolders]);

  return (
    <View
      loading={loading}
      acquisitionFile={acquisitionFile}
      interestHolders={apiInterestHolders}
      onEdit={onEdit}
    />
  );
};

export default StakeHolderContainer;
