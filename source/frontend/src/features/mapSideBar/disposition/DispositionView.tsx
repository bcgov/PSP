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

import FileLayout from '@/features/mapSideBar/layout/FileLayout';
import MapSideBarLayout from '@/features/mapSideBar/layout/MapSideBarLayout';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_CodeTypes_FileTypes } from '@/models/api/generated/ApiGen_CodeTypes_FileTypes';
import { ApiGen_Concepts_DispositionFile } from '@/models/api/generated/ApiGen_Concepts_DispositionFile';
import { ApiGen_Concepts_File } from '@/models/api/generated/ApiGen_Concepts_File';
import { stripTrailingSlash } from '@/utils';
import { getFilePropertyName } from '@/utils/mapPropertyUtils';

import { SideBarContext } from '../context/sidebarContext';
import { InventoryTabNames } from '../property/InventoryTabs';
import FilePropertyRouter from '../router/FilePropertyRouter';
import { FileTabType } from '../shared/detail/FileTabs';
import { PropertyForm } from '../shared/models';
import SidebarFooter from '../shared/SidebarFooter';
import { StyledFormWrapper } from '../shared/styles';
import UpdateProperties from '../shared/update/properties/UpdateProperties';
import { DispositionHeader } from './common/DispositionHeader';
import DispositionMenu from './common/DispositionMenu';
import DispositionRouter from './router/DispositionRouter';

export interface IDispositionViewProps {
  onClose: (() => void) | undefined;
  onSave: () => Promise<void>;
  onCancel: () => void;
  onMenuChange: (selectedIndex: number) => void;
  onShowPropertySelector: () => void;
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
  onMenuChange,
  onShowPropertySelector,
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

  const selectedMenuIndex = propertiesMatch !== null ? Number(propertiesMatch.params.menuIndex) : 0;

  const formTitle = isEditing
    ? getEditTitle(fileMatch, propertySelectorMatch, propertiesMatch)
    : 'Disposition File';

  const menuItems = dispositionFile?.fileProperties?.map(x => getFilePropertyName(x).value) || [];
  menuItems.unshift('File Summary');

  const closePropertySelector = () => {
    setIsEditing(false);
    history.push(`${match.url}`);
  };

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
          icon={<TbArrowBounce title="Disposition file Icon" size={26} />}
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
              <>
                <DispositionMenu
                  dispositionFile={dispositionFile}
                  items={menuItems}
                  selectedIndex={selectedMenuIndex}
                  onChange={onMenuChange}
                  onShowPropertySelector={onShowPropertySelector}
                />
              </>
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
