import { Button } from 'components/common/form';
import { FormikProps } from 'formik';
import { isEqual } from 'lodash';
import * as React from 'react';

import * as Styled from './styles';

interface IAddLeaseFormButtonsProps {
  onCancel: () => void;
  formikProps: FormikProps<any>;
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
        disabled={
          formikProps.isSubmitting ||
          !formikProps.dirty ||
          isEqual(formikProps.initialValues, formikProps.values)
        }
        isSubmitting={formikProps.isSubmitting}
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
