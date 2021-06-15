import { Formik } from 'formik';
import { noop } from 'lodash';
import * as React from 'react';
import { Form } from 'react-bootstrap';

interface IPropertyFormProps {
  formikRef: any;
}

/**
 * Property Form, displays fields from LTSA, Parcel Map, and Geocoder.
 * @param param0 formikRef used by the parent to interact with this form.
 */
const PropertyForm: React.FunctionComponent<IPropertyFormProps> = ({ formikRef }) => {
  return (
    <Formik innerRef={formikRef} onSubmit={noop} initialValues={{}}>
      <Form></Form>
    </Formik>
  );
};

export default PropertyForm;
