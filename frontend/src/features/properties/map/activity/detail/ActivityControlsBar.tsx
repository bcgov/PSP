import { Button } from 'components/common/buttons';
import EditButton from 'components/common/EditButton';
import { Claims } from 'constants/claims';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import * as React from 'react';
import styled from 'styled-components';

export interface IActivityControlsBarProps {
  editMode: boolean;
  setEditMode: (editMode: boolean) => void;
  onEditRelatedProperties: () => void;
}

export const ActivityControlsBar: React.FunctionComponent<IActivityControlsBarProps> = ({
  editMode,
  setEditMode,
  onEditRelatedProperties,
}) => {
  const { hasClaim } = useKeycloakWrapper();
  return (
    <>
      {hasClaim(Claims.ACTIVITY_EDIT) && (
        <RightAlignedRow>
          {!editMode && <EditButton onClick={() => setEditMode(true)} />}
          {hasClaim(Claims.PROPERTY_EDIT) && (
            <Button onClick={onEditRelatedProperties} variant="secondary mr-4">
              Related properties
            </Button>
          )}
        </RightAlignedRow>
      )}
    </>
  );
};

const RightAlignedRow = styled.div`
  display: flex;
  width: 100%;
  flex-direction: row-reverse;
  align-items: flex-end;
  .btn {
    width: fit-content;
  }
`;

export default ActivityControlsBar;
