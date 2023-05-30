import { LinkButton, StyledRemoveLinkButton } from 'components/common/buttons';
import { FastCurrencyInput, Input, Select, SelectOption } from 'components/common/form';
import { H3 } from 'components/common/styles';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { FieldArray, FormikProps, useFormikContext } from 'formik';
import { Col, Container, Row } from 'react-bootstrap';
import { FaTrash } from 'react-icons/fa';
import styled from 'styled-components';
import { stringToBoolean } from 'utils/formUtils';

import { CompensationRequisitionFormModel, FinacialActivityFormModel } from '../../models';

export interface IFinancialActivitiesSubFormProps {
  formikProps: FormikProps<CompensationRequisitionFormModel>;
  financialActivityOptions: SelectOption[];
  gstConstant: number;
}

export const FinancialActivitiesSubForm: React.FunctionComponent<
  IFinancialActivitiesSubFormProps
> = ({ formikProps, financialActivityOptions, gstConstant }) => {
  const { values, setFieldValue, handleChange } =
    useFormikContext<CompensationRequisitionFormModel>();

  const calculateActivityAmounts = (index: number): void => {
    setFieldValue(`financials[${index}].taxAmount`, '');
  };

  const onPreTaxAmountUpdated = (updateFunction: () => void) => {
    return (e: React.ChangeEvent<any>) => {
      updateFunction();
      handleChange(e);
    };
  };

  const updateGstRequiredAmounts = (index: number, option: string): void => {
    const isGstRequired = stringToBoolean(option);
    let totalAmount = 0;
    let taxAmount = 0;

    if (isGstRequired) {
      taxAmount = values.financials[index].pretaxAmount * gstConstant;
      totalAmount = values.financials[index].pretaxAmount + taxAmount;
    } else {
      totalAmount = values.financials[index].pretaxAmount;
    }

    setFieldValue(`financials[${index}].taxAmount`, taxAmount);
    setFieldValue(`financials[${index}].totalAmount`, totalAmount);
  };

  return (
    <FieldArray
      name="financials"
      render={arrayHelpers => {
        return (
          <>
            {values.financials.map((financial, index) => (
              <div key={`financial-act-${index}`}>
                <Container>
                  <H3>Activity {index + 1}</H3>
                  <Row className="align-items-end pb-4">
                    <Col />
                    <Col xs="auto">
                      <StyledRemoveLinkButton
                        title="Delete financial activity"
                        variant="light"
                        onClick={() => console.log(`removed`)}
                      >
                        <FaTrash size="2rem" />
                      </StyledRemoveLinkButton>
                    </Col>
                  </Row>

                  <SectionField label="Code & Description" labelWidth="4" required>
                    <Select
                      field={`financials[${index}].financialActivityCodeId`}
                      options={financialActivityOptions}
                      placeholder="Select..."
                    />
                  </SectionField>

                  <SectionField label={'Amount (before tax)'}>
                    <FastCurrencyInput
                      formikProps={formikProps}
                      field={`financials[${index}].pretaxAmount`}
                    />
                  </SectionField>

                  <SectionField label="GST applicable?" required>
                    <Select
                      field={`financials[${index}].isGstRequired`}
                      options={[
                        { label: 'Yes', value: 'true' },
                        { label: 'No', value: 'false' },
                      ]}
                      onChange={(e: React.ChangeEvent<HTMLSelectElement>) => {
                        const selectedValue = [].slice
                          .call(e.target.selectedOptions)
                          .map((option: HTMLOptionElement & number) => option.value)[0];
                        updateGstRequiredAmounts(index, selectedValue);
                      }}
                    />
                  </SectionField>

                  {financial.isGstRequired === 'true' && (
                    <SectionField label="GST amount">
                      <FastCurrencyInput
                        formikProps={formikProps}
                        field={`financials[${index}].taxAmount`}
                      />
                    </SectionField>
                  )}

                  <SectionField label="Total amount">
                    <FastCurrencyInput
                      formikProps={formikProps}
                      field={`financials[${index}].totalAmount`}
                      disabled
                    />
                  </SectionField>
                </Container>

                {index !== values.financials.length - 1 && <StyledSpacer className="my-5" />}
              </div>
            ))}
            <LinkButton
              data-testid="add=financial-activity"
              onClick={() => {
                const activity = new FinacialActivityFormModel();
                arrayHelpers.push(activity);
              }}
            >
              + Add an Activity
            </LinkButton>
          </>
        );
      }}
    ></FieldArray>
  );
};

export default FinancialActivitiesSubForm;

const StyledSpacer = styled.div`
  border-bottom: 0.1rem solid grey;
`;
