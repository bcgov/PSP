import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { getMockContactOrganizationWithOnePerson } from '@/mocks/contacts.mock';
import { getMockExpropriationFile } from '@/mocks/index.mock';
import { act, render, RenderOptions, userEvent } from '@/utils/test-utils';

import { FormikProps } from 'formik';
import { createRef } from 'react';
import { ExpropriationForm4Model } from '../models';
import ExpropriationForm4, { IExpropriationForm4Props } from './ExpropriationForm4';

// mock auth library

vi.mock('@/hooks/pims-api/useApiContacts');
const getContacts = vi.fn();
vi.mocked(useApiContacts, { partial: true }).mockReturnValue({
  getContacts,
});

const onGenerate = vi.fn();
const onError = vi.fn();

describe('Expropriation Form 4', () => {
  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<IExpropriationForm4Props> } = {},
  ) => {
    const formikRef = createRef<FormikProps<ExpropriationForm4Model>>();
    const utils = render(
      <ExpropriationForm4
        {...renderOptions.props}
        acquisitionFile={renderOptions.props?.acquisitionFile ?? getMockExpropriationFile()}
        onGenerate={onGenerate}
        formikRef={renderOptions.props?.formikRef ?? formikRef}
      ></ExpropriationForm4>,
      {
        ...renderOptions,
        useMockAuthentication: true,
      },
    );

    return {
      ...utils,
      formikRef,
      getContactManagerSearchButton: () => utils.getByTestId('search'),
    };
  };

  beforeEach(() => {
    const organization = getMockContactOrganizationWithOnePerson();
    getContacts.mockResolvedValue({
      data: {
        items: [organization],
        quantity: 1,
        total: 1,
        page: 1,
        pageIndex: 0,
      },
    });
  });

  afterEach(() => vi.clearAllMocks());

  it('matches snapshot', async () => {
    const { asFragment } = await setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('validates form values before generating', async () => {
    const { getByText, formikRef } = await setup();
    await act(async () => formikRef.current.submitForm());

    expect(getByText('Expropriation authority is required')).toBeInTheDocument();
    expect(getByText('At least one impacted property is required')).toBeInTheDocument();
  });

  it(`submits the form when Generate button is clicked`, async () => {
    const { getByText, getByTestId, getByTitle, formikRef } = await setup({});
    // pick an organization from contact manager
    await act(async () => userEvent.click(getByTitle('Select Contact')));
    await act(async () => userEvent.click(getByTestId('selectrow-O3')));
    await act(async () => userEvent.click(getByText('Select')));

    // fill other form fields
    await act(async () => userEvent.click(getByTestId('selectrow-1')));
    await act(async () => formikRef.current.submitForm());

    expect(onGenerate).toHaveBeenCalled();
    expect(onError).not.toHaveBeenCalled();
  });

  it(`clears the form when Clear form button is clicked`, async () => {
    const { getByText, getByTestId } = await setup();

    await act(async () => userEvent.click(getByTestId('selectrow-1')));
    expect(getByTestId('selectrow-1')).toBeChecked();

    await act(async () => userEvent.click(getByText('Clear Form')));
    expect(getByTestId('selectrow-1')).not.toBeChecked();
  });
});
