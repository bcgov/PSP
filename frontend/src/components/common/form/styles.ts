import styled from 'styled-components';

import { Form, Input } from '.';

export const InlineForm = styled(Form)`
  font-size: 1.44rem;
  display: flex;
  align-items: baseline;
  gap: 0.8rem;
`;

export const InlineInput = styled(Input)`
  display: flex;
  align-items: baseline;
  gap: 0.8rem;
  .form-label {
    flex-shrink: 0;
  }
`;
