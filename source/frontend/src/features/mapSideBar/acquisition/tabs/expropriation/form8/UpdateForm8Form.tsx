import { Formik } from 'formik';
import styled from 'styled-components';

import { Select, SelectOption, TextArea } from '@/components/common/form';
import { ContactInputContainer } from '@/components/common/form/ContactInput/ContactInputContainer';
import ContactInputView from '@/components/common/form/ContactInput/ContactInputView';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { RestrictContactType } from '@/components/contact/ContactManagerView/ContactFilterComponent/ContactFilterComponent';
import { PayeeOption } from '@/features/mapSideBar/acquisition/models/PayeeOptionModel';
import SidebarFooter from '@/features/mapSideBar/shared/SidebarFooter';
import { getCancelModalProps, useModalContext } from '@/hooks/useModalContext';
import { Api_ExpropriationPayment } from '@/models/api/ExpropriationPayment';

import Form8PaymentItemsSubForm from './Form8PaymentItemsSubForm';
import { Form8FormModel } from './models/Form8FormModel';
import { Form8FormModelYupSchema } from './models/Form8FormYupSchema';

export interface IForm8FormProps {
  initialValues: Form8FormModel | null;
  gstConstant: number;
  payeeOptions: PayeeOption[];
  onSave: (form8: Api_ExpropriationPayment) => Promise<Api_ExpropriationPayment | undefined>;
  onCancel: () => void;
  onSuccess: () => void;
}

export const UpdateForm8Form: React.FC<IForm8FormProps> = ({
  initialValues,
  gstConstant,
  payeeOptions,
  onSave,
  onCancel,
  onSuccess,
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
    initialValues && (
      <StyledFormWrapper>
        <Formik<Form8FormModel>
          enableReinitialize
          initialValues={initialValues}
          onSubmit={async (values, formikHelpers) => {
            await onSave(values.toApi(payeeOptions));
            formikHelpers.setSubmitting(false);
            onSuccess();
          }}
          validationSchema={Form8FormModelYupSchema}
          validateOnChange={false}
        >
          {formikProps => {
            return (
              <>
                <StyledContent>
                  <Section header="Form 8 Notice of Advance Payment">
                    <SectionField label="Payee" labelWidth="4" required>
                      <Select
                        field="payeeKey"
                        title={
                          payeeOptions.find(p => p.value === formikProps.values.payeeKey)?.fullText
                        }
                        options={payeeOptions.map<SelectOption>(x => {
                          return { label: x.text, value: x.value, title: x.fullText };
                        })}
                        placeholder="Select..."
                      />
                    </SectionField>
                    <SectionField label="Expropriation authority" required>
                      <ContactInputContainer
                        field="expropriationAuthority.contact"
                        View={ContactInputView}
                        restrictContactType={RestrictContactType.ONLY_ORGANIZATIONS}
                        displayErrorAsTooltip={false}
                      ></ContactInputContainer>
                    </SectionField>
                    <SectionField label="Description">
                      <TextArea field="description" />
                    </SectionField>
                  </Section>

                  <Section header="Payment details" isCollapsable initiallyExpanded>
                    <Form8PaymentItemsSubForm
                      form8Id={initialValues.id!}
                      formikProps={formikProps}
                      gstConstantPercentage={gstConstant}
                    ></Form8PaymentItemsSubForm>
                    {formikProps.errors?.paymentItems &&
                      typeof formikProps.errors?.paymentItems === 'string' && (
                        <div className="invalid-feedback" data-testid="item-type-dup-error">
                          {formikProps.errors.paymentItems.toString()}
                        </div>
                      )}
                  </Section>
                </StyledContent>
                <StyledFooter>
                  <SidebarFooter
                    onSave={() => formikProps.submitForm()}
                    isOkDisabled={formikProps.isSubmitting || !formikProps.dirty}
                    onCancel={() => cancelFunc(formikProps.resetForm, formikProps.dirty)}
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

export default UpdateForm8Form;

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
`;
