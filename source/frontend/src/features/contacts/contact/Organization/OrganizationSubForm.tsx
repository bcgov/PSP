import { useFormikContext } from 'formik';
import { AiOutlineExclamationCircle } from 'react-icons/ai';

import { Input } from '@/components/common/form';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import TooltipIcon from '@/components/common/TooltipIcon';
import * as Styled from '@/features/contacts/contact/edit/styles';

import { IEditableOrganizationForm } from '../../formModels';
import { ContactEmailList, ContactPhoneList } from '../create/components';

interface IOrganizationSubFormProps {
  isContactMethodInvalid: boolean;
}

export const OrganizationSubForm: React.FunctionComponent<IOrganizationSubFormProps> = ({
  isContactMethodInvalid,
}) => {
  const { values } = useFormikContext<IEditableOrganizationForm>();

  return (
    <>
      <Section header="Contact Details">
        <SectionField label="Organization name" required contentWidth="6">
          <Input field="name" />
        </SectionField>
        <SectionField label="Alias" contentWidth="6">
          <Input field="alias" />
        </SectionField>
        <SectionField label="Incorporation number" contentWidth="6">
          <Input field="incorporationNumber" />
        </SectionField>
      </Section>
      <Section
        header={
          <div className="d-flex align-items-center">
            <span>Contact Info</span>
            <TooltipIcon
              toolTipId="contactInfoToolTip"
              innerClassName="ml-4 mb-1"
              toolTip="Contacts must have a minimum of one method of contact to be saved. (ex: email,phone or address)"
            />
          </div>
        }
      >
        {isContactMethodInvalid && (
          <Styled.SectionMessage appearance="error" gap="0.5rem">
            <AiOutlineExclamationCircle size="1.8rem" className="mt-2" />
            <p>
              Contacts must have a minimum of one method of contact to be saved. <br />
              <em>(ex: email,phone or address)</em>
            </p>
          </Styled.SectionMessage>
        )}
        <SectionField label="Email">
          <ContactEmailList
            field="emailContactMethods"
            contactEmails={values.emailContactMethods}
          />
        </SectionField>
        <SectionField label="Phone" className="mt-3">
          <ContactPhoneList
            field="phoneContactMethods"
            contactPhones={values.phoneContactMethods}
          />
        </SectionField>
      </Section>
    </>
  );
};

export default OrganizationSubForm;
