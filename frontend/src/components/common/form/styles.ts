import styled from 'styled-components';

import { FastCurrencyInput, Form, Input } from '.';

export const InlineForm = styled(Form)`
  font-size: 1.44rem;
  display: flex;
  align-items: baseline;
  gap: 0.8rem;
`;

export const InlineFastCurrencyInput = styled(FastCurrencyInput)`
  display: flex;
  align-items: baseline;
  gap: 0.8rem;
  .form-label {
    flex-shrink: 0;
  }
`;

export const InlineInput = styled(Input)`
  display: flex;
  align-items: baseline;
  gap: 0.8rem;
  .form-label {
    flex-shrink: 0;
  }
`;

export const FormSection = styled.div`
  border-radius: 1rem;
  width: 100%;
  padding: 2.5rem;
  background-color: ${props => props.theme.css.filterBackgroundColor};
`;
