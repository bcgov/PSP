import { Button } from 'components/common/form';
import { FieldArray } from 'formik';
import { IEditableContactMethodForm } from 'interfaces/editable-contact';
import React from 'react';

import { ContactEmail } from './ContactEmail';

export interface IContactEmailList {
  field: string;
  contactEmails: IEditableContactMethodForm[];
}

/**
 * Formik Field array wrapper around email contacts.
 * @param {IContactEmailList} param0
 */
export const ContactEmailList: React.FunctionComponent<IContactEmailList> = ({
  field,
  contactEmails = [],
}) => {
  return (
    <FieldArray name={field}>
      {({ push, remove }) => (
        <>
          {contactEmails.map((email, index) => (
            <ContactEmail
              key={`${field}.${index}`}
              namespace={`${field}.${index}`}
              onRemove={index > 0 ? () => remove(index) : undefined}
            />
          ))}
          <Button variant="link" onClick={() => push({ value: '', contactMethodTypeCode: '' })}>
            + Add another email address
          </Button>
        </>
      )}
    </FieldArray>
  );
};
