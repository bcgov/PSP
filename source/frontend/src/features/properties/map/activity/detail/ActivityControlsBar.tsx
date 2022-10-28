import { Button } from 'components/common/buttons';
import EditButton from 'components/common/EditButton';
import { Select } from 'components/common/form';
import * as API from 'constants/API';
import { Claims } from 'constants/claims';
import { Section } from 'features/mapSideBar/tabs/Section';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { useFormikContext } from 'formik';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import * as React from 'react';
import { Col, Row } from 'react-bootstrap';

import { ActivityModel } from './models';

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
  const formikProps = useFormikContext<ActivityModel>();
  const handleStatusChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
    const selected = activityStatusTypeOptions.find(
      x => x.value === event.target.selectedOptions[0].value,
    );
    formikProps.setFieldValue('activityStatusTypeCode', {
      id: selected?.value,
      description: selected?.label,
    });
  };
  return (
    <>
      {hasClaim(Claims.ACTIVITY_EDIT) && (
        <Section>
          <Row>
            <Col>
              <SectionField label="Status" labelWidth="auto">
                <Select
                  disabled={!editMode}
                  field="activityStatusTypeCode.id"
                  options={activityStatusTypeOptions}
                  onChange={handleStatusChange}
                />
              </SectionField>
            </Col>
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

export default ActivityControlsBar;
