import { ReactComponent as RealEstateAgent } from 'assets/images/real-estate-agent.svg';
import { GenericModal } from 'components/common/GenericModal';
import { useMapSearch } from 'components/maps/hooks/useMapSearch';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import { FileTypes } from 'constants/index';
import FileLayout from 'features/mapSideBar/layout/FileLayout';
import MapSideBarLayout from 'features/mapSideBar/layout/MapSideBarLayout';
import { getFilePropertyName } from 'features/properties/selector/utils';
import { FormikProps } from 'formik';
import { Api_AcquisitionFile } from 'models/api/AcquisitionFile';
import React, { useCallback, useContext, useEffect, useReducer, useState } from 'react';
import styled from 'styled-components';

import { SideBarContext } from '../context/sidebarContext';
import SidebarFooter from '../shared/SidebarFooter';
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
  formikRef?: React.RefObject<FormikProps<any>>;
  selectedMenuIndex: number;
  showConfirmModal: boolean;
}

const initialState: AcquisitionContainerState = {
  isEditing: false,
  activeEditForm: undefined,
  formikRef: undefined,
  selectedMenuIndex: 0,
  showConfirmModal: false,
};

export const AcquisitionContainer: React.FunctionComponent<IAcquisitionContainerProps> = props => {
  // Load state from props and side-bar context
  const { acquisitionFileId, onClose } = props;
  const { setFile, setFileLoading } = useContext(SideBarContext);
  const { search } = useMapSearch();

  const [acquisitionFile, setAcquisitionFile] = useState<Api_AcquisitionFile | undefined>(
    undefined,
  );

  /**
   See here that we are using `newState: Partial<AcquisitionContainerState>` in our reducer
   so we can provide only the properties that are updated on our state
   */
  const [containerState, setContainerState] = useReducer(
    (prevState: AcquisitionContainerState, newState: Partial<AcquisitionContainerState>) => ({
      ...prevState,
      ...newState,
    }),
    initialState,
  );

  const {
    getAcquisitionFile: { execute: retrieveAcquisitionFile, loading: loadingAcquisitionFile },
    updateAcquisitionFileProperties,
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
    if (containerState.isEditing) {
      if (containerState.formikRef?.current?.dirty) {
        if (
          window.confirm('You have made changes on this form. Do you wish to leave without saving?')
        ) {
          handleCancelClick();
          setContainerState({ selectedMenuIndex: selectedIndex });
        }
      } else {
        handleCancelClick();
        setContainerState({ selectedMenuIndex: selectedIndex });
      }
    } else {
      setContainerState({ selectedMenuIndex: selectedIndex });
    }
  };

  const handleSaveClick = async () => {
    if (containerState.formikRef !== undefined) {
      containerState.formikRef.current?.setSubmitting(true);
      containerState.formikRef.current?.submitForm();
    }
  };

  const handleCancelClick = () => {
    if (containerState.formikRef !== undefined) {
      if (containerState.formikRef.current?.dirty) {
        setContainerState({ showConfirmModal: true });
      } else {
        handleCancelConfirm();
      }
    } else {
      handleCancelConfirm();
    }
  };

  const handleCancelConfirm = () => {
    if (containerState.formikRef !== undefined) {
      containerState.formikRef.current?.resetForm();
    }

    setContainerState({
      showConfirmModal: false,
      isEditing: false,
      activeEditForm: undefined,
    });
  };

  const onSuccess = () => {
    fetchAcquisitionFile();
    search();
    setContainerState({
      isEditing: false,
      activeEditForm: undefined,
    });
  };

  // UI components
  const formTitle = containerState.isEditing ? 'Update Acquisition File' : 'Acquisition File';

  const menuItems = acquisitionFile?.fileProperties?.map(x => getFilePropertyName(x).value) || [];
  menuItems.unshift('File Summary');

  if (loadingAcquisitionFile) {
    return <LoadingBackdrop show={true} parentScreen={true}></LoadingBackdrop>;
  }

  if (containerState.activeEditForm === EditFormNames.propertySelector && acquisitionFile) {
    return (
      <UpdateProperties
        file={acquisitionFile}
        setIsShowingPropertySelector={() =>
          setContainerState({ activeEditForm: undefined, isEditing: false })
        }
        onSuccess={onSuccess}
        updateFileProperties={updateAcquisitionFileProperties.execute}
      />
    );
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
      footer={
        containerState.isEditing && (
          <SidebarFooter
            isOkDisabled={containerState.formikRef?.current?.isSubmitting}
            onSave={handleSaveClick}
            onCancel={handleCancelClick}
          />
        )
      }
    >
      <FileLayout
        leftComponent={
          <AcquisitionMenu
            items={menuItems}
            selectedIndex={containerState.selectedMenuIndex}
            onChange={onMenuChange}
            setContainerState={setContainerState}
          />
        }
        bodyComponent={
          <StyledFormWrapper>
            <ViewSelector
              acquisitionFile={acquisitionFile}
              isEditing={containerState.isEditing}
              activeEditForm={containerState.activeEditForm}
              selectedMenuIndex={containerState.selectedMenuIndex}
              setContainerState={setContainerState}
              onSuccess={onSuccess}
            />

            <GenericModal
              display={containerState.showConfirmModal}
              title={'Confirm changes'}
              message={
                <>
                  <div>If you cancel now, this acquisition file will not be saved.</div>
                  <br />
                  <strong>Are you sure you want to Cancel?</strong>
                </>
              }
              handleOk={handleCancelConfirm}
              handleCancel={() => setContainerState({ showConfirmModal: false })}
              okButtonText="Ok"
              cancelButtonText="Resume editing"
              show
            />
          </StyledFormWrapper>
        }
      ></FileLayout>
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
