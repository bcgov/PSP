import axios, { AxiosError } from 'axios';
import { Formik } from 'formik';
import styled from 'styled-components';

import { FastCurrencyInput, FastDatePicker } from '@/components/common/form';
import { FastDateYearPicker } from '@/components/common/form/FastDateYearPicker';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { DispositionAppraisalFormModel } from '@/features/mapSideBar/disposition/models/DispositionAppraisalFormModel';
import SidebarFooter from '@/features/mapSideBar/shared/SidebarFooter';
import { getCancelModalProps, useModalContext } from '@/hooks/useModalContext';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_Concepts_DispositionFileAppraisal } from '@/models/api/generated/ApiGen_Concepts_DispositionFileAppraisal';

import { DispositionAppraisalFormYupSchema } from './DispostionAppraisalFormYupSchema';

export interface IDispositionAppraisalFormProps {
  initialValues: DispositionAppraisalFormModel;
  loading: boolean;
  onSave: (
    appraisal: ApiGen_Concepts_DispositionFileAppraisal,
  ) => Promise<ApiGen_Concepts_DispositionFileAppraisal | undefined>;
  onCancel: () => void;
  onSuccess: () => void;
  onError: (e: AxiosError<IApiError>) => void;
}

const DispositionAppraisalForm: React.FC<IDispositionAppraisalFormProps> = ({
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
      <Formik<DispositionAppraisalFormModel>
        enableReinitialize
        validationSchema={DispositionAppraisalFormYupSchema}
        initialValues={initialValues}
        onSubmit={async (values: DispositionAppraisalFormModel, formikHelpers) => {
          try {
            if (values.isEmpty()) {
              onSuccess();
            }

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
          return (
            <>
              <LoadingBackdrop
                show={formikProps.isSubmitting || loading}
                parentScreen={true}
              ></LoadingBackdrop>
              <StyledContent>
                <Section header="Appraisal and Assessment">
                  <SectionField label="Appraisal value ($)" contentWidth="5">
                    <FastCurrencyInput formikProps={formikProps} field="appraisedValueAmount" />
                  </SectionField>
                  <SectionField label="Appraisal date">
                    <FastDatePicker field="appraisalDate" formikProps={formikProps} />
                  </SectionField>
                  <SectionField label="BC assessment value ($)" contentWidth="5">
                    <FastCurrencyInput formikProps={formikProps} field="bcaValueAmount" />
                  </SectionField>
                  <SectionField label="BC assessment roll year">
                    <FastDateYearPicker field="bcaRollYear" formikProps={formikProps} />
                  </SectionField>
                  <SectionField label="List price ($)" contentWidth="5">
                    <FastCurrencyInput formikProps={formikProps} field="listPriceAmount" />
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

export default DispositionAppraisalForm;

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
