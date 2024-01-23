import { Formik, FormikHelpers, FormikProps } from 'formik';
import React, { useMemo } from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { ResetButton, SearchButton } from '@/components/common/buttons';
import { Form, Input, Multiselect, Select } from '@/components/common/form';
import { SelectInput } from '@/components/common/List/SelectInput';
import { ACQUISITION_FILE_STATUS_TYPES } from '@/constants/API';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { ApiGen_Concepts_AcquisitionFileTeam } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileTeam';
import { exists, mapLookupCode } from '@/utils';
import { formatApiPersonNames } from '@/utils/personUtils';

import {
  AcquisitionFilterModel,
  ApiGen_Concepts_AcquisitionFilter,
  MultiSelectOption,
} from '../interfaces';

export interface IAcquisitionFilterProps {
  filter?: ApiGen_Concepts_AcquisitionFilter;
  setFilter: (filter: ApiGen_Concepts_AcquisitionFilter) => void;
  acquisitionTeam: ApiGen_Concepts_AcquisitionFileTeam[];
}

/**
 * Filter bar for acquisition files.
 * @param {IAcquisitionFilterProps} props
 */
export const AcquisitionFilter: React.FC<React.PropsWithChildren<IAcquisitionFilterProps>> = ({
  filter,
  setFilter,
  acquisitionTeam,
}) => {
  const onSearchSubmit = (
    values: AcquisitionFilterModel,
    formikHelpers: FormikHelpers<AcquisitionFilterModel>,
  ) => {
    setFilter(values.toApi());
    formikHelpers.setSubmitting(false);
  };

  const resetFilter = () => {
    setFilter(new AcquisitionFilterModel().toApi());
  };

  const onResetClick = (formikProps: FormikProps<AcquisitionFilterModel>) => {
    resetFilter();
    formikProps.resetForm();
  };

  const lookupCodes = useLookupCodeHelpers();

  const acquisitionStatusOptions = lookupCodes
    .getByType(ACQUISITION_FILE_STATUS_TYPES)
    .map(c => mapLookupCode(c));

  const acquisitionTeamOptions = useMemo(() => {
    if (exists(acquisitionTeam)) {
      return acquisitionTeam?.map<MultiSelectOption>(x => ({
        id: x.personId ? `P-${x.personId}` : `O-${x.organizationId}`,
        text: x.personId ? formatApiPersonNames(x.person) : x.organization?.name ?? '',
      }));
    } else {
      return [];
    }
  }, [acquisitionTeam]);

  return (
    <Formik<AcquisitionFilterModel>
      enableReinitialize
      initialValues={
        filter
          ? AcquisitionFilterModel.fromApi(filter, acquisitionTeam || [])
          : new AcquisitionFilterModel()
      }
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
                <Col xl="7">
                  <Multiselect
                    field="acquisitionTeamMembers"
                    displayValue="text"
                    placeholder="Team member"
                    hidePlaceholder
                    options={acquisitionTeamOptions}
                    selectionLimit={1}
                  />
                </Col>
                <Col xl="4">
                  <Select
                    options={acquisitionStatusOptions}
                    field="acquisitionFileStatusTypeCode"
                    placeholder="All Status"
                  />
                </Col>
              </Row>
            </Col>
            <Col xl="5">
              <Row>
                <Col xl="12">
                  <Input
                    field="acquisitionFileNameOrNumber"
                    placeholder="Acquisition file number or name or historical file number"
                  />
                </Col>
              </Row>
              <Row>
                <Col xl="12">
                  <Input
                    field="projectNameOrNumber"
                    placeholder="Ministry or alternate project name or number"
                  />
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
                    onClick={() => onResetClick(formikProps)}
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

const ColButtons = styled(Col)`
  border-left: 0.2rem solid white;
`;
