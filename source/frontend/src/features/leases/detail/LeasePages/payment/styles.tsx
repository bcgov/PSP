import { Col } from 'react-bootstrap';
import styled from 'styled-components';

import { Button } from '@/components/common/buttons/Button';
import { InlineFlexDiv } from '@/components/common/styles';
import { Table } from '@/components/Table';

export const StyledFormBody = styled.div`
  margin-left: 1rem;
  font-size: 1.4rem;
  label {
    font-family: 'BCSans-Bold';
  }
  .check-field {
    label {
      width: 100%;
      color: ${props => props.theme.css.textColor};
    }
    display: flex;
    flex-wrap: wrap;
  }
  .form-group {
    display: flex;
    flex-direction: column;
    .small {
      width: 70%;
    }
  }
  grid-template-columns: [controls] 1fr;
  & > .form-label {
    grid-column: controls;
    font-family: 'BcSans-Bold';
  }
  .form-control {
    font-family: 'BcSans';
  }
  .required .form-label:after {
    content: ' *';
  }
`;

export const StyledTable = styled(Table)`
  &.table {
    min-width: 155rem;
    .thead {
      .tr {
        .th {
          padding: 0.5rem 0.5rem;
          font-size: 1.4rem;
          position: relative;
          background-color: ${props => props.theme.css.primaryLightColor};
          color: white;
          .tooltip-icon {
            color: white;
            float: right;
            align-self: baseline;
            margin-left: 0.5rem;
            height: 1.1rem;
            width: 1.1rem;
          }
          .sortable-column {
            width: 100%;
          }
        }
      }
    }
    > .tbody > .tr-wrapper > .tr > .td:nth-of-type(2) {
      margin-right: -5rem; /** TODO: PSP-4403 prevent expander from pushing tds to the right */
      padding-left: 0;
    }
    .td {
      word-break: normal;
    }
    .collapse {
      background-color: ${props => props.theme.css.selectedColor};
      color: ${props => props.theme.css.textColor};
      .receipt {
        color: ${props => props.theme.css.lightVariantColor};
      }
    }
  }
`;

export const StyledPaymentTable = styled(Table)`
  &&.table {
    width: 100%;
    .tbody {
      padding-left: 0;
    }
    .thead {
      .tr {
        .th {
          padding: 0.5rem 0.5rem;
          font-size: 1.4rem;
          position: relative;
          background-color: transparent;
          color: ${props => props.theme.css.formTextColor};
          .tooltip-icon {
            color: ${props => props.theme.css.slideOutBlue};
            float: right;
            align-self: baseline;
            margin-left: 0.5rem;
            height: 1.1rem;
            width: 1.1rem;
          }
          .sortable-column {
            width: 100%;
          }
          .td {
            word-break: normal;
          }
        }
      }
    }
    .tfoot {
      background-color: ${props => props.theme.css.summaryColor};
      border-top: 0.1rem solid ${props => props.theme.css.summaryBorderColor};
      border-bottom: 0.1rem solid ${props => props.theme.css.summaryBorderColor};
      font-family: 'BCSans-Bold';
      color: ${props => props.theme.css.textColor};
    }
  }
`;

export const FullWidthInlineFlexDiv = styled(InlineFlexDiv)`
  justify-content: space-between;
  align-items: end;
  .btn {
    margin: 1rem 0;
  }
`;

export const InlineCol = styled(Col)`
  display: flex;
  gap: 2rem;
  flex-direction: row;
`;

export const ActualPaymentBox = styled(InlineFlexDiv)`
  margin-left: -1rem;
  padding-left: 1rem;
  flex-wrap: wrap;
  border-radius: 0.5rem;
  input {
    width: 100%;
  }
  background-color: ${props => props.theme.css.filterBackgroundColor};
`;

export const FlexRight = styled.div`
  display: flex;
  flex-direction: row;
  width: 100%;
  justify-content: flex-end;
`;

export const FlexRowDiv = styled.div`
  display: flex;
  gap: 1rem;
  flex-direction: row;
`;

export const FlexColDiv = styled.div`
  display: flex;
  gap: 1rem;
  flex-direction: column;
`;

export const WarningTextBox = styled(InlineFlexDiv)`
  gap: 3rem;
  background-color: white;
  min-height: 6rem;
`;

export const AddActualButton = styled(Button)`
  &&& {
    background-color: ${props => props.theme.css.completedColor};
    color: white;
    &:hover {
      background-color: #3aba53;
    }
  }
`;

export const UnOrderedListNoStyle = styled.ul`
  list-style-type: none;
`;
