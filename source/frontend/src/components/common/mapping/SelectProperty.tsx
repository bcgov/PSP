import { ReactComponent as DraftSvg } from 'assets/images/pins/icon-draft.svg';
import * as React from 'react';
import ClickAwayListener from 'react-click-away-listener';
import styled from 'styled-components';

interface ISelectPropertyProps {
  onClick: () => void;
  onClickAway: () => void;
}

export const SelectProperty: React.FunctionComponent<ISelectPropertyProps> = ({
  onClick,
  onClickAway,
}) => {
  return (
    <div className="d-flex flex-column align-items-center">
      <ClickAwayListener onClickAway={onClickAway}>
        <StyledDraftSvg
          width={97}
          height={97}
          onClick={onClick}
          title="select properties on the map"
        />
      </ClickAwayListener>
      <ol>
        <li>Click pin above</li>
        <li>Position it on the map</li>
      </ol>
    </div>
  );
};

const StyledDraftSvg = styled(DraftSvg)`
  &:hover {
    cursor: pointer;
  }
`;

export default SelectProperty;
