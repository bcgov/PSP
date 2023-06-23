import { Form as FormikForm } from 'formik';
import styled from 'styled-components';

import { Form as FormBase } from '@/components/common/form';
import { FlexBox } from '@/components/common/styles';

// common ui styling
export * from '@/features/contacts/styles';

export const H2 = styled.h2`
  font-size: 2rem;
  font-weight: 700;
  color: ${props => props.theme.css.primaryColor};
  text-decoration: none solid rgb(0, 51, 102);
  line-height: 2.9rem;
`;

export const H3 = styled.h3`
  font-size: 1.6rem;
  font-weight: 700;
  color: ${props => props.theme.css.formTextColor};
  text-decoration: none solid rgb(33, 37, 41);
  line-height: 2rem;
`;

export const CreateFormLayout = styled(FlexBox).attrs({ column: true })`
  width: 100%;
  overflow: auto;
  padding-right: 1rem;
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

export const FormLabel = styled(FormBase.Label)`
  font-size: 1.6rem;
  font-weight: 700;
  color: ${props => props.theme.css.formTextColor};
  text-decoration: none solid rgb(33, 37, 41);
  line-height: 2rem;
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

export const SubtleText = styled.span`
  font-size: 1.6rem;
  font-weight: 400;
  color: ${props => props.theme.css.subtleColor};
  text-decoration: none solid rgb(170, 170, 170);
  line-height: 2rem;
`;

export const ButtonGroup = styled(FlexBox)`
  width: 100%;
  padding: 1rem 4rem;
  gap: 2rem;
  justify-content: flex-end;
  align-items: stretch;
`;
