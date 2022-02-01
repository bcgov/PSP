import { Button } from 'components/common/form';
import { FieldArray } from 'formik';
import { IEditableContactMethodForm } from 'interfaces/editable-contact';
import React from 'react';

import { ContactEmail } from './ContactEmail';

const emptyContactMethod: IEditableContactMethodForm = { value: '', contactMethodTypeCode: '' };

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
      {({ push, remove, replace }) => (
        <>
          {contactEmails.map((email, index) => (
            <ContactEmail
              key={`${field}.${index}`}
              namespace={`${field}.${index}`}
              onRemove={
                index >= 0 ? () => remove(index) : () => replace(index, { ...emptyContactMethod })
              }
            />
          ))}
          <Button variant="link" onClick={() => push({ ...emptyContactMethod })}>
            + Add another email address
          </Button>
        </>
      )}
    </FieldArray>
  );
};
