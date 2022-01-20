import { ContactMethodTypes } from 'constants/contactMethodType';
import { AddressTypes } from 'constants/index';
import { createMemoryHistory } from 'history';
import {
  IContactAddress,
  IContactMethod,
  IContactOrganization,
  IContactPerson,
} from 'interfaces/IContact';
import { phoneFormatter } from 'utils/formUtils';
import { render, RenderOptions } from 'utils/test-utils';

import OrganizationView, { OrganizationViewProps } from './Organization';

const fakeOrganization: IContactOrganization = {
  id: '123',
  isDisabled: false,
  name: 'Fake Corp Incorporated',
  alias: 'Fake Inc',
  incorporationNumber: '987',
  persons: [],
  addresses: [],
  contactMethods: [],
  comment: '',
};

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
    const { component } = setup({ organization: fakeOrganization });
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('Shows status information', () => {
    const { component } = setup({
      organization: {
        ...fakeOrganization,
        isDisabled: true,
      },
    });

    var statusElement = component.getByTestId('contact-organization-status');
    expect(statusElement.textContent).toBe('INACTIVE');
  });

  it('Shows base information', () => {
    const testName = 'My corp name';
    const testAlias = 'My corp alias';
    const testIncNumber = '77789';
    const { component } = setup({
      organization: {
        ...fakeOrganization,
        name: testName,
        alias: testAlias,
        incorporationNumber: testIncNumber,
      },
    });

    var statusElement = component.getByTestId('contact-organization-status');
    expect(statusElement.textContent).toBe('ACTIVE');

    var nameElement = component.getByTestId('contact-organization-fullname');
    expect(nameElement.textContent).toBe(testName);

    var aliasElement = component.getByTestId('contact-organization-alias');
    expect(aliasElement.textContent).toBe(testAlias);

    var incorporationElement = component.getByTestId('contact-organization-incorporationNumber');
    expect(incorporationElement.textContent).toBe(testIncNumber);
  });

  it('Shows email information', () => {
    const personalEmail: IContactMethod = {
      id: 1,
      rowVersion: 0,
      contactMethodType: {
        id: ContactMethodTypes.PersonalEmail,
        description: 'Personal Email',
        isDisabled: false,
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
      },
      value: 'test@bench.net',
    };

    const contactInfo: IContactMethod[] = [personalEmail, workEmail];
    const { component } = setup({
      organization: {
        ...fakeOrganization,
        contactMethods: contactInfo,
      },
    });

    var emailValueElements = component.getAllByTestId('email-value');
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
      organization: {
        ...fakeOrganization,
        contactMethods: contactInfo,
      },
    });

    var phoneValueElements = component.getAllByTestId('phone-value');
    expect(phoneValueElements.length).toBe(4);

    // Verify that the display is in the correct order
    expect(phoneValueElements[0].textContent).toBe(phoneFormatter(workMobile.value));
    expect(phoneValueElements[1].textContent).toBe(phoneFormatter(workPhone.value));
    expect(phoneValueElements[2].textContent).toBe(phoneFormatter(personalPhone.value));
    expect(phoneValueElements[3].textContent).toBe(phoneFormatter(faxPhone.value));
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
      },
    };

    const addressInfo: IContactAddress[] = [mailingAddress, residentialAddress];
    const { component } = setup({
      organization: {
        ...fakeOrganization,
        addresses: addressInfo,
      },
    });

    var addressElements = component.getAllByTestId('contact-organization-address');
    expect(addressElements.length).toBe(2);

    // Verify that the display is in the correct order
    expect(addressElements[0].textContent).toBe(
      `${mailingAddress.streetAddress1} ${mailingAddress.municipality} ${mailingAddress.province.provinceStateCode} ${mailingAddress.postal} ${mailingAddress.country?.description}`,
    );
    expect(addressElements[1].textContent).toBe(
      `${residentialAddress.streetAddress1} ${residentialAddress.municipality} ${residentialAddress.province.provinceStateCode} ${residentialAddress.postal} ${residentialAddress.country?.description}`,
    );
  });

  it('Shows individual contacts information', () => {
    const person1: IContactPerson = {
      id: 1,
      isDisabled: false,
      fullName: 'Sarah Dawn Abraham',
      preferredName: 'Saray',
      comment: '',
    };
    const person2: IContactPerson = {
      id: 2,
      isDisabled: false,
      fullName: 'Sam Johnson',
      preferredName: 'Sammi',
      comment: '',
    };

    const person3: IContactPerson = {
      id: 3,
      isDisabled: false,
      fullName: 'Pete Zamboni',
      preferredName: 'Peter',
      comment: '',
    };

    const personsInfo: IContactPerson[] = [person1, person2, person3];
    const { component } = setup({
      organization: {
        ...fakeOrganization,
        persons: personsInfo,
      },
    });

    var personElements = component.getAllByTestId('contact-organization-person');
    expect(personElements.length).toBe(3);

    // Verify that the display is in the correct order
    expect(personElements[0].textContent).toBe(person1.fullName);
    expect(personElements[1].textContent).toBe(person2.fullName);
    expect(personElements[2].textContent).toBe(person3.fullName);
  });

  it('Shows comment information', () => {
    const testComment: string = 'A test comment :)';
    const { component } = setup({
      organization: {
        ...fakeOrganization,
        comment: testComment,
      },
    });

    var commentElement = component.getByTestId('contact-organization-comment');
    expect(commentElement.textContent).toBe(testComment);
  });
});
