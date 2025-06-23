import L from 'leaflet';
import React, { useEffect } from 'react';
import { FaSearch } from 'react-icons/fa';
import styled from 'styled-components';

import { Button } from '@/components/common/buttons/Button';
import TooltipWrapper from '@/components/common/TooltipWrapper';

import Control from '../Control';

const SearchIcon = styled(FaSearch)`
  font-size: 3rem;
`;

export type ISearchControl = {
  /** set the slide out as open or closed */
  onToggle: () => void;
};

/**
 * Component to display the layers control on the map
 * @example ./LayersControl.md
 */
const LayersControl: React.FC<React.PropsWithChildren<ISearchControl>> = ({ onToggle }) => {
  useEffect(() => {
    const elem = L.DomUtil.get('layersContainer');
    if (elem) {
      L.DomEvent.on(elem, 'mousewheel', L.DomEvent.stopPropagation);
    }
  });

  return (
    <Control position="topright">
      <TooltipWrapper tooltipId="search-control-id" tooltip="Search Controls">
        <SearchButton id="searchControlButton" variant="outline-secondary" onClick={onToggle}>
          <SearchIcon />
        </SearchButton>
      </TooltipWrapper>
    </Control>
  );
};

export default LayersControl;

const SearchButton = styled(Button)`
  &.btn {
    width: 5.2rem;
    height: 5.2rem;
    margin-left: -5.1rem;
    background-color: #fff;
    color: ${({ theme }) => theme.bcTokens.surfaceColorPrimaryButtonDefault};
    border-color: ${({ theme }) => theme.bcTokens.surfaceColorPrimaryButtonDefault};
    box-shadow: -0.2rem 0.1rem 0.4rem rgba(0, 0, 0, 0.2);
  }
`;
