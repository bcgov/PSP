import { PropertyFilterOptions } from 'features/properties/filter';
import styled from 'styled-components';

import { Form, Input } from '.';
import { FastDatePicker } from './FastDatePicker';

export const InlineForm = styled(Form)`
  font-size: 0.9rem;
  display: flex;
  align-items: baseline;
  column-gap: 0.5rem;
  white-space: nowrap;
  flex-wrap: wrap;
  .form-group {
    margin-bottom: 0;
  }
`;

export const InlineInput = styled(Input)`
  display: flex;
  align-items: baseline;
  column-gap: 0.5rem;
  .form-label {
    flex-shrink: 0;
  }
`;

export const InlineDate = styled(FastDatePicker)`
  display: flex;
  align-items: baseline;
  column-gap: 0.5rem;
  .form-label {
    flex-shrink: 0;
  }
`;

export const StackedInlineForm = styled(Form)`
  font-size: 0.9rem;
  display: flex;
  align-items: baseline;
  column-gap: 0.5rem;
  white-space: nowrap;
  flex-wrap: wrap;
  .form-group {
    margin-bottom: 0;
    display: flex;
    flex-direction: column;
  }
`;

export const StackedPropertyFilterOptions = styled(PropertyFilterOptions)`
  display: flex;
  flex-direction: column;
  align-items: baseline;
  .label {
    font-weight: 700;
    width: 100%;
    margin-bottom: 0.5rem;
  }
  .input-group-content .form-control {
    width: 10rem;
  }
  flex-wrap: wrap;
`;

export const StackedInput = styled(Input)`
  display: flex;
  align-items: baseline;
  column-gap: 0.5rem;
  .form-label {
    flex-shrink: 0;
  }
`;

export const StackedDate = styled(FastDatePicker)`
  display: flex;
  align-items: baseline;
  column-gap: 0.5rem;
  .form-label {
    flex-shrink: 0;
  }
  .form-control {
    width: 8rem;
  }
`;
