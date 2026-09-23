import React from 'react';
import { FaSearch } from 'react-icons/fa';

import TooltipWrapper from '../TooltipWrapper';
import { Button, ButtonProps } from '.';

interface SearchButtonProps extends ButtonProps {
  dataTestId?: string;
}

/**
 * SearchButton displaying a magnifying glass icon, used to initiate search/filter actions.
 * @param param0
 */
export const SearchButton: React.FC<React.PropsWithChildren<SearchButtonProps>> = ({
  dataTestId,
  ...props
}) => {
  return (
    <TooltipWrapper tooltipId="map-filter-search-tooltip" tooltip="Search">
      <Button
        id="search-button"
        type={props.type ?? 'submit'}
        data-testid={dataTestId ?? 'search'}
        title="search"
        className={props.className ?? 'primary'}
        {...props}
        icon={<FaSearch size={20} />}
      />
    </TooltipWrapper>
  );
};
