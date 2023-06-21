import { FormikProps } from 'formik';
import React from 'react';
import { Route, Switch, useHistory, useRouteMatch } from 'react-router-dom';
import styled from 'styled-components';

import { ReactComponent as RealEstateAgent } from '@/assets/images/real-estate-agent.svg';
import GenericModal from '@/components/common/GenericModal';
import FileLayout from '@/features/mapSideBar/layout/FileLayout';
import MapSideBarLayout from '@/features/mapSideBar/layout/MapSideBarLayout';
import { Api_File } from '@/models/api/File';
import { getFilePropertyName } from '@/utils/mapPropertyUtils';

import SidebarFooter from '../shared/SidebarFooter';
import UpdateProperties from '../shared/update/properties/UpdateProperties';
import { AcquisitionContainerState } from './AcquisitionContainer';
import { AcquisitionRouter } from './AcquisitionRouter';
import AcquisitionHeader from './common/AcquisitionHeader';
import AcquisitionMenu from './common/AcquisitionMenu';
import { EditFormType } from './EditFormNames';
import { FilePropertyRouter } from './FilePropertyRouter';

export interface IAcquisitionViewProps {
  onClose: (() => void) | undefined;
  onSave: () => void;
  onCancel: () => void;
  onMenuChange: (selectedIndex: number) => void;
  onShowPropertySelector: () => void;
  onSuccess: () => void;
  onCancelConfirm: () => void;
  onUpdateProperties: (file: Api_File) => Promise<Api_File | undefined>;
  canRemove: (propertyId: number) => Promise<boolean>;
  isEditing: boolean;
  setIsEditing: (value: boolean) => void;
  containerState: AcquisitionContainerState;
  setContainerState: React.Dispatch<Partial<AcquisitionContainerState>>;
  formikRef: React.RefObject<FormikProps<any>>;
}

export const AcquisitionView: React.FunctionComponent<IAcquisitionViewProps> = ({
  onClose,
  onSave,
  onCancel,
  onMenuChange,
  onShowPropertySelector,
  onSuccess,
  onCancelConfirm,
  onUpdateProperties,
  canRemove,
  isEditing,
  setIsEditing,
  containerState,
  setContainerState,
  formikRef,
}) => {
  // match for the current route
  const history = useHistory();
  const match = useRouteMatch();

  const closePropertySelector = () => {
    setIsEditing(false);
    history.push(`${match.url}`);
  };

  // match for property menu routes - eg /property/1/ltsa
  const propertiesMatch = useRouteMatch<{ menuIndex: string }>(`${match.path}/property/:menuIndex`);
  const selectedMenuIndex = propertiesMatch !== null ? Number(propertiesMatch.params.menuIndex) : 0;

  const formTitle =
    isEditing && containerState.activeEditForm
      ? getEditTitle(containerState.activeEditForm)
      : 'Acquisition File';

  const menuItems =
    containerState.acquisitionFile?.fileProperties?.map(x => getFilePropertyName(x).value) || [];
  menuItems.unshift('File Summary');

  return (
    <Switch>
      <Route path={`${match.url}/property/selector`}>
        {containerState.acquisitionFile && (
          <UpdateProperties
            file={containerState.acquisitionFile}
            setIsShowingPropertySelector={closePropertySelector}
            onSuccess={onSuccess}
            updateFileProperties={onUpdateProperties}
            canRemove={canRemove}
            formikRef={formikRef}
          />
        )}
      </Route>
      <Route>
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
            isEditing && (
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
                  acquisitionFileId={containerState.acquisitionFile?.id || 0}
                  items={menuItems}
                  selectedIndex={selectedMenuIndex}
                  onChange={onMenuChange}
                  onShowPropertySelector={onShowPropertySelector}
                />
              </>
            }
            bodyComponent={
              <StyledFormWrapper>
                <AcquisitionRouter
                  formikRef={formikRef}
                  acquisitionFile={containerState.acquisitionFile}
                  isEditing={isEditing}
                  setIsEditing={setIsEditing}
                  activeEditForm={containerState.activeEditForm}
                  defaultFileTab={containerState.defaultFileTab}
                  defaultPropertyTab={containerState.defaultPropertyTab}
                  setContainerState={setContainerState}
                  onSuccess={onSuccess}
                />
                <Route
                  path={`${match.path}/property/:menuIndex`}
                  render={({ match }) => (
                    <FilePropertyRouter
                      formikRef={formikRef}
                      selectedMenuIndex={Number(match.params.menuIndex)}
                      acquisitionFile={containerState.acquisitionFile}
                      isEditing={isEditing}
                      setIsEditing={setIsEditing}
                      activeEditForm={containerState.activeEditForm}
                      defaultFileTab={containerState.defaultFileTab}
                      defaultPropertyTab={containerState.defaultPropertyTab}
                      setContainerState={setContainerState}
                      onSuccess={onSuccess}
                    />
                  )}
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
      </Route>
    </Switch>
  );
};

const getEditTitle = (editFormName: EditFormType) => {
  switch (editFormName) {
    case EditFormType.ACQUISITION_SUMMARY:
      return 'Update Acquisition File';
    case EditFormType.PROPERTY_DETAILS:
      return 'Update Property File Data';
    case EditFormType.TAKES:
      return 'Update Takes';
    case EditFormType.ACQUISITION_CHECKLIST:
      return 'Update Checklist';
    case EditFormType.PROPERTY_SELECTOR:
      return 'Updating Acquisition Properties';
    case EditFormType.AGREEMENTS:
      return 'Updating Agreements';
    case EditFormType.STAKEHOLDERS:
      return 'Updating Stakeholders';
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
