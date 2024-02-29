import axios, { AxiosError } from 'axios';
import { Formik } from 'formik';
import styled from 'styled-components';

import {
  FastCurrencyInput,
  FastDatePicker,
  Input,
  Select,
  TextArea,
} from '@/components/common/form';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import * as API from '@/constants/API';
import SidebarFooter from '@/features/mapSideBar/shared/SidebarFooter';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { getCancelModalProps, useModalContext } from '@/hooks/useModalContext';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_Concepts_DispositionFileOffer } from '@/models/api/generated/ApiGen_Concepts_DispositionFileOffer';

import { DispositionOfferFormModel } from '../models/DispositionOfferFormModel';
import { DispositionOfferFormYupSchema } from '../models/DispositionOfferFormYupSchema';

export interface IDispositionOfferFormProps {
  initialValues: DispositionOfferFormModel | null;
  showOfferStatusError: boolean;
  loading: boolean;
  onSave: (
    offer: ApiGen_Concepts_DispositionFileOffer,
  ) => Promise<ApiGen_Concepts_DispositionFileOffer | undefined>;
  onCancel: () => void;
  onSuccess: () => void;
  onError: (e: AxiosError<IApiError>) => void;
}

const DispositionOfferForm: React.FC<IDispositionOfferFormProps> = ({
  initialValues,
  showOfferStatusError,
  loading,
  onSave,
  onCancel,
  onSuccess,
  onError,
}) => {
  const { setModalContent, setDisplayModal } = useModalContext();
  const { getOptionsByType } = useLookupCodeHelpers();

  const offerStatusTypes = getOptionsByType(API.DISPOSITION_OFFER_STATUS_TYPES);

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
    initialValues && (
      <StyledFormWrapper>
        <Formik<DispositionOfferFormModel>
          enableReinitialize
          validationSchema={DispositionOfferFormYupSchema}
          initialValues={initialValues}
          onSubmit={async (values, formikHelpers) => {
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
            return (
              <>
                <LoadingBackdrop
                  show={formikProps.isSubmitting || loading}
                  parentScreen={true}
                ></LoadingBackdrop>
                <StyledContent>
                  <Section header="Offer">
                    <SectionField
                      label="Offer status"
                      contentWidth="6"
                      tooltip="Open = Offer has been received.
                    Rejected, = Offer was not responded to (due to receiving a better competing offer or the offer was just highly undesirable).
                    Countered, = Offer was responded to with a counteroffer. If counteroffer is accepted, new terms should be recorded in Notes.
                    Accepted= Offer was accepted as-is.
                    Collapsed= Offer was cancelled or abandoned."
                    >
                      <Select
                        field="dispositionOfferStatusTypeCode"
                        options={offerStatusTypes}
                        placeholder="Select..."
                      />
                      {showOfferStatusError && (
                        <div className="invalid-feedback" data-testid="team-profile-dup-error">
                          An Offer with &apos;Accepted&apos; status already exists.
                        </div>
                      )}
                    </SectionField>
                    <SectionField label="Offer name(s)" labelWidth="4" contentWidth="6" required>
                      <Input field="offerName" />
                    </SectionField>
                    <SectionField label="Offer date" required>
                      <FastDatePicker field="offerDate" formikProps={formikProps} />
                    </SectionField>
                    <SectionField label="Offer expiry date">
                      <FastDatePicker field="offerExpiryDate" formikProps={formikProps} />
                    </SectionField>
                    <SectionField label="Offer price ($)" contentWidth="5" required>
                      <FastCurrencyInput formikProps={formikProps} field="offerAmount" />
                    </SectionField>
                    <SectionField
                      label="Notes"
                      contentWidth="12"
                      tooltip="Provide any additional details such as offer terms or conditions, and any commentary on why the offer was accepted/countered/rejected."
                    >
                      <TextArea field="offerNote" />
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
    )
  );
};

export default DispositionOfferForm;

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
