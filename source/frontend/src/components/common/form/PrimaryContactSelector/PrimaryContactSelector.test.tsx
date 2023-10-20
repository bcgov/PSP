import { Formik, FormikProps } from 'formik';
import React from 'react';

import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { fromApiOrganization, fromApiPerson } from '@/interfaces/IContactSearchResult';
import { getMockPerson } from '@/mocks/contacts.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { getMockOrganization } from '@/mocks/organization.mock';
import { Api_Organization } from '@/models/api/Organization';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { getByName, render, RenderOptions, screen, waitForEffects } from '@/utils/test-utils';

import { IPrimaryContactSelectorProps, PrimaryContactSelector } from './PrimaryContactSelector';

jest.mock('@/hooks/pims-api/useApiContacts');
const getOrganizationConceptFn = jest.fn();
(useApiContacts as jest.Mock).mockImplementation(() => ({
  getOrganizationConcept: getOrganizationConceptFn,
}));

interface ITestProps extends IPrimaryContactSelectorProps {
  initialForm?: { primaryContactId: string };
}

describe('PrimaryContactSelector component', () => {
  const setup = (options: RenderOptions & { props?: Partial<ITestProps> } = {}) => {
    const ref = React.createRef<FormikProps<ITestProps>>();
    const utils = render(
      <Formik
        innerRef={ref}
        initialValues={options.props?.initialForm ?? ({ primaryContactId: '1' } as any)}
        onSubmit={jest.fn()}
      >
        <PrimaryContactSelector field="primaryContactId" contactInfo={options.props?.contactInfo} />
      </Formik>,
      {
        ...options,
        store: { [lookupCodesSlice.name]: { lookupCodes: mockLookups } },
      },
    );

    return {
      ...utils,
      getFormikRef: () => ref,
    };
  };

  beforeEach(() => {
    getOrganizationConceptFn.mockResolvedValue({
      data: {
        ...getMockOrganization(),
        organizationPersons: [
          {
            organizationId: 2,
            personId: 1,
            person: {
              id: 1,
              firstName: 'chester',
              surname: 'tester',
            },
            isDisabled: false,
            rowVersion: 1,
          },
          {
            organizationId: 2,
            personId: 3,
            person: {
              id: 3,
              firstName: 'first',
              surname: 'last',
            },
            isDisabled: false,
            rowVersion: 1,
          },
        ],
      } as Api_Organization,
    });
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('renders as expected', async () => {
    const orgContact = fromApiOrganization(getMockOrganization());
    const { asFragment } = setup({ props: { contactInfo: orgContact } });

    await waitForEffects();
    expect(asFragment()).toMatchSnapshot();
  });

  it(`returns 'null' for person contacts`, async () => {
    const personContact = fromApiPerson(
      getMockPerson({ id: 1, surname: 'last', firstName: 'first' }),
    );
    setup({ props: { contactInfo: personContact } });

    await waitForEffects();
    expect(screen.queryByText(/Select a primary contact/)).toBeNull();
  });

  it('renders primary contact dropdown when multiple contacts are available', async () => {
    setup({ props: { contactInfo: fromApiOrganization(getMockOrganization()) } });

    await waitForEffects();
    expect(getByName('primaryContactId')).toBeVisible();
    expect(await screen.findByText(/chester tester/)).toBeInTheDocument();
    expect(await screen.findByText(/first last/)).toBeInTheDocument();
  });

  it('renders primary contact name when one is available', async () => {
    getOrganizationConceptFn.mockResolvedValue({
      data: {
        ...getMockOrganization(),
        organizationPersons: [
          {
            organizationId: 2,
            personId: 1,
            person: {
              id: 1,
              firstName: 'chester',
              surname: 'tester',
            },
            isDisabled: false,
            rowVersion: 1,
          },
        ],
      } as Api_Organization,
    });

    setup({ props: { contactInfo: fromApiOrganization(getMockOrganization()) } });

    await waitForEffects();
    expect(getByName('primaryContactId')).toBeNull();
    expect(await screen.findByText(/chester tester/)).toBeInTheDocument();
  });

  it(`shows message 'No contacts available' when no primary contact is available`, async () => {
    getOrganizationConceptFn.mockResolvedValue({
      data: { ...getMockOrganization(), organizationPersons: [] } as Api_Organization,
    });

    setup({ props: { contactInfo: fromApiOrganization(getMockOrganization()) } });

    await waitForEffects();
    expect(getByName('primaryContactId')).toBeNull();
    expect(await screen.findByText(/No contacts available/i)).toBeVisible();
  });
});
