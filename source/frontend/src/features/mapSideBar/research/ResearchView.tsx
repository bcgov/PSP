import { FormikProps } from 'formik';
import { useContext } from 'react';
import { Route, Switch, useHistory, useRouteMatch } from 'react-router-dom';

import ResearchFileIcon from '@/assets/images/research-icon.svg?react';
import { Claims } from '@/constants';
import FileLayout from '@/features/mapSideBar/layout/FileLayout';
import MapSideBarLayout from '@/features/mapSideBar/layout/MapSideBarLayout';
import { InventoryTabNames } from '@/features/mapSideBar/property/InventoryTabs';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { ApiGen_CodeTypes_FileTypes } from '@/models/api/generated/ApiGen_CodeTypes_FileTypes';
import { ApiGen_Concepts_File } from '@/models/api/generated/ApiGen_Concepts_File';
import { ApiGen_Concepts_ResearchFile } from '@/models/api/generated/ApiGen_Concepts_ResearchFile';
import { exists, stripTrailingSlash } from '@/utils';

import { SideBarContext } from '../context/sidebarContext';
import FilePropertyRouter from '../router/FilePropertyRouter';
import { FileTabType } from '../shared/detail/FileTabs';
import FileMenuView from '../shared/FileMenuView';
import { PropertyForm } from '../shared/models';
import SidebarFooter from '../shared/SidebarFooter';
import { StyledFormWrapper } from '../shared/styles';
import UpdateProperties from '../shared/update/properties/UpdateProperties';
import { useFilePropertyIdFromUrl } from '../shared/usePropertyIndexFromUrl';
import ResearchHeader from './common/ResearchHeader';
import ResearchRouter from './ResearchRouter';
import ResearchStatusUpdateSolver from './tabs/fileDetails/ResearchStatusUpdateSolver';

export interface IResearchViewProps {
  // props
  researchFile?: ApiGen_Concepts_ResearchFile;
  formikRef: React.RefObject<FormikProps<any>>;
  isEditing: boolean;
  setEditMode: (isEditing: boolean) => void;
  isFormValid: boolean;
  // event handlers
  onClose: (() => void) | undefined;
  onSave: () => Promise<void>;
  onCancel: () => void;
  onSelectFileSummary: () => void;
  onSelectProperty: (propertyId: number) => void;
  onEditProperties: () => void;
  onUpdateProperties: (file: ApiGen_Concepts_File) => Promise<ApiGen_Concepts_File | undefined>;
  confirmBeforeAdd: (propertyForm: PropertyForm) => Promise<boolean>;
  canRemove: (propertyId: number) => Promise<boolean>;
  onSuccess: () => void;
}

const ResearchView: React.FunctionComponent<IResearchViewProps> = ({
  onClose,
  onSave,
  onCancel,
  onSelectFileSummary,
  onSelectProperty,
  onEditProperties,
  onSuccess,
  onUpdateProperties,
  confirmBeforeAdd,
  canRemove,
  isEditing,
  setEditMode,
  formikRef,
  isFormValid,
  researchFile,
}) => {
  // match for the current route
  const history = useHistory();
  const match = useRouteMatch();
  const { hasClaim } = useKeycloakWrapper();
  const { lastUpdatedBy } = useContext(SideBarContext);

  const closePropertySelector = () => {
    setEditMode(false);
    history.push(`${match.url}`);
  };

  // Extract the zero-based property index from the current URL path.
  // It will be null if route is not matched
  const currentFilePropertyId: number | null = useFilePropertyIdFromUrl();
  const statusSolver = new ResearchStatusUpdateSolver(researchFile);

  return (
    <Switch>
      <Route path={`${stripTrailingSlash(match.path)}/property/selector`}>
        {exists(researchFile) && (
          <UpdateProperties
            file={researchFile}
            setIsShowingPropertySelector={closePropertySelector}
            onSuccess={onSuccess}
            updateFileProperties={onUpdateProperties}
            confirmBeforeAdd={confirmBeforeAdd}
            confirmBeforeAddMessage={
              <>
                <p>This property has already been added to one or more research files.</p>
                <p>Do you want to acknowledge and proceed?</p>
              </>
            }
            canRemove={canRemove}
            formikRef={formikRef}
          />
        )}
      </Route>
      <Route>
        <MapSideBarLayout
          title={isEditing ? 'Update Research File' : 'Research File'}
          icon={<ResearchFileIcon title="Research file Icon" fill="currentColor" />}
          header={<ResearchHeader researchFile={researchFile} lastUpdatedBy={lastUpdatedBy} />}
          footer={
            isEditing && (
              <SidebarFooter
                isOkDisabled={formikRef?.current?.isSubmitting}
                onSave={onSave}
                onCancel={onCancel}
                displayRequiredFieldError={!isFormValid}
              />
            )
          }
          onClose={onClose}
          showCloseButton
        >
          <FileLayout
            leftComponent={
              <FileMenuView
                file={researchFile}
                currentFilePropertyId={currentFilePropertyId}
                canEdit={hasClaim(Claims.RESEARCH_EDIT)}
                isInNonEditableState={!statusSolver.canEditProperties()}
                onSelectFileSummary={onSelectFileSummary}
                onSelectProperty={onSelectProperty}
                onEditProperties={onEditProperties}
              />
            }
            bodyComponent={
              <StyledFormWrapper>
                <ResearchRouter
                  formikRef={formikRef}
                  isEditing={isEditing}
                  setIsEditing={setEditMode}
                  defaultFileTab={FileTabType.FILE_DETAILS}
                  defaultPropertyTab={InventoryTabNames.research}
                  onSuccess={onSuccess}
                  researchFile={researchFile}
                />
                <Route
                  path={`${stripTrailingSlash(match.path)}/property/:filePropertyId`}
                  render={({ match }) => (
                    <FilePropertyRouter
                      formikRef={formikRef}
                      selectedFilePropertyId={Number(match.params.filePropertyId)}
                      file={researchFile}
                      fileType={ApiGen_CodeTypes_FileTypes.Research}
                      isEditing={isEditing}
                      setIsEditing={setEditMode}
                      defaultPropertyTab={InventoryTabNames.research}
                      onSuccess={onSuccess}
                    />
                  )}
                />
              </StyledFormWrapper>
            }
          ></FileLayout>
        </MapSideBarLayout>
      </Route>
    </Switch>
  );
};

export default ResearchView;
