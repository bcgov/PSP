import { FieldArray, Formik, FormikProps } from 'formik';
import React from 'react';

import { Form } from '@/components/common/form/Form';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledSummarySection } from '@/components/common/Section/SectionStyles';
import { ApiGen_Concepts_Insurance } from '@/models/api/generated/ApiGen_Concepts_Insurance';
import { ILookupCode } from '@/store/slices/lookupCodes/interfaces';
import { withNameSpace } from '@/utils/formUtils';
import { exists } from '@/utils/utils';

import InsuranceForm from './InsuranceForm';
import { InsuranceYupSchema } from './InsuranceYupSchema';
import { FormInsurance, IUpdateFormInsurance } from './models';

export interface InsuranceEditContainerProps {
  leaseId: number;
  insuranceList: ApiGen_Concepts_Insurance[];
  insuranceTypes: ILookupCode[];
  onSave: (insurances: ApiGen_Concepts_Insurance[]) => Promise<void>;
  formikRef: React.RefObject<FormikProps<any>>;
}

const InsuranceEditContainer: React.FunctionComponent<
  React.PropsWithChildren<InsuranceEditContainerProps>
> = ({ leaseId, insuranceList, insuranceTypes, onSave, formikRef }) => {
  const handleOnChange = (e: any, codeType: ILookupCode, arrayHelpers: any) => {
    if (formikRef.current) {
      const found = initialInsurances.findIndex(x => x.insuranceType?.id === codeType.id);

      if (e.target.checked) {
        arrayHelpers.push(codeType.id.toString());
        formikRef.current.values.insurances[found].isShown = true;
      } else {
        const idx = formikRef.current?.values.visibleTypes.indexOf(codeType.id.toString());
        arrayHelpers.remove(idx);
        formikRef.current.values.insurances[found].isShown = false;
      }
    }
  };

  const initialInsurances = insuranceTypes.map<FormInsurance>(x => {
    const foundInsurance = insuranceList.find(i => i.insuranceType?.id === x.id);
    if (foundInsurance) {
      return FormInsurance.createFromModel(foundInsurance);
    } else {
      return FormInsurance.createEmpty(x, leaseId);
    }
  });

  const initialTypes = insuranceList.map(x => x?.insuranceType?.id).filter(exists);

  const initialValues: IUpdateFormInsurance = {
    insurances: initialInsurances,
    visibleTypes: initialTypes,
  };

  return (
    <Formik<IUpdateFormInsurance>
      initialValues={initialValues}
      validationSchema={InsuranceYupSchema}
      onSubmit={(values: IUpdateFormInsurance) => {
        return onSave(
          values.insurances
            .filter(i => i.isShown)
            .map<ApiGen_Concepts_Insurance>(x => x.toInterfaceModel()),
        );
      }}
      innerRef={formikRef}
    >
      {formikProps => (
        <StyledSummarySection>
          <Section header="Required Coverage">
            <SectionField label="Select coverage types">
              <FieldArray
                name={withNameSpace('visibleTypes')}
                render={arrayHelpers => (
                  <Form.Group>
                    {insuranceTypes.map((code: ILookupCode, index: number) => (
                      <Form.Check
                        id={`insurance-checkbox-${index}`}
                        type="checkbox"
                        name="checkedTypes"
                        key={`${index}-${code.id}`}
                      >
                        <Form.Check.Input
                          id={`insurance-${index}`}
                          data-testid="insurance-checkbox"
                          type="checkbox"
                          name="checkedTypes"
                          value={code.id.toString()}
                          checked={formikProps.values.visibleTypes.includes(code.id.toString())}
                          onChange={(e: any) => {
                            handleOnChange(e, code, arrayHelpers);
                          }}
                        />
                        <Form.Check.Label htmlFor={`insurance-${index}`}>
                          {code.name}
                        </Form.Check.Label>
                      </Form.Check>
                    ))}
                  </Form.Group>
                )}
              />
            </SectionField>
          </Section>

          <FieldArray
            name={withNameSpace('insurances')}
            render={() => (
              <>
                {formikProps.values.insurances.map(
                  (insurance: FormInsurance, index: number) =>
                    insurance.isShown && (
                      <InsuranceForm
                        nameSpace={withNameSpace(`insurances.${index}`)}
                        key={`insurances.${index}`}
                      />
                    ),
                )}
              </>
            )}
          />
        </StyledSummarySection>
      )}
    </Formik>
  );
};

export default InsuranceEditContainer;
