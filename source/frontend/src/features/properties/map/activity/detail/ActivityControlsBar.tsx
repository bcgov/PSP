import { Button } from 'components/common/buttons';
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
  onEditRelatedProperties: () => void;
}

export const ActivityControlsBar: React.FunctionComponent<IActivityControlsBarProps> = ({
  editMode,
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
        <Section header="Details">
          <Row>
            {editMode && (
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
            )}
            {hasClaim(Claims.PROPERTY_EDIT) && (
              <Col>
                <Button className="ml-auto" onClick={onEditRelatedProperties} variant="secondary ">
                  Related properties
                </Button>
              </Col>
            )}
          </Row>
        </Section>
      )}
    </>
  );
};
export default ActivityControlsBar;
