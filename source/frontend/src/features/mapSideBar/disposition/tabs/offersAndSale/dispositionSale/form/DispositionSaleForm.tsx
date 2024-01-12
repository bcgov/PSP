import axios, { AxiosError } from 'axios';
import { Formik, getIn, useFormikContext } from 'formik';
import styled from 'styled-components';

import { FastCurrencyInput, FastDatePicker } from '@/components/common/form';
import { ContactInputContainer } from '@/components/common/form/ContactInput/ContactInputContainer';
import ContactInputView from '@/components/common/form/ContactInput/ContactInputView';
import { FastDateYearPicker } from '@/components/common/form/FastDateYearPicker';
import { YesNoSelect } from '@/components/common/form/YesNoSelect';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { DispositionSaleFormModel } from '@/features/mapSideBar/disposition/models/DispositionSaleFormModel';
import SidebarFooter from '@/features/mapSideBar/shared/SidebarFooter';
import { getCancelModalProps, useModalContext } from '@/hooks/useModalContext';
import { IApiError } from '@/interfaces/IApiError';
import { Api_DispositionFileSale } from '@/models/api/DispositionFile';

import { DispositionSaleFormYupSchema } from './DispositionSaleFormYupSchema';
import DispositionSalePurchaserSubForm from './DispositionSalePurchasersSubForm';

export interface IDispositionSaleFormProps {
  initialValues: DispositionSaleFormModel;
  loading: boolean;
  onSave: (sale: Api_DispositionFileSale) => Promise<Api_DispositionFileSale | undefined>;
  onCancel: () => void;
  onSuccess: () => void;
  onError: (e: AxiosError<IApiError>) => void;
}

const DispositionSaleForm: React.FC<IDispositionSaleFormProps> = ({
  initialValues,
  loading,
  onSave,
  onCancel,
  onSuccess,
  onError,
}) => {
  const { setModalContent, setDisplayModal } = useModalContext();

  const cancelFunc = (resetForm: () => void, dirty: boolean) => {
    if (!dirty) {
      resetForm();
      onCancel();
    } else {
      setModalContent({
        ...getCancelModalProps(),
        handleOk: () => {
          resetForm();
          setDisplayModal(false);
          onCancel();
        },
      });
      setDisplayModal(true);
    }
  };

  return (
    <StyledFormWrapper>
      <Formik<DispositionSaleFormModel>
        enableReinitialize
        validationSchema={DispositionSaleFormYupSchema}
        initialValues={initialValues}
        onSubmit={async (values: DispositionSaleFormModel, formikHelpers) => {
          try {
            const createdOffer = await onSave(values.toApi());
            if (createdOffer && createdOffer.id) {
              onSuccess();
            }
          } catch (e) {
            if (axios.isAxiosError(e)) {
              const axiosError = e as AxiosError<IApiError>;
              onError && onError(axiosError);
            }
          } finally {
            formikHelpers.setSubmitting(false);
          }
        }}
      >
        {formikProps => {
          const isGstRequired = getIn(formikProps.values, 'isGstRequired');

          return (
            <>
              <LoadingBackdrop
                show={formikProps.isSubmitting || loading}
                parentScreen={true}
              ></LoadingBackdrop>
              <StyledContent>
                <Section header="Sales Details">
                  <SectionField label="Purchaser name(s)" labelWidth="4" contentWidth="8">
                    <DispositionSalePurchaserSubForm />
                  </SectionField>
                  <SectionField label="Purchaser agent" labelWidth="4" contentWidth="6">
                    <ContactInputContainer
                      field="dispositionPurchaserAgent.contact"
                      View={ContactInputView}
                    ></ContactInputContainer>
                  </SectionField>
                  <SectionField label="Purchaser solicitor" labelWidth="4" contentWidth="6">
                    <ContactInputContainer
                      field="dispositionPurchaserSolicitor.contact"
                      View={ContactInputView}
                    ></ContactInputContainer>
                  </SectionField>
                  <SectionField
                    label="Last condition removal date"
                    labelWidth="4"
                    contentWidth="5"
                    tooltip="For general sales, provide the date when the last condition(s) are to be removed. For road closures enter the condition precedent date."
                  >
                    <FastDatePicker field="finalConditionRemovalDate" formikProps={formikProps} />
                  </SectionField>
                  <SectionField label="Sale completion date" labelWidth="4" contentWidth="5">
                    <FastDatePicker field="saleCompletionDate" formikProps={formikProps} />
                  </SectionField>
                  <SectionField label="Fiscal year of sale" labelWidth="4" contentWidth="5">
                    <FastDateYearPicker field="saleFiscalYear" formikProps={formikProps} />
                  </SectionField>
                  <SectionField label="Final sale price ($)" labelWidth="4" contentWidth="5">
                    <FastCurrencyInput formikProps={formikProps} field="finalSaleAmount" />
                  </SectionField>
                  <SectionField label=" Realtor commission ($)" labelWidth="4" contentWidth="5">
                    <FastCurrencyInput formikProps={formikProps} field="realtorCommissionAmount" />
                  </SectionField>

                  <SectionField label="GST required" labelWidth="4" contentWidth="5">
                    <YesNoSelect field="isGstRequired" notNullable={true}></YesNoSelect>
                  </SectionField>
                  {isGstRequired && (
                    <SectionField
                      label="GST collected ($)"
                      labelWidth="4"
                      contentWidth="5"
                      tooltip="GST collected is calculated based upon Final Sales Price."
                    >
                      <FastCurrencyInput
                        allowNegative
                        formikProps={formikProps}
                        field="gstCollectedAmount"
                        // onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
                        //   onTaxAmountUpdated(index, e.target.value);
                        // }}
                      />
                    </SectionField>
                  )}

                  <SectionField label="Net Book Value ($)" labelWidth="4" contentWidth="5">
                    <FastCurrencyInput formikProps={formikProps} field="netBookAmount" disabled />
                  </SectionField>
                  <SectionField
                    label="Total cost of sales ($)"
                    labelWidth="4"
                    contentWidth="5"
                    tooltip="Sum of all costs incurred to prepare property for sale (e.g., appraisal, environmental and other consultants, legal fees, First Nations accommodation, etc.)."
                  >
                    <FastCurrencyInput formikProps={formikProps} field="totalCostAmount" disabled />
                  </SectionField>
                  <SectionField
                    label="Net proceeds before SPP cost ($)"
                    labelWidth="4"
                    contentWidth="5"
                  >
                    <FastCurrencyInput
                      formikProps={formikProps}
                      field="netProceedsBeforeSppAmount"
                      disabled
                    />
                  </SectionField>
                  <SectionField
                    label="SPP Amount ($)"
                    labelWidth="4"
                    contentWidth="5"
                    tooltip="Surplus Property Program (SPP) fee to be paid to CITZ."
                  >
                    <FastCurrencyInput formikProps={formikProps} field="sppAmount" />
                  </SectionField>

                  <SectionField
                    label="Net proceeds after SPP cost ($)"
                    labelWidth="4"
                    contentWidth="5"
                    tooltip="Net Proceeds after SPP Cost = Final Sales price, less Commissions, GST, Net Book Value, Total Cost of Sales,  and SPP Amount."
                  >
                    <FastCurrencyInput
                      formikProps={formikProps}
                      field="netProceedsAfterSppAmount"
                      disabled
                    />
                  </SectionField>
                  <SectionField label="Remediation cost ($)" labelWidth="4" contentWidth="5">
                    <FastCurrencyInput formikProps={formikProps} field="remediationAmount" />
                  </SectionField>
                </Section>
              </StyledContent>
              <StyledFooter>
                <SidebarFooter
                  onSave={() => formikProps.submitForm()}
                  isOkDisabled={formikProps.isSubmitting || !formikProps.dirty}
                  onCancel={() => cancelFunc(formikProps.resetForm, formikProps.dirty)}
                  displayRequiredFieldError={
                    formikProps.isValid === false && !!formikProps.submitCount
                  }
                />
              </StyledFooter>
            </>
          );
        }}
      </Formik>
    </StyledFormWrapper>
  );
};

export default DispositionSaleForm;

const StyledFormWrapper = styled.div`
  display: flex;
  flex-direction: column;
  flex-grow: 1;
  text-align: left;
  height: 100%;
  overflow-y: auto;
  padding-bottom: 1rem;
`;

const StyledContent = styled.div`
  background-color: ${props => props.theme.css.filterBackgroundColor};
`;

const StyledFooter = styled.div`
  margin-right: 1rem;
  padding-bottom: 1rem;
  z-index: 0;
`;
