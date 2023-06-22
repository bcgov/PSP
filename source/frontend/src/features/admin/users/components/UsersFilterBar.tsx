import { Formik } from 'formik';
import React from 'react';
import { Row } from 'react-bootstrap';
import Col from 'react-bootstrap/Col';

import { ReactComponent as Active } from '@/assets/images/active.svg';
import { ResetButton, SearchButton } from '@/components/common/buttons';
import { Input, Select } from '@/components/common/form';
import ActiveFilterCheck from '@/components/common/form/ActiveFilterCheck';
import { FilterBoxForm } from '@/components/common/styles';
import TooltipWrapper from '@/components/common/TooltipWrapper';
import * as API from '@/constants/API';
import { useLookupCodeHelpers } from '@/hooks/useLookupCodeHelpers';
import { IUsersFilter } from '@/interfaces';
interface IProps {
  values?: IUsersFilter;
  onChange: (filter: IUsersFilter) => void;
}

export const UsersFilterBar: React.FC<React.PropsWithChildren<IProps>> = ({ values, onChange }) => {
  const { getOptionsByType } = useLookupCodeHelpers();
  const roles = getOptionsByType(API.ROLE_TYPES);
  const regions = getOptionsByType(API.REGION_TYPES);

  return (
    <Formik<IUsersFilter>
      onSubmit={values => {
        onChange({
          ...values,
          region: values.region ? values.region : undefined,
          role: values.role ? values.role : undefined,
        });
      }}
      initialValues={{ ...defaultUserFilter, ...values }}
      enableReinitialize
    >
      {formikProps => (
        <FilterBoxForm className="p-3">
          <Row>
            <Col md={1} sm={1}>
              Search By:
            </Col>
            <Col md={6} sm={6}>
              <Row>
                <Col className="bar-item" md={6}>
                  <Select field="role" placeholder="All Roles" options={roles} type="number" />
                </Col>
                <Col className="bar-item" md={6}>
                  <Input field="businessIdentifierValue" placeholder="IDIR/Name" />
                </Col>
              </Row>
              <Row>
                <Col className="bar-item" md={6}>
                  <Select
                    field="region"
                    placeholder="All Regions"
                    options={regions}
                    type="number"
                  />
                </Col>

                <Col className="bar-item" md={6}>
                  <Input field="email" placeholder="Email" />
                </Col>
              </Row>
            </Col>
            <Col className="actions" md={2} sm={2}>
              <Row>
                <Col className="d-flex">
                  <TooltipWrapper toolTipId="map-filter-search-tooltip" toolTip="Search">
                    <SearchButton className="mr-2" />
                  </TooltipWrapper>
                  <TooltipWrapper toolTipId="map-filter-reset-tooltip" toolTip="Reset Filter">
                    <ResetButton
                      type=""
                      disabled={false}
                      onClick={() => {
                        formikProps.resetForm({ values: defaultUserFilter });
                        formikProps.submitForm();
                      }}
                    />
                  </TooltipWrapper>
                </Col>
              </Row>
            </Col>
            <Col md={3} sm={3}>
              <Row>
                <Col>
                  <ActiveFilterCheck<IUsersFilter> fieldName="activeOnly" setFilter={onChange} />
                  <Active />
                  <span className="ml-1">Show active users only</span>
                </Col>
              </Row>
            </Col>
          </Row>
        </FilterBoxForm>
      )}
    </Formik>
  );
};

export const defaultUserFilter: IUsersFilter = {
  businessIdentifierValue: '',
  email: '',
  role: undefined,
  region: undefined,
  activeOnly: true,
};
