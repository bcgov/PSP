import { FastDatePicker, Form, Input, Select } from 'components/common/form';
import ResetButton from 'components/common/form/ResetButton';
import SearchButton from 'components/common/form/SearchButton';
import TooltipIcon from 'components/common/TooltipIcon';
import { LEASE_PROGRAM_TYPES, LEASE_STATUS_TYPES, REGION_TYPES } from 'constants/API';
import { PropertyFilterOptions } from 'features/properties/filter';
import { Formik } from 'formik';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import Multiselect from 'multiselect-react-dropdown';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaTimes } from 'react-icons/fa';
import styled from 'styled-components';
import { mapLookupCode } from 'utils';

import { ILeaseFilter } from '../../interfaces';
import { LeaseFilterSchema } from './LeaseFilterYupSchema';

export interface ILeaseFilterProps {
  filter?: ILeaseFilter;
  setFilter: (filter: ILeaseFilter) => void;
}

interface MultiSelectOption {
  id: string;
  text: string;
}

export const defaultFilter: ILeaseFilter = {
  pinOrPid: '',
  lFileNo: '',
  searchBy: 'lFileNo',
  leaseStatusType: 'ACTIVE',
  programs: [],
  tenantName: '',
  expiryStartDate: '',
  expiryEndDate: '',
  regionType: '',
  details: '',
};

const idFilterOptions = [
  {
    label: 'PID/PIN',
    value: 'pinOrPid',
  },
  {
    label: 'L-File #',
    value: 'lFileNo',
  },
  {
    label: 'Address',
    value: 'address',
  },
];

const idFilterPlaceholders = {
  pinOrPid: 'Enter a PID or PIN',
  lFileNo: 'Enter an LIS File Number',
  address: 'Enter an address',
};

/**
 * Filter bar for leases and license.
 * @param {ILeaseFilterProps} props
 */
export const LeaseFilter: React.FunctionComponent<ILeaseFilterProps> = ({ filter, setFilter }) => {
  const onSearchSubmit = (values: ILeaseFilter, { setSubmitting }: any) => {
    let selectedPrograms: MultiSelectOption[] = multiselectRef.current?.getSelectedItems();
    let programIds = selectedPrograms.map<string>(x => x.id);
    values = { ...values, programs: programIds };
    setFilter(values);
    setSubmitting(false);
  };
  const resetFilter = () => {
    multiselectRef.current?.resetSelectedValues();
    setFilter(defaultFilter);
  };

  const multiselectRef = React.createRef<Multiselect>();

  const lookupCodes = useLookupCodeHelpers();

  const leaseStatusOptions = lookupCodes.getByType(LEASE_STATUS_TYPES).map(c => mapLookupCode(c));
  const regionOptions = lookupCodes.getByType(REGION_TYPES).map(c => mapLookupCode(c));

  const leaseProgramTypes = lookupCodes.getByType(LEASE_PROGRAM_TYPES);
  const programFilterOptions: MultiSelectOption[] = leaseProgramTypes.map<MultiSelectOption>(x => {
    return { id: x.id as string, text: x.name };
  });

  return (
    <Formik
      enableReinitialize
      initialValues={filter ?? defaultFilter}
      onSubmit={onSearchSubmit}
      validationSchema={LeaseFilterSchema}
    >
      {formikProps => (
        <FilterBoxForm className="p-3">
          <Row>
            <Col lg="6">
              <Row>
                <Col xl="auto">
                  <strong>Search by:</strong>
                </Col>
                <Col>
                  <Row>
                    <Col lg="8">
                      <PropertyFilterOptions
                        options={idFilterOptions}
                        placeholders={idFilterPlaceholders}
                      />
                    </Col>
                    <Col lg="4">
                      <Select
                        field="leaseStatusType"
                        options={leaseStatusOptions}
                        placeholder="Status"
                      />
                    </Col>
                  </Row>
                  <Row>
                    <Col lg="8">
                      <Multiselect
                        id="properties-selector"
                        ref={multiselectRef}
                        options={programFilterOptions}
                        displayValue="text"
                        placeholder="Select Program(s)"
                        customCloseIcon={<FaTimes size="18px" className="ml-3" />}
                        hidePlaceholder={true}
                        style={{
                          chips: {
                            background: '#F2F2F2',
                            borderRadius: '4px',
                            color: 'black',
                            fontSize: '16px',
                            marginRight: '1em',
                          },
                          multiselectContainer: {
                            width: 'auto',
                            color: 'black',
                            paddingBottom: '12px',
                          },
                          searchBox: {
                            background: 'white',
                            border: '1px solid #606060',
                          },
                        }}
                      />
                    </Col>
                    <Col lg="4">
                      <Input field="tenantName" placeholder="Tenant Name" />
                    </Col>
                  </Row>
                </Col>
              </Row>
            </Col>
            <Col lg="5">
              <Row>
                <Col xl="auto">
                  <strong>Expiry date:</strong>
                </Col>
                <Col>
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
                  <Row>
                    <Col>
                      <Select
                        field="regionType"
                        options={regionOptions}
                        placeholder="All Regions"
                      />
                    </Col>
                    <Col>
                      <Row>
                        <Col xs="10" className="pr-0 mr-0">
                          <Input field="details" placeholder="Keyword" />
                        </Col>
                        <Col xs="1" className="pl-0 ml-0">
                          <TooltipIcon
                            toolTipId="lease-search-keyword-tooltip"
                            toolTip="Search 'Lease description' and 'Notes' fields"
                          />
                        </Col>
                      </Row>
                    </Col>
                  </Row>
                </Col>
              </Row>
            </Col>
            <ColButtons lg="1">
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

export default LeaseFilter;

const FilterBoxForm = styled(Form)`
  background-color: ${({ theme }) => theme.css.filterBoxColor};
  border-radius: 0.5rem;
`;

const ColButtons = styled(Col)`
  border-left: 2px solid white;
`;
