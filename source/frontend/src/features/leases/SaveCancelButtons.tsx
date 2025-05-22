import { FormikProps } from 'formik/dist/types';
import { isEqual } from 'lodash';
import { Col } from 'react-bootstrap';
import styled from 'styled-components';

import { Button } from '@/components/common/buttons/Button';
import { exists } from '@/utils/utils';

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
      <Col xs="auto" className="pr-3">
        {exists(formikProps.errors) && formikProps.dirty && formikProps.submitCount >= 1 && (
          <StyledError>Please check form fields for errors.</StyledError>
        )}
      </Col>
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

const StyledError = styled.div`
  padding-top: 0.7rem;
  color: red;
`;

export default SaveCancelButtons;
