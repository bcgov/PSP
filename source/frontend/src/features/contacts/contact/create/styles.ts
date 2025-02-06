import { Form as FormikForm } from 'formik';
import styled from 'styled-components';

import { FlexBox } from '@/components/common/styles';

export const CreateFormLayout = styled(FlexBox).attrs({ column: true })`
  width: 100%;
  overflow: auto;
`;

export const Form = styled(FormikForm)`
  .form-group {
    &.required {
      label:after {
        content: ' *';
        color: red;
      }
    }
  }
`;

export const ErrorMessage = styled(FlexBox)`
  width: auto;
  color: #d8292f;
  font-size: 1.6rem;
  align-items: center;
`;

export const ButtonGroup = styled(FlexBox)`
  width: 100%;
  padding: 1rem 3rem;
  gap: 2rem;
  justify-content: flex-end;
  align-items: stretch;
`;
