import { Form, Input, Select } from 'components/common/form';
import ResetButton from 'components/common/form/ResetButton';
import SearchButton from 'components/common/form/SearchButton';
import { NoPaddingRow as Row } from 'components/common/styles';
import { REGION_TYPES, RESEARCH_FILE_STATUS_TYPES } from 'constants/API';
import { Formik } from 'formik';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import React from 'react';
import { Col } from 'react-bootstrap';
import styled from 'styled-components';
import { mapLookupCode } from 'utils';

import { IResearchFilter } from '../../interfaces';
import AppCreateUpdateBySelect from './AppCreateUpdateBySelect';
import { AppCreateUpdateRangeSelect } from './AppCreateUpdateRangeSelect';
import { ResearchFileSelect } from './ResearchFileSelect';

export interface IResearchFilterProps {
  filter?: IResearchFilter;
  setFilter: (filter: IResearchFilter) => void;
}

export const defaultFilter: IResearchFilter = {
  region: '',
  researchFileStatusCode: '',
  name: '',
  roadOrAlias: '',
  createdByIdir: '',
  updatedByIdir: '',
  createdOnEndDate: '',
  createdOnStartDate: '',
  updatedOnEndDate: '',
  updatedOnStartDate: '',
  rFileNumber: '',
  researchSearchBy: 'name',
  createOrUpdateRange: 'updatedOnStartDate',
  createOrUpdateBy: 'updatedByIdir',
};

/**
 * Filter bar for leases and license.
 * @param {IResearchFilterProps} props
 */
export const ResearchFilter: React.FunctionComponent<IResearchFilterProps> = ({
  filter,
  setFilter,
}) => {
  const onSearchSubmit = (values: IResearchFilter, { setSubmitting }: any) => {
    values = { ...values };
    setFilter(values);
    setSubmitting(false);
  };
  const resetFilter = () => {
    setFilter(defaultFilter);
  };

  const lookupCodes = useLookupCodeHelpers();

  const regionOptions = lookupCodes.getByType(REGION_TYPES).map(c => mapLookupCode(c));

  const researchStatusOptions = lookupCodes
    .getByType(RESEARCH_FILE_STATUS_TYPES)
    .map(c => mapLookupCode(c));

  return (
    <Formik enableReinitialize initialValues={filter ?? defaultFilter} onSubmit={onSearchSubmit}>
      {formikProps => (
        <FilterBoxForm className="p-3">
          <Row>
            <Col xl="1">
              <strong>Search by:</strong>
            </Col>
            <Col xl="5">
              <Row>
                <Col>
                  <Row>
                    <Col xl="4">
                      <Select options={regionOptions} field="region" placeholder="All Regions" />
                    </Col>
                    <Col xl="1"></Col>
                    <Col xl="7">
                      <ResearchFileSelect />
                    </Col>
                  </Row>
                </Col>
              </Row>
              <Row>
                <Col>
                  <Row>
                    <Col xl="4">
                      <Select
                        options={researchStatusOptions}
                        field="researchFileStatusTypeCode"
                        placeholder="Active"
                      />
                    </Col>
                    <Col xl="1"></Col>
                    <Col xl="7">
                      <Input field="roadOrAlias" placeholder="Road name or alias" />
                    </Col>
                  </Row>
                </Col>
              </Row>
            </Col>
            <Col xl="5">
              <Row>
                <Col>
                  <AppCreateUpdateRangeSelect />
                </Col>
              </Row>
              <Row>
                <Col md={12}>
                  <AppCreateUpdateBySelect />
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

export default ResearchFilter;

const FilterBoxForm = styled(Form)`
  background-color: ${({ theme }) => theme.css.filterBoxColor};
  border-radius: 0.5rem;
`;

const ColButtons = styled(Col)`
  border-left: 0.2rem solid white;
`;
