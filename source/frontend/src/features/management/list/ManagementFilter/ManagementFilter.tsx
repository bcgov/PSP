import { Formik, FormikHelpers } from 'formik';
import React from 'react';
import { Col, Row } from 'react-bootstrap';

import { ResetButton } from '@/components/common/buttons';
import { SearchButton } from '@/components/common/buttons/SearchButton';
import { Input, Select, SelectOption, TypeaheadSelect } from '@/components/common/form';
import { SelectInput } from '@/components/common/List/SelectInput';
import { ColButtons, FilterBoxForm } from '@/components/common/styles';
import { ApiGen_Concepts_ManagementFileTeam } from '@/models/api/generated/ApiGen_Concepts_ManagementFileTeam';
import { Api_ManagementFilter } from '@/models/api/ManagementFilter';
import { formatApiPersonNames } from '@/utils/personUtils';

import { ManagementFilterModel } from '../models';

export interface IManagementFilterProps {
  filter?: Api_ManagementFilter;
  setFilter: (filter: Api_ManagementFilter) => void;
  managementTeam: ApiGen_Concepts_ManagementFileTeam[];
  fileStatusOptions: SelectOption[];
  managementPurposeOptions: SelectOption[];
}

export const ManagementFilter: React.FC<IManagementFilterProps> = ({
  filter,
  setFilter,
  managementTeam,
  fileStatusOptions,
  managementPurposeOptions,
}) => {
  const onSearchSubmit = async (
    values: ManagementFilterModel,
    formikHelpers: FormikHelpers<ManagementFilterModel>,
  ) => {
    setFilter(values.toApi());
    formikHelpers.setSubmitting(false);
  };

  const resetFilter = () => {
    setFilter(new ManagementFilterModel().toApi());
  };

  const managementTeamOptions = React.useMemo<SelectOption[]>(() => {
    const arr = managementTeam ?? [];
    return arr.map<SelectOption>(t => ({
      value: t.personId ? `P-${t.personId}` : `O-${t.organizationId}`,
      label: t.personId && t.person ? formatApiPersonNames(t.person) : t.organization?.name ?? '',
    }));
  }, [managementTeam]);

  return (
    <Formik<ManagementFilterModel>
      enableReinitialize
      initialValues={
        filter
          ? ManagementFilterModel.fromApi(filter, managementTeamOptions ?? [])
          : new ManagementFilterModel()
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
                    Api_ManagementFilter
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
                  <TypeaheadSelect
                    field="managementTeamMember"
                    options={managementTeamOptions}
                    placeholder="Team Member"
                  />
                </Col>
                <Col xl="6">
                  <Select
                    options={fileStatusOptions}
                    field="managementFileStatusCode"
                    placeholder="All Status"
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
                    options={managementPurposeOptions}
                    field="managementFilePurposeCode"
                    placeholder="Select management purpose..."
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

export default ManagementFilter;
