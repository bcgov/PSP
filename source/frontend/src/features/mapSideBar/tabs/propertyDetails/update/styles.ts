import styled from 'styled-components';

export const StyledTable = styled.div`
  &.table {
    .no-rows-message {
      align-items: flex-start;
    }
    .tbody {
      .tr {
        display: flex;
        flex: 1 0 auto;
        min-width: 60px;

        .td {
          display: flex;
          flex: 100 0 auto;
          box-sizing: border-box;
          min-height: 3.6rem;
          min-width: 30px;
          width: 100px;
          flex-wrap: wrap;
          align-items: center;

          &.right {
            justify-content: right;
            text-align: right;
          }
          &.left {
            justify-content: left;
            text-align: left;
          }

          input {
            max-width: 12.5rem;
          }
        }
      }
    }
  }
`;
