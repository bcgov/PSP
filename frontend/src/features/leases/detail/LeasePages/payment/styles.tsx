import { Table } from 'components/Table';
import { ILeaseTerm } from 'interfaces/ILeaseTerm';
import styled from 'styled-components';

export const StyledFormBody = styled.div`
  margin-left: 1rem;
  font-size: 1.4rem;
  .form-group {
    display: flex;
    flex-direction: column;
    input {
      width: 70%;
    }
  }
  row-gap: 0.5rem;
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
  }
`;
