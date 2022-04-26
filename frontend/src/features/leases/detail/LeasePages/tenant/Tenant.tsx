import { FormSection } from 'components/common/form/styles';
import { ContactMethodTypes } from 'constants/contactMethodType';
import * as Styled from 'features/leases/detail/styles';
import { FieldArray, Formik, getIn, useFormikContext } from 'formik';
import { ILease, IOrganization, IProperty, ITenant } from 'interfaces';
import { findIndex } from 'lodash';
import { Api_PersonAddress } from 'models/api/Address';
import { Api_Person } from 'models/api/Person';
import * as React from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';
import { getContactMethodValue } from 'utils/contactMethodUtil';
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

  constructor(baseModel?: Api_PersonAddress[]) {
    if (baseModel === undefined || baseModel.length === 0) {
      return;
    }
    var firstAddress = baseModel[0];
    this.province = firstAddress.address?.province?.description;
    this.country = firstAddress.address?.country?.description;
    this.streetAddress1 = firstAddress.address?.streetAddress1;
    this.streetAddress2 = firstAddress.address?.streetAddress2;
    this.streetAddress3 = firstAddress.address?.streetAddress3;
    this.municipality = firstAddress.address?.municipality;
    this.postal = firstAddress.address?.postal;
  }
}
class FormPerson {
  public readonly id?: number;
  public readonly fullName?: string;
  public readonly mobile?: string;
  public readonly landline?: string;
  public readonly email?: string;
  public readonly address?: FormAddress;

  constructor(baseModel: Api_Person) {
    this.id = baseModel.id;
    this.fullName = formatApiPersonNames(baseModel);
    this.mobile =
      getContactMethodValue(baseModel.contactMethods, ContactMethodTypes.PersonalMobile) ||
      getContactMethodValue(baseModel.contactMethods, ContactMethodTypes.WorkMobile);
    this.landline =
      getContactMethodValue(baseModel.contactMethods, ContactMethodTypes.PersonalPhone) ||
      getContactMethodValue(baseModel.contactMethods, ContactMethodTypes.WorkPhone);
    this.email = getContactMethodValue(baseModel.contactMethods, ContactMethodTypes.WorkEmail);
    this.address = new FormAddress(baseModel.personAddresses);
  }
}

class FormTenant {
  public readonly programName?: string;
  public readonly motiName?: string;
  public readonly amount?: number;
  public readonly renewalCount: number;
  public readonly description?: string;
  public readonly isResidential: boolean;
  public readonly isCommercialBuilding: boolean;
  public readonly note?: string;
  public readonly tenantNotes: string[];
  public readonly tenants: ITenant[];
  public readonly properties: IProperty[];
  public readonly persons: FormPerson[];
  public readonly organizations: IOrganization[];

  constructor(baseModel: ILease) {
    this.programName = baseModel.programName;
    this.motiName = baseModel.motiName;
    this.amount = baseModel.amount;
    this.renewalCount = baseModel.renewalCount;
    this.description = baseModel.description;
    this.isResidential = baseModel.isResidential;
    this.isCommercialBuilding = baseModel.isCommercialBuilding;
    this.note = baseModel.note;
    this.tenantNotes = baseModel.tenantNotes;
    this.tenants = baseModel.tenants;
    this.properties = baseModel.properties;
    this.persons = baseModel.persons.map(p => new FormPerson(p));
    this.organizations = baseModel.organizations;
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
  const persons: Api_Person[] = getIn(lease, withNameSpace(nameSpace, 'persons')) ?? [];

  const tenantForm = new FormTenant(lease);
  const organizations: IOrganization[] =
    getIn(lease, withNameSpace(nameSpace, 'organizations')) ?? [];

  return (
    <FormSectionOne>
      <Formik initialValues={tenantForm} onSubmit={() => {}} enableReinitialize>
        <FieldArray
          name={withNameSpace(nameSpace, 'properties')}
          render={renderProps => (
            <>
              {persons.map((person: FormPerson, index) => (
                <Styled.SpacedInlineListItem key={`person-${index}`}>
                  <FormSection>
                    <Row>
                      <Col>
                        <TenantPersonContactInfo
                          disabled={true}
                          nameSpace={withNameSpace(nameSpace, `persons.${index}`)}
                        />
                      </Col>
                      <Col>{getTenantPersonNotes(person, lease.tenants)}</Col>
                    </Row>
                  </FormSection>
                </Styled.SpacedInlineListItem>
              ))}
              {organizations.map((organization: IOrganization, index) => (
                <Styled.SpacedInlineListItem key={`organizations-${index}`}>
                  <FormSection>
                    <Row>
                      <Col>
                        <TenantOrganizationContactInfo
                          disabled={true}
                          nameSpace={withNameSpace(nameSpace, `organizations.${index}`)}
                        />
                      </Col>
                      <Col>{getTenantOrganizationNotes(organization, lease.tenants)}</Col>
                    </Row>
                  </FormSection>
                </Styled.SpacedInlineListItem>
              ))}
              {persons.length === 0 && organizations.length === 0 && (
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

const getTenantPersonNotes = (person: Api_Person, tenants: ITenant[]) => {
  const personNoteIndex = findIndex(tenants, (tenant: ITenant) => tenant.personId === person.id);
  return personNoteIndex >= 0 ? (
    <TenantNotes disabled={true} nameSpace={`tenants.${personNoteIndex}`} />
  ) : null;
};

const getTenantOrganizationNotes = (organization: IOrganization, tenants: ITenant[]) => {
  const organizationNodeIndex = findIndex(
    tenants,
    (tenant: ITenant) => tenant.organizationId === organization.id,
  );
  return organizationNodeIndex >= 0 ? (
    <TenantNotes disabled={true} nameSpace={`tenants.${organizationNodeIndex}`} />
  ) : null;
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
