import { Button } from 'components/common/form';
import { FieldArray } from 'formik';
import { IEditableContactMethodForm } from 'interfaces/editable-contact';
import React from 'react';

import { ContactPhone } from './ContactPhone';

export interface IContactPhoneList {
  field: string;
  contactPhones: IEditableContactMethodForm[];
}

/**
 * Formik Field array wrapper around phone contacts.
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
          {contactPhones.map((phone, index) => (
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
