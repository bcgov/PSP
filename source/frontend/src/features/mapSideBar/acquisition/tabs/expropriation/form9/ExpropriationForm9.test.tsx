import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { getMockContactOrganizationWithOnePerson } from '@/mocks/contacts.mock';
import { getMockExpropriationFile } from '@/mocks/index.mock';
import { act, findAllByText, render, RenderOptions, userEvent } from '@/utils/test-utils';

import ExpropriationForm9, { IExpropriationForm9Props } from './ExpropriationForm9';
import { ExpropriationForm9Model } from '../models';
import { FormikProps } from 'formik';
import { createRef } from 'react';

// mock auth library

vi.mock('@/hooks/pims-api/useApiContacts');
const getContacts = vi.fn();
vi.mocked(useApiContacts).mockReturnValue({
  getContacts,
} as unknown as ReturnType<typeof useApiContacts>);

const onGenerate = vi.fn();

describe('Expropriation Form 9', () => {
  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<IExpropriationForm9Props> } = {},
  ) => {
    const formikRef = createRef<FormikProps<ExpropriationForm9Model>>();
    const utils = render(
      <ExpropriationForm9
        {...renderOptions.props}
        acquisitionFile={renderOptions.props?.acquisitionFile ?? getMockExpropriationFile()}
        onGenerate={onGenerate}
        formikRef={renderOptions.props?.formikRef ?? formikRef}
      ></ExpropriationForm9>,
      {
        ...renderOptions,
        useMockAuthentication: true,
      },
    );

    return {
      ...utils,
      formikRef,
      getRegisteredPlanNumbers: () =>
        utils.container.querySelector(`input[name="registeredPlanNumbers"]`) as HTMLInputElement,
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

  it('validates form 9 values before generating', async () => {
    const { getByText, formikRef } = await setup();
    await act(async () => formikRef.current.submitForm());

    expect(getByText('Expropriation authority is required')).toBeInTheDocument();
    expect(getByText('At lease one impacted property is required')).toBeInTheDocument();
  });

  it(`submits the form when Generate form 9 button is clicked`, async () => {
    const { getByText, getByTestId, getByTitle, getRegisteredPlanNumbers, formikRef } =
      await setup();

    // pick an organization from contact manager
    await act(async () => userEvent.click(getByTitle('Select Contact')));
    await act(async () => userEvent.click(getByTestId('selectrow-O3')));
    await act(async () => userEvent.click(getByText('Select')));

    // fill other form fields
    await act(async () => userEvent.click(getByTestId('selectrow-1')));
    await act(async () => userEvent.paste(getRegisteredPlanNumbers(), 'testing'));

    await act(async () => formikRef.current.submitForm());

    expect(onGenerate).toHaveBeenCalled();
  });

  it(`clears the form when Clear Form button is clicked`, async () => {
    const { getByText, getByTestId, getRegisteredPlanNumbers } = await setup();

    await act(async () => userEvent.click(getByTestId('selectrow-1')));
    await act(async () => userEvent.paste(getRegisteredPlanNumbers(), 'testing'));

    expect(getByTestId('selectrow-1')).toBeChecked();
    expect(getRegisteredPlanNumbers()).toHaveValue();

    await act(async () => userEvent.click(getByText('Clear Form')));

    expect(getByTestId('selectrow-1')).not.toBeChecked();
    expect(getRegisteredPlanNumbers()).not.toHaveValue();
  });
});