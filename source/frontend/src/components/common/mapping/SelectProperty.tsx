import * as React from 'react';
import styled from 'styled-components';

import { ReactComponent as DraftSvg } from '@/assets/images/pins/icon-draft.svg';

interface ISelectPropertyProps {
  onClick: () => void;
}

export const SelectProperty: React.FunctionComponent<
  React.PropsWithChildren<ISelectPropertyProps>
> = ({ onClick }) => {
  return (
    <div className="d-flex flex-column align-items-center">
      <StyledDraftSvg
        width={97}
        height={97}
        onClick={onClick}
        title="Click once to enter property selection mode."
      />
      <ol>
        <li>Single-click blue marker above</li>
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
