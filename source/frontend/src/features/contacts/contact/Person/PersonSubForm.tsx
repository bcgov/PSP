import { getIn, useFormikContext } from 'formik';
import { AiOutlineExclamationCircle } from 'react-icons/ai';
import styled from 'styled-components';

import { AsyncTypeahead, Check, Input, TextArea } from '@/components/common/form';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import TooltipIcon from '@/components/common/TooltipIcon';
import * as Styled from '@/features/contacts/contact/edit/styles';
import { usePersonOrganizationTypeahead } from '@/features/contacts/hooks/usePersonOrganizationTypeahead';

import { IEditablePersonForm } from '../../formModels';
import { Address, ContactEmailList, ContactPhoneList } from '../create/components';

interface IPersonSubFormProps {
  isContactMethodInvalid?: boolean;
}

const PersonSubForm: React.FunctionComponent<React.PropsWithChildren<IPersonSubFormProps>> = ({
  isContactMethodInvalid,
}) => {
  const { handleTypeaheadSearch, isTypeaheadLoading, matchedOrgs } =
    usePersonOrganizationTypeahead();
  const { values } = useFormikContext<IEditablePersonForm>();
  const organizationId = getIn(values, 'organization.id');
  const useOrganizationAddress = getIn(values, 'useOrganizationAddress');

  return (
    <>
      <Section header="Contact Details">
        <SectionField label="First name" required>
          <Input field="firstName" />
        </SectionField>
        <SectionField label="Middle">
          <Input field="middleNames" />
        </SectionField>
        <SectionField label="Last name" required>
          <Input field="surname" />
        </SectionField>
        <SectionField label="Preferred name">
          <Input field="preferredName" />
        </SectionField>
      </Section>
      <Section header="Organization">
        <SectionField label="Link to an existing organization">
          <AsyncTypeahead
            field="organization"
            labelKey="text"
            isLoading={isTypeaheadLoading}
            options={matchedOrgs}
            onSearch={handleTypeaheadSearch}
          />
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
      <Section
        isCollapsable
        initiallyExpanded
        header={
          <div className="d-flex align-items-center">
            <span>Mailing Address</span>
            <StyledCheckBox
              field="useOrganizationAddress"
              postLabel="Use mailing address from organization"
              disabled={!organizationId}
              className="ml-auto mb-0"
            />
          </div>
        }
      >
        <Address namespace="mailingAddress" disabled={useOrganizationAddress} />
      </Section>
      <Section header="Property Address" isCollapsable initiallyExpanded>
        <Address namespace="propertyAddress" />
      </Section>
      <Section header="Billing Address" isCollapsable initiallyExpanded>
        <Address namespace="billingAddress" />
      </Section>
      <Section header="Comments">
        <TextArea rows={5} field="comment" />
      </Section>
    </>
  );
};

export default PersonSubForm;

const StyledCheckBox = styled(Check)`
  font-size: 1.6rem;
  font-weight: normal;
  label {
    margin-bottom: 0;
  }
`;
