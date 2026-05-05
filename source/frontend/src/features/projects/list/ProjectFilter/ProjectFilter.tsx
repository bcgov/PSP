import { Formik, FormikHelpers } from 'formik';
import React from 'react';
import { Col, Row } from 'react-bootstrap';

import { ResetButton, SearchButton } from '@/components/common/buttons';
import { Input, Multiselect, Select } from '@/components/common/form';
import { ColButtons, FilterBoxForm } from '@/components/common/styles';
import { PROJECT_STATUS_TYPES } from '@/constants/API';
import { IProjectFilter } from '@/features/projects/interfaces';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { MultiSelectOption } from '@/interfaces/MultiSelectOption';
import { mapLookupCode } from '@/utils';

import { ProjectFilterModel } from './models/ProjectFilterModel';

export interface IProjectFilterProps {
  initialValues: ProjectFilterModel;
  pimsRegionsOptions: MultiSelectOption[];
  setFilter: (filter: IProjectFilter) => void;
  onResetFilter: () => void;
}

/**
 * Filter bar for Projects.
 * @param {IProjectFilterProps} props
 */
export const ProjectFilter: React.FunctionComponent<
  React.PropsWithChildren<IProjectFilterProps>
> = ({ initialValues, pimsRegionsOptions, setFilter, onResetFilter }) => {
  const onSearchSubmit = (
    values: ProjectFilterModel,
    formikHelpers: FormikHelpers<ProjectFilterModel>,
  ) => {
    setFilter(values.toApi());
    formikHelpers.setSubmitting(false);
  };

  const lookupCodes = useLookupCodeHelpers();
  const projectStatusOptions = lookupCodes
    .getByType(PROJECT_STATUS_TYPES)
    .map(c => mapLookupCode(c));

  return (
    <Formik<ProjectFilterModel>
      enableReinitialize
      initialValues={initialValues}
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
                    <Col xl="4">
                      <Input field="projectNumber" placeholder="Project number" />
                    </Col>
                    <Col xl="8">
                      <Input field="projectName" placeholder="Project name" />
                    </Col>
                  </Row>
                </Col>
              </Row>
            </Col>

            <Col xl="4">
              <Row>
                <Col xl="6">
                  <Multiselect
                    field="regions"
                    options={pimsRegionsOptions}
                    displayValue="text"
                    placeholder="Select Region(s)"
                  />
                </Col>
                <Col xl="6">
                  <Select
                    field="projectStatusCode"
                    options={projectStatusOptions}
                    placeholder="All Status"
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
                      onResetFilter();
                      formikProps.resetForm();
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
