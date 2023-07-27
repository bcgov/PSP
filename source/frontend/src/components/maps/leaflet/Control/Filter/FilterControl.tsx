import clsx from 'classnames';
import React from 'react';
import { FiMapPin } from 'react-icons/fi';
import styled from 'styled-components';

import { Button } from '@/components/common/buttons/Button';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import TooltipWrapper from '@/components/common/TooltipWrapper';

import Control from '../Control';
import { FilterContainer } from './FilterContainer';

const FilterButton = styled(Button as any)`
  &.btn {
    width: 5.2rem;
    height: 5.2rem;
    top: 0;

    background-color: #fff;
    color: ${({ theme }) => theme.css.slideOutBlue};
    border-color: ${({ theme }) => theme.css.slideOutBlue};
    box-shadow: -0.2rem 0.1rem 0.4rem rgba(0, 0, 0, 0.2);
    &.open {
      border-top-right-radius: 0;
      border-bottom-right-radius: 0;
      top: 2.6rem;
    }
  }
`;

export type IFilterControlProps = {};

export const FilterControl: React.FC<React.PropsWithChildren<IFilterControlProps>> = () => {
  const target = React.useRef(null);

  const { toggleMapFilter, isFiltering } = useMapStateMachine();

  return (
    <Control position="topright">
      <LayersContainer id="layersContainer" className={clsx({ closed: !isFiltering })}>
        <TooltipWrapper
          toolTipId="advanced-filterId"
          toolTip={isFiltering ? undefined : 'Advanced Filter'}
        >
          <FilterButton ref={target} onClick={toggleMapFilter}>
            <FiMapPin />
          </FilterButton>
        </TooltipWrapper>
        <FilterContent className={clsx({ isFiltering })}>
          <FilterContainer />
        </FilterContent>
      </LayersContainer>
    </Control>
  );
};

const LayersContainer = styled.div`
  width: 34.1rem;
  min-height: 5.2rem;
  height: 50rem;
  max-height: 50rem;
  background-color: #fff;
  position: relative;
  border-radius: 0.4rem;
  box-shadow: -0.2rem 0.1rem 0.4rem rgba(0, 0, 0, 0.2);
  z-index: 1000;

  &.closed {
    width: 0rem;
    height: 0rem;
  }
`;

const FilterContent = styled.div`
  width: 100%;
  height: calc(100% - 8rem);

  &.open {
    overflow-y: scroll;
  }
`;
