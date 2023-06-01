import { LinkButton, StyledRemoveLinkButton } from 'components/common/buttons';
import { FastCurrencyInput, Select, SelectOption } from 'components/common/form';
import GenericModal from 'components/common/GenericModal';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { FieldArray, FormikProps, useFormikContext } from 'formik';
import { useState } from 'react';
import { Container } from 'react-bootstrap';
import { FaTrash } from 'react-icons/fa';
import styled from 'styled-components';
import { stringToBoolean } from 'utils/formUtils';

import { CompensationRequisitionFormModel, FinacialActivityFormModel } from '../../models';

export interface IFinancialActivitiesSubFormProps {
  formikProps: FormikProps<CompensationRequisitionFormModel>;
  compensationRequisitionId: number;
  financialActivityOptions: SelectOption[];
  gstConstant: number;
}

export const FinancialActivitiesSubForm: React.FunctionComponent<
  IFinancialActivitiesSubFormProps
> = ({ formikProps, compensationRequisitionId, financialActivityOptions, gstConstant }) => {
  const { values, setFieldValue } = useFormikContext<CompensationRequisitionFormModel>();
  const [showModal, setShowModal] = useState(false);
  const [rowToDelete, setRowToDelete] = useState<number | undefined>(undefined);

  const updateGstApplicable = (index: number, gstOption: string): void => {
    const isGstRequired = stringToBoolean(gstOption);
    setAmountFields(index, isGstRequired, values.financials[index].pretaxAmount);
    updatePayeeCheque();
  };

  const onPretaxAmountUpdated = (index: number, newValue: string): void => {
    const isGstRequired = stringToBoolean(values.financials[index].isGstRequired);
    const cleanValue = getCurrenclyCleanValue(newValue);

    setAmountFields(index, isGstRequired, cleanValue);
    updatePayeeCheque();
  };

  const updateGstAmount = (index: number, newValue: string): void => {
    const totalAmount = values.financials[index].pretaxAmount + getCurrenclyCleanValue(newValue);

    setFieldValue(`financials[${index}].totalAmount`, totalAmount);
    updatePayeeCheque();
  };

  const setAmountFields = (index: number, gstRequired: boolean, pretaxAmount: number): void => {
    let totalAmount = 0;
    let taxAmount = 0;

    if (gstRequired) {
      taxAmount = pretaxAmount * gstConstant;
      totalAmount = taxAmount + pretaxAmount;
    } else {
      totalAmount = pretaxAmount;
    }

    setFieldValue(`financials[${index}].taxAmount`, taxAmount);
    setFieldValue(`financials[${index}].totalAmount`, totalAmount);
  };

  const updatePayeeCheque = (): void => {
    const chequePretaxAmount = values.financials.reduce(
      (total, item) => total + item.pretaxAmount,
      0,
    );

    const chequeTaxAmount = values.financials.reduce((total, item) => total + item.taxAmount, 0);
    const chequeTotalAmount = values.financials.reduce(
      (total, item) => total + item.totalAmount,
      0,
    );

    setFieldValue(`payees.0.cheques.0.pretaxAmount`, chequePretaxAmount);
    setFieldValue(`payees.0.cheques.0.taxAmount`, chequeTaxAmount);
    setFieldValue(`payees.0.cheques.0.totalAmount`, chequeTotalAmount);
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
                  <StyledSubHeader>
                    <label>Activity {index + 1}</label>
                    <StyledRemoveLinkButton
                      title="Delete financial activity"
                      variant="light"
                      onClick={() => {
                        setRowToDelete(index);
                        setShowModal(true);
                      }}
                    >
                      <FaTrash size="2rem" />
                    </StyledRemoveLinkButton>
                  </StyledSubHeader>

                  <SectionField label="Code & Description" labelWidth="4" required>
                    <Select
                      field={`financials[${index}].financialActivityCodeId`}
                      options={financialActivityOptions}
                      placeholder="Select..."
                      required
                    />
                  </SectionField>

                  <SectionField label={'Amount (before tax)'} required>
                    <FastCurrencyInput
                      formikProps={formikProps}
                      field={`financials[${index}].pretaxAmount`}
                      onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
                        onPretaxAmountUpdated(index, e.target.value);
                      }}
                    />
                  </SectionField>

                  <SectionField label="GST applicable?" required>
                    <Select
                      field={`financials[${index}].isGstRequired`}
                      options={[
                        { label: 'No', value: 'false' },
                        { label: 'Yes', value: 'true' },
                      ]}
                      onChange={(e: React.ChangeEvent<HTMLSelectElement>) => {
                        const selectedValue = [].slice
                          .call(e.target.selectedOptions)
                          .map((option: HTMLOptionElement & number) => option.value)[0];
                        updateGstApplicable(index, selectedValue);
                      }}
                    />
                  </SectionField>

                  {financial.isGstRequired === 'true' && (
                    <SectionField label="GST amount">
                      <FastCurrencyInput
                        formikProps={formikProps}
                        field={`financials[${index}].taxAmount`}
                        onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
                          updateGstAmount(index, e.target.value);
                        }}
                        disabled
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
                {index !== values.financials.length - 1 && <StyledSpacer className="my-3" />}
              </div>
            ))}
            <LinkButton
              data-testid="add=financial-activity"
              onClick={() => {
                const activity = new FinacialActivityFormModel(null, compensationRequisitionId);
                arrayHelpers.push(activity);
              }}
            >
              + Add an Activity
            </LinkButton>

            <GenericModal
              display={showModal}
              title="Remove financial activity"
              message={'Are you sure you want to remove this financial activity?'}
              okButtonText="Remove"
              cancelButtonText="Cancel"
              handleOk={() => {
                setShowModal(false);
                arrayHelpers.remove(rowToDelete!);
                setRowToDelete(undefined);
                updatePayeeCheque();
              }}
              handleCancel={() => {
                setShowModal(false);
                setRowToDelete(undefined);
              }}
            />
          </>
        );
      }}
    ></FieldArray>
  );
};

export default FinancialActivitiesSubForm;

const getCurrenclyCleanValue = (stringValue: string): number => {
  return Number(stringValue.replace(/[^0-9.]/g, ''));
};

const StyledSpacer = styled.div`
  border-bottom: 0.1rem solid grey;
`;

const StyledSubHeader = styled.div`
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  align-items: center;
  border-bottom: solid 0.2rem ${props => props.theme.css.discardedColor};
  margin-bottom: 2rem;

  label {
    color: ${props => props.theme.css.primaryColor};
    font-family: 'BCSans-Bold';
    font-size: 1.75rem;
    width: 100%;
    text-align: left;
  }

  button {
    margin-bottom: 1rem;
  }
`;
