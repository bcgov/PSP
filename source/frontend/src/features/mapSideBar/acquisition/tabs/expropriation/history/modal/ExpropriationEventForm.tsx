import { Formik, FormikProps } from 'formik';
import React from 'react';

import { FastDatePicker, Select, SelectOption } from '@/components/common/form';
import { SectionField } from '@/components/common/Section/SectionField';
import * as API from '@/constants/API';
import { PayeeOption } from '@/features/mapSideBar/acquisition/models/PayeeOptionModel';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';

import { ExpropriationEventFormModel } from '../models';

export interface IExpropriationEventFormProps {
  formikRef: React.Ref<FormikProps<ExpropriationEventFormModel>>;
  initialValues: ExpropriationEventFormModel;
  payeeOptions: PayeeOption[];
  onSave: (values: ExpropriationEventFormModel) => void;
}

/**
 * Internal form intended to be displayed within a modal window.
 * @param {IExpropriationEventFormProps} props
 */
export const ExpropriationEventForm: React.FunctionComponent<IExpropriationEventFormProps> = ({
  formikRef,
  initialValues,
  payeeOptions,
  onSave,
}) => {
  const { getOptionsByType } = useLookupCodeHelpers();
  const expropriationEventOptions = getOptionsByType(API.ACQUISITION_EXPROPRIATION_EVENT_TYPES);

  return (
    <Formik<ExpropriationEventFormModel>
      innerRef={formikRef}
      enableReinitialize
      initialValues={initialValues}
      onSubmit={values => onSave(values)}
    >
      {formikProps => (
        <>
          <SectionField label="Owner">
            <Select
              field="payeeKey"
              title={payeeOptions.find(p => p.value === formikProps.values.payeeKey)?.fullText}
              options={payeeOptions.map<SelectOption>(x => {
                return { label: x.text, value: x.value, title: x.fullText };
              })}
              placeholder="Select..."
            />
          </SectionField>
          <SectionField label="Event">
            <Select
              options={expropriationEventOptions}
              field="eventTypeCode"
              placeholder="Select type..."
            />
          </SectionField>
          <SectionField label="Date">
            <FastDatePicker formikProps={formikProps} field="eventDate" />
          </SectionField>
        </>
      )}
    </Formik>
  );
};
