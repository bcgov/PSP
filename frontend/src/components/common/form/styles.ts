import { Col } from 'react-bootstrap';
import styled from 'styled-components';

import { FastCurrencyInput, Form, Input, Select } from '.';
import { FastDatePicker } from './FastDatePicker';

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

export const InlineSelect = styled(Select)`
  display: flex;
  align-items: baseline;
  gap: 0.8rem;
  .form-label {
    flex-shrink: 0;
  }
`;

export const InlineCol = styled(Col)`
  display: flex;
  align-items: baseline !important;
  gap: 2rem;
  flex-direction: row;
`;

export const InlineFastDatePicker = styled(FastDatePicker)`
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
