import { FormikProps } from 'formik/dist/types';
import { isEqual } from 'lodash';

import { Button } from '@/components/common/buttons/Button';

import * as Styled from './add/styles';

interface ISaveCancelButtonsProps {
  onCancel: () => void;
  onSaveOverride?: () => Promise<void>;
  formikProps: FormikProps<any>;
  className?: string;
}

const SaveCancelButtons: React.FunctionComponent<
  React.PropsWithChildren<ISaveCancelButtonsProps>
> = ({ onCancel, formikProps, onSaveOverride, className }) => {
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
