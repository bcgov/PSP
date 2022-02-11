import { ReactComponent as Active } from 'assets/images/active.svg';
import { SearchButton } from 'components/common/form';
import { RadioGroup } from 'components/common/form/RadioGroup';
import ResetButton from 'components/common/form/ResetButton';
import { InlineInput } from 'components/common/form/styles';
import { Form, Formik } from 'formik';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaRegBuilding, FaRegUser } from 'react-icons/fa';
import styled from 'styled-components';

import { IContactFilterComponent } from '../../interfaces';
import ActiveFilterCheck from './ActiveFilterCheck';

export const defaultFilter: IContactFilterComponent = {
  summary: '',
  municipality: '',
  searchBy: 'all',
  activeContactsOnly: true,
};

export interface IContactFilterProps {
  filter?: IContactFilterComponent;
  setFilter: (filter: IContactFilterComponent) => void;
  showActiveSelector?: boolean;
}

/**
 * Filter bar for contact list.
 * @param {IContactFilterProps} param0
 */
export const ContactFilter: React.FunctionComponent<IContactFilterProps> = ({
  filter,
  setFilter,
  showActiveSelector,
}: IContactFilterProps) => {
  const resetFilter = (values: IContactFilterComponent) => {
    setFilter({ ...defaultFilter, searchBy: values.searchBy });
  };
  return (
    <Formik
      enableReinitialize
      initialValues={filter ?? defaultFilter}
      onSubmit={(values, { setSubmitting }) => {
        setFilter(values);
        setSubmitting(false);
      }}
      validateOnChange={true}
    >
      {({ resetForm, isSubmitting, values, submitForm }) => (
        <FilterBoxForm>
          <Row>
            <Col xs="auto">
              <RadioGroup
                label="Search by:"
                field="searchBy"
                radioGroupClassName="pb-3"
                radioValues={[
                  {
                    radioLabel: (
                      <>
                        <FaRegBuilding size={20} />
                        <span>Organizations</span>
                      </>
                    ),
                    radioValue: 'organizations',
                  },
                  {
                    radioLabel: (
                      <>
                        <FaRegUser size={20} />
                        <span>Individuals</span>
                      </>
                    ),
                    radioValue: 'persons',
                  },
                  {
                    radioLabel: (
                      <>
                        <FaRegBuilding size={20} />+<FaRegUser size={20} />
                        <span>All</span>
                      </>
                    ),
                    radioValue: 'all',
                  },
                ]}
              />
            </Col>
            <Col>
              <Row>
                <Col className="pl-0">
                  <NameInput field="summary" placeholder="Name of person or organization" />
                </Col>
                <Col xs="auto" className="pl-0">
                  <CityInput field="municipality" label="City" />
                </Col>
              </Row>
            </Col>
            <Col xs="auto">
              <Row className="align-items-center">
                <ColButton xs="auto">
                  <SearchButton
                    type="button"
                    disabled={isSubmitting}
                    onClick={() => {
                      submitForm();
                    }}
                  />
                </ColButton>
                <ColButton xs="auto">
                  <ResetButton
                    type=""
                    disabled={isSubmitting}
                    onClick={() => {
                      resetForm({ values: { ...defaultFilter, searchBy: values.searchBy } });
                      resetFilter(values);
                    }}
                  />
                </ColButton>

                <Col className="pl-0">
                  {showActiveSelector && (
                    <>
                      <ActiveFilterCheck setFilter={setFilter} />
                      <Active />
                      <span>Show Active contacts only</span>
                    </>
                  )}
                </Col>
              </Row>
            </Col>
          </Row>
        </FilterBoxForm>
      )}
    </Formik>
  );
};

const FilterBoxForm = styled(Form)`
  background-color: ${({ theme }) => theme.css.filterBoxColor};
  border-radius: 0.4rem;
  padding: 1rem;
  max-width: 85em;
`;

const ColButton = styled(Col)`
  padding-right: 0rem;
  padding-left: 0rem;
`;

export const NameInput = styled(InlineInput)`
  max-width: 32em;
`;

export const CityInput = styled(InlineInput)`
  max-width: 20rem;
`;
