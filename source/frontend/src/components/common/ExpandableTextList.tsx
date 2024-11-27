import { Fragment, ReactElement, useState } from 'react';

import { exists } from '@/utils/utils';

import { LinkButton } from './buttons';

export interface IExpandableTextListProps<T> {
  items: T[];
  renderFunction: (item: T, index?: number) => ReactElement;
  keyFunction: (item: T, index: number) => string;
  delimiter?: ReactElement | string;
  maxCollapsedLength?: number;
  className?: string;
  moreText?: string;
  hideText?: string;
}

/**
 * Generic component that allows a list of items to be expanded based on a predefined list size.
 * @param {IExpandableTextListProps} param0
 */
export function ExpandableTextList<T>({
  items,
  keyFunction,
  renderFunction,
  delimiter,
  maxCollapsedLength,
  moreText,
  hideText,
}: IExpandableTextListProps<T>) {
  const [isExpanded, setIsExpanded] = useState(false);
  const displayedItemsLength = !isExpanded ? maxCollapsedLength ?? items.length : items.length;
  const displayedItems = items.slice(0, displayedItemsLength);
  return (
    <Fragment>
      {displayedItems.map((item: T, index: number) => (
        <span key={keyFunction(item, index)}>
          {renderFunction(item, index)}
          {index < items.length - 1 && delimiter}
        </span>
      ))}
      {exists(maxCollapsedLength) && maxCollapsedLength < items.length && (
        <LinkButton data-testid="expand" onClick={() => setIsExpanded(collapse => !collapse)}>
          {isExpanded
            ? hideText ?? 'hide'
            : `[+${items.length - displayedItemsLength} ${moreText ?? 'more...'}]`}
        </LinkButton>
      )}
    </Fragment>
  );
}

export default ExpandableTextList;
