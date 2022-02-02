import * as CommonStyled from 'components/common/styles';
import { FormikProps } from 'formik';
import { IFormLeaseTerm } from 'interfaces/ILeaseTerm';
import * as React from 'react';
import { useRef } from 'react';

import { TermForm } from './TermForm';

export interface ITermModalProps {
  initialValues?: IFormLeaseTerm;
  displayModal?: boolean;
  onCancel: () => void;
  onSave: (values: IFormLeaseTerm) => void;
}

/**
 * Modal displaying form allowing add/update lease terms. Save button triggers internal formik validation and submit.
 * @param {ITermModalProps} props
 */
export const TermModal: React.FunctionComponent<ITermModalProps> = ({
  initialValues,
  displayModal,
  onCancel,
  onSave,
}) => {
  const formikRef = useRef<FormikProps<IFormLeaseTerm>>(null);
  console.log(initialValues);
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
