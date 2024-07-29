import { useCallback, useEffect, useState } from 'react';
import ReactPaginate from 'react-paginate';
import styled from 'styled-components';

export interface ISimplePaginationProps<T extends object> {
  items: T[];
  children?: (props: T) => JSX.Element;
}

export const SimplePagination = <T extends object>(
  props: React.PropsWithChildren<ISimplePaginationProps<T>>,
) => {
  const [currentItemIndex, setCurrentItemIndex] = useState(0);
  const [currentItem, setCurrentItem] = useState<T | null>(null);

  const handleChangePage = useCallback((event: { selected: number }) => {
    const selected = event.selected;
    setCurrentItemIndex(selected);
  }, []);

  useEffect(() => {
    setCurrentItem(props.items[currentItemIndex]);
  }, [currentItemIndex, props.items]);

  const children = props.children;

  return (
    <>
      <StyledWrapper>
        <ReactPaginate
          previousLabel={'<'}
          nextLabel={'>'}
          breakLabel={'...'}
          pageCount={props.items.length}
          marginPagesDisplayed={2}
          pageRangeDisplayed={5}
          onPageChange={handleChangePage}
          //forcePage={pageIndex}
          // css
          activeClassName="active"
          breakClassName="page-item"
          breakLinkClassName="page-link"
          containerClassName="pagination"
          pageClassName="page-item"
          pageLinkClassName="page-link"
          previousClassName="page-item"
          previousLinkClassName="page-link"
          nextClassName="page-item"
          nextLinkClassName="page-link"
        />
      </StyledWrapper>
      <StyledDivider />
      <>{children(currentItem)}</>
    </>
  );
};

export default SimplePagination;

const StyledWrapper = styled.div`
  height: 3.1rem;
`;

const StyledDivider = styled.div`
  margin-top: 0.5rem;
  margin-bottom: 0.5rem;
  border-bottom-style: solid;
  border-bottom-color: grey;
  border-bottom-width: 0.1rem;
`;
