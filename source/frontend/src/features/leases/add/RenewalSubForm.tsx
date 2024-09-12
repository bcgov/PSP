import { FieldArray } from 'formik';
import { FormikProps } from 'formik/dist/types';
import { Col, Row } from 'react-bootstrap';

import { LinkButton, RemoveButton } from '@/components/common/buttons';
import { FastDatePicker, TextArea } from '@/components/common/form';
import { YesNoSelect } from '@/components/common/form/YesNoSelect';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import TooltipIcon from '@/components/common/TooltipIcon';
import { getDeleteModalProps, useModalContext } from '@/hooks/useModalContext';

import { FormLeaseRenewal, LeaseFormModel } from '../models';

export interface IRenewalSubFormProps {
  formikProps: FormikProps<LeaseFormModel>;
}

export const RenewalSubForm: React.FunctionComponent<IRenewalSubFormProps> = ({ formikProps }) => {
  const { values } = formikProps;
  const { setModalContent, setDisplayModal } = useModalContext();

  const fieldName = 'renewals';

  return (
    <Section header="Renewal Options">
      <FieldArray
        name={fieldName}
        render={arrayHelpers => (
          <>
            {values.renewals.map((renewal, index) => (
              <Row key={index}>
                <Col>
                  <Section header={`Renewal ${index + 1}`} noPadding>
                    <Row>
                      <Col xs="7">
                        <SectionField label="Exercised?" labelWidth="6" required>
                          <YesNoSelect field={`${fieldName}.${index}.isExercised`} notNullable />
                        </SectionField>
                      </Col>
                    </Row>
                    <Row>
                      <Col xs="7">
                        <SectionField
                          label="Commencement"
                          labelWidth="6"
                          required={renewal.isExercised === true}
                          tooltip={
                            <TooltipIcon
                              toolTipId="lease-renewal-commencement-tooltip"
                              toolTip="The start date defined in the original agreement or renewal, as applicable"
                              placement="right"
                            />
                          }
                        >
                          <FastDatePicker
                            field={`${fieldName}.${index}.commencementDt`}
                            formikProps={formikProps}
                          />
                        </SectionField>
                      </Col>
                      <Col>
                        <SectionField
                          label="Expiry"
                          labelWidth="4"
                          required={renewal.isExercised === true}
                          tooltip={
                            <TooltipIcon
                              toolTipId="lease-renewal-expiry-tooltip"
                              toolTip="The end date specified in the original agreement or renewal, as applicable"
                              placement="right"
                            />
                          }
                        >
                          <FastDatePicker
                            field={`${fieldName}.${index}.expiryDt`}
                            formikProps={formikProps}
                          />
                        </SectionField>
                      </Col>
                    </Row>
                    <SectionField label="Notes" contentWidth="12">
                      <TextArea field={`${fieldName}.${index}.renewalNote`} />
                    </SectionField>
                  </Section>
                </Col>
                <Col xs="2" className="pt-2">
                  <RemoveButton
                    dataTestId={`renewal.${index}.remove-button`}
                    onRemove={() => {
                      setModalContent({
                        ...getDeleteModalProps(),
                        title: 'Remove Renewal',
                        message: `Removing a renewal will also delete any renewal data.
                          Do you want to proceed?`,
                        okButtonText: 'Yes',
                        cancelButtonText: 'No',
                        handleOk: async () => {
                          arrayHelpers.remove(index);
                          setDisplayModal(false);
                        },
                        handleCancel: () => {
                          setDisplayModal(false);
                        },
                      });
                      setDisplayModal(true);
                    }}
                  />
                </Col>
              </Row>
            ))}
            <LinkButton
              data-testid="add-file-owner"
              onClick={() => {
                const renewal = new FormLeaseRenewal();
                arrayHelpers.push(renewal);
              }}
            >
              + Add a Renewal
            </LinkButton>
          </>
        )}
      />
    </Section>
  );
};

export default RenewalSubForm;
