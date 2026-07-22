import { Formik } from 'formik';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { ResetButton, SearchButton } from '@/components/common/buttons';
import { Form, Input, Multiselect, Select } from '@/components/common/form';
import { ColButtons } from '@/components/common/styles';
import { REGION_TYPES, RESEARCH_FILE_STATUS_TYPES } from '@/constants/API';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { MultiSelectOption } from '@/interfaces/MultiSelectOption';
import { mapLookupCode } from '@/utils';

import { IResearchFilter } from '../../interfaces';
import { AppCreateUpdateRangeSelect } from './AppCreateUpdateRangeSelect';
import { ResearchFileSelect } from './ResearchFileSelect';

export interface IResearchFilterProps {
  filter?: IResearchFilter;
  createdByOptions: MultiSelectOption[];
  initialValues?: IResearchFilter;
  setFilter: (filter: IResearchFilter) => void;
}

export const defaultResearchFilter: IResearchFilter = {
  pid: '',
  pin: '',
  regionCodes: [],
  researchFileStatusTypeCode: '',
  name: '',
  roadOrAlias: '',
  appCreateUserid: '',
  appLastUpdateUserid: '',
  createdOnEndDate: '',
  createdOnStartDate: '',
  updatedOnEndDate: '',
  updatedOnStartDate: '',
  rfileNumber: '',
  researchSearchBy: 'pid',
  createOrUpdateRange: 'updatedOnStartDate',
  createOrUpdateBy: 'appLastUpdateUserid',
  selectedUser: [],
};

/**
 * Filter bar for research files.
 * @param {IResearchFilterProps} props
 */
export const ResearchFilter: React.FunctionComponent<
  React.PropsWithChildren<IResearchFilterProps>
> = ({ filter, createdByOptions, setFilter }) => {
  const onSearchSubmit = (values: IResearchFilter, { setSubmitting }: any) => {
    const selectedUser = values.selectedUser?.[0]?.id as string | undefined;

    const nextValues = {
      ...values,
      appCreateUserid: values.createOrUpdateBy === 'appCreateUserid' ? selectedUser ?? '' : '',
      appLastUpdateUserid:
        values.createOrUpdateBy === 'appLastUpdateUserid' ? selectedUser ?? '' : '',
    };
    setFilter(nextValues);
    setSubmitting(false);
  };

  const lookupCodes = useLookupCodeHelpers();
  const regionOptions = lookupCodes.getOptionsByType(REGION_TYPES).map(c => ({
    id: c.code,
    text: c.label,
  }));
  const initialFilterValues: IResearchFilter = {
    ...defaultResearchFilter,
    ...filter,
  };

  const resetFilter = () => {
    setFilter({
      ...defaultResearchFilter,
    });
  };

  const researchStatusOptions = lookupCodes
    .getByType(RESEARCH_FILE_STATUS_TYPES)
    .map(c => mapLookupCode(c));

  return (
    <Formik enableReinitialize initialValues={initialFilterValues} onSubmit={onSearchSubmit}>
      {formikProps => (
        <FilterBoxForm className="p-3">
          <Row>
            <Col lg="1">
              <strong>Search by:</strong>
            </Col>
            <Col lg="5">
              <Row>
                <Col lg="12">
                  <Row>
                    <Col lg="4">
                      <Input field="roadOrAlias" placeholder="Road name or alias" />
                    </Col>
                    <StyledSelectInputCol lg="8">
                      <ResearchFileSelect />
                    </StyledSelectInputCol>
                  </Row>
                </Col>
              </Row>
              <Row>
                <Col lg="12">
                  <Row>
                    <Col lg="4">
                      <Select
                        placeholder="All Status"
                        options={researchStatusOptions}
                        field="researchFileStatusTypeCode"
                      />
                    </Col>
                    <Col lg="8">
                      <Multiselect
                        options={regionOptions}
                        field="regionCodes"
                        displayValue="text"
                        placeholder="Select Region(s)"
                      />
                    </Col>
                  </Row>
                </Col>
              </Row>
            </Col>
            <Col lg="5">
              <Row>
                <Col lg="12">
                  <AppCreateUpdateRangeSelect />
                </Col>
              </Row>
              <Row>
                <Col lg="9">
                  <Row className="gx-1 align-items-start">
                    <Col lg="4" className="ps-0">
                      <Select
                        field="createOrUpdateBy"
                        options={[
                          { value: 'appCreateUserid', label: 'Created by' },
                          { value: 'appLastUpdateUserid', label: 'Last updated by' },
                        ]}
                      />
                    </Col>
                    <Col lg="6" className="pl-0 ps-0">
                      <Multiselect
                        field="selectedUser"
                        displayValue="text"
                        placeholder="Search user"
                        options={createdByOptions}
                        selectionLimit={1}
                      />
                    </Col>
                  </Row>
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

export default ResearchFilter;

const StyledSelectInputCol = styled(Col)`
  .form-select {
    max-width: 10rem;
  }
`;

const FilterBoxForm = styled(Form)`
  background-color: ${({ theme }) => theme.css.filterBoxColor};
  border-radius: 0.5rem;
  .idir-input-group {
    .input-group-prepend select {
      width: 16rem;
    }
  }
`;
