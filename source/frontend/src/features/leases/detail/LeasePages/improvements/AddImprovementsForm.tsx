import { FieldArray, Formik, FormikProps } from 'formik';
import React, { ReactNode } from 'react';
import { Prompt } from 'react-router';

import { Input, TextArea } from '@/components/common/form';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledSummarySection } from '@/components/common/Section/SectionStyles';
import TooltipIcon from '@/components/common/TooltipIcon';
import { PropertyImprovementTypes } from '@/constants/propertyImprovementTypes';
import { withNameSpace } from '@/utils/formUtils';

import { AddImprovementsYupSchema } from './AddImprovementsYupSchema';
import { sectionTitles } from './components/Improvement/Improvement';
import { ILeaseImprovementForm, ILeaseImprovementsForm } from './models';

export interface IAddImprovementsFormProps {
  loading?: boolean;
  onSubmit: (lease: ILeaseImprovementsForm) => Promise<void>;
  initialValues?: ILeaseImprovementsForm;
  formikRef: React.Ref<FormikProps<ILeaseImprovementsForm>>;
}

export const AddImprovementsForm: React.FunctionComponent<
  React.PropsWithChildren<IAddImprovementsFormProps>
> = ({ onSubmit, initialValues, formikRef, children, loading }) => {
  return (
    <>
      <LoadingBackdrop show={loading} parentScreen />
      <Formik
        validationSchema={AddImprovementsYupSchema}
        onSubmit={values => onSubmit(values)}
        innerRef={formikRef}
        enableReinitialize
        initialValues={{ ...new ILeaseImprovementsForm(), ...initialValues }}
      >
        {formikProps => (
          <StyledSummarySection>
            <Prompt
              when={formikProps.dirty && !formikProps.isSubmitting}
              message="You have made changes on this form. Do you wish to leave without saving?"
            />
            <FieldArray
              name="improvements"
              render={() =>
                formikProps.values.improvements.map((improvement: ILeaseImprovementForm, index) => {
                  const title = sectionTitles.get(improvement.propertyImprovementTypeId) ?? 'N/A';
                  const nameSpace = `improvements.${index}`;

                  const HeaderDisplayNode = (typeId: string): ReactNode => {
                    if (typeId === 'OTHER') {
                      return (
                        <div className="d-flex align-items-center">
                          <span>{title}</span>
                          <TooltipIcon
                            toolTipId="contactInfoToolTip"
                            innerClassName="ml-4 mb-1"
                            toolTip="This could include lighting, fencing, irrigation, parking etc"
                          />
                        </div>
                      );
                    }

                    return title;
                  };

                  return (
                    <Section
                      header={HeaderDisplayNode(improvement.propertyImprovementTypeId)}
                      key={nameSpace}
                    >
                      <SectionField label="Unit #" labelWidth={{ xs: 3 }}>
                        <Input field={withNameSpace(nameSpace, 'address')} />{' '}
                      </SectionField>
                      <SectionField
                        label={
                          improvement.propertyImprovementTypeId ===
                          PropertyImprovementTypes.Residential
                            ? 'House size'
                            : 'Building size'
                        }
                        labelWidth={{ xs: 3 }}
                      >
                        <Input field={withNameSpace(nameSpace, 'structureSize')} />
                      </SectionField>
                      <SectionField label="Description" labelWidth={{ xs: 12 }}>
                        <TextArea
                          innerClassName="description"
                          rows={5}
                          field={withNameSpace(nameSpace, 'description')}
                          placeholder="Reason for improvement and improvement details"
                        />
                      </SectionField>
                    </Section>
                  );
                })
              }
            />
            {children}
          </StyledSummarySection>
        )}
      </Formik>
    </>
  );
};
