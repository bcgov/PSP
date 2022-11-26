import React from 'react';
import { FaSearch } from 'react-icons/fa';

import TooltipWrapper from '../TooltipWrapper';
import { Button, ButtonProps } from '.';

/**
 * SearchButton displaying a magnifying glass icon, used to initiate search/filter actions.
 * @param param0
 */
export const SearchButton: React.FC<React.PropsWithChildren<ButtonProps>> = ({ ...props }) => {
  return (
    <TooltipWrapper toolTipId="map-filter-search-tooltip" toolTip="Search">
      <Button
        id="search-button"
        type={props.type ?? 'submit'}
        data-testid="search"
        title="search"
        className={props.className ?? 'primary'}
        {...props}
        icon={<FaSearch size={20} />}
      />
    </TooltipWrapper>
  );
};
