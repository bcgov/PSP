import { ReactComponent as RealEstateAgent } from 'assets/images/real-estate-agent.svg';
import MapSideBarLayout from 'features/mapSideBar/layout/MapSideBarLayout';
import { useCallback } from 'react';

interface IAddAcquisitionContainerProps {
  onClose?: () => void;
}

const AddAcquisitionContainer: React.FC<IAddAcquisitionContainerProps> = props => {
  const { onClose } = props;

  const handleClose = useCallback(() => onClose && onClose(), [onClose]);

  return (
    <MapSideBarLayout
      title="Create Acquisition File"
      icon={<RealEstateAgent title="User Profile" width="2.5rem" className="mr-2" />}
      showCloseButton
      onClose={handleClose}
    ></MapSideBarLayout>
  );
};

export default AddAcquisitionContainer;
