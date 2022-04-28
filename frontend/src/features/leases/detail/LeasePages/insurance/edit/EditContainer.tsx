import { Button } from 'components/common/buttons/Button';
import { Form } from 'components/common/form/Form';
import { FormSectionClear } from 'components/common/form/styles';
import { FieldArray, Formik, FormikProps } from 'formik';
import { IInsurance } from 'interfaces';
import { IBatchUpdateRequest, IEntryModification, UpdateOperation } from 'interfaces/batchUpdate';
import React, { useRef } from 'react';
import { ButtonToolbar, Col, Row } from 'react-bootstrap';
import { ILookupCode } from 'store/slices/lookupCodes/interfaces';
import { withNameSpace } from 'utils/formUtils';

import { useUpdateInsurance } from './hooks/useUpdateInsurance';
import InsuranceForm from './InsuranceForm';
import { FormInsurance, IUpdateFormInsurance } from './models';

export interface InsuranceEditContainerProps {
  leaseId: number;
  insuranceList: IInsurance[];
  insuranceTypes: ILookupCode[];
  onCancel: () => void;
  onSuccess: () => void;
}

const InsuranceEditContainer: React.FunctionComponent<InsuranceEditContainerProps> = ({
  leaseId,
  insuranceList,
  insuranceTypes,
  onSuccess,
  onCancel,
}) => {
  const formikRef = useRef<FormikProps<IUpdateFormInsurance>>(null);

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

  const { batchUpdateInsurances } = useUpdateInsurance();

  const handleSave = async (lease: IBatchUpdateRequest<IInsurance>) => {
    const leaseResponse = await batchUpdateInsurances(leaseId, lease);
    if (leaseResponse?.errorMessages.length === 0) {
      onSuccess();
    }
  };

  const handleCancel = () => {
    onCancel();
  };

  const initialInsurances = insuranceTypes.map<FormInsurance>(x => {
    let foundInsurance = insuranceList.find(i => i.insuranceType.id === x.id);
    if (foundInsurance) {
      return FormInsurance.createFromModel(foundInsurance);
    } else {
      return FormInsurance.createEmpty(x);
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
      onSubmit={(values: IUpdateFormInsurance, formikHelpers) => {
        const updatedVals = values.insurances.reduce(
          (accumulator: IEntryModification<IInsurance>[], entry: FormInsurance) => {
            const existingEntry = initialValues.insurances.find(i => i.id === entry.id && !i.isNew);
            if (existingEntry) {
              if (!entry.isShown) {
                accumulator.push({
                  entry: entry.toInterfaceModel(),
                  operation: UpdateOperation.DELETE,
                });
              } else if (!existingEntry.isEqual(entry)) {
                accumulator.push({
                  entry: entry.toInterfaceModel(),
                  operation: UpdateOperation.UPDATE,
                });
              }
            } else if (entry.isNew && entry.isShown) {
              accumulator.push({
                entry: entry.toInterfaceModel(),
                operation: UpdateOperation.ADD,
              });
            }

            return accumulator;
          },
          [],
        );

        formikHelpers.setSubmitting(false);

        if (updatedVals.length > 0) {
          const updateRequest: IBatchUpdateRequest<IInsurance> = { payload: updatedVals };
          handleSave(updateRequest);
        }
      }}
      enableReinitialize
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
          <Row className="justify-content-md-end">
            <ButtonToolbar className="cancelSave">
              <Col>
                <Button className="mr-5" variant="secondary" type="button" onClick={handleCancel}>
                  Cancel
                </Button>
              </Col>
              <Col>
                <Button
                  className="mr-5"
                  type="button"
                  disabled={!formikProps.dirty}
                  onClick={formikProps.submitForm}
                >
                  Save
                </Button>
              </Col>
            </ButtonToolbar>
          </Row>
        </>
      )}
    </Formik>
  );
};

export default InsuranceEditContainer;
