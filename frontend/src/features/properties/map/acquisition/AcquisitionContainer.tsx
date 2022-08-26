import { ReactComponent as RealEstateAgent } from 'assets/images/real-estate-agent.svg';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import MapSideBarLayout from 'features/mapSideBar/layout/MapSideBarLayout';
import { Api_AcquisitionFile } from 'models/api/AcquisitionFile';
import React, { useCallback, useEffect, useState } from 'react';

import { AcquisitionHeader } from './common/AcquisitionHeader';
import { useAcquisitionProvider } from './hooks/useAcquisitionProvider';

export interface IAcquisitionContainerProps {
  acquisitionFileId: number;
  onClose: () => void;
}

export const AcquisitionContainer: React.FunctionComponent<IAcquisitionContainerProps> = props => {
  const { acquisitionFileId, onClose } = props;
  const { getAcquisitionFile } = useAcquisitionProvider();
  const getById = getAcquisitionFile.execute;
  const loading = getAcquisitionFile.loading;
  const [acquisitionFile, setAcquisitionFile] = useState<Api_AcquisitionFile | undefined>(
    undefined,
  );

  const fetchAcquisitionFile = useCallback(async () => {
    var retrieved = await getById(acquisitionFileId);
    setAcquisitionFile(retrieved);
  }, [acquisitionFileId, getById]);

  useEffect(() => {
    fetchAcquisitionFile();
  }, [fetchAcquisitionFile]);

  const close = useCallback(() => onClose && onClose(), [onClose]);

  if (loading) {
    return <LoadingBackdrop show={true} parentScreen={true}></LoadingBackdrop>;
  }

  // TODO: Placeholder UI until details view gets implemented
  return (
    <MapSideBarLayout
      showCloseButton
      onClose={close}
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
      header={<AcquisitionHeader acquisitionFile={acquisitionFile} />}
      footer={null}
    >
      <></>
    </MapSideBarLayout>
  );
};

export default AcquisitionContainer;
