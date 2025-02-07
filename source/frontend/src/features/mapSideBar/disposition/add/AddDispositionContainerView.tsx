import { FormikHelpers, FormikProps } from 'formik';
import { useCallback } from 'react';
import { useHistory } from 'react-router-dom';

import DispositionFileIcon from '@/assets/images/disposition-icon.svg?react';
import ConfirmNavigation from '@/components/common/ConfirmNavigation';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';

import MapSideBarLayout from '../../layout/MapSideBarLayout';
import { PropertyForm } from '../../shared/models';
import SidebarFooter from '../../shared/SidebarFooter';
import { StyledFormWrapper } from '../../shared/styles';
import DispositionForm from '../form/DispositionForm';
import { DispositionFormModel } from '../models/DispositionFormModel';

export interface IAddDispositionContainerViewProps {
  formikRef: React.RefObject<FormikProps<DispositionFormModel>>;
  dispositionInitialValues: DispositionFormModel;
  loading: boolean;
  displayFormInvalid: boolean;
  onSubmit: (
    values: DispositionFormModel,
    formikHelpers: FormikHelpers<DispositionFormModel>,
  ) => void | Promise<any>;
  onCancel: () => void;
  onSave: () => void;
  confirmBeforeAdd: (propertyForm: PropertyForm) => Promise<boolean>;
}

const AddDispositionContainerView: React.FunctionComponent<IAddDispositionContainerViewProps> = ({
  formikRef,
  dispositionInitialValues,
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
      title="Create Disposition File"
      icon={
        <DispositionFileIcon
          title="Disposition file Icon"
          width="2.8rem"
          height="2.8rem"
          fill="currentColor"
        />
      }
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
        <DispositionForm
          formikRef={formikRef}
          initialValues={dispositionInitialValues}
          onSubmit={onSubmit}
          confirmBeforeAdd={confirmBeforeAdd}
        />
      </StyledFormWrapper>
      <ConfirmNavigation navigate={history.push} shouldBlockNavigation={checkState} />
    </MapSideBarLayout>
  );
};

export default AddDispositionContainerView;
