import { FieldArray, Formik, FormikProps } from 'formik';
import React from 'react';

import { Form } from '@/components/common/form/Form';
import { FormSectionClear } from '@/components/common/form/styles';
import { Api_Insurance } from '@/models/api/Insurance';
import { ILookupCode } from '@/store/slices/lookupCodes/interfaces';
import { withNameSpace } from '@/utils/formUtils';

import InsuranceForm from './InsuranceForm';
import { InsuranceYupSchema } from './InsuranceYupSchema';
import { FormInsurance, IUpdateFormInsurance } from './models';

export interface InsuranceEditContainerProps {
  leaseId: number;
  insuranceList: Api_Insurance[];
  insuranceTypes: ILookupCode[];
  onSave: (insurances: Api_Insurance[]) => Promise<void>;
  formikRef: React.RefObject<FormikProps<any>>;
}

const InsuranceEditContainer: React.FunctionComponent<
  React.PropsWithChildren<InsuranceEditContainerProps>
> = ({ leaseId, insuranceList, insuranceTypes, onSave, formikRef }) => {
  const handleOnChange = (e: any, codeType: any, arrayHelpers: any) => {
    if (formikRef.current) {
      let found = initialInsurances.findIndex(x => x.insuranceType.id === codeType.id);

      if (e.target.checked) {
        arrayHelpers.push(codeType.id);
        formikRef.current.values.insurances[found].isShown = true;
      } else {
        const idx = formikRef.current?.values.visibleTypes.indexOf(codeType.id + '');
        arrayHelpers.remove(idx);
        formikRef.current.values.insurances[found].isShown = false;
      }
    }
  };

  const initialInsurances = insuranceTypes.map<FormInsurance>(x => {
    let foundInsurance = insuranceList.find(i => i.insuranceType.id === x.id);
    if (foundInsurance) {
      return FormInsurance.createFromModel(foundInsurance);
    } else {
      return FormInsurance.createEmpty(x, leaseId);
    }
  });

  const initialTypes = insuranceList.map<string>(x => x.insuranceType.id);

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
          values.insurances.filter(i => i.isShown).map<Api_Insurance>(x => x.toInterfaceModel()),
        );
      }}
      innerRef={formikRef}
    >
      {formikProps => (
        <>
          <h2>Required coverage</h2>
          <div>Select the coverage types that are required for this lease or license.</div>

          <FormSectionClear>
            <FieldArray
              name={withNameSpace('visibleTypes')}
              render={arrayHelpers => (
                <Form.Group>
                  {insuranceTypes.map((code: ILookupCode, index: number) => (
                    <Form.Check
                      id={`insurance-checbox-${index}`}
                      type="checkbox"
                      name="checkedTypes"
                      key={index + '-' + code.id}
                    >
                      <Form.Check.Input
                        id={'insurance-' + index}
                        data-testid="insurance-checkbox"
                        type="checkbox"
                        name="checkedTypes"
                        value={code.id + ''}
                        checked={formikProps.values.visibleTypes.includes(code.id + '')}
                        onChange={(e: any) => {
                          handleOnChange(e, code, arrayHelpers);
                        }}
                      />
                      <Form.Check.Label htmlFor={'insurance-' + index}>
                        {code.name}
                      </Form.Check.Label>
                    </Form.Check>
                  ))}
                </Form.Group>
              )}
            />
          </FormSectionClear>

          <h2>Coverage details</h2>
          <FieldArray
            name={withNameSpace('insurances')}
            render={() => (
              <div>
                {formikProps.values.insurances.map(
                  (insurance: FormInsurance, index: number) =>
                    insurance.isShown && (
                      <InsuranceForm
                        nameSpace={withNameSpace(`insurances.${index}`)}
                        key={`insurances.${index}`}
                      />
                    ),
                )}
              </div>
            )}
          />
        </>
      )}
    </Formik>
  );
};

export default InsuranceEditContainer;
