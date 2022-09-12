import { ResetButton, SearchButton } from 'components/common/buttons';
import { Form, Select } from 'components/common/form';
import * as API from 'constants/API';
import { Formik } from 'formik';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import { defaultActivityFilter, IActivityFilter } from 'interfaces/IActivityResults';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

export interface IActivityFilterFormProps {
  activityFilter?: IActivityFilter;
  onSetFilter: (filterValues: IActivityFilter) => void;
}

export const ActivityFilterForm = (props: IActivityFilterFormProps) => {
  const { getOptionsByType } = useLookupCodeHelpers();
  // TODO: change to activity status once available
  const activityStatusTypeOptions = getOptionsByType(API.ACTIVITY_INSTANCE_STATUS_TYPE);
  const activityTypeOptions = getOptionsByType(API.ACTIVITY_TEMPLATE_TYPE);

  return (
    <Formik<IActivityFilter>
      enableReinitialize
      initialValues={props.activityFilter ?? defaultActivityFilter}
      onSubmit={(values: IActivityFilter, { setSubmitting }: any) => {
        props.onSetFilter(values);
        setSubmitting(false);
      }}
    >
      {formikProps => (
        <FilterBoxForm className="p-3">
          <Row>
            <Col md={1}>
              <label>Filter by:</label>
            </Col>
            <Col md={4}>
              <Select
                data-testid="activity-type"
                field="activityTypeId"
                placeholder="All activity types"
                options={activityTypeOptions}
              />
            </Col>
            <Col md={4}>
              <Select
                field="status"
                data-testid="activity-status"
                placeholder="All statuses"
                options={activityStatusTypeOptions}
              />
            </Col>
            <Col md={1}>
              <SearchButton disabled={formikProps.isSubmitting} />
            </Col>
            <Col md={1}>
              <ResetButton
                disabled={formikProps.isSubmitting}
                onClick={() => {
                  formikProps.resetForm();
                  props.onSetFilter(defaultActivityFilter);
                }}
              />
            </Col>
          </Row>
        </FilterBoxForm>
      )}
    </Formik>
  );
};

const FilterBoxForm = styled(Form)`
  background-color: ${({ theme }) => theme.css.filterBoxColor};
  border-radius: 0.5rem;
`;
