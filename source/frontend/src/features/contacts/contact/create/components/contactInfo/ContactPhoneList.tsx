import { ArrayHelpers, FieldArray } from 'formik';
import React from 'react';

import { LinkButton } from '@/components/common/buttons';
import { IEditableContactMethodForm } from '@/features/contacts/formModels';

import { ContactPhone } from './ContactPhone';

export interface IContactPhoneList {
  field: string;
  contactPhones: IEditableContactMethodForm[];
}

/**
 * Formik Field array wrapper around phone contacts.
 * @param {IContactPhoneList} param0
 */
export const ContactPhoneList: React.FunctionComponent<
  React.PropsWithChildren<IContactPhoneList>
> = ({ field, contactPhones = [] }) => {
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
          {contactPhones.map((phone, index, array) => (
            <ContactPhone
              key={`${field}.${index}`}
              namespace={`${field}.${index}`}
              onRemove={onRemove(array, index, arrayHelpers)}
            />
          ))}
          <LinkButton onClick={() => arrayHelpers.push(new IEditableContactMethodForm())}>
            + Add phone number
          </LinkButton>
        </>
      )}
    </FieldArray>
  );
};
