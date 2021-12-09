import { Button } from 'components/common/form';
import { FormikProps } from 'formik';
import { IAddFormLease } from 'interfaces';
import * as React from 'react';

import * as Styled from './styles';

interface IAddLeaseFormButtonsProps {
  onCancel: () => void;
  formikProps: FormikProps<IAddFormLease>;
}

const AddLeaseFormButtons: React.FunctionComponent<IAddLeaseFormButtonsProps> = ({
  onCancel,
  formikProps,
}) => {
  return (
    <Styled.FormButtons>
      <Button variant="secondary" onClick={onCancel}>
        Cancel
      </Button>
      <Button
        disabled={formikProps.isSubmitting}
        isSubmitting={formikProps.isSubmitting}
        variant="secondary"
        onClick={async () => {
          formikProps.setSubmitting(true);
          formikProps.submitForm();
        }}
      >
        Save
      </Button>
    </Styled.FormButtons>
  );
};

export default AddLeaseFormButtons;
