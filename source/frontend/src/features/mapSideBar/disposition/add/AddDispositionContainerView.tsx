import { FormikProps } from 'formik';
import { MdAirlineStops } from 'react-icons/md';
import styled from 'styled-components';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';

import MapSideBarLayout from '../../layout/MapSideBarLayout';
import SidebarFooter from '../../shared/SidebarFooter';
import DispositionForm from '../form/DispositionForm';
import { DispositionFormModel } from '../models/DispositionFormModel';

export interface AddDispositionContainerViewProps {
  formikRef: React.RefObject<FormikProps<DispositionFormModel>>;
  dispositionInitialValues: DispositionFormModel;
  loading: boolean;
  displayFormInvalid: boolean;
  // onSubmit: (dispositionFile: Api_DispositionFile) => Promise<Api_DispositionFile | undefined>;
  onSubmit: (
    values: DispositionFormModel,
    setSubmitting: (isSubmitting: boolean) => void,
    userOverrides: UserOverrideCode[],
  ) => void | Promise<any>;
  onCancel: () => void;
  onSave: () => void;
}

const AddDispositionContainerView: React.FunctionComponent<
  React.PropsWithChildren<AddDispositionContainerViewProps>
> = ({
  formikRef,
  dispositionInitialValues,
  loading,
  displayFormInvalid,
  onSubmit,
  onSave,
  onCancel,
}) => {
  return (
    <MapSideBarLayout
      showCloseButton
      title="Create Disposition File"
      icon={
        <MdAirlineStops
          title="Disposition file Icon"
          width="2.6rem"
          height="2.6rem"
          fill="currentColor"
          className="mr-2"
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
          ref={formikRef}
          initialValues={dispositionInitialValues}
          onSubmit={onSubmit}
        ></DispositionForm>
      </StyledFormWrapper>
    </MapSideBarLayout>
  );
};

export default AddDispositionContainerView;

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