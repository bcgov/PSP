import { Button } from 'components/common/buttons/Button';
import { FastDatePicker, Form, TextArea } from 'components/common/form';
import { InlineInput, InlineSelect } from 'components/common/form/styles';
import { Col } from 'react-bootstrap';
import styled from 'styled-components';

export const PropertyCol = styled(Col)`
  display: flex;
  div {
    align-items: baseline !important;
  }
  gap: 3rem;
  .form-label {
    min-width: 0 !important;
  }
`;

export const RemoveButton = styled(Button)`
  &&.btn {
    padding: 0;
    color: ${props => props.theme.css.formBackgroundColor};
    &:hover {
      color: red;
    }
  }
`;

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

export const LargeInlineInput = styled(InlineInput)`
  input.form-control {
    min-width: 50rem;
    max-width: 50rem;
  }
`;

export const SmallInlineInput = styled(InlineInput)`
  input.form-control {
    min-width: 12rem;
    max-width: 12rem;
  }
`;

export const SmallInlineSelect = styled(InlineSelect)`
  select.form-control {
    min-width: 12rem;
    max-width: 12rem;
  }
`;

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

export const StackedDatePicker = styled(FastDatePicker)`
  display: flex;
  flex-direction: column;
`;

export const EndAlignCol = styled(Col)`
  display: flex;
  align-items: flex-end;
`;

export const LeaseForm = styled(Form)`
  padding: 0 2.5rem;
  height: 100%;
  overflow-y: auto;
  text-align: left;
  .required .form-label:after {
    content: ' *';
  }
  .row {
    padding: 0.5rem 0;
  }
  .form-label {
    font-weight: 700;
    color: ${props => props.theme.css.textColor};
  }
  .row .col:first-of-type .form-label {
    min-width: 13.5rem;
  }
  .form-control {
    min-width: 25rem;
    max-width: 25rem;
  }
  .col {
    align-items: end;
    width: 100%;
  }
`;
