import { Formik, FormikHelpers } from 'formik';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { ResetButton, SearchButton } from '@/components/common/buttons';
import { Check, Form, Input, Multiselect, Select, SelectOption } from '@/components/common/form';
import { SelectInput } from '@/components/common/List/SelectInput';
import { ColButtons } from '@/components/common/styles';
import { MultiSelectOption } from '@/interfaces/MultiSelectOption';

import { AcquisitionFilterModel, ApiGen_Concepts_AcquisitionFilter } from '../interfaces';

export interface IAcquisitionFilterProps {
  initialValues: AcquisitionFilterModel;
  pimsRegionsOptions: MultiSelectOption[];
  acquisitionTeamOptions: MultiSelectOption[];
  acquisitionStatusOptions: SelectOption[];
  setFilter: (filter: ApiGen_Concepts_AcquisitionFilter) => void;
  onResetFilter: () => void;
}

/**
 * Filter bar for acquisition files.
 * @param {IAcquisitionFilterProps} props
 */
export const AcquisitionFilter: React.FC<React.PropsWithChildren<IAcquisitionFilterProps>> = ({
  initialValues,
  pimsRegionsOptions,
  acquisitionTeamOptions,
  acquisitionStatusOptions,
  setFilter,
  onResetFilter,
}) => {
  const onSearchSubmit = (
    values: AcquisitionFilterModel,
    formikHelpers: FormikHelpers<AcquisitionFilterModel>,
  ) => {
    setFilter(values.toApi());
    formikHelpers.setSubmitting(false);
  };

  return (
    <Formik<AcquisitionFilterModel>
      enableReinitialize
      initialValues={initialValues}
      onSubmit={onSearchSubmit}
    >
      {formikProps => (
        <FilterBoxForm className="p-3">
          <Row>
            <Col lg="1">
              <strong>Search by:</strong>
            </Col>
            <Col lg="5">
              <Row>
                <Col>
                  <SelectInput<
                    {
                      address: string;
                      pin: string;
                      pid: string;
                    },
                    ApiGen_Concepts_AcquisitionFilter
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
                    className="idir-input-group"
                  />
                </Col>
              </Row>
              <Row>
                <Col lg="5">
                  <Multiselect
                    field="acquisitionTeamMembers"
                    displayValue="text"
                    placeholder="Team member"
                    hidePlaceholder
                    options={acquisitionTeamOptions}
                    selectionLimit={1}
                  />
                </Col>
                <Col lg="4">
                  <Input field="ownerName" placeholder="Owner" />
                </Col>
                <Col lg="3">
                  <Select
                    options={acquisitionStatusOptions}
                    field="acquisitionFileStatusTypeCode"
                    placeholder="All Status"
                  />
                </Col>
              </Row>
            </Col>
            <Col lg="5">
              <Row>
                <Col lg="6">
                  <Input
                    field="acquisitionFileNameOrNumber"
                    placeholder="Acquisition file number or name or historical file number"
                  />
                </Col>
                <Col lg="6">
                  {' '}
                  <Input
                    field="projectNameOrNumber"
                    placeholder="Ministry or alternate project name or number"
                  />
                </Col>
              </Row>
              <Row>
                <Col lg="9">
                  <Multiselect
                    field="regions"
                    options={pimsRegionsOptions}
                    displayValue="text"
                    placeholder="Select Region(s)"
                  />
                </Col>
                <Col lg="3">
                  <StyledCheckBox
                    field="hasNoticeOfClaim"
                    postLabel="File has NOC"
                    onChange={formikProps.handleChange}
                  ></StyledCheckBox>
                </Col>
              </Row>
            </Col>
            <ColButtons lg="1">
              <Row>
                <Col lg="auto" className="pr-0">
                  <SearchButton disabled={formikProps.isSubmitting} />
                </Col>
                <Col lg="auto">
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

export default AcquisitionFilter;

const FilterBoxForm = styled(Form)`
  background-color: ${({ theme }) => theme.css.filterBoxColor};
  border-radius: 0.5rem;
  .idir-input-group {
    .input-group-prepend select {
      width: 16rem;
    }
    input {
      max-width: 100%;
    }
  }
`;

const StyledCheckBox = styled(Check)`
  font-size: 1.6rem;
  font-weight: normal;
  label {
    margin-bottom: 0;
  }
`;
