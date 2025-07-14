import { Formik } from 'formik';
import Multiselect from 'multiselect-react-dropdown';
import React, { useEffect, useMemo, useState } from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaTimes } from 'react-icons/fa';
import styled from 'styled-components';

import { ResetButton, SearchButton } from '@/components/common/buttons';
import { FastDatePicker, Form, Input } from '@/components/common/form';
import { UserRegionSelectContainer } from '@/components/common/form/UserRegionSelect/UserRegionSelectContainer';
import { SelectInput } from '@/components/common/List/SelectInput';
import { SectionField } from '@/components/common/Section/SectionField';
import { ColButtons } from '@/components/common/styles';
import TooltipIcon from '@/components/common/TooltipIcon';
import { LEASE_PROGRAM_TYPES, LEASE_STATUS_TYPES } from '@/constants/API';
import { getParameterIdFromOptions } from '@/features/acquisition/list/interfaces';
import { useLeaseRepository } from '@/hooks/repositories/useLeaseRepository';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { exists, isValidId } from '@/utils';
import { formatApiPersonNames } from '@/utils/personUtils';

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
  pid: '',
  pin: '',
  lFileNo: '',
  searchBy: 'lFileNo',
  leaseStatusTypes: [
    'ACTIVE',
    'ARCHIVED',
    'DISCARD',
    'DRAFT',
    'DUPLICATE',
    'EXPIRED',
    'INACTIVE',
    'TERMINATED',
  ],
  programs: [],
  tenantName: '',
  expiryStartDate: '',
  expiryEndDate: '',
  regionType: '',
  details: '',
  leaseTeamOrganizationId: null,
  leaseTeamPersonId: null,
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
    const selectedTeam: MultiSelectOption[] = multiselectTeamRef.current?.getSelectedItems();
    const programIds = selectedPrograms.map<string>(x => x.id);
    const statuses = selectedStatuses.map<string>(x => x.id);
    const leaseTeamPersonId = exists(selectedTeam)
      ? +getParameterIdFromOptions(selectedTeam, 'P')
      : undefined;
    const leaseTeamOrganizationId = exists(selectedTeam)
      ? +getParameterIdFromOptions(selectedTeam, 'O')
      : undefined;
    values = {
      ...values,
      programs: programIds,
      leaseStatusTypes: statuses,
      leaseTeamPersonId: isValidId(leaseTeamPersonId) ? leaseTeamPersonId : null,
      leaseTeamOrganizationId:
        exists(selectedTeam) && isValidId(leaseTeamOrganizationId)
          ? leaseTeamOrganizationId
          : undefined,
    };
    setFilter(values);
    setSubmitting(false);
  };
  const resetFilter = () => {
    multiselectProgramRef.current?.resetSelectedValues();
    multiselectTeamRef.current?.resetSelectedValues();
    setSelectedStatus(
      statusFilterOptions.filter(x => defaultFilter.leaseStatusTypes.includes(x.id)),
    );

    setFilter(defaultFilter);
  };

  const {
    getAllLeaseTeamMembers: { response: leaseTeam, execute: loadLeaseTeam },
  } = useLeaseRepository();

  useEffect(() => {
    loadLeaseTeam();
  }, [loadLeaseTeam]);

  const multiselectProgramRef = React.createRef<Multiselect>();
  const multiselectStatusRef = React.createRef<Multiselect>();
  const multiselectTeamRef = React.createRef<Multiselect>();

  const lookupCodes = useLookupCodeHelpers();

  const leaseStatusOptions = lookupCodes.getByType(LEASE_STATUS_TYPES);

  const leaseProgramTypes = lookupCodes.getByType(LEASE_PROGRAM_TYPES);
  const programFilterOptions: MultiSelectOption[] = leaseProgramTypes.map<MultiSelectOption>(x => {
    return { id: x.id as string, text: x.name };
  });

  const statusFilterOptions: MultiSelectOption[] = leaseStatusOptions.map<MultiSelectOption>(x => {
    return { id: x.id as string, text: x.name };
  });

  const leaseTeamOptions = useMemo(() => {
    if (exists(leaseTeam)) {
      return leaseTeam?.map<MultiSelectOption>(x => ({
        id: x.personId ? `P-${x.personId}` : `O-${x.organizationId}`,
        text: x.personId ? formatApiPersonNames(x.person) : x.organization?.name ?? '',
      }));
    } else {
      return [];
    }
  }, [leaseTeam]);

  const initialLeaseStatusList = statusFilterOptions.filter(x =>
    defaultFilter.leaseStatusTypes.includes(x.id),
  );

  const [selectedStatus, setSelectedStatus] = useState<MultiSelectOption[]>(initialLeaseStatusList);

  // Necessary since the lookup codes might have not been loaded before the first render
  useEffect(() => {
    setSelectedStatus([
      { id: 'ACTIVE', text: 'Active' },
      { id: 'ARCHIVED', text: 'Archived' },
      { id: 'DISCARD', text: 'Cancelled' },
      { id: 'DRAFT', text: 'Draft' },
      { id: 'DUPLICATE', text: 'Duplicate' },
      { id: 'EXPIRED', text: 'Expired' },
      { id: 'INACTIVE', text: 'Hold' },
      { id: 'TERMINATED', text: 'Terminated' },
    ]);
  }, []);

  function onSelectedStatusChange(selectedList: MultiSelectOption[]) {
    setSelectedStatus(selectedList);
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
                  <Row>
                    <Col xl={7}>
                      <StyledMultiselect
                        field="acquisitionTeamMembers"
                        ref={multiselectTeamRef}
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
                    <UserRegionSelectContainer field="regionType" placeholder="All Regions" />
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
