import { FormikHelpers, FormikProps } from 'formik';
import { useCallback } from 'react';
import { MdLibraryAdd } from 'react-icons/md';
import { useHistory } from 'react-router-dom';

import ConfirmNavigation from '@/components/common/ConfirmNavigation';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';

import MapSideBarLayout from '../../layout/MapSideBarLayout';
import { PropertyForm } from '../../shared/models';
import SidebarFooter from '../../shared/SidebarFooter';
import { StyledFormWrapper } from '../../shared/styles';
import ManagementForm from '../form/ManagementForm';
import { ManagementFormModel } from '../models/ManagementFormModel';

export interface IAddManagementContainerViewProps {
  formikRef: React.RefObject<FormikProps<ManagementFormModel>>;
  managementInitialValues: ManagementFormModel;
  loading: boolean;
  displayFormInvalid: boolean;
  onSubmit: (
    values: ManagementFormModel,
    formikHelpers: FormikHelpers<ManagementFormModel>,
  ) => void | Promise<any>;
  onCancel: () => void;
  onSave: () => void;
  confirmBeforeAdd: (propertyForm: PropertyForm) => Promise<boolean>;
}

const AddManagementContainerView: React.FunctionComponent<IAddManagementContainerViewProps> = ({
  formikRef,
  managementInitialValues,
  loading,
  displayFormInvalid,
  onSubmit,
  onSave,
  onCancel,
  confirmBeforeAdd,
}) => {
  const history = useHistory();

  const checkState = useCallback(() => {
    return formikRef?.current?.dirty && !formikRef?.current?.isSubmitting;
  }, [formikRef]);

  return (
    <MapSideBarLayout
      showCloseButton
      title="Create Management File"
      icon={<MdLibraryAdd title="Management file Icon" fill="currentColor" />}
      onClose={onCancel}
      footer={
        <SidebarFooter
          isOkDisabled={loading}
          onSave={onSave}
          onCancel={onCancel}
          displayRequiredFieldError={displayFormInvalid}
        />
      }
    >
      <StyledFormWrapper>
        <LoadingBackdrop show={loading} parentScreen={true} />
        <ManagementForm
          formikRef={formikRef}
          initialValues={managementInitialValues}
          onSubmit={onSubmit}
          confirmBeforeAdd={confirmBeforeAdd}
        />
      </StyledFormWrapper>
      <ConfirmNavigation navigate={history.push} shouldBlockNavigation={checkState} />
    </MapSideBarLayout>
  );
};

export default AddManagementContainerView;
