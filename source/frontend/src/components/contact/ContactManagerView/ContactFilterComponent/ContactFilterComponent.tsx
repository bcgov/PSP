import { Form, Formik } from 'formik';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import BootstrapForm from 'react-bootstrap/Form';
import styled from 'styled-components';

import { ResetButton, SearchButton } from '@/components/common/buttons';
import { CheckGroup, CheckGroupOption } from '@/components/common/form/CheckGroup';
import { InlineInput } from '@/components/common/form/styles';
import { ColButtons } from '@/components/common/styles';
import { IContactFilter } from '@/components/contact/ContactManagerView/IContactFilter';
import { allContactTypes, RestrictContactType } from '@/constants/contacts';

export const defaultFilter: IContactFilter = {
  summary: '',
  municipality: '',
  searchBy: allContactTypes,
  activeContactsOnly: true,
};

const contactTypeOptions: CheckGroupOption[] = [
  {
    label: 'Organizations',
    value: RestrictContactType.ONLY_ORGANIZATIONS,
  },
  {
    label: 'Individuals',
    value: RestrictContactType.ONLY_INDIVIDUALS,
  },
  {
    label: 'PIMS users',
    value: RestrictContactType.ONLY_PIMSUSERS,
  },
];

export interface IContactFilterComponentProps {
  filter?: IContactFilter;
  setFilter: (filter: IContactFilter) => void;
  showActiveSelector?: boolean;
  restrictContactType?: RestrictContactType;
}

/**
 * Filter bar for contact list.
 * @param {IContactFilterComponentProps} param0
 */
export const ContactFilterComponent: React.FunctionComponent<
  React.PropsWithChildren<IContactFilterComponentProps>
> = ({
  filter,
  setFilter,
  showActiveSelector,
  restrictContactType,
}: IContactFilterComponentProps) => {
  const resetFilter = (values: IContactFilter) => {
    setFilter({ ...defaultFilter, searchBy: [...values.searchBy] });
  };

  return (
    <Formik<IContactFilter>
      enableReinitialize
      initialValues={
        filter ?? {
          ...defaultFilter,
          searchBy: restrictContactType ? [restrictContactType] : allContactTypes,
        }
      }
      onSubmit={(values, { setSubmitting }) => {
        setFilter(values);
        setSubmitting(false);
      }}
      validateOnChange={true}
    >
      {({ resetForm, isSubmitting, submitForm, values, setFieldValue, setFieldTouched }) => (
        <StyledFilterBoxForm
          onKeyUp={(e: any) => {
            if (e.keyCode === 13) {
              submitForm();
            }
          }}
        >
          <Row>
            <Col lg="10" className="pr-0 pt-5">
              <Row>
                <Col sm="auto"></Col>
                <Col lg="auto" className="pr-0">
                  <StyledNameInput field="summary" placeholder="Name" />
                </Col>
                <Col lg="auto" className="pr-0">
                  <StyledCityInput field="municipality" placeholder="City" />
                </Col>
                <Col lg="auto" className="pr-0">
                  <Row className="pl-5 small">
                    <CheckGroup
                      label="Show results by:"
                      isLabelBold={true}
                      field="searchBy"
                      flexDirection="row"
                      checkValues={getRestrictedCheckValues(restrictContactType)}
                    >
                      {showActiveSelector && (
                        <BootstrapForm.Check
                          id="input-activeContactsOnly"
                          name="activeContactsOnly"
                          label="Active"
                          type="checkbox"
                          checked={values.activeContactsOnly}
                          onChange={e => {
                            setFieldValue('activeContactsOnly', e.target.checked);
                          }}
                          onBlur={() => {
                            setFieldTouched('activeContactsOnly', true);
                          }}
                        />
                      )}
                    </CheckGroup>
                  </Row>
                </Col>
              </Row>
            </Col>
            <ColButtons>
              <Row className="pb-10 pt-10">
                <Col lg="auto" className="pr-0">
                  <SearchButton
                    data-testid="contact-filter-search"
                    disabled={isSubmitting || values.searchBy.length === 0}
                    onClick={() => {
                      submitForm();
                    }}
                  >
                    <span>Search</span>
                  </SearchButton>
                </Col>
                <Col lg="auto">
                  <ResetButton
                    disabled={isSubmitting}
                    onClick={() => {
                      const resetValues = {
                        ...defaultFilter,
                        searchBy: restrictContactType
                          ? [restrictContactType]
                          : [...defaultFilter.searchBy],
                      };
                      resetForm({ values: resetValues });
                      resetFilter(resetValues);
                    }}
                  />
                </Col>
              </Row>
            </ColButtons>
          </Row>
        </StyledFilterBoxForm>
      )}
    </Formik>
  );
};

const getRestrictedCheckValues = (
  restrictContactType?: RestrictContactType,
): CheckGroupOption[] => {
  switch (restrictContactType) {
    case RestrictContactType.ONLY_INDIVIDUALS:
      return [{ label: 'Individuals', value: 'persons' }];

    case RestrictContactType.ONLY_ORGANIZATIONS:
      return [{ label: 'Organizations', value: 'organizations' }];

    case RestrictContactType.ONLY_PIMSUSERS:
      return [{ label: 'Pims users', value: 'pimsusers' }];

    default:
      return contactTypeOptions;
  }
};

const StyledFilterBoxForm = styled(Form)`
  background-color: ${({ theme }) => theme.css.filterBoxColor};
  border-radius: 0.4rem;
  margin: 1rem;
  padding-right: 3rem;
  max-width: 95%;
`;

export const StyledNameInput = styled(InlineInput)`
  max-width: 30em;
`;

export const StyledCityInput = styled(InlineInput)`
  max-width: 25rem;
`;
