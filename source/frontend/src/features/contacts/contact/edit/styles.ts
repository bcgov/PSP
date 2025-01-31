import { Form as FormikForm } from 'formik';
import styled from 'styled-components';

import { FlexBox } from '@/components/common/styles';

// common ui styling
export * from '@/features/contacts/styles';

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

export const SectionMessage = styled(FlexBox)<{ appearance: 'information' | 'error' }>`
  color: ${({ appearance = 'information' }) => (appearance === 'error' ? '#d8292f' : '#494949')};
  font-size: 1.6rem;
  text-decoration: none;
`;

export const ButtonGroup = styled(FlexBox)`
  width: 100%;
  padding: 1rem 4rem;
  gap: 2rem;
  justify-content: flex-end;
  align-items: stretch;
`;
