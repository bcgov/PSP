import React from 'react';

interface IAcquisitionContainerProps {
  acquisitionFileId: number;
  onClose: () => void;
}

export const AcquisitionContainer: React.FunctionComponent<IAcquisitionContainerProps> = props => {
  // TODO: Placeholder UI until details view gets implemented
  return <></>;
};

export default AcquisitionContainer;
