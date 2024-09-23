import { createMemoryHistory } from 'history';

import { ContactMethodTypes } from '@/constants/contactMethodType';
import { phoneFormatter, toTypeCodeNullable } from '@/utils/formUtils';
import { render, RenderOptions } from '@/utils/test-utils';

import OrganizationView, { OrganizationViewProps } from './Organization';
import { fakeAddresses } from './utils';
import { ApiGen_CodeTypes_AddressUsageTypes } from '@/models/api/generated/ApiGen_CodeTypes_AddressUsageTypes';
import { getMockOrganization } from '@/mocks/organization.mock';
import { ApiGen_Concepts_ContactMethod } from '@/models/api/generated/ApiGen_Concepts_ContactMethod';
import { ApiGen_Concepts_OrganizationAddress } from '@/models/api/generated/ApiGen_Concepts_OrganizationAddress';
import { getMockAddresses } from '@/mocks/bcAssessment.mock';
import { ApiGen_Concepts_Person } from '@/models/api/generated/ApiGen_Concepts_Person';
import { getMockPerson } from '@/mocks/contacts.mock';
import { ApiGen_Concepts_PersonOrganization } from '@/models/api/generated/ApiGen_Concepts_PersonOrganization';
import { getMockApiAddress } from '@/mocks/address.mock';

const history = createMemoryHistory();

describe('Contact OrganizationView component', () => {
  const setup = (renderOptions: RenderOptions & OrganizationViewProps) => {
    // render component under test
    const component = render(<OrganizationView organization={renderOptions.organization} />, {
      ...renderOptions,
      history,
    });

    return {
      component,
    };
  };
  it('renders as expected', () => {
    const { component } = setup({ organization: getMockOrganization() });
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('Shows status information', () => {
    const { component } = setup({
      organization: {
        ...getMockOrganization(),
        isDisabled: true,
      },
    });

    const statusElement = component.getByTestId('contact-organization-status');
    expect(statusElement.textContent).toBe('INACTIVE');
  });

  it('Shows base information', () => {
    const testName = 'My corp name';
    const testAlias = 'My corp alias';
    const testIncNumber = '77789';
    const { component } = setup({
      organization: {
        ...getMockOrganization(),
        name: testName,
        alias: testAlias,
        incorporationNumber: testIncNumber,
      },
    });

    const statusElement = component.getByTestId('contact-organization-status');
    expect(statusElement.textContent).toBe('ACTIVE');

    const nameElement = component.getByTestId('contact-organization-organizationName');
    expect(nameElement.textContent).toBe(testName);

    const aliasElement = component.getByTestId('contact-organization-alias');
    expect(aliasElement.textContent).toBe(testAlias);

    const incorporationElement = component.getByTestId('contact-organization-incorporationNumber');
    expect(incorporationElement.textContent).toBe(testIncNumber);
  });

  it('Shows email information', () => {
    const personalEmail: ApiGen_Concepts_ContactMethod = {
      personId: null,
      organizationId: 1,
      id: 1,
      rowVersion: 0,
      contactMethodType: {
        id: ContactMethodTypes.PersonalEmail,
        description: 'Personal Email',
        isDisabled: false,
        displayOrder: null,
      },
      value: 'test@bench.com',
    };
    const workEmail: ApiGen_Concepts_ContactMethod = {
      personId: null,
      organizationId: 1,
      id: 2,
      rowVersion: 0,
      contactMethodType: {
        id: ContactMethodTypes.WorkEmail,
        description: 'Work Email',
        displayOrder: null,
        isDisabled: false,
      },
      value: 'test@bench.net',
    };

    const contactInfo: ApiGen_Concepts_ContactMethod[] = [personalEmail, workEmail];
    const { component } = setup({
      organization: {
        ...getMockOrganization(),
        contactMethods: contactInfo,
      },
    });

    const emailValueElements = component.getAllByTestId('email-value');
    expect(emailValueElements.length).toBe(2);

    // Verify that the display is in the correct order
    expect(emailValueElements[0].textContent).toBe(workEmail.value);
    expect(emailValueElements[1].textContent).toBe(personalEmail.value);
  });

  it('Shows phone information', () => {
    const faxPhone: ApiGen_Concepts_ContactMethod = {
      organizationId: 1,
      personId: null,
      id: 1,
      rowVersion: 0,
      contactMethodType: {
        id: ContactMethodTypes.Fax,
        description: 'Fax',
        displayOrder: null,
        isDisabled: false,
      },
      value: '123456789',
    };
    const personalPhone: ApiGen_Concepts_ContactMethod = {
      organizationId: 1,
      personId: null,
      id: 2,
      rowVersion: 0,
      contactMethodType: {
        id: ContactMethodTypes.PersonalPhone,
        description: 'Personal Phone',
        displayOrder: null,
        isDisabled: false,
      },
      value: '800123123',
    };
    const workPhone: ApiGen_Concepts_ContactMethod = {
      organizationId: 1,
      personId: null,
      id: 3,
      rowVersion: 0,
      contactMethodType: {
        id: ContactMethodTypes.WorkPhone,
        description: 'Work Phone',
        displayOrder: null,
        isDisabled: false,
      },
      value: '555123123',
    };
    const workMobile: ApiGen_Concepts_ContactMethod = {
      organizationId: 1,
      personId: null,
      id: 4,
      rowVersion: 0,
      contactMethodType: {
        id: ContactMethodTypes.WorkMobile,
        description: 'Work mobil',
        displayOrder: null,
        isDisabled: false,
      },
      value: '800123123',
    };
    const personalMobile: ApiGen_Concepts_ContactMethod = {
      organizationId: 1,
      personId: null,
      id: 5,
      rowVersion: 0,
      contactMethodType: {
        id: ContactMethodTypes.PersonalMobile,
        description: 'Personal Mobile',
        displayOrder: null,
        isDisabled: false,
      },
      value: '750748789',
    };

    const contactInfo: ApiGen_Concepts_ContactMethod[] = [
      faxPhone,
      personalPhone,
      workPhone,
      workMobile,
      personalMobile,
    ];
    const { component } = setup({
      organization: {
        ...getMockOrganization(),
        contactMethods: contactInfo,
      },
    });

    const phoneValueElements = component.getAllByTestId('phone-value');
    expect(phoneValueElements.length).toBe(5);

    // Verify that the display is in the correct order
    expect(phoneValueElements[0].textContent).toBe(phoneFormatter(workMobile.value));
    expect(phoneValueElements[1].textContent).toBe(phoneFormatter(workPhone.value));
    expect(phoneValueElements[2].textContent).toBe(phoneFormatter(personalMobile.value));
    expect(phoneValueElements[3].textContent).toBe(phoneFormatter(faxPhone.value));
    expect(phoneValueElements[4].textContent).toBe(phoneFormatter(personalPhone.value));
  });

  it('Shows address information', () => {
    const mailingAddress: ApiGen_Concepts_OrganizationAddress = {
      id: 1,
      rowVersion: 0,
      organizationId: 1,
      address: {
        ...getMockApiAddress(),
        streetAddress1: 'Test Street',
        province: { id: 1, code: 'BC', description: 'BC', displayOrder: 1 },
      },
      addressUsageType: {
        id: ApiGen_CodeTypes_AddressUsageTypes.MAILING,
        description: 'Mailing Address',
        displayOrder: null,
        isDisabled: false,
      },
    };
    const residentialAddress: ApiGen_Concepts_OrganizationAddress = {
      id: 2,
      rowVersion: 0,
      organizationId: 1,
      address: {
        ...getMockApiAddress(),
        streetAddress1: 'Fixture Street',
        province: { id: 1, code: 'BC', description: 'BC', displayOrder: 1 },
      },
      addressUsageType: {
        id: ApiGen_CodeTypes_AddressUsageTypes.RESIDNT,
        description: 'Residential Address',
        displayOrder: null,
        isDisabled: false,
      },
    };

    const addressInfo: ApiGen_Concepts_OrganizationAddress[] = [mailingAddress, residentialAddress];
    const { component } = setup({
      organization: {
        ...getMockOrganization(),
        organizationAddresses: addressInfo,
      },
    });

    const addressElements = component.getAllByTestId('contact-organization-address');
    expect(addressElements.length).toBe(2);

    // Verify that the display is in the correct order
    expect(addressElements[0].textContent).toBe(
      `${mailingAddress.address.streetAddress1} N/A ${mailingAddress.address.municipality} ${
        mailingAddress.address.province!.code
      } ${mailingAddress.address.postal} ${mailingAddress.address.country?.description}`,
    );
    expect(addressElements[1].textContent).toBe(
      `${residentialAddress.address.streetAddress1} N/A ${
        residentialAddress.address.municipality
      } ${residentialAddress.address.province!.code} ${residentialAddress.address.postal} ${
        residentialAddress.address.country?.description
      }`,
    );
  });

  it(`Shows address information when 'Other' country selected and no province is supplied`, () => {
    const mailingAddress: ApiGen_Concepts_OrganizationAddress = {
      id: 1,
      rowVersion: 0,
      organizationId: 1,
      address: {
        ...getMockApiAddress(),
        streetAddress1: 'Test Street',
        countryOther: 'other country info',
        country: { id: 1, code: 'OTHER', description: 'Other', displayOrder: 1 },
      },
      addressUsageType: {
        id: ApiGen_CodeTypes_AddressUsageTypes.MAILING,
        description: 'Mailing Address',
        displayOrder: null,
        isDisabled: false,
      },
    };

    const addressInfo: ApiGen_Concepts_OrganizationAddress[] = [mailingAddress];
    const { component } = setup({
      organization: {
        ...getMockOrganization(),
        organizationAddresses: addressInfo,
      },
    });

    const addressElement = component.getByTestId('contact-organization-address');

    // Verify that the display is in the correct order
    expect(addressElement.textContent).toBe(
      `${mailingAddress.address.streetAddress1} N/A ${mailingAddress.address.municipality} ${mailingAddress.address.province.code} ${mailingAddress.address.postal} ${mailingAddress.address.countryOther}`,
    );
  });

  it('Shows individual contacts information', () => {
    const personsInfo: ApiGen_Concepts_PersonOrganization[] = [
      {
        personId: 1,
        person: getMockPerson({ id: 1, firstName: 'Sarah', surname: 'Abraham' }),
        organization: null,
        organizationId: null,
        rowVersion: 0,
        id: 1,
      },
      {
        personId: 2,
        person: getMockPerson({ id: 1, firstName: 'Sam', surname: 'Johnson' }),
        organization: null,
        organizationId: null,
        rowVersion: 0,
        id: 2,
      },
      {
        personId: 3,
        person: getMockPerson({ id: 1, firstName: 'Pete', surname: 'Zamboni' }),
        organization: null,
        organizationId: null,
        rowVersion: 0,
        id: 3,
      },
    ];
    const { component } = setup({
      organization: {
        ...getMockOrganization(),
        organizationPersons: personsInfo,
      },
    });

    const personElements = component.getAllByTestId(`contact-organization-person`);
    expect(personElements.length).toBe(3);

    // Verify that the display is in the correct order
    expect(personElements[0].textContent).toBe('Sarah Abraham');
    expect(personElements[1].textContent).toBe('Sam Johnson');
    expect(personElements[2].textContent).toBe('Pete Zamboni');
  });

  it('Orders address information correctly', () => {
    const fakeAddresses: ApiGen_Concepts_OrganizationAddress[] = [
      {
        organizationId: 1,
        addressUsageType: toTypeCodeNullable(ApiGen_CodeTypes_AddressUsageTypes.MAILING),
        address: { ...getMockApiAddress(), streetAddress1: 'Mailing address' },
        id: 1,
        rowVersion: 1,
      },
      {
        organizationId: 1,
        addressUsageType: toTypeCodeNullable(ApiGen_CodeTypes_AddressUsageTypes.RESIDNT),
        address: { ...getMockApiAddress(), streetAddress1: 'Property address' },
        id: 1,
        rowVersion: 1,
      },
      {
        organizationId: 1,
        addressUsageType: toTypeCodeNullable(ApiGen_CodeTypes_AddressUsageTypes.BILLING),
        address: { ...getMockApiAddress(), streetAddress1: 'Billing address' },
        id: 1,
        rowVersion: 1,
      },
    ];
    const { component } = setup({
      organization: {
        ...getMockOrganization(),
        organizationAddresses: fakeAddresses,
      },
    });

    const addressElements = component.getAllByTestId('contact-organization-address');
    expect(addressElements.length).toBe(3);

    expect(addressElements[0].children[0]).toHaveTextContent('Mailing address');
    expect(addressElements[1].children[0]).toHaveTextContent('Property address');
    expect(addressElements[2].children[0]).toHaveTextContent('Billing address');
  });

  it('Shows comment information', () => {
    const testComment = 'A test comment :)';
    const { component } = setup({
      organization: {
        ...getMockOrganization(),
        comment: testComment,
      },
    });

    const commentElement = component.getByTestId('contact-organization-comment');
    expect(commentElement.textContent).toBe(testComment);
  });
});
