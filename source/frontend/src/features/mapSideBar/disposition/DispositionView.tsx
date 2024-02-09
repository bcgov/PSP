import { AxiosError } from 'axios';
import { FormikProps } from 'formik';
import React, { useContext } from 'react';
import {
  match,
  matchPath,
  Route,
  Switch,
  useHistory,
  useLocation,
  useRouteMatch,
} from 'react-router-dom';
import styled from 'styled-components';

import { ReactComponent as RealEstateAgent } from '@/assets/images/real-estate-agent.svg';
import { FileTypes } from '@/constants';
import FileLayout from '@/features/mapSideBar/layout/FileLayout';
import MapSideBarLayout from '@/features/mapSideBar/layout/MapSideBarLayout';
import { IApiError } from '@/interfaces/IApiError';
import { Api_DispositionFile } from '@/models/api/DispositionFile';
import { Api_File } from '@/models/api/File';
import { stripTrailingSlash } from '@/utils';
import { getFilePropertyName } from '@/utils/mapPropertyUtils';

import { SideBarContext } from '../context/sidebarContext';
import { InventoryTabNames } from '../property/InventoryTabs';
import FilePropertyRouter from '../router/FilePropertyRouter';
import { FileTabType } from '../shared/detail/FileTabs';
import SidebarFooter from '../shared/SidebarFooter';
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
  onUpdateProperties: (file: Api_File) => Promise<Api_File | undefined>;
  canRemove: (propertyId: number) => Promise<boolean>;
  isEditing: boolean;
  setIsEditing: (value: boolean) => void;
  formikRef: React.RefObject<FormikProps<any>>;
  isFormValid: boolean;
  error: AxiosError<IApiError, any> | undefined;
  dispositionFile?: Api_DispositionFile;
}

export const DispositionView: React.FunctionComponent<IDispositionViewProps> = ({
  onClose,
  onSave,
  onCancel,
  onMenuChange,
  onShowPropertySelector,
  onSuccess,
  onUpdateProperties,
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
              title="Disposition file Icon"
              width="2.6rem"
              height="2.6rem"
              fill="currentColor"
              className="mr-2"
            />
          }
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
                  dispositionFileId={dispositionFile?.id || 0}
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
                      fileType={FileTypes.Disposition}
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

export default DispositionView;
