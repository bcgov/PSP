import { Formik } from 'formik';
import Multiselect from 'multiselect-react-dropdown';
import React, { useEffect, useState } from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaTimes } from 'react-icons/fa';
import styled from 'styled-components';

import { ResetButton, SearchButton } from '@/components/common/buttons';
import { FastDatePicker, Input } from '@/components/common/form';
import { UserRegionSelectContainer } from '@/components/common/form/UserRegionSelect/UserRegionSelectContainer';
import { SelectInput } from '@/components/common/List/SelectInput';
import { FilterBoxForm } from '@/components/common/styles';
import TooltipIcon from '@/components/common/TooltipIcon';
import { LEASE_PROGRAM_TYPES, LEASE_STATUS_TYPES } from '@/constants/API';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';

import { ILeaseFilter, ILeaseSearchBy } from '../../interfaces';
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
  leaseStatusTypes: ['ACTIVE'],
  programs: [],
  tenantName: '',
  expiryStartDate: '',
  expiryEndDate: '',
  regionType: '',
  details: '',
};

/**
 * Filter bar for leases and license.
 * @param {ILeaseFilterProps} props
 */
export const LeaseFilter: React.FunctionComponent<React.PropsWithChildren<ILeaseFilterProps>> = ({
  filter,
  setFilter,
}) => {
  const onSearchSubmit = (values: ILeaseFilter, { setSubmitting }: any) => {
    const selectedPrograms: MultiSelectOption[] = multiselectProgramRef.current?.getSelectedItems();
    const selectedStatuses: MultiSelectOption[] = multiselectStatusRef.current?.getSelectedItems();
    const programIds = selectedPrograms.map<string>(x => x.id);
    const statuses = selectedStatuses.map<string>(x => x.id);
    values = { ...values, programs: programIds, leaseStatusTypes: statuses };
    setFilter(values);
    setSubmitting(false);
  };
  const resetFilter = () => {
    multiselectProgramRef.current?.resetSelectedValues();
    setInitialSelectedStatus(
      statusFilterOptions.filter(x => defaultFilter.leaseStatusTypes.includes(x.id)),
    );

    setFilter(defaultFilter);
  };

  const multiselectProgramRef = React.createRef<Multiselect>();
  const multiselectStatusRef = React.createRef<Multiselect>();

  const lookupCodes = useLookupCodeHelpers();

  const leaseStatusOptions = lookupCodes.getByType(LEASE_STATUS_TYPES);

  const leaseProgramTypes = lookupCodes.getByType(LEASE_PROGRAM_TYPES);
  const programFilterOptions: MultiSelectOption[] = leaseProgramTypes.map<MultiSelectOption>(x => {
    return { id: x.id as string, text: x.name };
  });

  const statusFilterOptions: MultiSelectOption[] = leaseStatusOptions.map<MultiSelectOption>(x => {
    return { id: x.id as string, text: x.name };
  });
  const initialLeaseStatusList = statusFilterOptions.filter(x =>
    defaultFilter.leaseStatusTypes.includes(x.id),
  );

  const [selectedStatus, setInitialSelectedStatus] =
    useState<MultiSelectOption[]>(initialLeaseStatusList);

  // Necessary since the lookup codes might have not been loaded before the first render
  useEffect(() => {
    setInitialSelectedStatus([{ id: 'ACTIVE', text: 'Active' }]);
  }, []);

  function onSelectedStatusChange(selectedList: MultiSelectOption[]) {
    setInitialSelectedStatus(selectedList);
  }

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
            <Col xl="6">
              <Row>
                <Col xl="auto">
                  <strong>Search by:</strong>
                </Col>
                <Col>
                  <Row>
                    <Col xl="7">
                      <SelectInput<ILeaseSearchBy, ILeaseFilter>
                        field="searchBy"
                        defaultKey="pinOrPid"
                        selectOptions={[
                          { label: 'PID/PIN', key: 'pinOrPid', placeholder: 'Enter a PID or PIN' },
                          { label: 'Address', key: 'address', placeholder: 'Enter an address' },
                          {
                            label: 'L-File #',
                            key: 'lFileNo',
                            placeholder: 'Enter an L-File number',
                          },
                          {
                            label: 'Historical File #',
                            key: 'historical',
                            placeholder: 'Enter a LIS or PS file Number',
                          },
                        ]}
                        className="idir-input-group"
                      />
                    </Col>
                    <Col xl="5">
                      <Multiselect
                        id="status-selector"
                        ref={multiselectStatusRef}
                        options={statusFilterOptions}
                        onSelect={onSelectedStatusChange}
                        onRemove={onSelectedStatusChange}
                        selectedValues={selectedStatus}
                        displayValue="text"
                        placeholder="Select Status(s)"
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
                  </Row>
                  <Row>
                    <Col xl="7">
                      <Multiselect
                        id="properties-selector"
                        ref={multiselectProgramRef}
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
                    <Col xl="5">
                      <Input field="tenantName" placeholder="Tenant Name" />
                    </Col>
                  </Row>
                </Col>
              </Row>
            </Col>
            <Col xl="5">
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
                      <UserRegionSelectContainer field="regionType" placeholder="All Regions" />
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

export default LeaseFilter;

const ColButtons = styled(Col)`
  border-left: 2px solid white;
`;
