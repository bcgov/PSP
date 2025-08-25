import React from 'react';
import { FaSearch } from 'react-icons/fa';
import styled from 'styled-components';

import { Button } from '@/components/common/buttons/Button';
import TooltipWrapper from '@/components/common/TooltipWrapper';

const SearchIcon = styled(FaSearch)`
  font-size: 3rem;
`;

export type ISearchControl = {
  /** whether the button should be displayed as active  */
  active?: boolean;
  /** set the slide out as open or closed */
  onToggle: () => void;
};

/**
 * Component to display the layers control on the map
 * @example ./LayersControl.md
 */
const SearchControl: React.FC<React.PropsWithChildren<ISearchControl>> = ({ active, onToggle }) => {
  return (
    <TooltipWrapper tooltipId="search-control-id" tooltip="Search Controls">
      <SearchButton
        id="searchControlButton"
        variant="outline-secondary"
        $active={active}
        onClick={onToggle}
      >
        <SearchIcon />
      </SearchButton>
    </TooltipWrapper>
  );
};

export default SearchControl;

const SearchButton = styled(Button)<{ $active?: boolean }>`
  &.btn {
    width: 5.2rem;
    height: 5.2rem;
    background-color: ${({ theme, $active }) =>
      $active ? theme.bcTokens.surfaceColorPrimaryButtonDefault : '#FFFFFF'};
    color: ${({ theme, $active }) =>
      $active ? '#FFFFFF' : theme.bcTokens.surfaceColorPrimaryButtonDefault};
    border-color: ${({ theme }) => theme.bcTokens.surfaceColorPrimaryButtonDefault};
    box-shadow: -0.2rem 0.1rem 0.4rem rgba(0, 0, 0, 0.2);

    &:hover {
      opacity: 1;
    }
  }
`;
