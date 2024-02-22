import { ArrayHelpers, FieldArray } from 'formik';
import React from 'react';

import { LinkButton } from '@/components/common/buttons';
import { IEditableContactMethodForm } from '@/features/contacts/formModels';

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
  // clear out existing values instead of removing last item from array
  const onRemove = (array: Array<any>, index: number, arrayHelpers: ArrayHelpers) => {
    if (index >= 1) {
      return () => arrayHelpers.remove(index);
    }
    if (array.length === 1) {
      return () => arrayHelpers.replace(index, new IEditableContactMethodForm());
    }
    return undefined;
  };

  return (
    <FieldArray name={field}>
      {arrayHelpers => (
        <>
          {contactEmails.map((email, index, array) => (
            <ContactEmail
              key={`${field}.${index}`}
              namespace={`${field}.${index}`}
              onRemove={onRemove(array, index, arrayHelpers)}
            />
          ))}
          <LinkButton onClick={() => arrayHelpers.push(new IEditableContactMethodForm())}>
            + Add email address
          </LinkButton>
        </>
      )}
    </FieldArray>
  );
};
