import { FieldArray, Formik } from 'formik';
import * as React from 'react';
import { Col, Row } from 'react-bootstrap';
import { MdClose } from 'react-icons/md';
import styled from 'styled-components';

import { Button, LinkButton, RemoveButton } from '@/components/common/buttons';
import { Input } from '@/components/common/form';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';

import { defaultAgreementFormData, IAgreementFormData } from './models';

export interface IAgreementFormProps {
  onSubmit: (values: IAgreementFormData) => Promise<void>;
}

//TODO: POC implementation only.
export const AgreementForm: React.FunctionComponent<IAgreementFormProps> = ({ onSubmit }) => {
  return (
    <Formik
      initialValues={defaultAgreementFormData}
      onSubmit={async (values, helpers) => {
        try {
          helpers.setSubmitting(true);
          await onSubmit(values);
        } finally {
          helpers.setSubmitting(false);
        }
      }}
    >
      {({ values, submitForm, isSubmitting }) => (
        <>
          <Section header="Project">
            <SectionField label="Project name">
              <Input field="project_name" />
            </SectionField>
            <SectionField label="Project number">
              <Input field="project_number" />
            </SectionField>
            <SectionField label="Product name">
              <Input field="road" />
            </SectionField>

            <SectionField label="PS file number">
              <Input field="ps_file_no" />
            </SectionField>
          </Section>
          <Section header="Price (in CAD)">
            <SectionField label="Purchase price">
              <Input field="purchase_price" />
            </SectionField>
            <SectionField label="Deposit amount">
              <Input field="deposit_amount" />
            </SectionField>
          </Section>
          <Section header="Participants">
            <Section header="Owners">
              <FieldArray
                name="owners"
                render={arrayHelpers => (
                  <>
                    {values.owners.map((owner, index) => (
                      <Section
                        key={`owner-${index}`}
                        header={
                          <div className="d-flex justify-content-between">
                            Owner #{index + 1}
                            <RemoveButton onRemove={() => arrayHelpers.remove(index)}>
                              <MdClose size="2rem" /> <span className="text">Remove</span>
                            </RemoveButton>
                          </div>
                        }
                      >
                        <React.Fragment>
                          <SectionField label="First name">
                            <Input field={`owners.${index}.firstname`} />
                          </SectionField>
                          <SectionField label="Last name">
                            <Input field={`owners.${index}.lastname`} />
                          </SectionField>
                          <SectionField label="Organization name">
                            <Input field={`owners.${index}.organization_name`} />
                          </SectionField>
                          <SectionField label="Incorporation number">
                            <Input field={`owners.${index}.incorporation_number`} />
                          </SectionField>
                          <SectionField label="Reg number">
                            <Input field={`owners.${index}.reg_no`} />
                          </SectionField>
                        </React.Fragment>
                      </Section>
                    ))}
                    <LinkButton
                      onClick={() =>
                        arrayHelpers.push({
                          firstname: '',
                          organization_name: '',
                          incorporation_number: '',
                          reg_no: '',
                          lastname: '',
                        })
                      }
                    >
                      + Add another owner
                    </LinkButton>
                  </>
                )}
              />
            </Section>
            <Section header="Agents">
              <FieldArray
                name="agents"
                render={arrayHelpers => (
                  <>
                    {values.agents.map((owner, index) => (
                      <Section
                        key={`agent-${index}`}
                        header={
                          <div className="d-flex justify-content-between">
                            Agent #{index + 1}
                            <RemoveButton onRemove={() => arrayHelpers.remove(index)}>
                              <MdClose size="2rem" /> <span className="text">Remove</span>
                            </RemoveButton>
                          </div>
                        }
                      >
                        <React.Fragment>
                          <SectionField label="Phone # (work landline)">
                            <Input field={`agents.${index}.workphone`} />
                          </SectionField>
                          <SectionField label="Phone # (work mobile)">
                            <Input field={`agents.${index}.mobilephone`} />
                          </SectionField>
                          <SectionField label="Organization name">
                            <Input field={`agents.${index}.name`} />
                          </SectionField>
                        </React.Fragment>
                      </Section>
                    ))}
                    <LinkButton
                      onClick={() =>
                        arrayHelpers.push({
                          firstname: '',
                          organization_name: '',
                          incorporation_number: '',
                          reg_no: '',
                          lastname: '',
                        })
                      }
                    >
                      + Add another agent
                    </LinkButton>
                  </>
                )}
              />
            </Section>
          </Section>
          <Section header="Dates">
            <SectionField label="Sale completion date">
              <Input field="completion_date" />
            </SectionField>
          </Section>
          <Section header="Properties">
            <FieldArray
              name="properties"
              render={arrayHelpers => (
                <>
                  {values.properties.map((property, index) => (
                    <Section
                      key={`property-${index}`}
                      header={
                        <div className="d-flex justify-content-between">
                          Property #{index + 1}
                          <RemoveButton onRemove={() => arrayHelpers.remove(index)}>
                            <MdClose size="2rem" /> <span className="text">Remove</span>
                          </RemoveButton>
                        </div>
                      }
                    >
                      <SectionField label="PID">
                        <Input field={`properties.${index}.pid`} />
                      </SectionField>
                      <SectionField label="Legal description">
                        <Input field={`properties.${index}.full_legal`} />
                      </SectionField>
                    </Section>
                  ))}
                  <LinkButton onClick={() => arrayHelpers.push({ pid: '', full_legal: '' })}>
                    + Add another property
                  </LinkButton>
                </>
              )}
            />
          </Section>
          <SidebarFooterBar className="justify-content-end mt-auto no-gutters">
            <Col xs="auto">
              <Button onClick={submitForm} disabled={isSubmitting}>
                Generate
              </Button>
            </Col>
          </SidebarFooterBar>
        </>
      )}
    </Formik>
  );
};

const SidebarFooterBar = styled(Row)`
  position: sticky;
  padding-top: 2rem;
  bottom: 0;
  background: white;
  z-index: 10;
`;

export default AgreementForm;
