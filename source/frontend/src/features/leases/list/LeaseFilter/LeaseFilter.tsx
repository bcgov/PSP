import { Formik } from 'formik';
import Multiselect from 'multiselect-react-dropdown';
import React, { useEffect, useMemo, useState } from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaTimes } from 'react-icons/fa';
import styled from 'styled-components';

import { ResetButton, SearchButton } from '@/components/common/buttons';
import { FastDatePicker, Input } from '@/components/common/form';
import { UserRegionSelectContainer } from '@/components/common/form/UserRegionSelect/UserRegionSelectContainer';
import { SelectInput } from '@/components/common/List/SelectInput';
import { SectionField } from '@/components/common/Section/SectionField';
import { FilterBoxForm, InlineFlexDiv } from '@/components/common/styles';
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
  leaseStatusTypes: ['ACTIVE'],
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
    setInitialSelectedStatus(
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
      initialValues={exists(filter) ? filter : defaultFilter}
      onSubmit={onSearchSubmit}
      validationSchema={LeaseFilterSchema}
    >
      {formikProps => (
        <FilterBoxForm className="p-3">
          <Row>
            <Col xs="1">
              <strong>Search by:</strong>
            </Col>
            <Col xs="3">
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
              <StyledMultiselect
                id="program-selector"
                ref={multiselectProgramRef}
                options={programFilterOptions}
                displayValue="text"
                placeholder="Select Program(s)"
                customCloseIcon={<FaTimes size="18px" className="ml-3" />}
                hidePlaceholder={true}
              />
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
            <Col xl="2">
              <StyledMultiselect
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
              />
              <Input field="tenantName" placeholder="Tenant Name" />
            </Col>
            <Col xs="3" className="justify-content-end">
              <SectionField label="Expiry date" labelWidth="3" contentWidth="9">
                <StyledFastDatePicker
                  field="expiryStartDate"
                  formikProps={formikProps}
                  placeholderText="from date"
                />
                <UserRegionSelectContainer field="regionType" placeholder="All Regions" />
              </SectionField>
            </Col>
            <Col>
              <StyledFastDatePicker
                field="expiryEndDate"
                formikProps={formikProps}
                placeholderText="to date"
              />
              <InlineFlexDiv>
                <Input field="details" placeholder="Keyword" />
                <StyledTooltip
                  toolTipId="lease-search-keyword-tooltip"
                  toolTip="Search 'Lease description' and 'Notes' fields"
                />
              </InlineFlexDiv>
            </Col>
            <Col xs="1" className="pr-0">
              <SearchButton disabled={formikProps.isSubmitting} />
              <ResetButton
                disabled={formikProps.isSubmitting}
                onClick={() => {
                  formikProps.resetForm();
                  resetFilter();
                }}
              />
            </Col>
          </Row>
        </FilterBoxForm>
      )}
    </Formik>
  );
};

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

const StyledFastDatePicker = styled(FastDatePicker)`
  .react-datepicker-wrapper.d-block {
    max-width: 100%;
  }
`;

const StyledTooltip = styled(TooltipIcon)`
  position: absolute;
  right: -0.5rem;
`;

export default LeaseFilter;
