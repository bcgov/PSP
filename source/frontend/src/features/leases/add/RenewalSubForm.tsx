import { FieldArray } from 'formik';
import { FormikProps } from 'formik/dist/types';

import { LinkButton } from '@/components/common/buttons';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';

import { FormLeaseRenewal } from '../detail/LeasePages/deposits/models/FormLeaseDepositReturn';
import { LeaseFormModel } from '../models';

export interface IRenewalSubFormProps {
  formikProps: FormikProps<LeaseFormModel>;
}

export const RenewalSubForm: React.FunctionComponent<IRenewalSubFormProps> = ({ formikProps }) => {
  const { values, setFieldValue } = formikProps;
  console.log(values, setFieldValue);

  return (
    <Section header="Renewal Option">
      <FieldArray
        name="renewals"
        render={arrayHelpers => (
          <>
            {values.renewals.map((renewal, index) => (
              <Section key={index} header={`Renewal ${index + 1}`}>
                <SectionField label="Ministry project" labelWidth="2">
                  {renewal.startDate}
                </SectionField>
              </Section>
            ))}
            <LinkButton
              data-testid="add-file-owner"
              onClick={() => {
                const renewal = new FormLeaseRenewal();
                arrayHelpers.push(renewal);
              }}
            >
              + Add renewal
            </LinkButton>
          </>
        )}
      />
    </Section>
  );
};

export default RenewalSubForm;
