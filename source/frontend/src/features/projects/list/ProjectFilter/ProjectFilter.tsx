import { Formik } from 'formik';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { ResetButton, SearchButton } from '@/components/common/buttons';
import { Input, Select } from '@/components/common/form';
import { UserRegionSelectContainer } from '@/components/common/form/UserRegionSelect/UserRegionSelectContainer';
import { FilterBoxForm } from '@/components/common/styles';
import { PROJECT_STATUS_TYPES } from '@/constants/API';
import { ProjectStatusTypes } from '@/constants/projectStatusTypes';
import { IProjectFilter } from '@/features/projects/interfaces';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { mapLookupCode } from '@/utils';

export interface IProjectFilterProps {
  filter?: IProjectFilter;
  setFilter: (filter: IProjectFilter) => void;
  initialFilter?: IProjectFilter;
}

export const defaultFilter: IProjectFilter = {
  projectName: '',
  projectNumber: '',
  projectRegionCode: '',
  projectStatusCode: ProjectStatusTypes.Active,
};

/**
 * Filter bar for Projects.
 * @param {IProjectFilterProps} props
 */
export const ProjectFilter: React.FunctionComponent<
  React.PropsWithChildren<IProjectFilterProps>
> = ({ setFilter, initialFilter }) => {
  const onSearchSubmit = (values: IProjectFilter, { setSubmitting }: any) => {
    setFilter(values);
    setSubmitting(false);
  };

  const resetFilter = () => {
    setFilter(initialFilter ?? defaultFilter);
  };

  const lookupCodes = useLookupCodeHelpers();
  const projectStatusOptions = lookupCodes
    .getByType(PROJECT_STATUS_TYPES)
    .map(c => mapLookupCode(c));

  return (
    <Formik
      enableReinitialize
      initialValues={initialFilter ?? defaultFilter}
      onSubmit={onSearchSubmit}
    >
      {formikProps => (
        <FilterBoxForm className="p-3">
          <Row>
            <Col xl="7">
              <Row>
                <Col xl="auto">
                  <strong>Search by:</strong>
                </Col>
                <Col>
                  <Row>
                    <Col xl="3">
                      <Input field="projectNumber" placeholder="Project number" />
                    </Col>
                    <Col xl="9">
                      <Input field="projectName" placeholder="Project name" />
                    </Col>
                  </Row>
                </Col>
              </Row>
            </Col>

            <Col xl="4">
              <Row>
                <Col xl="4">
                  <UserRegionSelectContainer field="projectRegionCode" includeAll />
                </Col>
                <Col xl="4">
                  <Select
                    field="projectStatusCode"
                    options={projectStatusOptions}
                    placeholder="All Statuses"
                    value={initialFilter?.projectStatusCode}
                  />
                </Col>
              </Row>
            </Col>

            <ColButtons xl="1">
              <Row>
                <Col xs="auto" className="pr-0">
                  <SearchButton disabled={formikProps.isSubmitting} />
                </Col>
                <Col xs="auto">
                  <ResetButton
                    disabled={formikProps.isSubmitting}
                    onClick={() => {
                      formikProps.resetForm();
                      resetFilter();
                    }}
                  />
                </Col>
              </Row>
            </ColButtons>
          </Row>
        </FilterBoxForm>
      )}
    </Formik>
  );
};

export default ProjectFilter;

const ColButtons = styled(Col)`
  border-left: 2px solid white;
`;
