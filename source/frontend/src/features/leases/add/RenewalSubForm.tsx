import { FieldArray } from 'formik';
import { FormikProps } from 'formik/dist/types';
import { Col, Row } from 'react-bootstrap';

import { LinkButton, RemoveButton } from '@/components/common/buttons';
import { FastDatePicker, TextArea } from '@/components/common/form';
import { YesNoSelect } from '@/components/common/form/YesNoSelect';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { getDeleteModalProps, useModalContext } from '@/hooks/useModalContext';

import { FormLeaseRenewal, LeaseFormModel } from '../models';

export interface IRenewalSubFormProps {
  formikProps: FormikProps<LeaseFormModel>;
}

export const RenewalSubForm: React.FunctionComponent<IRenewalSubFormProps> = ({ formikProps }) => {
  const { values, setFieldValue } = formikProps;
  const { setModalContent, setDisplayModal } = useModalContext();
  console.log(values, setFieldValue);

  return (
    <Section header="Renewal Option">
      <FieldArray
        name="renewals"
        render={arrayHelpers => (
          <>
            {values.renewals.map((renewal, index) => (
              <Row key={index}>
                <Col>
                  <Section header={`Renewal ${index + 1}`} noPadding>
                    <Row>
                      <Col xs="7">
                        <SectionField label="Excercised?" labelWidth="6" required>
                          <YesNoSelect field="isExerciced" notNullable />
                        </SectionField>
                      </Col>
                    </Row>
                    <Row>
                      <Col xs="7">
                        <SectionField label="Commencement" labelWidth="6" required>
                          <FastDatePicker field="commencementDt" formikProps={formikProps} />
                        </SectionField>
                      </Col>
                      <Col>
                        <SectionField label="Expiry" labelWidth="4" required>
                          <FastDatePicker field="expiryDt" formikProps={formikProps} />
                        </SectionField>
                      </Col>
                    </Row>
                    <SectionField label="Notes" contentWidth="12">
                      <TextArea field="renewalNote" />
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
              + Add a Renew
            </LinkButton>
          </>
        )}
      />
    </Section>
  );
};

export default RenewalSubForm;
