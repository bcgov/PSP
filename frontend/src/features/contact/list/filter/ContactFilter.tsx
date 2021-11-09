import { ReactComponent as Active } from 'assets/images/active.svg';
import { RadioGroup } from 'components/common/form/RadioGroup';
import ResetButton from 'components/common/form/ResetButton';
import { Label } from 'components/common/Label';
import { Formik } from 'formik';
import React from 'react';
import { FaRegBuilding, FaRegUser } from 'react-icons/fa';

import { IContactFilter } from '../../interfaces';
import * as Styled from '../styles';
import ActiveFilterCheck from './ActiveFilterCheck';

export interface IContactFilterProps {
  filter?: IContactFilter;
  setFilter: (filter: IContactFilter) => void;
}

/**
 * Filter bar for contact list.
 * @param {IContactFilterProps} param0
 */
export const ContactFilter: React.FunctionComponent<IContactFilterProps> = ({
  filter,
  setFilter,
}: IContactFilterProps) => {
  const resetFilter = (values: IContactFilter) => {
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
      {({ resetForm, isSubmitting, values }) => (
        <>
          <Styled.FilterBox>
            <RadioGroup
              label="Search by:"
              field="searchBy"
              radioValues={[
                {
                  radioLabel: (
                    <>
                      <FaRegBuilding size={20} />
                      <p>Organizations</p>
                    </>
                  ),
                  radioValue: 'organizations',
                },
                {
                  radioLabel: (
                    <>
                      <FaRegUser size={20} />
                      <p>Individuals</p>
                    </>
                  ),
                  radioValue: 'persons',
                },
                {
                  radioLabel: (
                    <>
                      <FaRegBuilding size={20} />+<FaRegUser size={20} />
                      <p>All</p>
                    </>
                  ),
                  radioValue: 'all',
                },
              ]}
            />
            <Styled.Spacer />
            <Styled.LongInlineInput field="summary" placeholder="Name of person or organization" />
            <Styled.ShortInlineInput field="municipality" label="City" />
            <Styled.SmallSearchButton disabled={isSubmitting} />
            <ResetButton
              type=""
              disabled={isSubmitting}
              onClick={() => {
                resetForm({ values: { ...defaultFilter, searchBy: values.searchBy } });
                resetFilter(values);
              }}
            />
            <ActiveFilterCheck setFilter={setFilter} />
            <Active />
            <Label>Show Active contacts only</Label>
          </Styled.FilterBox>
        </>
      )}
    </Formik>
  );
};

export const defaultFilter: IContactFilter = {
  summary: '',
  municipality: '',
  searchBy: 'all',
  activeContactsOnly: true,
};

export default ContactFilter;
