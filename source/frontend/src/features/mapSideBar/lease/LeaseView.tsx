import { FormikProps } from 'formik';
import { useContext } from 'react';
import { Route, Switch, useRouteMatch } from 'react-router-dom';

import LeaseIcon from '@/assets/images/lease-icon.svg?react';
import { Claims, Roles } from '@/constants';
import { LeaseFormModel } from '@/features/leases/models';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { ApiGen_CodeTypes_FileTypes } from '@/models/api/generated/ApiGen_CodeTypes_FileTypes';
import { ApiGen_Concepts_File } from '@/models/api/generated/ApiGen_Concepts_File';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { exists, stripTrailingSlash } from '@/utils';

import { useFilePropertyIdFromUrl } from '../../../hooks/useFilePropertyIdFromUrl';
import GenerateFormView from '../acquisition/common/GenerateForm/GenerateFormView';
import { SideBarContext } from '../context/sidebarContext';
import FileLayout from '../layout/FileLayout';
import MapSideBarLayout from '../layout/MapSideBarLayout';
import { InventoryTabNames } from '../property/InventoryTabs';
import FilePropertyRouter from '../router/FilePropertyRouter';
import FileMenuView from '../shared/FileMenuView';
import { FileForm, PropertyForm } from '../shared/models';
import SidebarFooter from '../shared/SidebarFooter';
import { StyledFormWrapper } from '../shared/styles';
import UpdatePropertiesContainer from '../shared/update/properties/UpdatePropertiesContainer';
import LeaseHeader from './common/LeaseHeader';
import { LeaseContainerState } from './LeaseContainer';
import LeaseGenerateContainer from './LeaseGenerateContainer';
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
  setIsShowingPropertySelector: (isShowing: boolean) => void;
  onUpdateProperties: (file: FileForm) => Promise<ApiGen_Concepts_File | undefined>;
  confirmBeforeAddProperty: (propertyForm: PropertyForm) => Promise<boolean>;
  canRemoveProperty: (propertyId: number) => Promise<boolean>;
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
  setIsShowingPropertySelector,
  onUpdateProperties,
  confirmBeforeAddProperty,
  canRemoveProperty,
}) => {
  // match for the current route
  const currentRoute = useRouteMatch();
  const { hasClaim, hasRole } = useKeycloakWrapper();
  const { lastUpdatedBy } = useContext(SideBarContext);

  // Extract the zero-based property index from the current URL path.
  // It will be null if route is not matched
  const { filePropertyId: currentFilePropertyId } = useFilePropertyIdFromUrl();

  return (
    <Switch>
      <Route path={`${stripTrailingSlash(currentRoute.path)}/property/selector`}>
        {exists(lease) && (
          <UpdatePropertiesContainer
            formFile={LeaseFormModel.fromApi(lease)}
            setIsShowingPropertySelector={setIsShowingPropertySelector}
            onSuccess={onPropertyUpdateSuccess}
            updateFileProperties={onUpdateProperties}
            confirmBeforeAdd={confirmBeforeAddProperty}
            confirmBeforeAddMessage={
              <>
                <p>
                  You have selected a property not previously in the inventory. Do you want to add
                  this property to the lease?
                </p>
                <p>Do you want to acknowledge and proceed?</p>
              </>
            }
            canRemove={canRemoveProperty}
            canUploadShapefiles={false}
            canReposition={true}
            formikRef={formikRef}
            showArea={true}
          />
        )}
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
                currentFilePropertyId={currentFilePropertyId}
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
                    path={`${stripTrailingSlash(currentRoute.path)}/property/:filePropertyId`}
                    render={({ match }) => (
                      <FilePropertyRouter
                        formikRef={formikRef}
                        selectedFilePropertyId={Number(match.params.filePropertyId)}
                        file={lease}
                        fileType={ApiGen_CodeTypes_FileTypes.Lease}
                        isEditing={containerState.isEditing}
                        setIsEditing={setIsEditing}
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
