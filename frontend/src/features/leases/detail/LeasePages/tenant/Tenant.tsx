import { FormSection } from 'components/common/form/styles';
import { ContactMethodTypes } from 'constants/contactMethodType';
import { getApiPersonOrOrgMailingAddress, getDefaultContact } from 'features/contacts/contactUtils';
import * as Styled from 'features/leases/detail/styles';
import { FieldArray, Formik, getIn, useFormikContext } from 'formik';
import { IAddress, IContactSearchResult, ILease } from 'interfaces';
import ITypeCode from 'interfaces/ITypeCode';
import { Api_Address } from 'models/api/Address';
import { Api_LeaseTenant } from 'models/api/LeaseTenant';
import { Api_OrganizationPerson } from 'models/api/Organization';
import { Api_Person } from 'models/api/Person';
import * as React from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';
import { getPreferredContactMethodValue } from 'utils/contactMethodUtil';
import { withNameSpace } from 'utils/formUtils';
import { formatApiPersonNames } from 'utils/personUtils';

import TenantNotes from './TenantNotes';
import TenantOrganizationContactInfo from './TenantOrganizationContactInfo';
import TenantPersonContactInfo from './TenantPersonContactInfo';

class FormAddress {
  public readonly country?: string;
  public readonly streetAddress1?: string;
  public readonly streetAddress2?: string;
  public readonly streetAddress3?: string;
  public readonly municipality?: string;
  public readonly postal?: string;
  public readonly province?: string;

  constructor(baseModel?: Api_Address) {
    this.province = baseModel?.province?.description;
    this.country = baseModel?.country?.description;
    this.streetAddress1 = baseModel?.streetAddress1;
    this.streetAddress2 = baseModel?.streetAddress2;
    this.streetAddress3 = baseModel?.streetAddress3;
    this.municipality = baseModel?.municipality;
    this.postal = baseModel?.postal;
  }
}

export class FormTenant {
  public readonly id?: string;
  public readonly personId?: number;
  public readonly summary?: string;
  public readonly leaseId?: number;
  public readonly rowVersion?: number;
  public readonly leaseTenantId?: number;
  public readonly email?: string;
  public readonly mailingAddress?: FormAddress;
  public readonly municipalityName?: string;
  public readonly note?: string;
  public readonly organizationId?: number;
  public readonly landline?: string;
  public readonly mobile?: string;
  public readonly isDisabled?: boolean;
  public readonly organizationPersons?: Api_OrganizationPerson[];
  public readonly primaryContactId?: number;
  public readonly initialPrimaryContact?: Api_Person;
  public readonly lessorTypeCode?: ITypeCode<string>;
  public readonly original?: IContactSearchResult;

  constructor(apiModel?: Api_LeaseTenant, selectedContactModel?: IContactSearchResult) {
    if (!!apiModel) {
      // convert an api tenant to a form tenant.
      const tenant = apiModel.person ?? apiModel.organization;
      const address = !!tenant ? getApiPersonOrOrgMailingAddress(tenant) : undefined;
      this.id =
        apiModel.lessorTypeCode?.id === 'PER'
          ? `P${apiModel.personId}`
          : `O${apiModel.organizationId}`;
      this.personId = apiModel.personId;
      this.summary = apiModel.person ? formatApiPersonNames(tenant) : apiModel.organization?.name;
      this.leaseId = apiModel.leaseId;
      this.rowVersion = apiModel.rowVersion;
      this.email = getPreferredContactMethodValue(
        tenant?.contactMethods,
        ContactMethodTypes.WorkEmail,
        ContactMethodTypes.PersonalEmail,
      );
      this.mailingAddress = new FormAddress(address);
      this.municipalityName = address?.municipality ?? '';
      this.note = apiModel.note ?? '';
      this.organizationPersons = apiModel?.organization?.organizationPersons;
      this.organizationId = apiModel.organizationId;
      this.landline = getPreferredContactMethodValue(
        tenant?.contactMethods,
        ContactMethodTypes.WorkPhone,
      );
      this.mobile = getPreferredContactMethodValue(
        tenant?.contactMethods,
        ContactMethodTypes.PersonalPhone,
      );
      this.lessorTypeCode = apiModel.lessorTypeCode;
      this.primaryContactId = apiModel.primaryContactId;
      this.initialPrimaryContact = apiModel.primaryContact;
    } else if (!!selectedContactModel) {
      // In this case, construct a tenant using a contact.
      const primaryContact = getDefaultContact(selectedContactModel.organization);
      this.id = selectedContactModel?.id;
      this.personId = selectedContactModel.personId;
      this.summary = selectedContactModel.summary;
      this.email = selectedContactModel.email;
      this.mailingAddress = { streetAddress1: selectedContactModel.mailingAddress } as IAddress;
      this.municipalityName = selectedContactModel?.municipalityName;
      this.note = selectedContactModel.note;
      this.organizationId = selectedContactModel.organizationId;
      this.landline = selectedContactModel.landline;
      this.mobile = selectedContactModel.mobile;
      this.lessorTypeCode = !!this.personId ? { id: 'PER' } : { id: 'ORG' };
      this.organizationPersons = selectedContactModel?.organization?.organizationPersons;
      this.primaryContactId = primaryContact?.id;
      this.initialPrimaryContact = primaryContact;
      this.original = selectedContactModel;
    }
  }
}

export interface ITenantProps {
  nameSpace?: string;
}

/**
 * Tenant lease page displays all tenant information (persons and organizations)
 * @param {ITenantProps} param0
 */
export const Tenant: React.FunctionComponent<ITenantProps> = ({ nameSpace }) => {
  const { values: lease } = useFormikContext<ILease>();

  const tenants: Api_LeaseTenant[] = getIn(lease, withNameSpace(nameSpace, 'tenants')) ?? [];

  console.log(tenants);
  return (
    <FormSectionOne>
      <Formik initialValues={lease} onSubmit={() => {}} enableReinitialize>
        <FieldArray
          name={withNameSpace(nameSpace, 'properties')}
          render={renderProps => (
            <>
              {tenants.map((tenant: Api_LeaseTenant, index) => (
                <Styled.SpacedInlineListItem key={`tenants-${index}`}>
                  <FormSection>
                    <Row>
                      <Col>
                        {!!tenant.personId ? (
                          <TenantPersonContactInfo
                            disabled={true}
                            nameSpace={withNameSpace(nameSpace, `tenants.${index}`)}
                          />
                        ) : (
                          <TenantOrganizationContactInfo
                            disabled={true}
                            nameSpace={withNameSpace(nameSpace, `tenants.${index}`)}
                          />
                        )}
                      </Col>
                      <Col>
                        <TenantNotes disabled={true} nameSpace={`tenants.${index}`} />
                      </Col>
                    </Row>
                  </FormSection>
                </Styled.SpacedInlineListItem>
              ))}
              {tenants.length === 0 && (
                <>
                  <p>There are no tenants associated to this lease.</p>
                  <p>Click the edit icon to add tenants.</p>
                </>
              )}
            </>
          )}
        />
      </Formik>
    </FormSectionOne>
  );
};

export const FormSectionOne = styled(FormSection)`
  column-count: 1;
  & > * {
    break-inside: avoid-column;
  }
  column-gap: 10rem;
  background-color: white;
  li {
    list-style-type: none;
    padding: 2rem 0;
    margin: 0;
  }
  @media only screen and (max-width: 1500px) {
    column-count: 1;
  }
  min-width: 75rem;
  .form-control {
    color: ${props => props.theme.css.formTextColor};
  }
`;

export default Tenant;
