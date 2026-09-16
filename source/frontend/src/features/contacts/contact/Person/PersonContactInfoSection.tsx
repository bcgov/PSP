import { useFormikContext } from 'formik';
import { AiOutlineExclamationCircle } from 'react-icons/ai';

import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import TooltipIcon from '@/components/common/TooltipIcon';
import * as Styled from '@/features/contacts/contact/edit/styles';

import { IEditablePersonForm } from '../../formModels';
import { ContactEmailList, ContactPhoneList } from '../create/components';

export interface IPersonContactInfoSectionProps {
  isContactMethodInvalid?: boolean;
}

export const PersonContactInfoSection: React.FunctionComponent<IPersonContactInfoSectionProps> = ({
  isContactMethodInvalid,
}) => {
  const { values } = useFormikContext<IEditablePersonForm>();

  return (
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
        <ContactEmailList field="emailContactMethods" contactEmails={values.emailContactMethods} />
      </SectionField>
      <SectionField label="Phone" className="mt-3">
        <ContactPhoneList field="phoneContactMethods" contactPhones={values.phoneContactMethods} />
      </SectionField>
    </Section>
  );
};

export default PersonContactInfoSection;
