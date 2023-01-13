import { ResetButton, SearchButton } from 'components/common/buttons';
import { Input, Select } from 'components/common/form';
import { SelectInput } from 'components/common/List/SelectInput';
import { FilterBoxForm } from 'components/common/styles';
import TooltipIcon from 'components/common/TooltipIcon';
import { LEASE_PROGRAM_TYPES, LEASE_STATUS_TYPES, REGION_TYPES } from 'constants/API';
import { IProjectFilter, IProjectSearchBy } from 'features/projects/interfaces';
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
  regionType: '',
  projectStatusType: 'ACTIVE',
};

/**
 * Filter bar for Projects.
 * @param {IProjectFilterProps} props
 */
export const ProjectFilter: React.FunctionComponent<
  React.PropsWithChildren<IProjectFilterProps>
> = ({ filter, setFilter }) => {
  const onSearchSubmit = (values: IProjectFilter, { setSubmitting }: any) => {
    setFilter(values);
    setSubmitting(false);
  };

  const resetFilter = () => {
    setFilter(defaultFilter);
  };

  const lookupCodes = useLookupCodeHelpers();
  const regionOptions = lookupCodes.getByType(REGION_TYPES).map(c => mapLookupCode(c));

  // const leaseStatusOptions = lookupCodes.getByType(LEASE_STATUS_TYPES);

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
                  <Select field="regionType" options={regionOptions} placeholder="All Regions" />
                </Col>
                <Col xl="4">
                  <Select field="projectStatusType" options={regionOptions} />
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
