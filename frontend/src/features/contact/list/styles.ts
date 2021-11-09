import { Check, SearchButton } from 'components/common/form';
import { InlineForm, InlineInput } from 'components/common/form/styles';
import { Scrollable as ScrollableBase } from 'components/common/Scrollable/Scrollable';
import Button from 'react-bootstrap/Button';
import styled from 'styled-components';

export * from 'components/common/form/styles';

export const ListPage = styled.div`
  display: flex;
  flex-direction: column;
  flex-grow: 1;
  width: 100%;
  gap: 2.5rem;
  padding: 0;
  font-size: 14px;
  .table .tr .td:first-of-type,
  .table .tr .td:nth-of-type(2),
  .table .tr .th:first-of-type,
  .table .tr .th:nth-of-type(2) {
    border-left: 0;
    border-right: 0;
    padding: 0;
  }
  h3 {
    margin-bottom: 1.75rem;
  }
  .btn {
    min-height: unset;
  }
`;

export const Scrollable = styled(ScrollableBase)`
  padding: 1.6rem 3.2rem;
  width: 100%;
`;

export const PageHeader = styled.h3`
  text-align: left;
`;

export const PageToolbar = styled.div`
  display: flex;
  flex-direction: row;
  align-items: baseline;
  padding: 0;
`;

export const Spacer = styled.div`
  display: flex;
  flex: 1 1 auto;
`;

export const FilterBox = styled(InlineForm)`
  background-color: ${({ theme }) => theme.css.filterBoxColor};
  flex: 1 1 auto;
  padding: 0.5rem 1.5rem;
  padding-bottom: 0;
  .form-check-label {
    display: flex;
    p {
      margin-left: 1rem;
    }
  }
`;

export const FilterCheck = styled(Check)`
  .check-field {
    padding: 0;
    .form-check {
      margin-right: 0;
    }
  }
`;

export const PrimaryButton = styled(Button)`
  padding: 0.4rem 0.6rem;
  white-space: nowrap;
  gap: 1rem;
`;

export const SmallSearchButton = styled(SearchButton)`
  min-height: 2rem;
  white-space: nowrap;
  gap: 1rem;
`;

export const LongInlineInput = styled(InlineInput)`
  flex: 3 1 auto;
  max-width: 31rem;
`;

export const ShortInlineInput = styled(InlineInput)`
  flex: 1 4 auto;
  max-width: 20rem;
`;

export const FileIcon = styled(Button)`
  background-color: #fff !important;
  color: ${({ theme, disabled }) =>
    disabled ? theme.css.disabledColor : theme.css.primaryColor} !important;
  padding: 6px 0.5rem;
`;
