import { Formik, FormikHelpers } from 'formik';
import React from 'react';
import { Col, Row } from 'react-bootstrap';

import { ResetButton, SearchButton } from '@/components/common/buttons';
import { Input, Select, SelectOption, TypeaheadSelect } from '@/components/common/form';
import { SelectInput } from '@/components/common/List/SelectInput';
import { FilterBoxForm } from '@/components/common/styles';
import { Api_DispositionFilter } from '@/models/api/DispositionFilter';
import { ApiGen_Concepts_DispositionFileTeam } from '@/models/api/generated/ApiGen_Concepts_DispositionFileTeam';
import { formatApiPersonNames } from '@/utils/personUtils';

import { DispositionFilterModel } from '../models';
import { ColButtons } from '../styles';

export interface IDispositionFilterProps {
  filter?: Api_DispositionFilter;
  setFilter: (filter: Api_DispositionFilter) => void;
  dispositionTeam: ApiGen_Concepts_DispositionFileTeam[];
  fileStatusOptions: SelectOption[];
  dispositionStatusOptions: SelectOption[];
  dispositionTypeOptions: SelectOption[];
}

export const DispositionFilter: React.FC<IDispositionFilterProps> = ({
  filter,
  setFilter,
  dispositionTeam,
  fileStatusOptions,
  dispositionStatusOptions,
  dispositionTypeOptions,
}) => {
  const onSearchSubmit = async (
    values: DispositionFilterModel,
    formikHelpers: FormikHelpers<DispositionFilterModel>,
  ) => {
    setFilter(values.toApi());
    formikHelpers.setSubmitting(false);
  };

  const resetFilter = () => {
    setFilter(new DispositionFilterModel().toApi());
  };

  const dispositionTeamOptions = React.useMemo<SelectOption[]>(() => {
    const arr = dispositionTeam || [];
    return arr.map<SelectOption>(t => ({
      value: t.personId ? `P-${t.personId}` : `O-${t.organizationId}`,
      label: t.personId && t.person ? formatApiPersonNames(t.person) : t.organization?.name ?? '',
    }));
  }, [dispositionTeam]);

  return (
    <Formik<DispositionFilterModel>
      enableReinitialize
      initialValues={
        filter
          ? DispositionFilterModel.fromApi(filter, dispositionTeamOptions || [])
          : new DispositionFilterModel()
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
                    Api_DispositionFilter
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
                <Col xl="7">
                  <TypeaheadSelect
                    field="dispositionTeamMember"
                    options={dispositionTeamOptions}
                    placeholder="Team Member"
                  />
                </Col>
                <Col xl="4">
                  <Select
                    options={fileStatusOptions}
                    field="dispositionFileStatusCode"
                    placeholder="Select file status..."
                  />
                </Col>
              </Row>
            </Col>
            <Col xl="5">
              <Row>
                <Col xl="12">
                  <Input
                    field="fileNameOrNumberOrReference"
                    placeholder="Disposition file number or name or reference number"
                  />
                </Col>
              </Row>
              <Row>
                <Col xl="6">
                  <Select
                    options={dispositionStatusOptions}
                    field="dispositionStatusCode"
                    placeholder="Select disposition status..."
                  />
                </Col>
                <Col xl="6">
                  <Select
                    options={dispositionTypeOptions}
                    field="dispositionTypeCode"
                    placeholder="Select disposition type..."
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

export default DispositionFilter;
