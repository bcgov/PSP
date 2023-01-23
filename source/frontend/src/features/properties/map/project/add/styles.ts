import { TextArea } from 'components/common/form';
import styled from 'styled-components';

export const MediumTextArea = styled(TextArea)`
  textarea.form-control {
    min-width: 80rem;
    height: 7rem;
    resize: none;
  }
`;

export const LargeTextArea = styled(TextArea)`
  textarea.form-control {
    min-width: 80rem;
    height: 18rem;
    width: 80rem;
    resize: none;
  }
`;
