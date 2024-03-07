import { createMemoryHistory } from 'history';

import { ContactMethodTypes } from '@/constants/contactMethodType';
import { AddressTypes } from '@/constants/index';
import {
  IContactAddress,
  IContactMethod,
  IContactOrganization,
  IContactPerson,
} from '@/interfaces/IContact';
import { phoneFormatter } from '@/utils/formUtils';
import { render, RenderOptions } from '@/utils/test-utils';

import PersonView, { PersonViewProps } from './Person';
import { fakeAddresses } from './utils';

const fakePerson: IContactPerson = {
  id: 1,
  isDisabled: false,
  fullName: 'Test name full',
  preferredName: 'Preferred',
  organizations: [],
  addresses: [],
  contactMethods: [],
  comment: '',
};

const history = createMemoryHistory();

describe('Contact PersonView component', () => {
  const setup = (renderOptions: RenderOptions & PersonViewProps) => {
    // render component under test
    const component = render(<PersonView person={renderOptions.person} />, {
      ...renderOptions,
      history,
    });

    return {
      component,
    };
  };
  it('renders as expected', () => {
    const { component } = setup({ person: fakePerson });
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('Shows status information', () => {
    const { component } = setup({
      person: {
        ...fakePerson,
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
        ...fakePerson,
        fullName: testName,
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
    const personalEmail: IContactMethod = {
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
    const workEmail: IContactMethod = {
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

    const contactInfo: IContactMethod[] = [personalEmail, workEmail];
    const { component } = setup({
      person: {
        ...fakePerson,
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
    const faxPhone: IContactMethod = {
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
    const personalPhone: IContactMethod = {
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
    const workPhone: IContactMethod = {
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
    const workMobile: IContactMethod = {
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
    const personalMobile: IContactMethod = {
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

    const contactInfo: IContactMethod[] = [
      faxPhone,
      personalPhone,
      workPhone,
      workMobile,
      personalMobile,
    ];
    const { component } = setup({
      person: {
        ...fakePerson,
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
    const organization: IContactOrganization = {
      id: '1',
      isDisabled: false,
      name: 'Test Corp Incorporated',
      alias: 'Test Inc',
      incorporationNumber: '123456',
      comment: 'test comment',
    };

    const organizationsInfo: IContactOrganization[] = [organization];
    const { component } = setup({
      person: {
        ...fakePerson,
        organizations: organizationsInfo,
      },
    });

    const organizationElements = component.getAllByTestId('contact-person-organization');
    expect(organizationElements.length).toBe(1);

    // Verify that the display is in the correct order
    expect(organizationElements[0].textContent).toBe(organization.name);
  });

  it('Shows address information', () => {
    const mailingAddress: IContactAddress = {
      id: 1,
      rowVersion: 0,
      streetAddress1: 'Test Street',
      municipality: 'Victoria',
      province: {
        provinceStateId: 1,
        provinceStateCode: 'BC',
        description: 'British Columbia',
      },
      country: { countryId: 1, countryCode: 'CA', description: 'Canada' },
      postal: 'v0v 1v1',
      addressType: {
        id: AddressTypes.Mailing,
        description: 'Mailing Address',
        isDisabled: false,
        displayOrder: null,
      },
    };
    const residentialAddress: IContactAddress = {
      id: 2,
      rowVersion: 0,
      streetAddress1: 'Fixture Street',
      municipality: 'Vancouver',
      province: {
        provinceStateId: 1,
        provinceStateCode: 'BC',
        description: 'British Columbia',
      },
      country: { countryId: 1, countryCode: 'CA', description: 'Canada' },
      postal: 'v0v 1v1',
      addressType: {
        id: AddressTypes.Residential,
        description: 'Residential Address',
        isDisabled: false,
        displayOrder: null,
      },
    };

    const addressInfo: IContactAddress[] = [mailingAddress, residentialAddress];
    const { component } = setup({
      person: {
        ...fakePerson,
        addresses: addressInfo,
      },
    });

    const addressElements = component.getAllByTestId('contact-person-address');
    expect(addressElements.length).toBe(2);

    // Verify that the display is in the correct order
    expect(addressElements[0].textContent).toBe(
      `${mailingAddress.streetAddress1} ${mailingAddress.municipality} ${
        mailingAddress.province!.provinceStateCode
      } ${mailingAddress.postal} ${mailingAddress.country?.description}`,
    );
    expect(addressElements[1].textContent).toBe(
      `${residentialAddress.streetAddress1} ${residentialAddress.municipality} ${
        residentialAddress.province!.provinceStateCode
      } ${residentialAddress.postal} ${residentialAddress.country?.description}`,
    );
  });

  it(`Shows address information when 'Other' country selected and no province is supplied`, () => {
    const mailingAddress: IContactAddress = {
      id: 1,
      rowVersion: 0,
      streetAddress1: 'Test Street',
      municipality: 'Amsterdam',
      province: undefined,
      country: { countryId: 4, countryCode: 'OTHER', description: 'Other' },
      countryOther: 'Netherlands',
      postal: '123456',
      addressType: {
        id: AddressTypes.Mailing,
        description: 'Mailing Address',
        isDisabled: false,
        displayOrder: null,
      },
    };

    const addressInfo: IContactAddress[] = [mailingAddress];
    const { component } = setup({
      person: {
        ...fakePerson,
        addresses: addressInfo,
      },
    });

    const addressElement = component.getByTestId('contact-person-address');

    // Verify that the display is in the correct order
    expect(addressElement.textContent).toBe(
      `${mailingAddress.streetAddress1} ${mailingAddress.municipality} ${mailingAddress.postal} ${mailingAddress.countryOther}`,
    );
  });

  it('Shows comment information', () => {
    const testComment = 'A test comment :)';
    const { component } = setup({
      person: {
        ...fakePerson,
        comment: testComment,
      },
    });

    const commentElement = component.getByTestId('contact-person-comment');
    expect(commentElement.textContent).toBe(testComment);
  });

  it('Orders address information correctly', () => {
    const { component } = setup({
      person: {
        ...fakePerson,
        addresses: fakeAddresses,
      },
    });

    const addressElements = component.getAllByTestId('contact-person-address');
    expect(addressElements.length).toBe(3);

    expect(addressElements[0].children[0]).toHaveTextContent('Mailing address');
    expect(addressElements[1].children[0]).toHaveTextContent('Property address');
    expect(addressElements[2].children[0]).toHaveTextContent('Billing address');
  });
});
