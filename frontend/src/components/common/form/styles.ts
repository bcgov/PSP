import styled from 'styled-components';

import { Form, Input } from '.';

export const InlineForm = styled(Form)`
  font-size: 0.9rem;
  display: flex;
  align-items: baseline;
  gap: 0.5rem;
`;

export const InlineInput = styled(Input)`
  display: flex;
  align-items: baseline;
  gap: 0.5rem;
  .form-label {
    flex-shrink: 0;
  }
`;
