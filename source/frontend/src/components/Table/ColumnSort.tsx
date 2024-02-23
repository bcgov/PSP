import { has } from 'lodash';
import { IoMdArrowDropdown, IoMdArrowDropup } from 'react-icons/io';
import styled from 'styled-components';

import variables from '@/assets/scss/_variables.module.scss';

import { ColumnInstanceWithProps } from '.';
import { TableSort } from './TableSort';

interface IColumnSortProps<T extends object = object> {
  column: ColumnInstanceWithProps<T>;
  sort?: TableSort<T>;
  onSort: () => void;
}

const Wrapper = styled('div')`
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  margin-left: 0.5rem;
`;

interface Props {
  $active?: boolean;
}

const Down = styled(IoMdArrowDropdown)<Props>`
  color: ${props => (props.$active ? variables.activeColor : undefined)};
  width: 1.6rem;
  height: 1.6rem;
`;

const Up = styled(IoMdArrowDropup)<Props>`
  color: ${props => (props.$active ? variables.activeColor : undefined)};
  width: 1.6rem;
  height: 1.6rem;
`;

function ColumnSort<T extends object = object>({ column, onSort, sort }: IColumnSortProps<T>) {
  if (!column.sortable) {
    return null;
  }

  const overrideSort = has(sort, column.id);
  return (
    <Wrapper onClick={onSort} data-testid={`sort-column-${column.id}`}>
      {overrideSort && column.isSorted && !column.isSortedDesc && <Up $active={true} />}
      {overrideSort && column.isSorted && column.isSortedDesc && <Down $active={true} />}

      {(!column.isSorted || !overrideSort) && (
        <>
          <Up style={{ marginBottom: -8 }} />
          <Down />
        </>
      )}
    </Wrapper>
  );
}

export default ColumnSort;
