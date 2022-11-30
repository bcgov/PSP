import { Button } from 'components/common/buttons/Button';
import GenericModal from 'components/common/GenericModal';
import { useMapSearch } from 'components/maps/hooks/useMapSearch';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import { Claims } from 'constants/claims';
import { FileTypes } from 'constants/fileTypes';
import FileLayout from 'features/mapSideBar/layout/FileLayout';
import MapSideBarLayout from 'features/mapSideBar/layout/MapSideBarLayout';
import { getFilePropertyName } from 'features/properties/selector/utils';
import { FormikProps } from 'formik';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { Api_ResearchFile } from 'models/api/ResearchFile';
import * as React from 'react';
import { useEffect, useRef } from 'react';
import { useState } from 'react';
import { MdLocationPin, MdTopic } from 'react-icons/md';
import styled from 'styled-components';

import { SideBarContext } from '../context/sidebarContext';
import SidebarFooter from '../shared/SidebarFooter';
import { UpdateProperties } from '../shared/update/properties/UpdateProperties';
import ResearchHeader from './common/ResearchHeader';
import ResearchMenu from './common/ResearchMenu';
import { FormKeys } from './FormKeys';
import { useGetResearch } from './hooks/useGetResearch';
import { useUpdateResearchProperties } from './hooks/useUpdateResearchProperties';
import ViewSelector from './ViewSelector';

export interface IResearchContainerProps {
  researchFileId: number;
  onClose: () => void;
}

export const ResearchContainer: React.FunctionComponent<
  React.PropsWithChildren<IResearchContainerProps>
> = props => {
  const {
    retrieveResearchFile: { execute: getResearchFile, loading: loadingResearchFile },
  } = useGetResearch();

  const [researchFile, setResearchFile] = useState<Api_ResearchFile | undefined>(undefined);
  const { setFile, setFileLoading } = React.useContext(SideBarContext);

  const [selectedMenuIndex, setSelectedMenuIndex] = useState<number>(0);
  const [isEditing, setIsEditing] = useState<boolean>(false);
  const [editKey, setEditKey] = useState(FormKeys.none);

  const [isShowingPropertySelector, setIsShowingPropertySelector] = useState<boolean>(false);

  const formikRef = useRef<FormikProps<any>>(null);

  const [showConfirmModal, setShowConfirmModal] = useState<boolean>(false);
  const { search } = useMapSearch();
  const { hasClaim } = useKeycloakWrapper();

  const menuItems = researchFile?.fileProperties?.map(x => getFilePropertyName(x).value) || [];
  menuItems.unshift('RFile Summary');

  const { updateResearchFileProperties } = useUpdateResearchProperties();

  useEffect(() => setFileLoading(loadingResearchFile), [loadingResearchFile, setFileLoading]);

  const fetchResearchFile = React.useCallback(async () => {
    var retrieved = await getResearchFile(props.researchFileId);
    setResearchFile(retrieved);
    setFile({ ...retrieved, fileType: FileTypes.Research });
  }, [getResearchFile, props.researchFileId, setFile]);

  React.useEffect(() => {
    if (researchFile === undefined) {
      fetchResearchFile();
    }
  }, [fetchResearchFile, researchFile]);

  if (researchFile === undefined && loadingResearchFile) {
    return (
      <>
        <LoadingBackdrop show={true} parentScreen={true}></LoadingBackdrop>
      </>
    );
  }

  const onMenuChange = (selectedIndex: number) => {
    if (isEditing) {
      if (formikRef?.current?.dirty) {
        if (
          window.confirm('You have made changes on this form. Do you wish to leave without saving?')
        ) {
          handleCancelClick();
          setSelectedMenuIndex(selectedIndex);
        }
      } else {
        handleCancelClick();
        setSelectedMenuIndex(selectedIndex);
      }
    } else {
      setSelectedMenuIndex(selectedIndex);
    }
  };

  const handleSaveClick = async () => {
    if (formikRef !== undefined) {
      formikRef.current?.setSubmitting(true);
      formikRef.current?.submitForm();
    }
  };

  const handleCancelClick = () => {
    if (formikRef !== undefined) {
      if (formikRef.current?.dirty) {
        setShowConfirmModal(true);
      } else {
        handleCancelConfirm();
      }
    } else {
      handleCancelConfirm();
    }
  };

  const handleCancelConfirm = () => {
    if (formikRef !== undefined) {
      formikRef.current?.resetForm();
    }
    setShowConfirmModal(false);
    setIsEditing(false);
    setEditKey(FormKeys.none);
  };

  const onSuccess = () => {
    fetchResearchFile();
    setIsEditing(false);
    setEditKey(FormKeys.none);
    search();
  };

  const showPropertiesSelector = () => {
    setIsShowingPropertySelector(true);
  };

  if (isShowingPropertySelector && researchFile) {
    return (
      <UpdateProperties
        file={researchFile}
        setIsShowingPropertySelector={setIsShowingPropertySelector}
        onSuccess={onSuccess}
        updateFileProperties={updateResearchFileProperties}
      />
    );
  } else {
    return (
      <MapSideBarLayout
        title={isEditing ? 'Update Research File' : 'Research File'}
        icon={<MdTopic title="User Profile" size="2.5rem" className="mr-2" />}
        header={<ResearchHeader researchFile={researchFile} />}
        footer={
          isEditing && (
            <SidebarFooter
              isOkDisabled={formikRef?.current?.isSubmitting}
              onSave={handleSaveClick}
              onCancel={handleCancelClick}
            />
          )
        }
        onClose={props.onClose}
        showCloseButton
      >
        <FileLayout
          leftComponent={
            <>
              {selectedMenuIndex === 0 &&
              hasClaim(Claims.RESEARCH_EDIT) &&
              researchFile !== undefined ? (
                <Button variant="success" onClick={showPropertiesSelector}>
                  <MdLocationPin size={'2.5rem'} />
                  Edit properties
                </Button>
              ) : null}

              <ResearchMenu
                items={menuItems}
                selectedIndex={selectedMenuIndex}
                onChange={onMenuChange}
              />
            </>
          }
          bodyComponent={
            <StyledFormWrapper>
              <>
                <GenericModal
                  display={showConfirmModal}
                  title={'Confirm changes'}
                  message={
                    <>
                      <div>If you cancel now, this research file will not be saved.</div>
                      <br />
                      <strong>Are you sure you want to Cancel?</strong>
                    </>
                  }
                  handleOk={handleCancelConfirm}
                  handleCancel={() => setShowConfirmModal(false)}
                  okButtonText="Ok"
                  cancelButtonText="Resume editing"
                  show
                />
                <ViewSelector
                  researchFile={researchFile}
                  selectedIndex={selectedMenuIndex}
                  isEditMode={isEditing}
                  editKey={editKey}
                  onSuccess={onSuccess}
                  setEditMode={setIsEditing}
                  setEditKey={setEditKey}
                  ref={formikRef}
                />
              </>
            </StyledFormWrapper>
          }
        ></FileLayout>
      </MapSideBarLayout>
    );
  }
};

export default ResearchContainer;

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
