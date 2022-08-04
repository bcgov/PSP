import { ReactComponent as RealEstateAgent } from 'assets/images/real-estate-agent.svg';
import MapSideBarLayout from 'features/mapSideBar/layout/MapSideBarLayout';
import React, { useCallback } from 'react';

interface IAcquisitionContainerProps {
  acquisitionFileId: number;
  onClose: () => void;
}

export const AcquisitionContainer: React.FunctionComponent<IAcquisitionContainerProps> = props => {
  const { onClose } = props;
  const close = useCallback(() => onClose && onClose(), [onClose]);

  // TODO: Placeholder UI until details view gets implemented
  return (
    <MapSideBarLayout
      showCloseButton
      title="Acquisition File"
      icon={
        <RealEstateAgent
          title="Acquisition file Icon"
          width="2.6rem"
          height="2.6rem"
          fill="currentColor"
          className="mr-2"
        />
      }
      onClose={close}
      footer={null}
    >
      <></>
    </MapSideBarLayout>
  );
};

export default AcquisitionContainer;
