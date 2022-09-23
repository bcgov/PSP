import { ReactComponent as RealEstateAgent } from 'assets/images/real-estate-agent.svg';
import { Button } from 'components/common/buttons';
import { useMapSearch } from 'components/maps/hooks/useMapSearch';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import { FileTypes } from 'constants/index';
import { Claims } from 'constants/claims';
import FileLayout from 'features/mapSideBar/layout/FileLayout';
import MapSideBarLayout from 'features/mapSideBar/layout/MapSideBarLayout';
import { getAcquisitionPropertyName } from 'features/properties/selector/utils';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { Api_AcquisitionFile } from 'models/api/AcquisitionFile';
import React, { useCallback, useContext, useEffect, useReducer, useState } from 'react';
import styled from 'styled-components';
import { MdLocationPin } from 'react-icons/md';

import { SideBarContext } from '../context/sidebarContext';
import { UpdateProperties } from '../shared/update/properties/UpdateProperties';
import { AcquisitionHeader } from './common/AcquisitionHeader';
import AcquisitionMenu from './common/AcquisitionMenu';
import { EditFormNames } from './EditFormNames';
import { useAcquisitionProvider } from './hooks/useAcquisitionProvider';
import ViewSelector from './ViewSelector';

export interface IAcquisitionContainerProps {
  acquisitionFileId: number;
  onClose: () => void;
}

// Interface for our internal state
export interface AcquisitionContainerState {
  isEditing: boolean;
  activeEditForm?: EditFormNames;
  selectedMenuIndex: number;
}

export const AcquisitionContainer: React.FunctionComponent<IAcquisitionContainerProps> = props => {
  // Load state from props and side-bar context
  const { acquisitionFileId, onClose } = props;
  const { setFile, setFileLoading } = useContext(SideBarContext);
  const { getAcquisitionFile, updateAcquisitionFile } = useAcquisitionProvider();

  const [acquisitionFile, setAcquisitionFile] = useState<Api_AcquisitionFile | undefined>(
    undefined,
  );
  const [isShowingPropertySelector, setIsShowingPropertySelector] = useState<boolean>(false);
  const { hasClaim } = useKeycloakWrapper();
  const { search } = useMapSearch();

  /**
   See here that we are using `newState: Partial<AcquisitionContainerState>` in our reducer
   so we can provide only the properties that are updated on our state
   */
  const [containerState, setContainerState] = useReducer(
    (prevState: AcquisitionContainerState, newState: Partial<AcquisitionContainerState>) => ({
      ...prevState,
      ...newState,
    }),
    {
      isEditing: false,
      activeEditForm: undefined,
      selectedMenuIndex: 0,
    },
  );

  const {
    getAcquisitionFile: { execute: retrieveAcquisitionFile, loading: loadingAcquisitionFile },
  } = useAcquisitionProvider();

  // Retrieve acquisition file from API and save it to local state and side-bar context
  const fetchAcquisitionFile = useCallback(async () => {
    var retrieved = await retrieveAcquisitionFile(acquisitionFileId);
    setAcquisitionFile(retrieved);
    setFile({ ...retrieved, fileType: FileTypes.Acquisition });
  }, [acquisitionFileId, retrieveAcquisitionFile, setFile]);

  useEffect(() => {
    if (acquisitionFile === undefined) {
      fetchAcquisitionFile();
    }
  }, [acquisitionFile, fetchAcquisitionFile]);

  useEffect(() => setFileLoading(loadingAcquisitionFile), [loadingAcquisitionFile, setFileLoading]);

  const close = useCallback(() => onClose && onClose(), [onClose]);

  const onMenuChange = (selectedIndex: number) => {
    setContainerState({ selectedMenuIndex: selectedIndex });
  };

  // UI components
  const formTitle = containerState.isEditing ? 'Update Acquisition File' : 'Acquisition File';

  const menuItems =
    acquisitionFile?.acquisitionProperties?.map(x => getAcquisitionPropertyName(x).value) || [];
  menuItems.unshift('File Summary');

  if (acquisitionFile === undefined && loadingAcquisitionFile) {
    return <LoadingBackdrop show={true} parentScreen={true}></LoadingBackdrop>;
  }

  return (
    <MapSideBarLayout
      showCloseButton
      onClose={close}
      title={formTitle}
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
      <FileLayout
        leftComponent={
        <>
          <AcquisitionMenu
            items={menuItems}
            selectedIndex={containerState.selectedMenuIndex}
            onChange={onMenuChange}
            setContainerState={setContainerState}
          />
          {hasClaim(Claims.ACQUISITION_EDIT) && acquisitionFile !== undefined ? (
          <Button variant="success" onClick={() => setIsShowingPropertySelector(true)}>
            <MdLocationPin size={'2.5rem'} />
            Edit properties
          </Button>
        ) : null}
        {!!acquisitionFile && isShowingPropertySelector && (
          <UpdateProperties
            file={acquisitionFile}
            setIsShowingPropertySelector={setIsShowingPropertySelector}
            onSuccess={onSuccess}
            updateFileProperties={updateAcquisitionFile.execute}
          />
        )}
        </>
        }
        bodyComponent={
          <StyledFormWrapper>
            <ViewSelector acquisitionFile={acquisitionFile} setContainerState={setContainerState} />
          </StyledFormWrapper>
        }
      ></FileLayout>
      </>
    </MapSideBarLayout>
  );
};

export default AcquisitionContainer;

const StyledFormWrapper = styled.div`
  display: flex;
  flex-direction: column;
  flex-grow: 1;
  text-align: left;
  height: 100%;
  overflow-y: auto;
  padding-right: 1rem;
  padding-bottom: 1rem;
`;
