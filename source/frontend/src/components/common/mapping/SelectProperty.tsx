import { ReactComponent as DraftSvg } from 'assets/images/pins/icon-draft.svg';
import * as React from 'react';
import ClickAwayListener from 'react-click-away-listener';
import styled from 'styled-components';

interface ISelectPropertyProps {
  onClick: () => void;
  onClickAway: () => void;
}

export const SelectProperty: React.FunctionComponent<
  React.PropsWithChildren<ISelectPropertyProps>
> = ({ onClick, onClickAway }) => {
  return (
    <div className="d-flex flex-column align-items-center">
      <ClickAwayListener onClickAway={onClickAway}>
        <StyledDraftSvg
          width={97}
          height={97}
          onClick={onClick}
          title="Click once to enter property selection mode."
        />
      </ClickAwayListener>
      <ol>
        <li>Single-click pin above</li>
        <li>Mouse to a parcel on the map</li>
        <li>Single-click on parcel to select it</li>
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
