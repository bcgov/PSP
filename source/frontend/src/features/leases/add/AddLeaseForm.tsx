import { Formik, FormikProps } from 'formik';
import { IProperty } from 'interfaces';
import { Api_Lease } from 'models/api/Lease';
import * as React from 'react';
import { Prompt } from 'react-router-dom';

import { FormLease, FormLeaseProperty, getDefaultFormLease } from '../models';
import SaveCancelButtons from '../SaveCancelButtons';
import { LeaseSchema } from './AddLeaseYupSchema';
import AdministrationSubForm from './AdministrationSubForm';
import LeaseDatesSubForm from './LeaseDatesSubForm';
import NoteSubForm from './NoteSubForm';
import PropertyInformationSubForm from './PropertyInformationSubForm';
import ReferenceSubForm from './ReferenceSubForm';
import * as Styled from './styles';

interface IAddLeaseFormProps {
  onCancel: () => void;
  onSubmit: (lease: Api_Lease) => void;
  formikRef: React.Ref<FormikProps<FormLease>>;
  propertyInfo: IProperty | null;
}

const AddLeaseForm: React.FunctionComponent<React.PropsWithChildren<IAddLeaseFormProps>> = ({
  onCancel,
  onSubmit,
  formikRef,
  propertyInfo,
}) => {
  const defaultFormLease = getDefaultFormLease();
  if (propertyInfo) {
    defaultFormLease.properties = [];
    defaultFormLease.properties.push(
      FormLeaseProperty.fromApi({
        property: {
          pid: propertyInfo.pid ? +propertyInfo.pid : undefined,
          pin: propertyInfo?.pin ? +propertyInfo.pin : undefined,
          latitude: propertyInfo.latitude,
          longitude: propertyInfo.longitude,
          location: { coordinate: { x: propertyInfo.longitude, y: propertyInfo.latitude } },
        },
        leaseArea: propertyInfo.landArea,
        areaUnitType: { id: propertyInfo.areaUnit },
      }),
    );
    defaultFormLease.region = propertyInfo.regionId ? { id: propertyInfo.regionId } : undefined;
  }
  return (
    <Formik<FormLease>
      initialValues={defaultFormLease}
      onSubmit={async (values: FormLease, formikHelpers) => {
        const apiLease = values.toApi();
        formikHelpers.setSubmitting(false);
        onSubmit(apiLease);
      }}
      validationSchema={LeaseSchema}
      innerRef={formikRef}
    >
      {formikProps => (
        <>
          <Prompt
            when={formikProps.dirty && formikProps.submitCount === 0}
            message="You have made changes on this form. Do you wish to leave without saving?"
          />
          <Styled.LeaseForm>
            <LeaseDatesSubForm formikProps={formikProps}></LeaseDatesSubForm>
            <AdministrationSubForm formikProps={formikProps}></AdministrationSubForm>
            <ReferenceSubForm />
            <PropertyInformationSubForm />
            <NoteSubForm />
            <SaveCancelButtons formikProps={formikProps} onCancel={onCancel} />
          </Styled.LeaseForm>
        </>
      )}
    </Formik>
  );
};

export default AddLeaseForm;
