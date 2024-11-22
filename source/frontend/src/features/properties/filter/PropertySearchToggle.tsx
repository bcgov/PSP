import React from 'react';
import styled from 'styled-components';

import MapSvg from '@/assets/images/icon-map.svg?react';
import TableSvg from '@/assets/images/icon-table.svg?react';
import TooltipWrapper from '@/components/common/TooltipWrapper';

export enum SearchToggleOption {
  /** The map is the active page */
  Map = 'map',
  /** Property List View is the active page */
  List = 'list',
}

interface IPropertySearchToggleProps {
  /** set the id of the tool tip use for on hover of the plus buttons */
  toolId: string;
  /** Which toggle view is currently active */
  toggle?: SearchToggleOption;
  onPageToggle: (option: SearchToggleOption) => void;
}

/**
 * SearchToggle displaying two buttons which act like a toggle to navigate from the map to the property list view (and vise-versa).
 * @param param0
 */
export const PropertySearchToggle: React.FunctionComponent<
  React.PropsWithChildren<IPropertySearchToggleProps>
> = ({ toggle = SearchToggleOption.Map, toolId, onPageToggle }) => {
  return (
    <StyledToggle toggle={toggle}>
      <StyledNav>
        <StyledLink>
          <TooltipWrapper tooltipId={`${toolId}-map`} tooltip="Map View">
            <MapSvg
              title="map-view"
              onClick={() => {
                if (toggle !== SearchToggleOption.Map) {
                  onPageToggle(SearchToggleOption.Map);
                }
              }}
            />
          </TooltipWrapper>
        </StyledLink>
      </StyledNav>
      <StyledNav>
        <StyledLink>
          <TooltipWrapper tooltipId={`${toolId}-list`} tooltip="List View">
            <TableSvg
              title="list-view"
              onClick={() => {
                if (toggle !== SearchToggleOption.List) {
                  onPageToggle(SearchToggleOption.List);
                }
              }}
            />
          </TooltipWrapper>
        </StyledLink>
      </StyledNav>
    </StyledToggle>
  );
};

interface ISearchToggleStyleProps {
  toggle?: SearchToggleOption;
}

const StyledToggle = styled('div')<ISearchToggleStyleProps>`
  display: flex;

  div:nth-child(${props => (props.toggle === SearchToggleOption.Map ? 1 : 2)}) {
    div:nth-child(1) {
      cursor: auto;
      svg {
        fill: #909090;
      }
    }
  }

  div:nth-child(${props => (props.toggle === SearchToggleOption.Map ? 2 : 1)}) {
    div:nth-child(1) {
      cursor: pointer;
      svg {
        fill: #003366;
      }
    }
  }
`;

const StyledNav = styled('div')`
  flex: 1;
  width: 100%;
`;

const StyledLink = styled('div')`
  display: flex;
  flex-shrink: 0;
  flex-direction: row;
  align-items: center;
  svg {
    min-width: max-content;
  }
`;

export default PropertySearchToggle;
