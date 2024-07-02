import { Form, Formik } from 'formik';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaRegBuilding, FaRegUser } from 'react-icons/fa';
import { FaCircle } from 'react-icons/fa';
import styled from 'styled-components';

import { ResetButton, SearchButton } from '@/components/common/buttons';
import ActiveFilterCheck from '@/components/common/form/ActiveFilterCheck';
import { RadioGroup } from '@/components/common/form/RadioGroup';
import { InlineInput } from '@/components/common/form/styles';
import { VerticalBar } from '@/components/common/VerticalBar';
import { IContactFilter } from '@/components/contact/ContactManagerView/IContactFilter';

export const defaultFilter: IContactFilter = {
  summary: '',
  municipality: '',
  searchBy: 'all',
  activeContactsOnly: true,
};

export enum RestrictContactType {
  ONLY_INDIVIDUALS = 'persons',
  ONLY_ORGANIZATIONS = 'organizations',
  ALL = 'all',
}

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
    setFilter({ ...defaultFilter, searchBy: values.searchBy });
  };

  return (
    <Formik
      enableReinitialize
      initialValues={
        filter ?? {
          ...defaultFilter,
          searchBy: restrictContactType?.toString() ?? RestrictContactType.ALL,
        }
      }
      onSubmit={(values, { setSubmitting }) => {
        setFilter(values);
        setSubmitting(false);
      }}
      validateOnChange={true}
    >
      {({ resetForm, isSubmitting, values, submitForm }) => (
        <StyledFilterBoxForm
          onKeyUp={(e: any) => {
            if (e.keyCode === 13) {
              submitForm();
            }
          }}
        >
          <Row className="p-5">
            <Row className="pb-5">
              <Col xs="auto">
                <RadioGroup
                  label="Search by:"
                  field="searchBy"
                  radioGroupClassName="pb-3"
                  radioValues={getRestrictedRadioValues(restrictContactType)}
                />
              </Col>
              <Col lg="auto" className="pl-0">
                <StyledNameInput field="summary" placeholder="Name of person or organization" />
              </Col>
            </Row>
            <Col xl="auto">
              <Row className="ml-10">
                <Col className="pl-0">
                  <StyledCityInput
                    field="municipality"
                    label="City"
                    placeholder="City of contact's address"
                  />
                </Col>
                <Col className="pl-0">
                  {showActiveSelector && (
                    <>
                      <ActiveFilterCheck<IContactFilter>
                        fieldName="activeContactsOnly"
                        setFilter={setFilter}
                      />
                      <ActiveIndicator>
                        <FaCircle size={16} />
                      </ActiveIndicator>
                      <span>
                        <b>Active</b> contacts only
                      </span>
                    </>
                  )}
                </Col>
              </Row>
            </Col>
            <Col md="auto" className="ml-2">
              <Row>
                <Col className="pr-0" xs="auto">
                  <VerticalBar />
                </Col>
                <Col>
                  <Row>
                    <StyledColButton xs="auto">
                      <SearchButton
                        type="button"
                        disabled={isSubmitting}
                        onClick={() => {
                          submitForm();
                        }}
                      />
                    </StyledColButton>
                    <StyledColButton xs="auto">
                      <ResetButton
                        type=""
                        disabled={isSubmitting}
                        onClick={() => {
                          resetForm({ values: { ...defaultFilter, searchBy: values.searchBy } });
                          resetFilter(values);
                        }}
                      />
                    </StyledColButton>
                  </Row>
                </Col>
              </Row>
            </Col>
          </Row>
        </StyledFilterBoxForm>
      )}
    </Formik>
  );
};

const getRestrictedRadioValues = (restrictContactType?: RestrictContactType) => {
  if (restrictContactType === RestrictContactType.ONLY_INDIVIDUALS) {
    return [
      {
        radioLabel: (
          <>
            <FaRegUser size={20} />
            <span>Individuals</span>
          </>
        ),
        radioValue: 'persons',
      },
    ];
  } else if (restrictContactType === RestrictContactType.ONLY_ORGANIZATIONS) {
    return [
      {
        radioLabel: (
          <>
            <FaRegUser size={20} />
            <span>Organizations</span>
          </>
        ),
        radioValue: 'organizations',
      },
    ];
  } else {
    return [
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
    ];
  }
};

const StyledFilterBoxForm = styled(Form)`
  background-color: ${({ theme }) => theme.css.filterBoxColor};
  border-radius: 0.4rem;
  padding: 1rem;
  max-width: 85%;
  @media only screen and (max-width: 1199px) {
  max-width: 50%;
`;

const StyledColButton = styled(Col)`
  padding-right: 0rem;
  padding-left: 1rem;
`;

export const StyledNameInput = styled(InlineInput)`
  max-width: 30em;
`;

export const StyledCityInput = styled(InlineInput)`
  max-width: 25rem;
`;

export const ActiveIndicator = styled.div`
  display: inline-block;
  margin: 0rem 0.5rem;
  padding: 0.2rem;
  color: ${props => props.theme.bcTokens.iconsColorSuccess};
`;
