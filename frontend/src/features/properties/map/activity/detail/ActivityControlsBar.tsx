import { Button } from 'components/common/buttons';
import EditButton from 'components/common/EditButton';
import { Select } from 'components/common/form';
import * as API from 'constants/API';
import { Claims } from 'constants/claims';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
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
  const { getOptionsByType } = useLookupCodeHelpers();
  const activityStatusTypeOptions = getOptionsByType(API.ACTIVITY_STATUS_TYPE);
  return (
    <>
      {hasClaim(Claims.ACTIVITY_EDIT) && (
        <RightAlignedRow>
          <SectionField label="Status">
            <Select
              disabled={!editMode && !hasClaim(Claims.PROPERTY_EDIT)}
              field="status"
              options={activityStatusTypeOptions}
            />
          </SectionField>

          {hasClaim(Claims.PROPERTY_EDIT) && (
            <Button onClick={onEditRelatedProperties} variant="secondary mr-4">
              Related properties
            </Button>
          )}
          {!editMode && <EditButton onClick={() => setEditMode(true)} />}
        </RightAlignedRow>
      )}
    </>
  );
};
const StyledRow = styled.div`
  display: flex;
  flex-direction: row;
  width: 100%;
  flex-direction: row;
  justify-content: space-between;
  .btn {
    width: fit-content;
  }
`;

export default ActivityControlsBar;
