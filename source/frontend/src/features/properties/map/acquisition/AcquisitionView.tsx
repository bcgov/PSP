import { ReactComponent as RealEstateAgent } from 'assets/images/real-estate-agent.svg';
import GenericModal from 'components/common/GenericModal';
import FileLayout from 'features/mapSideBar/layout/FileLayout';
import MapSideBarLayout from 'features/mapSideBar/layout/MapSideBarLayout';
import { FormikProps } from 'formik';
import { Api_File } from 'models/api/File';
import * as React from 'react';
import styled from 'styled-components';
import { getFilePropertyName } from 'utils/mapPropertyUtils';

import SidebarFooter from '../shared/SidebarFooter';
import UpdateProperties from '../shared/update/properties/UpdateProperties';
import { AcquisitionContainerState } from './AcquisitionContainer';
import AcquisitionHeader from './common/AcquisitionHeader';
import AcquisitionMenu from './common/AcquisitionMenu';
import { EditFormNames } from './EditFormNames';
import ViewSelector from './ViewSelector';

export interface IAcquisitionViewProps {
  onClose: (() => void) | undefined;
  onSave: () => void;
  onCancel: () => void;
  onMenuChange: (selectedIndex: number) => void;
  onSuccess: () => void;
  onCancelConfirm: () => void;
  onUpdateProperties: (file: Api_File) => Promise<Api_File | undefined>;
  canRemove: (propertyId: number) => Promise<boolean>;
  containerState: AcquisitionContainerState;
  setContainerState: React.Dispatch<Partial<AcquisitionContainerState>>;
  formikRef: React.RefObject<FormikProps<any>>;
}

export const AcquisitionView: React.FunctionComponent<IAcquisitionViewProps> = ({
  onClose,
  onSave,
  onCancel,
  onMenuChange,
  onSuccess,
  onCancelConfirm,
  onUpdateProperties,
  canRemove,
  containerState,
  setContainerState,
  formikRef,
}) => {
  const formTitle =
    containerState.isEditing && containerState.activeEditForm
      ? getEditTitle(containerState.activeEditForm)
      : 'Acquisition File';

  const menuItems =
    containerState.acquisitionFile?.fileProperties?.map(x => getFilePropertyName(x).value) || [];
  menuItems.unshift('File Summary');

  if (
    containerState.activeEditForm === EditFormNames.propertySelector &&
    containerState.acquisitionFile
  ) {
    return (
      <UpdateProperties
        file={containerState.acquisitionFile}
        setIsShowingPropertySelector={() =>
          setContainerState({ activeEditForm: undefined, isEditing: false })
        }
        onSuccess={onSuccess}
        updateFileProperties={onUpdateProperties}
        canRemove={canRemove}
      />
    );
  }

  return (
    <MapSideBarLayout
      showCloseButton
      onClose={onClose}
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
      header={<AcquisitionHeader acquisitionFile={containerState.acquisitionFile} />}
      footer={
        containerState.isEditing && (
          <SidebarFooter
            isOkDisabled={formikRef?.current?.isSubmitting}
            onSave={onSave}
            onCancel={onCancel}
          />
        )
      }
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
          </>
        }
        bodyComponent={
          <StyledFormWrapper>
            <ViewSelector
              ref={formikRef}
              acquisitionFile={containerState.acquisitionFile}
              isEditing={containerState.isEditing}
              activeEditForm={containerState.activeEditForm}
              selectedMenuIndex={containerState.selectedMenuIndex}
              defaultPropertyTab={containerState.defaultPropertyTab}
              setContainerState={setContainerState}
              onSuccess={onSuccess}
            />

            <GenericModal
              display={containerState.showConfirmModal}
              title={'Confirm changes'}
              message={
                <>
                  <div>If you cancel now, this form will not be saved.</div>
                  <br />
                  <strong>Are you sure you want to Cancel?</strong>
                </>
              }
              handleOk={onCancelConfirm}
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

const getEditTitle = (editFormName: EditFormNames) => {
  switch (editFormName) {
    case EditFormNames.acquisitionSummary:
      return 'Update Acquisition File';
    case EditFormNames.propertyDetails:
      return 'Update Property File Data';
    case EditFormNames.takes:
      return 'Update Takes';
    case EditFormNames.propertySelector:
      return 'Updating Acquisition Properties';
    default:
      throw Error('Cannot edit this type of form');
  }
};

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

export default AcquisitionView;
