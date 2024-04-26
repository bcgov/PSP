import { FormikProps } from 'formik';
import { useContext } from 'react';
import { MdTopic } from 'react-icons/md';
import { matchPath, Route, useHistory, useRouteMatch } from 'react-router-dom';
import styled from 'styled-components';

import { FileTypes } from '@/constants';
import FileLayout from '@/features/mapSideBar/layout/FileLayout';
import MapSideBarLayout from '@/features/mapSideBar/layout/MapSideBarLayout';
import { InventoryTabNames } from '@/features/mapSideBar/property/InventoryTabs';
import { ApiGen_Concepts_File } from '@/models/api/generated/ApiGen_Concepts_File';
import { ApiGen_Concepts_ResearchFile } from '@/models/api/generated/ApiGen_Concepts_ResearchFile';
import { exists, getFilePropertyName, stripTrailingSlash } from '@/utils';

import { SideBarContext } from '../context/sidebarContext';
import FilePropertyRouter from '../router/FilePropertyRouter';
import { FileTabType } from '../shared/detail/FileTabs';
import { PropertyForm } from '../shared/models';
import SidebarFooter from '../shared/SidebarFooter';
import UpdateProperties from '../shared/update/properties/UpdateProperties';
import ResearchHeader from './common/ResearchHeader';
import ResearchMenu from './common/ResearchMenu';
import ResearchRouter from './ResearchRouter';

export interface IResearchViewProps {
  // props
  researchFile?: ApiGen_Concepts_ResearchFile;
  formikRef: React.RefObject<FormikProps<any>>;
  isEditing: boolean;
  setEditMode: (isEditing: boolean) => void;
  isShowingPropertySelector: boolean;
  setIsShowingPropertySelector: React.Dispatch<React.SetStateAction<boolean>>;
  isFormValid: boolean;
  // event handlers
  onClose: (() => void) | undefined;
  onSave: () => Promise<void>;
  onCancel: () => void;
  onMenuChange: (selectedIndex: number) => void;
  onUpdateProperties: (file: ApiGen_Concepts_File) => Promise<ApiGen_Concepts_File | undefined>;
  confirmBeforeAdd: (propertyForm: PropertyForm) => Promise<boolean>;
  canRemove: (propertyId: number) => Promise<boolean>;
  onSuccess: () => void;
}

const ResearchView: React.FunctionComponent<IResearchViewProps> = props => {
  const match = useRouteMatch();
  const { location } = useHistory();
  const propertiesMatch = matchPath<Record<string, string>>(
    location.pathname,
    `${stripTrailingSlash(match.path)}/property/:menuIndex/:tab?`,
  );

  const selectedMenuIndex = propertiesMatch !== null ? Number(propertiesMatch.params.menuIndex) : 0;
  const menuItems =
    props.researchFile?.fileProperties?.map(x => getFilePropertyName(x).value) || [];
  menuItems.unshift('File Summary');

  const { lastUpdatedBy } = useContext(SideBarContext);

  if (props.isShowingPropertySelector && exists(props.researchFile)) {
    return (
      <UpdateProperties
        file={props.researchFile}
        setIsShowingPropertySelector={props.setIsShowingPropertySelector}
        onSuccess={props.onSuccess}
        updateFileProperties={props.onUpdateProperties}
        confirmBeforeAdd={props.confirmBeforeAdd}
        confirmBeforeAddMessage={
          <>
            <p>This property has already been added to one or more research files.</p>
            <p>Do you want to acknowledge and proceed?</p>
          </>
        }
        canRemove={props.canRemove}
        formikRef={props.formikRef}
      />
    );
  } else {
    return (
      <MapSideBarLayout
        title={props.isEditing ? 'Update Research File' : 'Research File'}
        icon={<MdTopic title="User Profile" size="2.5rem" className="mr-2" />}
        header={<ResearchHeader researchFile={props.researchFile} lastUpdatedBy={lastUpdatedBy} />}
        footer={
          props.isEditing && (
            <SidebarFooter
              isOkDisabled={props.formikRef?.current?.isSubmitting}
              onSave={props.onSave}
              onCancel={props.onCancel}
              displayRequiredFieldError={!props.isFormValid}
            />
          )
        }
        onClose={props.onClose}
        showCloseButton
      >
        <FileLayout
          leftComponent={
            <ResearchMenu
              items={menuItems}
              selectedIndex={selectedMenuIndex}
              onChange={props.onMenuChange}
              onEdit={() => {
                props.setIsShowingPropertySelector(true);
              }}
            />
          }
          bodyComponent={
            <StyledFormWrapper>
              <ResearchRouter
                formikRef={props.formikRef}
                isEditing={props.isEditing}
                setIsEditing={props.setEditMode}
                defaultFileTab={FileTabType.FILE_DETAILS}
                defaultPropertyTab={InventoryTabNames.research}
                onSuccess={props.onSuccess}
                researchFile={props.researchFile}
              />
              <Route
                path={`${stripTrailingSlash(match.path)}/property/:menuIndex`}
                render={() => (
                  <FilePropertyRouter
                    formikRef={props.formikRef}
                    selectedMenuIndex={selectedMenuIndex}
                    file={props.researchFile}
                    fileType={FileTypes.Research}
                    isEditing={props.isEditing}
                    setIsEditing={props.setEditMode}
                    defaultFileTab={FileTabType.FILE_DETAILS}
                    defaultPropertyTab={InventoryTabNames.research}
                    onSuccess={props.onSuccess}
                  />
                )}
              />
            </StyledFormWrapper>
          }
        ></FileLayout>
      </MapSideBarLayout>
    );
  }
};

export default ResearchView;

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
