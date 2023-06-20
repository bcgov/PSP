import { FieldArray, Formik, FormikProps } from 'formik';
import { Prompt } from 'react-router';
import styled from 'styled-components';

import { Input } from '@/components/common/form';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { withNameSpace } from '@/utils/formUtils';

import { AddImprovementsYupSchema } from './AddImprovementsYupSchema';
import { sectionTitles } from './components/Improvement/Improvement';
import { ILeaseImprovementForm, ILeaseImprovementsForm } from './models';
import * as Styled from './styles';

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
          <>
            <Prompt
              when={formikProps.dirty && !formikProps.isSubmitting}
              message="You have made changes on this form. Do you wish to leave without saving?"
            />
            <StyledFormBody>
              <Styled.ImprovementsContainer className="improvements">
                <FieldArray
                  name="improvements"
                  render={renderProps =>
                    formikProps.values.improvements.map(
                      (improvement: ILeaseImprovementForm, index) => {
                        const title =
                          sectionTitles.get(improvement.propertyImprovementTypeId) ?? 'N/A';
                        const nameSpace = `improvements.${index}`;
                        return (
                          <>
                            <Section header={title}>
                              <SectionField label="Unit #" labelWidth="3">
                                <Input field={withNameSpace(nameSpace, 'address')} />{' '}
                              </SectionField>
                              <SectionField label="Building size" labelWidth="3">
                                <Input field={withNameSpace(nameSpace, 'structureSize')} />
                              </SectionField>
                              <SectionField label="Description" labelWidth="3">
                                <Styled.FormDescriptionBody
                                  innerClassName="description"
                                  rows={5}
                                  field={withNameSpace(nameSpace, 'description')}
                                  placeholder="Reason for improvement and improvement details"
                                />
                              </SectionField>
                            </Section>
                          </>
                        );
                      },
                    )
                  }
                ></FieldArray>
              </Styled.ImprovementsContainer>
              {children}
            </StyledFormBody>
          </>
        )}
      </Formik>
    </>
  );
};

const StyledFormBody = styled.form`
  margin-left: 1rem;
  .form-group {
    flex-direction: column;
    input {
      border-left: 1;
      width: 70%;
    }
    textarea {
      width: 85%;
      resize: none;
    }
  }
  .improvements .formgrid {
    row-gap: 0.5rem;
    grid-template-columns: [controls] 1fr;
    & > .form-label {
      grid-column: controls;
      font-family: 'BcSans-Bold';
    }
    & > .input {
      border-left: 0;
    }
    .form-control {
      font-family: 'BcSans';
    }
    h5 {
      padding-top: 0;
    }
  }
`;

export default AddImprovementsForm;
