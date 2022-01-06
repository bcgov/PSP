import { Button } from 'components/common/form';
import { FieldArray } from 'formik';
import { ICreateContactMethodForm } from 'interfaces/ICreateContact';
import React from 'react';

import { ContactPhone } from './ContactPhone';

export interface IContactPhoneList {
  field: string;
  contactPhones: ICreateContactMethodForm[];
}

/**
 * Formik Field array wrapper around email contacts.
 * @param {IContactPhoneList} param0
 */
export const ContactPhoneList: React.FunctionComponent<IContactPhoneList> = ({
  field,
  contactPhones = [],
}) => {
  return (
    <FieldArray name={field}>
      {({ push, remove }) => (
        <>
          {contactPhones.map((email, index) => (
            <ContactPhone
              key={`${field}.${index}`}
              namespace={`${field}.${index}`}
              onRemove={index > 0 ? () => remove(index) : undefined}
            />
          ))}
          <Button variant="link" onClick={() => push({ value: '', contactMethodTypeCode: '' })}>
            + Add another phone number
          </Button>
        </>
      )}
    </FieldArray>
  );
};
