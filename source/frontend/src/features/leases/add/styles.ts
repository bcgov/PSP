import styled from 'styled-components';

import { TextArea } from '@/components/common/form';

export const FormButtons = styled.div`
  z-index: 100;
  width: 100%;
  padding: 0.5rem 0;
  display: flex;
  justify-content: flex-end;
  gap: 1rem;
  .btn {
    min-height: 3rem;
  }
  position: sticky;
  bottom: 0rem;
  background-color: white;
`;

export const MediumTextArea = styled(TextArea)`
  textarea.form-control {
    min-width: 80rem;
    height: 7rem;
    resize: none;
  }
`;
