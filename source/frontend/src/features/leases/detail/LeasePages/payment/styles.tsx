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
      color: ${props => props.theme.bcTokens.typographyColorSecondary};
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
  .required .datepicker-label:after {
    content: ' *';
  }
`;

export const StyledPaymentTable = styled(Table)`
  &&.table {
    width: 100%;
    .thead {
      .tr {
        .th {
          padding: 0.5rem 0.5rem;
          font-size: 1.4rem;
          position: relative;
          background-color: transparent;
          color: ${props => props.theme.bcTokens.typographyColorPrimary};
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
      background-color: ${props => props.theme.css.warningBackgroundColor};
      border-top: 0.1rem solid ${props => props.theme.bcTokens.themeGold100};
      border-bottom: 0.1rem solid ${props => props.theme.bcTokens.themeGold100};
      font-family: 'BCSans-Bold';
      color: ${props => props.theme.bcTokens.typographyColorSecondary};
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
  background-color: ${props => props.theme.css.highlightBackgroundColor};
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
    background-color: ${props => props.theme.bcTokens.iconsColorSuccess};
    color: white;
    min-height: 5rem;
    &:hover {
      background-color: #3aba53;
    }
  }
`;

export const UnOrderedListNoStyle = styled.ul`
  list-style-type: none;
`;
