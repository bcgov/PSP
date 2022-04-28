import { Button } from 'components/common/buttons/Button';
import { FormikProps } from 'formik';
import { ILease } from 'interfaces';
import { isEqual } from 'lodash';
import * as React from 'react';

import * as Styled from './add/styles';

interface ISaveCancelButtonsProps {
  onCancel: () => void;
  onSaveOverride?: () => Promise<ILease | undefined>;
  formikProps: FormikProps<any>;
  className?: string;
}

const SaveCancelButtons: React.FunctionComponent<ISaveCancelButtonsProps> = ({
  onCancel,
  formikProps,
  onSaveOverride,
  className,
}) => {
  return (
    <Styled.FormButtons className={className}>
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
          if (onSaveOverride) {
            onSaveOverride();
          } else {
            formikProps.setSubmitting(true);
            formikProps.submitForm();
          }
        }}
      >
        Save
      </Button>
    </Styled.FormButtons>
  );
};

export default SaveCancelButtons;
