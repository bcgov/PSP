import { AxiosError } from 'axios';
import { FormikProps } from 'formik';
import React, { useContext } from 'react';
import { TbArrowBounce } from 'react-icons/tb';
import {
  match,
  matchPath,
  Route,
  Switch,
  useHistory,
  useLocation,
  useRouteMatch,
} from 'react-router-dom';

import { Claims } from '@/constants';
import FileLayout from '@/features/mapSideBar/layout/FileLayout';
import MapSideBarLayout from '@/features/mapSideBar/layout/MapSideBarLayout';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_CodeTypes_FileTypes } from '@/models/api/generated/ApiGen_CodeTypes_FileTypes';
import { ApiGen_Concepts_DispositionFile } from '@/models/api/generated/ApiGen_Concepts_DispositionFile';
import { ApiGen_Concepts_File } from '@/models/api/generated/ApiGen_Concepts_File';
import { stripTrailingSlash } from '@/utils';

import { SideBarContext } from '../context/sidebarContext';
import { InventoryTabNames } from '../property/InventoryTabs';
import FilePropertyRouter from '../router/FilePropertyRouter';
import { FileTabType } from '../shared/detail/FileTabs';
import FileMenuView from '../shared/FileMenuView';
import { PropertyForm } from '../shared/models';
import SidebarFooter from '../shared/SidebarFooter';
import { StyledFormWrapper } from '../shared/styles';
import UpdateProperties from '../shared/update/properties/UpdateProperties';
import { usePropertyIndexFromUrl } from '../shared/usePropertyIndexFromUrl';
import { DispositionHeader } from './common/DispositionHeader';
import DispositionRouter from './router/DispositionRouter';
import DispositionStatusUpdateSolver from './tabs/fileDetails/detail/DispositionStatusUpdateSolver';

export interface IDispositionViewProps {
  onClose: (() => void) | undefined;
  onSave: () => Promise<void>;
  onCancel: () => void;
  onSelectFileSummary: () => void;
  onSelectProperty: (propertyId: number) => void;
  onEditProperties: () => void;
  onSuccess: (updateProperties?: boolean, updateFile?: boolean) => void;
  onUpdateProperties: (file: ApiGen_Concepts_File) => Promise<ApiGen_Concepts_File | undefined>;
  confirmBeforeAdd: (propertyForm: PropertyForm) => Promise<boolean>;
  canRemove: (propertyId: number) => Promise<boolean>;
  isEditing: boolean;
  setIsEditing: (value: boolean) => void;
  formikRef: React.RefObject<FormikProps<any>>;
  isFormValid: boolean;
  error: AxiosError<IApiError, any> | undefined;
  dispositionFile?: ApiGen_Concepts_DispositionFile;
}

export const DispositionView: React.FunctionComponent<IDispositionViewProps> = ({
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
  setIsEditing,
  formikRef,
  isFormValid,
  error,
  dispositionFile,
}) => {
  // match for the current route
  const location = useLocation();
  const history = useHistory();
  const match = useRouteMatch();
  const { hasClaim } = useKeycloakWrapper();
  const { lastUpdatedBy } = useContext(SideBarContext);

  // match for property menu routes - eg /property/1/ltsa
  const fileMatch = matchPath<Record<string, string>>(location.pathname, `${match.path}/:tab`);
  const propertySelectorMatch = matchPath<Record<string, string>>(
    location.pathname,
    `${stripTrailingSlash(match.path)}/property/selector`,
  );
  const propertiesMatch = matchPath<Record<string, string>>(
    location.pathname,
    `${stripTrailingSlash(match.path)}/property/:menuIndex/:tab`,
  );

  const formTitle = isEditing
    ? getEditTitle(fileMatch, propertySelectorMatch, propertiesMatch)
    : 'Disposition File';

  const closePropertySelector = () => {
    setIsEditing(false);
    history.push(`${match.url}`);
  };

  // Extract the zero-based property index from the current URL path.
  // It will be null if route is not matched
  const currentPropertyIndex: number | null = usePropertyIndexFromUrl();
  const statusSolver = new DispositionStatusUpdateSolver(dispositionFile);

  return (
    <Switch>
      <Route path={`${stripTrailingSlash(match.path)}/property/selector`}>
        {dispositionFile && (
          <UpdateProperties
            file={dispositionFile}
            setIsShowingPropertySelector={closePropertySelector}
            onSuccess={onSuccess}
            updateFileProperties={onUpdateProperties}
            confirmBeforeAdd={confirmBeforeAdd}
            canRemove={canRemove}
            formikRef={formikRef}
            confirmBeforeAddMessage={
              <>
                <p>This property has already been added to one or more disposition files.</p>
                <p>Do you want to acknowledge and proceed?</p>
              </>
            }
          />
        )}
      </Route>
      <Route>
        <MapSideBarLayout
          showCloseButton
          onClose={onClose}
          title={formTitle}
          icon={<TbArrowBounce title="Disposition file Icon" size={28} />}
          header={
            <DispositionHeader dispositionFile={dispositionFile} lastUpdatedBy={lastUpdatedBy} />
          }
          footer={
            isEditing && (
              <SidebarFooter
                isOkDisabled={formikRef?.current?.isSubmitting}
                onSave={onSave}
                onCancel={onCancel}
                displayRequiredFieldError={isFormValid === false}
              />
            )
          }
        >
          <FileLayout
            leftComponent={
              <FileMenuView
                file={dispositionFile}
                currentPropertyIndex={currentPropertyIndex}
                canEdit={hasClaim(Claims.DISPOSITION_EDIT)}
                isInNonEditableState={!statusSolver.canEditProperties()}
                onSelectFileSummary={onSelectFileSummary}
                onSelectProperty={onSelectProperty}
                onEditProperties={onEditProperties}
              />
            }
            bodyComponent={
              <StyledFormWrapper>
                {error && (
                  <b>
                    Failed to load Disposition File. Check the detailed error in the top right for
                    more details.
                  </b>
                )}
                <DispositionRouter
                  formikRef={formikRef}
                  dispositionFile={dispositionFile}
                  isEditing={isEditing}
                  setIsEditing={setIsEditing}
                  defaultFileTab={FileTabType.FILE_DETAILS}
                  defaultPropertyTab={InventoryTabNames.property}
                  onSuccess={onSuccess}
                />
                <Route
                  path={`${stripTrailingSlash(match.path)}/property/:menuIndex`}
                  render={({ match }) => (
                    <FilePropertyRouter
                      formikRef={formikRef}
                      selectedMenuIndex={Number(match.params.menuIndex)}
                      file={dispositionFile}
                      fileType={ApiGen_CodeTypes_FileTypes.Disposition}
                      isEditing={isEditing}
                      setIsEditing={setIsEditing}
                      defaultFileTab={FileTabType.FILE_DETAILS}
                      defaultPropertyTab={InventoryTabNames.property}
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

// Set header title based on current tab route
const getEditTitle = (
  fileMatch: match<Record<string, string>> | null,
  propertySelectorMatch: match<Record<string, string>> | null,
  propertiesMatch: match<Record<string, string>> | null,
) => {
  if (fileMatch !== null) {
    const fileTab = fileMatch.params.tab;
    switch (fileTab) {
      case FileTabType.FILE_DETAILS:
        return 'Update Disposition File';
    }
  }

  if (propertySelectorMatch !== null) {
    return 'Update Disposition Properties';
  }

  if (propertiesMatch !== null) {
    const propertyTab = propertiesMatch.params.tab;
    switch (propertyTab) {
      case InventoryTabNames.property:
        return 'Update Property File Data';
    }
  }

  return 'Disposition File';
};

export default DispositionView;
