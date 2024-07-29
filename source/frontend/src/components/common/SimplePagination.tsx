import './SimplePagination.scss';

import { useCallback, useEffect, useState } from 'react';
import { MdArrowLeft, MdArrowRight } from 'react-icons/md';
import ReactPaginate from 'react-paginate';
import styled from 'styled-components';

export interface ISimplePaginationProps<T extends object> {
  items: T[];
  children?: (item: T) => JSX.Element;
}

export const SimplePagination = <T extends object>(props: ISimplePaginationProps<T>) => {
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
      <ReactPaginate
        previousLabel={<ArrowLeftIcon />}
        nextLabel={<ArrowRightIcon />}
        breakLabel={'...'}
        pageCount={props.items.length}
        marginPagesDisplayed={0}
        pageRangeDisplayed={0}
        onPageChange={handleChangePage}
        // css
        activeClassName="simple-pagination-active"
        breakClassName="simple-pagination-break"
        breakLinkClassName="simple-pagination-break-link"
        containerClassName="simple-pagination-container"
        pageClassName="simple-pagination-page"
        pageLinkClassName="simple-pagination-page-link"
        previousClassName="simple-pagination-previous"
        previousLinkClassName="simple-pagination-previous-link"
        nextClassName="simple-pagination-next"
        nextLinkClassName="simple-pagination-next-link"
        pageLabelBuilder={currentPage => (
          <>
            <strong>{currentPage}</strong> of <span>{props.items.length}</span>
          </>
        )}
      />
      <StyledDivider />
      <>{children(currentItem)}</>
    </>
  );
};

export default SimplePagination;

export const ArrowLeftIcon = styled(MdArrowLeft)`
  float: right;
  cursor: pointer;
  padding: 0rem;
  margin: 0rem;
`;
export const ArrowRightIcon = styled(MdArrowRight)`
  float: right;
  cursor: pointer;
`;

export const StyledDivider = styled.div`
  margin-bottom: 1rem;
  border-bottom-style: solid;
  border-bottom-color: grey;
  border-bottom-width: 0.1rem;
`;
