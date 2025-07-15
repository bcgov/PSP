import { Formik } from 'formik';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { ResetButton, SearchButton } from '@/components/common/buttons';
import { Form, Input, Select } from '@/components/common/form';
import { SelectInput } from '@/components/common/List/SelectInput';
import { ColButtons } from '@/components/common/styles';
import { REGION_TYPES, RESEARCH_FILE_STATUS_TYPES } from '@/constants/API';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { mapLookupCode } from '@/utils';

import { IResearchFilter } from '../../interfaces';
import { AppCreateUpdateRangeSelect } from './AppCreateUpdateRangeSelect';
import { ResearchFileSelect } from './ResearchFileSelect';

export interface IResearchFilterProps {
  filter?: IResearchFilter;
  setFilter: (filter: IResearchFilter) => void;
}

export const defaultResearchFilter: IResearchFilter = {
  pid: '',
  pin: '',
  regionCode: '',
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
};

/**
 * Filter bar for research files.
 * @param {IResearchFilterProps} props
 */
export const ResearchFilter: React.FunctionComponent<
  React.PropsWithChildren<IResearchFilterProps>
> = ({ filter, setFilter }) => {
  const onSearchSubmit = (values: IResearchFilter, { setSubmitting }: any) => {
    values = { ...values };
    setFilter(values);
    setSubmitting(false);
  };
  const resetFilter = () => {
    setFilter(defaultResearchFilter);
  };

  const lookupCodes = useLookupCodeHelpers();

  const regionOptions = lookupCodes.getByType(REGION_TYPES).map(c => mapLookupCode(c));

  const researchStatusOptions = lookupCodes
    .getByType(RESEARCH_FILE_STATUS_TYPES)
    .map(c => mapLookupCode(c));

  return (
    <Formik
      enableReinitialize
      initialValues={filter ?? defaultResearchFilter}
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
                <Col lg="12">
                  <Row>
                    <Col lg="4">
                      <Select
                        options={regionOptions}
                        field="regionCode"
                        placeholder="All Regions"
                      />
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
                      <Input field="roadOrAlias" placeholder="Road name or alias" />
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
                <Col lg="8">
                  <SelectInput<
                    {
                      appCreateUserid: string;
                      appLastUpdateUserid: string;
                    },
                    IResearchFilterProps
                  >
                    field="createOrUpdateBy"
                    defaultKey="appLastUpdateUserid"
                    selectOptions={[
                      {
                        key: 'appCreateUserid',
                        placeholder: `User's IDIR`,
                        label: 'Created by',
                      },
                      {
                        key: 'appLastUpdateUserid',
                        placeholder: `User's IDIR`,
                        label: 'Updated by',
                      },
                    ]}
                    className="idir-input-group"
                  />
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
