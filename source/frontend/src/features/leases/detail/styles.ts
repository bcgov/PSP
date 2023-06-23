import { Form } from 'react-bootstrap';
import styled from 'styled-components';

import caretRightSvgUrl from '@/assets/images/caret-right.svg';
import { TextArea } from '@/components/common/form';
import { InlineFastCurrencyInput, InlineInput } from '@/components/common/form/styles';
import { InlineFlexDiv } from '@/components/common/styles';
import { Table } from '@/components/Table';

export const LeaseH1 = styled.h1`
  padding: 2rem;
  font-family: 'BCSans-Bold';
`;

export const LeaseH2 = styled.h2`
  font-family: 'BCSans-Bold';
  font-size: 3.2rem;
  line-height: 4.2rem;
  text-align: left;
  color: ${props => props.theme.css.textColor};
  border-bottom: solid 0.4rem ${props => props.theme.css.primaryColor};
`;

export const LeaseH3 = styled.h3`
  font-family: 'BcSans-Bold';
  font-size: 2rem;
  margin-bottom: 1rem;
  text-align: left;
  padding: 1rem 0 0.5rem 0;
  color: ${props => props.theme.css.textColor};
  border-bottom: solid 0.3rem ${props => props.theme.css.primaryColor};
`;

export const LeaseH4 = styled.h4`
  font-size: 1.8rem;
  color: white;
  text-align: center;
  padding-bottom: 1rem;
`;

export const LeaseH5 = styled.h5`
  font-size: 1.6rem;
  color: black;
  text-align: left;
  padding-top: 1.5rem;
  font-family: BcSans-Bold;
`;

export const LeaseH6 = styled.h6`
  font-size: 2rem;
  color: ${props => props.theme.css.textColor};
  text-align: left;
  font-family: BcSans-Bold;
`;

export const LeaseHeaderRight = styled.div`
  max-width: 60%;
  margin: 0 auto;
  display: flex;
  flex-direction: column;
  align-items: flex-start;
  text-align: left;
  @media only screen and (max-width: 1250px) {
    align-items: center;
    text-align: center;
  }
`;

export const LeaseHeaderText = styled(InlineFlexDiv)`
  justify-content: center;
  display: flex;
  padding: 0 2rem;
  white-space: nowrap;
`;

export const ExpiredWarning = styled(InlineFlexDiv)`
  color: ${props => props.theme.css.dangerColor};
  background-color: ${props => props.theme.css.dangerBackgroundColor};
  border-radius: 0.4rem;
  letter-spacing: 0.1rem;
  padding: 0.2rem;
  margin-right: 0.5rem;
  font-family: 'BCSans-Bold';
  font-size: 1.4rem;
  align-items: center;
`;

export const LeaseHeader = styled(InlineFlexDiv)`
  border-radius: 1rem 1rem 0 0;
  grid-area: leaseheader;
  background-color: ${props => props.theme.css.slideOutBlue};
  color: white;
  align-items: center;
  justify-content: center;
  label {
    padding-right: 1rem;
    margin: 0;
  }
  h1 {
    min-width: 23rem;
  }

  && .btn.btn-link {
    color: white;
    &:hover {
      color: white;
    }
  }

  @media only screen and (max-width: 1250px) {
    flex-direction: column;
    gap: 0.5rem;
  }
`;

export const FormDescriptionLabel = styled(Form.Label)`
  font-size: 1.6rem;
  font-family: 'BCSans-Bold';
`;

export const FormDescriptionBody = styled(TextArea)`
  font-family: 'BCSans';
`;

export const FormControl = styled(Form.Control)`
  grid-column: controls;
  grid-row: auto;
  border-left: 1px solid black !important;
  border-radius: 0;
`;

export const FieldValue = styled.p`
  grid-column: controls;
  border-left: 1px solid black !important;
  padding: 0.6rem 1.2rem;
  color: #495057;
  font-family: 'BCSans-Bold';
`;

export const FormGrid = styled.div`
  display: grid;
  grid-template-columns: [labels] minMax(min-content, 1fr) [controls] 5fr;
  grid-auto-flow: row;
  .form-label,
  .form-group {
    margin: 0 0.5rem 0 0;
  }

  & > .form-group,
  & > .notes {
    grid-column: controls;
    grid-row: auto;
    border-left: 1px solid #666666;
  }

  & .form-control:not(.description, .notes) {
    font-family: 'BCSans-Bold';
  }

  & > label,
  & > fieldset {
    grid-column: labels;
    grid-row: auto;
  }

  & > .textarea :not(.notes),
  & > h3,
  & > h5 {
    grid-column: span 2;
    border-left: none;
  }
  .form-label {
    display: flex;
    align-items: center;
  }
`;

export const LeftInlineFastCurrencyInput = styled(InlineFastCurrencyInput)`
  input.form-control {
    text-align: left;
  }
`;

export const TableHeadFields = styled(InlineFlexDiv)`
  gap: 5rem;
  .form-control:disabled {
    border: none;
    background: none;
  }
`;

export const TermsTable = styled(Table)`
  background-color: white;
  padding-left: 8rem;
  &.table .thead .th {
    font-family: 'BCSans-Bold';
    border-top: none;
    padding: 1rem 0.5rem;
    background-color: white;
    font-size: 14px;
  }
  &.table .tbody .td {
    border: none;
    font-size: 14px;
  }
  &.table .tbody .tr-wrapper:nth-child(even) {
    background-color: #f2f2f2;
  }
  &.table .tbody .tr-wrapper .tr.selected {
    background-color: ${props => props.theme.css.accentColor};
    font-family: 'BCSans-Bold';
    margin-left: -8rem;
    &:before {
      content: 'current';
      padding-right: 1.5rem;
      background-color: ${props => props.theme.css.accentColor};
      background-image: url(${caretRightSvgUrl});
      background-repeat: no-repeat;
      background-position: right;
      width: 8rem;
      display: flex;
      align-items: center;
      justify-content: flex-end;
    }
  }
` as typeof Table;

export const SpacedInlineListItem = styled.li`
  column-gap: 5rem;
  display: flex;
  flex-wrap: nowrap;
`;

export const NestedInlineField = styled(InlineInput)`
  padding: 0.6rem 1.2rem;
  display: flex;
  .form-label {
    min-width: 7rem;
  }
`;

export const SectionHeader = styled(FormDescriptionLabel)`
  color: ${props => props.theme.css.primaryColor};
  font-size: 1.8rem;
  margin-bottom: 2rem;
`;
