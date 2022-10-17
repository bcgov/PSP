import { Button } from 'components/common/buttons';
import { Claims } from 'constants/claims';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import * as React from 'react';
import { Col, Row } from 'react-bootstrap';

export interface IActivityControlsBarProps {
  onEditRelatedProperties: () => void;
}

export const ActivityControlsBar: React.FunctionComponent<IActivityControlsBarProps> = ({
  onEditRelatedProperties,
}) => {
  const { hasClaim } = useKeycloakWrapper();

  return (
    <>
      {hasClaim(Claims.ACTIVITY_EDIT) && (
        <StyledRow>
          <RightAligned>
            {hasClaim(Claims.PROPERTY_EDIT) && (
              <Col xs="auto" className="pr-0 mr-0">
                <Button onClick={onEditRelatedProperties} variant="secondary ">
                  Related properties
                </Button>
              </Col>
            )}
            {!editMode && (
              <Col xs="auto" className="px-0 mx-0">
                <EditButton onClick={() => setEditMode(true)} />
              </Col>
            )}
          </Row>
        </Section>
      )}
    </>
  );
};
const StyledRow = styled.div`
  display: flex;
  flex-direction: row;
  width: 100%;
`;
const RightAligned = styled.div`
  width: inherit;
  display: flex;
  flex-direction: row-reverse;
`;

export default ActivityControlsBar;
