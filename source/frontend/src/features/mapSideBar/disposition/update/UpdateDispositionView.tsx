import { FormikProps } from 'formik';
import styled from 'styled-components';

import { UserOverrideCode } from '@/models/api/UserOverrideCode';

import DispositionForm from '../form/DispositionForm';
import { DispositionFormModel } from '../models/DispositionFormModel';

export interface IUpdateDispositionViewProps {
  formikRef: React.Ref<FormikProps<DispositionFormModel>>;
  dispositionInitialValues: DispositionFormModel;
  loading: boolean;
  // displayFormInvalid: boolean;
  onSubmit: (
    values: DispositionFormModel,
    setSubmitting: (isSubmitting: boolean) => void,
    userOverrides: UserOverrideCode[],
  ) => void | Promise<any>;
  // onCancel: () => void;
  // onSave: () => void;
}

const UpdateDispositionView: React.FunctionComponent<
  React.PropsWithChildren<IUpdateDispositionViewProps>
> = ({
  formikRef,
  dispositionInitialValues,
  loading,
  // displayFormInvalid,
  onSubmit,
  // onSave,
  // onCancel,
}) => {
  return (
    <StyledFormWrapper>
      <DispositionForm
        formikRef={formikRef}
        initialValues={dispositionInitialValues}
        onSubmit={onSubmit}
      ></DispositionForm>
    </StyledFormWrapper>
  );
};

export default UpdateDispositionView;

const StyledFormWrapper = styled.div`
  background-color: ${props => props.theme.css.filterBackgroundColor};
  display: flex;
  flex-direction: column;
  flex-grow: 1;
  text-align: left;
  height: 100%;
  overflow-y: auto;
  padding-right: 1rem;
  padding-bottom: 1rem;
`;
