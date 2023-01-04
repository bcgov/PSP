import { ReactElement, useState } from 'react';

import { LinkButton } from './buttons';

export interface IExpandableTextListProps<T> {
  items: T[];
  renderFunction: (item: T) => ReactElement;
  keyFunction: (item: T, index: number) => string;
  delimiter?: ReactElement | string;
  maxCollapsedLength?: number;
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
}: IExpandableTextListProps<T>) {
  const [isExpanded, setIsExpanded] = useState(false);
  const displayedItemsLength = !isExpanded ? maxCollapsedLength ?? items.length : items.length;
  const displayedItems = items.slice(0, displayedItemsLength);
  return (
    <div>
      {displayedItems.map((item: T, index: number) => (
        <span key={keyFunction(item, index)}>
          {renderFunction(item)}
          {index < items.length - 1 && delimiter}
        </span>
      ))}
      {!!maxCollapsedLength && maxCollapsedLength < items.length && (
        <LinkButton data-testid="expand" onClick={() => setIsExpanded(collapse => !collapse)}>
          {isExpanded ? 'hide' : `[+${items.length - displayedItemsLength} more...]`}
        </LinkButton>
      )}
    </div>
  );
}

export default ExpandableTextList;
