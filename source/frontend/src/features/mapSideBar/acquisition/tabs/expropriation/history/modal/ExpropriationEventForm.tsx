import { Formik, FormikProps } from 'formik';
import React from 'react';

import { FastDatePicker } from '@/components/common/form';
import { SectionField } from '@/components/common/Section/SectionField';

import { ExpropriationEventFormModel } from '../models';

export interface IExpropriationEventFormProps {
  formikRef: React.Ref<FormikProps<ExpropriationEventFormModel>>;
  initialValues: ExpropriationEventFormModel;
  onSave: (values: ExpropriationEventFormModel) => void;
}

/**
 * Internal form intended to be displayed within a modal window.
 * @param {IExpropriationEventFormProps} props
 */
export const ExpropriationEventForm: React.FunctionComponent<IExpropriationEventFormProps> = ({
  formikRef,
  initialValues,
  onSave,
}) => {
  return (
    <Formik<ExpropriationEventFormModel>
      innerRef={formikRef}
      enableReinitialize
      initialValues={initialValues}
      onSubmit={values => onSave(values)}
    >
      {formikProps => (
        <>
          <SectionField label="Owner">{' TODO '}</SectionField>
          <SectionField label="Event">{' TODO '}</SectionField>
          <SectionField label="Date">
            <FastDatePicker formikProps={formikProps} field="eventDate" />
          </SectionField>
        </>
      )}
    </Formik>
  );
};
