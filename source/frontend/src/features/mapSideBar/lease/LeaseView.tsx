import { FormikProps } from 'formik';
import { useContext } from 'react';
import { Route, Switch, useRouteMatch } from 'react-router-dom';

import LeaseIcon from '@/assets/images/lease-icon.svg?react';
import { Claims, Roles } from '@/constants';
import LeaseUpdatePropertySelector from '@/features/leases/shared/propertyPicker/LeaseUpdatePropertySelector';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { ApiGen_CodeTypes_FileTypes } from '@/models/api/generated/ApiGen_CodeTypes_FileTypes';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { exists, stripTrailingSlash } from '@/utils';

import GenerateFormView from '../acquisition/common/GenerateForm/GenerateFormView';
import { SideBarContext } from '../context/sidebarContext';
import FileLayout from '../layout/FileLayout';
import MapSideBarLayout from '../layout/MapSideBarLayout';
import { InventoryTabNames } from '../property/InventoryTabs';
import FilePropertyRouter from '../router/FilePropertyRouter';
import { FileTabType } from '../shared/detail/FileTabs';
import FileMenuView from '../shared/FileMenuView';
import SidebarFooter from '../shared/SidebarFooter';
import { StyledFormWrapper } from '../shared/styles';
import { usePropertyIndexFromUrl } from '../shared/usePropertyIndexFromUrl';
import LeaseHeader from './common/LeaseHeader';
import { LeaseContainerState } from './LeaseContainer';
import LeaseGenerateContainer from './LeaseGenerateFormContainer';
import ViewSelector from './ViewSelector';

export interface ILeaseViewProps {
  onClose: (() => void) | undefined;
  onSave: () => Promise<void>;
  onCancel: () => void;
  onSelectFileSummary: () => void;
  onSelectProperty: (propertyId: number) => void;
  onEditProperties: () => void;
  onPropertyUpdateSuccess: () => void;
  onChildSuccess: () => void;
  refreshLease: () => void;
  setLease: (lease: ApiGen_Concepts_Lease) => void;
  containerState: LeaseContainerState;
  setContainerState: React.Dispatch<Partial<LeaseContainerState>>;
  setIsEditing: (value: boolean) => void;
  formikRef: React.RefObject<FormikProps<any>>;
  isFormValid: boolean;
  lease?: ApiGen_Concepts_Lease;
}

export const LeaseView: React.FunctionComponent<ILeaseViewProps> = ({
  onClose,
  onSave,
  onCancel,
  onSelectFileSummary,
  onSelectProperty,
  onEditProperties,
  onPropertyUpdateSuccess,
  onChildSuccess,
  refreshLease,
  setLease,
  containerState,
  setContainerState,
  setIsEditing,
  formikRef,
  isFormValid,
  lease,
}) => {
  // match for the current route
  const currentRoute = useRouteMatch();
  const { hasClaim, hasRole } = useKeycloakWrapper();
  const { lastUpdatedBy } = useContext(SideBarContext);

  // Extract the zero-based property index from the current URL path.
  // It will be null if route is not matched
  const currentPropertyIndex: number | null = usePropertyIndexFromUrl();

  return (
    <Switch>
      <Route path={`${stripTrailingSlash(currentRoute.path)}/property/selector`}>
        {exists(lease) && <LeaseUpdatePropertySelector lease={lease} />}
      </Route>
      <Route>
        <MapSideBarLayout
          showCloseButton
          onClose={onClose}
          title={containerState.isEditing ? 'Update Lease / Licence' : 'Lease / Licence'}
          icon={<LeaseIcon title="Lease file icon" fill="currentColor" />}
          header={<LeaseHeader lease={lease} lastUpdatedBy={lastUpdatedBy} />}
          footer={
            containerState.isEditing && (
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
                file={lease}
                currentPropertyIndex={currentPropertyIndex}
                canEdit={hasRole(Roles.SYSTEM_ADMINISTRATOR) || hasClaim(Claims.LEASE_EDIT)}
                isInNonEditableState={false}
                onSelectFileSummary={onSelectFileSummary}
                onSelectProperty={onSelectProperty}
                onEditProperties={onEditProperties}
              >
                <LeaseGenerateContainer lease={lease} View={GenerateFormView} />
              </FileMenuView>
            }
            bodyComponent={
              <StyledFormWrapper>
                <Switch>
                  <Route
                    path={`${stripTrailingSlash(currentRoute.path)}/property/:menuIndex`}
                    render={({ match }) => (
                      <FilePropertyRouter
                        formikRef={formikRef}
                        selectedMenuIndex={Number(match.params.menuIndex)}
                        file={lease}
                        fileType={ApiGen_CodeTypes_FileTypes.Lease}
                        isEditing={containerState.isEditing}
                        setIsEditing={setIsEditing}
                        defaultFileTab={FileTabType.FILE_DETAILS}
                        defaultPropertyTab={InventoryTabNames.property}
                        onSuccess={onPropertyUpdateSuccess}
                      />
                    )}
                  />
                  <Route
                    path={[`${stripTrailingSlash(currentRoute.path)}`]}
                    render={() => (
                      <ViewSelector
                        formikRef={formikRef}
                        lease={lease}
                        refreshLease={refreshLease}
                        setLease={setLease}
                        isEditing={containerState.isEditing}
                        activeEditForm={containerState.activeEditForm}
                        activeTab={containerState.activeTab}
                        setContainerState={setContainerState}
                        onSuccess={onChildSuccess}
                      />
                    )}
                  />
                </Switch>
              </StyledFormWrapper>
            }
          />
        </MapSideBarLayout>
      </Route>
    </Switch>
  );
};

export default LeaseView;
