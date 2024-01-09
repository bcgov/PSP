import { FormikProps } from 'formik';
import React from 'react';
import styled from 'styled-components';

import { UserOverrideCode } from '@/models/api/UserOverrideCode';

import DispositionForm from '../form/DispositionForm';
import { DispositionFormModel } from '../models/DispositionFormModel';

export interface IUpdateDispositionViewProps {
  formikRef: React.Ref<FormikProps<DispositionFormModel>>;
  dispositionInitialValues: DispositionFormModel;
  onSubmit: (
    values: DispositionFormModel,
    setSubmitting: (isSubmitting: boolean) => void,
    userOverrides: UserOverrideCode[],
  ) => void | Promise<any>;
}

const UpdateDispositionView: React.FunctionComponent<
  React.PropsWithChildren<IUpdateDispositionViewProps>
> = ({ formikRef, dispositionInitialValues, onSubmit }) => {
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
