import { ResetButton, SearchButton } from 'components/common/buttons';
import { Input, Select } from 'components/common/form';
import { FilterBoxForm } from 'components/common/styles';
import { PROJECT_STATUS_TYPES, REGION_TYPES } from 'constants/API';
import { ProjectStatusTypes } from 'constants/projectStatusTypes';
import { IProjectFilter } from 'features/projects/interfaces';
import { Formik } from 'formik';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import React, { useEffect, useState } from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';
import { mapLookupCode } from 'utils';

export interface IProjectFilterProps {
  filter?: IProjectFilter;
  setFilter: (filter: IProjectFilter) => void;
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
> = ({ filter, setFilter }) => {
  const [selectedStatus, setInitialSelectedStatus] = useState<string>(ProjectStatusTypes.Active);

  useEffect(() => {
    setInitialSelectedStatus(ProjectStatusTypes.Active);
  }, []);

  const onSearchSubmit = (values: IProjectFilter, { setSubmitting }: any) => {
    console.log(values);
    setFilter(values);
    setSubmitting(false);
  };

  const resetFilter = () => {
    setFilter(defaultFilter);
  };

  const lookupCodes = useLookupCodeHelpers();
  const regionOptions = lookupCodes.getByType(REGION_TYPES).map(c => mapLookupCode(c));
  const projectStatusOptions = lookupCodes
    .getByType(PROJECT_STATUS_TYPES)
    .map(c => mapLookupCode(c));

  return (
    <Formik enableReinitialize initialValues={filter ?? defaultFilter} onSubmit={onSearchSubmit}>
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
                  <Select
                    field="projectRegionCode"
                    options={regionOptions}
                    placeholder="All Regions"
                  />
                </Col>
                <Col xl="4">
                  <Select
                    field="projectStatusCode"
                    options={projectStatusOptions}
                    value={selectedStatus}
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
