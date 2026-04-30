import { Formik, FormikHelpers } from 'formik';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { ResetButton } from '@/components/common/buttons';
import { SearchButton } from '@/components/common/buttons/SearchButton';
import {
  Check,
  Input,
  Multiselect,
  Select,
  SelectOption,
  TypeaheadSelect,
} from '@/components/common/form';
import { SelectInput } from '@/components/common/List/SelectInput';
import { ColButtons, FilterBoxForm } from '@/components/common/styles';
import { MultiSelectOption } from '@/interfaces/MultiSelectOption';
import { Api_ManagementFilter } from '@/models/api/ManagementFilter';

import { ManagementFilterModel } from '../models';

export interface IManagementFilterProps {
  initialValues: ManagementFilterModel;
  fileStatusOptions: SelectOption[];
  managementPurposeOptions: SelectOption[];
  pimsRegionsOptions: MultiSelectOption[];
  managementTeamOptions: SelectOption[];
  setFilter: (filter: Api_ManagementFilter) => void;
  onResetFilter: () => void;
}

export const ManagementFilter: React.FC<IManagementFilterProps> = ({
  initialValues,
  fileStatusOptions,
  managementPurposeOptions,
  pimsRegionsOptions,
  managementTeamOptions,
  setFilter,
  onResetFilter,
}) => {
  const onSearchSubmit = async (
    values: ManagementFilterModel,
    formikHelpers: FormikHelpers<ManagementFilterModel>,
  ) => {
    setFilter(values.toApi());
    formikHelpers.setSubmitting(false);
  };

  return (
    <Formik<ManagementFilterModel>
      enableReinitialize
      initialValues={initialValues}
      onSubmit={onSearchSubmit}
    >
      {formikProps => (
        <FilterBoxForm className="p-3">
          <Row>
            <Col xl="1">
              <strong>Search by:</strong>
            </Col>
            <Col xl="5">
              <Row>
                <Col>
                  <SelectInput<
                    {
                      address: string;
                      pin: string;
                      pid: string;
                    },
                    Api_ManagementFilter
                  >
                    field="searchBy"
                    defaultKey="address"
                    selectOptions={[
                      {
                        label: 'Civic Address',
                        key: 'address',
                        placeholder: 'Enter an address',
                      },
                      { label: 'PID', key: 'pid', placeholder: 'Enter a PID' },
                      {
                        label: 'PIN',
                        key: 'pin',
                        placeholder: 'Enter a PIN',
                      },
                    ]}
                  />
                </Col>
              </Row>
              <Row>
                <Col xl="6">
                  <TypeaheadSelect
                    field="managementTeamMember"
                    options={managementTeamOptions}
                    placeholder="Team Member"
                  />
                </Col>
                <Col xl="6">
                  <Select
                    options={fileStatusOptions}
                    field="managementFileStatusCode"
                    placeholder="All Status"
                  />
                </Col>
              </Row>
              <Row>
                <Col xs={12}>
                  <Multiselect
                    field="regions"
                    options={pimsRegionsOptions}
                    displayValue="text"
                    placeholder="Select Region(s)"
                  />
                </Col>
              </Row>
            </Col>
            <Col xl="5">
              <Row>
                <Col xl="12">
                  <Input
                    field="fileNameOrNumberOrReference"
                    placeholder="Management file number or name or reference number"
                  />
                </Col>
              </Row>
              <Row>
                <Col xl="6">
                  <Select
                    options={managementPurposeOptions}
                    field="managementFilePurposeCode"
                    placeholder="Select management purpose..."
                  />
                </Col>
                <Col xl="6">
                  <Input
                    field="projectNameOrNumber"
                    placeholder="Enter a project name or number..."
                  />
                </Col>
              </Row>
              <Row>
                <Col xs={6} style={{ textAlign: 'left' }}>
                  <StyledCheckBox
                    field="hasNoticeOfClaim"
                    postLabel="File has NOC"
                    onChange={formikProps.handleChange}
                  ></StyledCheckBox>
                </Col>
              </Row>
            </Col>
            <ColButtons xl="1">
              <Row>
                <Col xl="auto" className="pr-0">
                  <SearchButton disabled={formikProps.isSubmitting} />
                </Col>
                <Col xl="auto">
                  <ResetButton
                    disabled={formikProps.isSubmitting}
                    onClick={() => {
                      formikProps.resetForm();
                      onResetFilter();
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

export default ManagementFilter;

const StyledCheckBox = styled(Check)`
  font-size: 1.6rem;
  font-weight: normal;
  label {
    margin-bottom: 0;
  }
`;
