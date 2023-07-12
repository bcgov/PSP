import { Col } from 'react-bootstrap';
import styled from 'styled-components';

import { Button } from '@/components/common/buttons/Button';
import { FastDatePicker, TextArea } from '@/components/common/form';
import { InlineInput, InlineSelect } from '@/components/common/form/styles';

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
