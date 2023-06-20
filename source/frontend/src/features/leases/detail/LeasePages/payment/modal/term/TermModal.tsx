import { FormikProps } from 'formik';
import * as React from 'react';
import { useRef } from 'react';

import * as CommonStyled from '@/components/common/styles';

import { FormLeaseTerm } from '../../models';
import { TermForm } from './TermForm';

export interface ITermModalProps {
  initialValues?: FormLeaseTerm;
  displayModal?: boolean;
  onCancel: () => void;
  onSave: (values: FormLeaseTerm) => void;
}

/**
 * Modal displaying form allowing add/update lease terms. Save button triggers internal formik validation and submit.
 * @param {ITermModalProps} props
 */
export const TermModal: React.FunctionComponent<React.PropsWithChildren<ITermModalProps>> = ({
  initialValues,
  displayModal,
  onCancel,
  onSave,
}) => {
  const formikRef = useRef<FormikProps<FormLeaseTerm>>(null);
  return (
    <CommonStyled.PrimaryGenericModal
      title="Add a Term"
      display={displayModal}
      okButtonText="Save term"
      cancelButtonText="Cancel"
      handleCancel={onCancel}
      handleOk={() => formikRef?.current?.submitForm()}
      message={<TermForm formikRef={formikRef} initialValues={initialValues} onSave={onSave} />}
    />
  );
};

export default TermModal;
