import { ArrayHelpers, FieldArray, FormikProps, getIn } from 'formik';
import React from 'react';
import { Col, Row } from 'react-bootstrap';

import { LinkButton } from '@/components/common/buttons';
import { ContactInputContainer } from '@/components/common/form/ContactInput/ContactInputContainer';
import ContactInputView from '@/components/common/form/ContactInput/ContactInputView';
import { RestrictContactType } from '@/components/contact/ContactManagerView/ContactFilterComponent/ContactFilterComponent';
import { IContactSearchResult } from '@/interfaces';

import { PropertyActivityFormModel } from './models';

export interface IContactListForm {
  field: string;
  contactType: RestrictContactType;
  formikProps: FormikProps<PropertyActivityFormModel>;
}

export const ContactListForm: React.FunctionComponent<
  React.PropsWithChildren<IContactListForm>
> = ({ field, contactType = RestrictContactType.ALL, formikProps }) => {
  // clear out existing values instead of removing last item from array
  const onRemove = (array: Array<any>, index: number, arrayHelpers: ArrayHelpers) => {
    if (array.length > 1) {
      arrayHelpers.remove(index);
    } else {
      arrayHelpers.replace(index, null);
    }
    return;
  };

  const contacts = getIn(formikProps.values, field) as IContactSearchResult[];
  return (
    <FieldArray name={field}>
      {arrayHelpers => (
        <>
          {contacts.map((_, index) => (
            <Row key={`${field}.${index}`} className="no-gutters pr-4 mr-2">
              <Col className="col-11">
                <ContactInputContainer
                  field={`${field}[${index}]`}
                  View={ContactInputView}
                  restrictContactType={contactType}
                />
              </Col>
              {contacts.length > 1 && (
                <Col className="col-1 pl-3">
                  <LinkButton onClick={() => onRemove(contacts, index, arrayHelpers)}>X</LinkButton>
                </Col>
              )}
            </Row>
          ))}
          <LinkButton onClick={() => arrayHelpers.push(null)}>+ Add contact</LinkButton>
        </>
      )}
    </FieldArray>
  );
};
