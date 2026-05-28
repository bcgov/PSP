import { Formik, FormikHelpers } from 'formik';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { ResetButton, SearchButton } from '@/components/common/buttons';
import { FastDatePicker, Form, Input, Multiselect, Select } from '@/components/common/form';
import { SelectInput } from '@/components/common/List/SelectInput';
import { SectionField } from '@/components/common/Section/SectionField';
import { ColButtons } from '@/components/common/styles';
import TooltipIcon from '@/components/common/TooltipIcon';
import { MultiSelectOption } from '@/interfaces/MultiSelectOption';

import { ILeaseFilter, ILeaseSearchBy } from '../../interfaces';
import { LeaseFilterSchema } from './LeaseFilterYupSchema';
import { LeaseFilterModel } from './models/LeaseFilterModel';

export interface ILeaseFilterProps {
  initialValues: LeaseFilterModel;
  pimsRegionsOptions: MultiSelectOption[];
  leaseTeamOptions: MultiSelectOption[];
  leaseStatusOptions: MultiSelectOption[];
  leaseProgramOptions: MultiSelectOption[];
  setFilter: (filter: ILeaseFilter) => void;
  onResetFilter: () => void;
}

/**
 * Filter bar for leases and license.
 * @param {ILeaseFilterProps} props
 */
export const LeaseFilter: React.FunctionComponent<React.PropsWithChildren<ILeaseFilterProps>> = ({
  initialValues,
  pimsRegionsOptions,
  leaseTeamOptions,
  leaseStatusOptions,
  leaseProgramOptions,
  setFilter,
  onResetFilter,
}) => {
  const onSearchSubmit = (
    values: LeaseFilterModel,
    formikHelpers: FormikHelpers<LeaseFilterModel>,
  ) => {
    setFilter(values.toApi());
    formikHelpers.setSubmitting(false);
  };

  return (
    <Formik<LeaseFilterModel>
      enableReinitialize
      initialValues={initialValues}
      onSubmit={onSearchSubmit}
      validationSchema={LeaseFilterSchema}
    >
      {formikProps => (
        <FilterBoxForm className="p-3">
          <Row>
            <Col xl="6">
              <Row>
                <Col xs="auto">
                  <strong>Search by:</strong>
                </Col>
                <Col>
                  <Row>
                    <Col xl="7">
                      <SelectInput<ILeaseSearchBy, ILeaseFilter>
                        field="searchBy"
                        defaultKey="pid"
                        defaultValue={''}
                        selectOptions={[
                          { label: 'PID', key: 'pid', placeholder: 'Enter a PID' },
                          { label: 'PIN', key: 'pin', placeholder: 'Enter a PIN' },
                          { label: 'Address', key: 'address', placeholder: 'Enter an address' },
                          {
                            label: 'L-File #',
                            key: 'lFileNo',
                            placeholder: 'Enter an L-File number',
                          },
                          {
                            label: 'Historical File #',
                            key: 'historical',
                            placeholder: 'Enter a Historical file# (LIS, PS, etc.)',
                          },
                        ]}
                        className="idir-input-group"
                      />
                    </Col>
                    <Col xl="5">
                      <Multiselect
                        field="leaseStatusTypes"
                        options={leaseStatusOptions}
                        displayValue="text"
                        placeholder="Select Status(s)"
                        hidePlaceholder={true}
                      />
                    </Col>
                  </Row>
                  <Row>
                    <Col xl="7">
                      <Multiselect
                        field="programs"
                        options={leaseProgramOptions}
                        displayValue="text"
                        placeholder="Select Program(s)"
                      />
                    </Col>
                    <Col xl="5">
                      <Input field="tenantName" placeholder="Tenant Name" />
                    </Col>
                  </Row>
                  <Row>
                    <Col xl={7}>
                      <StyledMultiselect
                        field="leaseTeamMembers"
                        displayValue="text"
                        placeholder="Team member"
                        hidePlaceholder
                        options={leaseTeamOptions}
                        selectionLimit={1}
                      />
                    </Col>
                  </Row>
                </Col>
              </Row>
            </Col>
            <Col xl="5" xs="12">
              <SectionField className="pb-0" label="Expiry date" labelWidth={{ xl: '2' }}>
                <Row>
                  <Col>
                    <FastDatePicker
                      field="expiryStartDate"
                      formikProps={formikProps}
                      placeholderText="from date"
                    />
                  </Col>
                  <Col>
                    <FastDatePicker
                      field="expiryEndDate"
                      formikProps={formikProps}
                      placeholderText="to date"
                    />
                  </Col>
                </Row>
              </SectionField>
              <SectionField label="" labelWidth={{ xl: '2' }}>
                <Row>
                  <Col>
                    <Select
                      options={[
                        { value: 'null', label: 'All Account Types' },
                        { value: 'false', label: 'Payable' },
                        { value: 'true', label: 'Receivable' },
                      ]}
                      field="isReceivable"
                    />
                  </Col>
                  <Col>
                    <Row noGutters>
                      <Col xs="9">
                        <Input field="details" placeholder="Keyword" />
                      </Col>
                      <Col xs="1">
                        <TooltipIcon
                          toolTipId="lease-search-keyword-tooltip"
                          toolTip="Search 'Lease description' and 'Notes' fields"
                        />
                      </Col>
                    </Row>
                  </Col>
                </Row>
              </SectionField>
              <SectionField label="" labelWidth={{ xl: '2' }}>
                <Row>
                  <Col>
                    <Multiselect
                      field="regions"
                      options={pimsRegionsOptions}
                      displayValue="text"
                      placeholder="Select Region(s)"
                    />
                  </Col>
                </Row>
              </SectionField>
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

const FilterBoxForm = styled(Form)`
  background-color: ${({ theme }) => theme.css.filterBoxColor};
  border-radius: 0.5rem;
  .idir-input-group {
    .form-select {
      @media only screen and (max-width: 1199px) {
        width: 8rem;
      }
    }
  }
`;

const StyledMultiselect = styled(Multiselect)`
  .chip {
    background: #f2f2f2;
    border-radius: 0.4rem;
    color: black;
    font-size: 1.6rem;
    margin-right: 1em;
  }
  &.multiselect-container {
    width: auto;
    color: black;
    padding-bottom: 1.2rem;
    .searchWrapper {
      background: white;
      border: 1px solid #606060;
      min-height: 2.4rem;
      padding: 0.5rem;
      input {
        margin: 0;
      }
    }
  }
`;

export default LeaseFilter;
