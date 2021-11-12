import variables from '_variables.module.scss';
import * as React from 'react';
import { IoMdArrowDropdown, IoMdArrowDropup } from 'react-icons/io';
import styled from 'styled-components';

import { ColumnInstanceWithProps } from '.';

interface IColumnSortProps<T extends object = {}> {
  column: ColumnInstanceWithProps<T>;
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
  active?: boolean;
}

const Down = styled(IoMdArrowDropdown)<Props>`
  color: ${props => (props.active ? variables.activeColor : undefined)};
  width: 1.6rem;
  height: 1.6rem;
`;

const Up = styled(IoMdArrowDropup)<Props>`
  color: ${props => (props.active ? variables.activeColor : undefined)};
  width: 1.6rem;
  height: 1.6rem;
`;

function ColumnSort<T extends object = {}>({ column, onSort }: IColumnSortProps<T>) {
  if (!column.sortable) {
    return null;
  }

  return (
    <Wrapper onClick={onSort}>
      {column.isSorted && !column.isSortedDesc && <Up active={true} />}
      {column.isSorted && column.isSortedDesc && <Down active={true} />}

      {!column.isSorted && (
        <>
          <Up style={{ marginBottom: -8 }} />
          <Down />
        </>
      )}
    </Wrapper>
  );
}

export default ColumnSort;
