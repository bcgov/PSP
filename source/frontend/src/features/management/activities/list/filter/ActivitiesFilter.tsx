import { Formik, FormikHelpers } from 'formik';
import React from 'react';
import { Col, Row } from 'react-bootstrap';

import { ResetButton } from '@/components/common/buttons';
import { SearchButton } from '@/components/common/buttons/SearchButton';
import { Input, Select, SelectOption } from '@/components/common/form';
import { SelectInput } from '@/components/common/List/SelectInput';
import { ColButtons, FilterBoxForm } from '@/components/common/styles';
import { Api_ManagementActivityFilter } from '@/models/api/ManagementActivityFilter';

import { ManagementActivityFilterModel } from '../../models/ManagementActivityFilterModel';

export interface IActivitiesFilterProps {
  filter?: Api_ManagementActivityFilter;
  setFilter: (filter: Api_ManagementActivityFilter) => void;
  activityStatusOptions: SelectOption[];
  activityTypesOptions: SelectOption[];
  activitySubTypesOptions: SelectOption[];
}

export const ActivitiesFilter: React.FC<IActivitiesFilterProps> = ({
  filter,
  setFilter,
  activityStatusOptions,
  activityTypesOptions,
  activitySubTypesOptions,
}) => {
  const onSearchSubmit = async (
    values: ManagementActivityFilterModel,
    formikHelpers: FormikHelpers<ManagementActivityFilterModel>,
  ) => {
    setFilter(values.toApi());
    formikHelpers.setSubmitting(false);
  };

  const resetFilter = () => {
    setFilter(new ManagementActivityFilterModel().toApi());
  };

  return (
    <Formik<ManagementActivityFilterModel>
      enableReinitialize
      initialValues={
        filter ? ManagementActivityFilterModel.fromApi(filter) : new ManagementActivityFilterModel()
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
                    Api_ManagementActivityFilter
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
                  />
                </Col>
              </Row>
              <Row>
                <Col xl="6">
                  <Select
                    options={activityStatusOptions}
                    field="activityStatusCode"
                    placeholder="Select activity status..."
                  />
                </Col>
              </Row>
            </Col>
            <Col xl="5">
              <Row>
                <Col xl="12">
                  <Input
                    field="fileNameOrNumberOrReference"
                    placeholder="Management file number or name or reference number"
                  />
                </Col>
              </Row>
              <Row>
                <Col xl="6">
                  <Select
                    options={activityTypesOptions}
                    field="activityTypeCode"
                    placeholder="Select activity type..."
                  />
                </Col>
                <Col xl="6">
                  <Select
                    options={activitySubTypesOptions}
                    field="activitySubTypeCode"
                    placeholder="Select activity sub-type..."
                  />
                </Col>
                <Col xl="6">
                  <Input
                    field="projectNameOrNumber"
                    placeholder="Enter a project name or number..."
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

export default ActivitiesFilter;
