import { createMemoryHistory } from 'history';

import { ContactMethodTypes } from '@/constants/contactMethodType';
import { phoneFormatter } from '@/utils/formUtils';
import { render, RenderOptions } from '@/utils/test-utils';

import PersonFormView, { PersonFormViewProps } from './Person';
import { fakeAddresses } from './utils';
import { ApiGen_CodeTypes_AddressUsageTypes } from '@/models/api/generated/ApiGen_CodeTypes_AddressUsageTypes';
import { getMockPerson } from '@/mocks/contacts.mock';
import { ApiGen_Concepts_ContactMethod } from '@/models/api/generated/ApiGen_Concepts_ContactMethod';
import { ApiGen_Concepts_Organization } from '@/models/api/generated/ApiGen_Concepts_Organization';
import { getMockOrganization } from '@/mocks/organization.mock';
import { ApiGen_Concepts_PersonOrganization } from '@/models/api/generated/ApiGen_Concepts_PersonOrganization';
import { ApiGen_Concepts_Address } from '@/models/api/generated/ApiGen_Concepts_Address';
import { ApiGen_Concepts_PersonAddress } from '@/models/api/generated/ApiGen_Concepts_PersonAddress';
import { getMockAddresses } from '@/mocks/bcAssessment.mock';
import { getMockApiAddress } from '@/mocks/address.mock';

const history = createMemoryHistory();

describe('Contact PersonView component', () => {
  const setup = (renderOptions: RenderOptions & PersonFormViewProps) => {
    // render component under test
    const component = render(<PersonFormView person={renderOptions.person} />, {
      ...renderOptions,
      history,
    });

    return {
      component,
    };
  };
  it('renders as expected', () => {
    const { component } = setup({
      person: getMockPerson({ id: 1, surname: 'person', firstName: 'test' }),
    });
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('Shows status information', () => {
    const { component } = setup({
      person: {
        ...getMockPerson({ id: 1, surname: 'person', firstName: 'test' }),
        isDisabled: true,
      },
    });

    const statusElement = component.getByTestId('contact-person-status');
    expect(statusElement.textContent).toBe('INACTIVE');
  });

  it('Shows base information', () => {
    const testName = 'John Doe Smith';
    const preferredName = 'My corp alias';
    const { component } = setup({
      person: {
        ...getMockPerson({ id: 1, surname: 'person', firstName: 'test' }),
        firstName: 'John',
        middleNames: 'Doe',
        surname: 'Smith',
        preferredName: preferredName,
      },
    });

    const statusElement = component.getByTestId('contact-person-status');
    expect(statusElement.textContent).toBe('ACTIVE');

    const nameElement = component.getByTestId('contact-person-fullname');
    expect(nameElement.textContent).toBe(testName);

    const preferredElement = component.getByTestId('contact-person-preferred');
    expect(preferredElement.textContent).toBe(preferredName);
  });

  it('Shows email information', () => {
    const personalEmail: ApiGen_Concepts_ContactMethod = {
      personId: 1,
      organizationId: null,
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
      personId: 1,
      organizationId: null,
      id: 2,
      rowVersion: 0,
      contactMethodType: {
        id: ContactMethodTypes.WorkEmail,
        description: 'Work Email',
        isDisabled: false,
        displayOrder: null,
      },
      value: 'test@bench.net',
    };

    const contactInfo: ApiGen_Concepts_ContactMethod[] = [personalEmail, workEmail];
    const { component } = setup({
      person: {
        ...getMockPerson({ id: 1, surname: 'person', firstName: 'test' }),
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
      personId: 1,
      organizationId: null,
      id: 1,
      rowVersion: 0,
      contactMethodType: {
        id: ContactMethodTypes.Fax,
        description: 'Fax',
        isDisabled: false,
        displayOrder: null,
      },
      value: '123456789',
    };
    const personalPhone: ApiGen_Concepts_ContactMethod = {
      personId: 1,
      organizationId: null,
      id: 2,
      rowVersion: 0,
      contactMethodType: {
        id: ContactMethodTypes.PersonalPhone,
        description: 'Personal Phone',
        isDisabled: false,
        displayOrder: null,
      },
      value: '800123123',
    };
    const workPhone: ApiGen_Concepts_ContactMethod = {
      personId: 1,
      organizationId: null,
      id: 3,
      rowVersion: 0,
      contactMethodType: {
        id: ContactMethodTypes.WorkPhone,
        description: 'Work Phone',
        isDisabled: false,
        displayOrder: null,
      },
      value: '555123123',
    };
    const workMobile: ApiGen_Concepts_ContactMethod = {
      personId: 1,
      organizationId: null,
      id: 4,
      rowVersion: 0,
      contactMethodType: {
        id: ContactMethodTypes.WorkMobile,
        description: 'Work mobil',
        isDisabled: false,
        displayOrder: null,
      },
      value: '800123123',
    };
    const personalMobile: ApiGen_Concepts_ContactMethod = {
      personId: 1,
      organizationId: null,
      id: 5,
      rowVersion: 0,
      contactMethodType: {
        id: ContactMethodTypes.PersonalMobile,
        description: 'Personal Mobile',
        isDisabled: false,
        displayOrder: null,
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
      person: {
        ...getMockPerson({ id: 1, surname: 'person', firstName: 'test' }),
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

  it('Shows organization information', () => {
    const organizationsInfo: ApiGen_Concepts_PersonOrganization[] = [
      {
        id: 1,
        organizationId: 2,
        person: null,
        personId: null,
        organization: getMockOrganization(),
        rowVersion: 1,
      },
    ];
    const { component } = setup({
      person: {
        ...getMockPerson({ id: 1, surname: 'person', firstName: 'test' }),
        personOrganizations: organizationsInfo,
      },
    });

    const organizationElements = component.getAllByTestId('contact-person-organization');
    expect(organizationElements.length).toBe(1);

    // Verify that the display is in the correct order
    expect(organizationElements[0].textContent).toBe(organizationsInfo[0].organization.name);
  });

  it('Shows address information', () => {
    const mailingAddress: ApiGen_Concepts_PersonAddress = {
      personId: 1,
      id: 2,
      rowVersion: 0,
      address: getMockApiAddress(),
      addressUsageType: {
        id: ApiGen_CodeTypes_AddressUsageTypes.MAILING,
        description: 'Residential Mailing',
        isDisabled: false,
        displayOrder: null,
      },
    };
    const residentialAddress: ApiGen_Concepts_PersonAddress = {
      personId: 1,
      id: 2,
      rowVersion: 0,
      address: getMockApiAddress(),
      addressUsageType: {
        id: ApiGen_CodeTypes_AddressUsageTypes.RESIDNT,
        description: 'Residential Address',
        isDisabled: false,
        displayOrder: null,
      },
    };

    const addressInfo: ApiGen_Concepts_PersonAddress[] = [mailingAddress, residentialAddress];
    const { component } = setup({
      person: {
        ...getMockPerson({ id: 1, surname: 'person', firstName: 'test' }),
        personAddresses: addressInfo,
      },
    });

    const addressElements = component.getAllByTestId('contact-person-address');
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
    const mailingAddress: ApiGen_Concepts_PersonAddress = {
      personId: 1,
      id: 2,
      rowVersion: 0,
      address: {
        ...getMockApiAddress(),
        countryOther: 'other country info',
        country: { id: 1, code: 'OTHER', description: 'Other', displayOrder: 1 },
      },
      addressUsageType: {
        id: ApiGen_CodeTypes_AddressUsageTypes.MAILING,
        description: 'Mailing Address',
        isDisabled: false,
        displayOrder: null,
      },
    };

    const addressInfo: ApiGen_Concepts_PersonAddress[] = [mailingAddress];
    const { component } = setup({
      person: {
        ...getMockPerson({ id: 1, surname: 'person', firstName: 'test' }),
        personAddresses: addressInfo,
      },
    });

    const addressElement = component.getByTestId('contact-person-address');

    // Verify that the display is in the correct order
    expect(addressElement.textContent).toBe(
      `${mailingAddress.address.streetAddress1} N/A ${mailingAddress.address.municipality} ${mailingAddress.address.province.code} ${mailingAddress.address.postal} ${mailingAddress.address.countryOther}`,
    );
  });

  it('Shows comment information', () => {
    const testComment = 'A test comment :)';
    const { component } = setup({
      person: {
        ...getMockPerson({ id: 1, surname: 'person', firstName: 'test' }),
        comment: testComment,
      },
    });

    const commentElement = component.getByTestId('contact-person-comment');
    expect(commentElement.textContent).toBe(testComment);
  });

  it('Orders address information correctly', () => {
    const { component } = setup({
      person: {
        ...getMockPerson({ id: 1, surname: 'person', firstName: 'test' }),
        personAddresses: fakeAddresses,
      },
    });

    const addressElements = component.getAllByTestId('contact-person-address');
    expect(addressElements.length).toBe(3);

    expect(addressElements[0].children[0]).toHaveTextContent('Mailing address');
    expect(addressElements[1].children[0]).toHaveTextContent('Property address');
    expect(addressElements[2].children[0]).toHaveTextContent('Billing address');
  });
});
