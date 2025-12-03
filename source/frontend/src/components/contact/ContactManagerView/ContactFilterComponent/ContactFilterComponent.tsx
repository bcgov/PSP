import { Form, Formik } from 'formik';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaBuilding, FaUser } from 'react-icons/fa';
import styled from 'styled-components';

import ActiveIndicator from '@/components/common/ActiveIndicator';
import { ResetButton, SearchButton } from '@/components/common/buttons';
import ActiveFilterCheck from '@/components/common/form/ActiveFilterCheck';
import { RadioGroup } from '@/components/common/form/RadioGroup';
import { InlineInput } from '@/components/common/form/styles';
import { ColButtons, FlexRowNoGap } from '@/components/common/styles';
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
          <Row>
            <Col lg="10" className="pr-0">
              <Row className="p-5">
                <RadioGroup
                  label="Search by:"
                  isLabelBold={true}
                  field="searchBy"
                  radioGroupClassName="pb-3"
                  flexDirection="row"
                  radioValues={getRestrictedRadioValues(restrictContactType)}
                />
              </Row>
              <Row>
                <Col sm="auto"></Col>
                <Col lg="auto" className="pr-0">
                  <StyledNameInput field="summary" placeholder="Name" />
                </Col>
                <Col lg="auto" className="pr-0">
                  <StyledCityInput
                    field="municipality"
                    label="City"
                    placeholder="City of contact's address"
                  />
                </Col>
                <Col lg="auto" className="pr-0">
                  {showActiveSelector && (
                    <FlexRowNoGap>
                      <ActiveFilterCheck<IContactFilter>
                        fieldName="activeContactsOnly"
                        setFilter={setFilter}
                      />
                      <ActiveIndicator active size={16} />
                      <span className="ml-1">Show active only</span>
                    </FlexRowNoGap>
                  )}
                </Col>
              </Row>
            </Col>
            <ColButtons>
              <Row className="pb-10 pt-10">
                <Col lg="auto" className="pr-0">
                  <SearchButton
                    disabled={isSubmitting}
                    onClick={() => {
                      submitForm();
                    }}
                  />
                </Col>
                <Col lg="auto">
                  <ResetButton
                    disabled={isSubmitting}
                    onClick={() => {
                      resetForm({ values: { ...defaultFilter, searchBy: values.searchBy } });
                      resetFilter(values);
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

const getRestrictedRadioValues = (restrictContactType?: RestrictContactType) => {
  if (restrictContactType === RestrictContactType.ONLY_INDIVIDUALS) {
    return [
      {
        radioLabel: (
          <>
            <FaUser size={20} fill="#1a5496" />
            <span> Individuals</span>
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
            <FaUser size={20} fill="#1a5496" />
            <span> Organizations</span>
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
            <FaBuilding size={20} fill="#1a5496" />
            <span> Organizations</span>
          </>
        ),
        radioValue: 'organizations',
      },
      {
        radioLabel: (
          <>
            <FaUser size={20} fill="#1a5496" />
            <span> Individuals</span>
          </>
        ),
        radioValue: 'persons',
      },
      {
        radioLabel: (
          <>
            <FaBuilding size={20} fill="#1a5496" />+<FaUser size={20} fill="#1a5496" />
            <span> All</span>
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
  max-width: 95%;
`;

export const StyledNameInput = styled(InlineInput)`
  max-width: 30em;
`;

export const StyledCityInput = styled(InlineInput)`
  max-width: 25rem;
`;
