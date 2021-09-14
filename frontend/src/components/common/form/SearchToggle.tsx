import { ReactComponent as MapSvg } from 'assets/images/icon-map.svg';
import { ReactComponent as TableSvg } from 'assets/images/icon-table.svg';
import React from 'react';
import { useHistory } from 'react-router';
import styled from 'styled-components';

import TooltipWrapper from '../TooltipWrapper';
import { ButtonProps } from '.';

export enum SearchToggleOption {
  /** The map is the active page */
  Map = 'map',
  /** Property List View is the active page */
  List = 'list',
}

interface ISearchToggleProps extends ButtonProps {
  /** set the id of the tool tip use for on hover of the plus buttons */
  toolId: string;
  /** Which toggle view is currently active */
  toggle?: SearchToggleOption;
}

/**
 * SearchToggle displaying two buttons which act like a toggle to navigate from the map to the property list view (and vise-versa).
 * @param param0
 */
export const SearchToggle: React.FC<ISearchToggleProps> = ({
  toggle = SearchToggleOption.Map,
  toolId,
  ...props
}) => {
  const history = useHistory();

  return (
    <StyledToggle toggle={toggle}>
      <StyledNav>
        <StyledLink>
          <TooltipWrapper toolTipId={`${toolId}-map`} toolTip="Map View">
            <MapSvg onClick={() => history.push('/mapview')} />
          </TooltipWrapper>
        </StyledLink>
      </StyledNav>
      <StyledNav>
        <StyledLink>
          <TooltipWrapper toolTipId={`${toolId}-list`} toolTip="List View">
            <TableSvg onClick={() => history.push('/properties/list')} />
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

export default SearchToggle;
