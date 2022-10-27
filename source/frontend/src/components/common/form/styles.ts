import { Col } from 'react-bootstrap';
import styled, { css } from 'styled-components';

import { FastCurrencyInput, Form, Input, Select } from '.';
import { FastDatePicker } from './FastDatePicker';

export const InlineForm = styled(Form)`
  font-size: 1.44rem;
  display: flex;
  align-items: baseline;
  gap: 0.8rem;
`;

export const InlineFilterBox = styled.div`
  font-size: 1.44rem;
  display: flex;
  align-items: baseline;
  gap: 0.8rem;
`;

export const SaveTableWrapper = styled.div`
  margin-top: 3rem;
  font-size: 1.4rem;
  .table .tr .td:first-of-type,
  .table .tr .td:nth-of-type(2),
  .table .tr .th:first-of-type,
  .table .tr .th:nth-of-type(2) {
    border-left: 0;
    border-right: 0;
    padding: 0;
  }
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

export const FormSectionClear = styled.div`
  border-radius: 1rem;
  width: 100%;
  padding-top: 1rem;
  padding-left: 2.5rem;
  padding-bottom: 1rem;
`;

// These are common base styles to apply to every form.
// For now it is opt-in but should be refactored to include on all forms by default
export const sharedFormStyles = css`
  .form-control {
    &.is-invalid {
      border: 2px solid #d8292f;
    }
  }

  .invalid-feedback {
    color: #d8292f;
  }

  .form-group {
    label {
      font-size: 1.6rem;
      font-weight: 700;
      color: ${props => props.theme.css.formTextColor};
      text-decoration: none solid rgb(33, 37, 41);
      line-height: 2rem;
    }

    input,
    select,
    textarea {
      border: 2px solid #606060;
      border-radius: 4px;
      background-color: #ffffff;
      font-size: 1.6rem;
      color: #000000;
      text-decoration: none;

      &.form-control {
        &:disabled,
        &[readonly] {
          background-color: ${props => props.theme.css.disabledFieldBackgroundColor};
          opacity: 1;
        }
      }
    }

    &.required {
      label:after {
        content: ' *';
        color: red;
      }
    }
  }
`;
