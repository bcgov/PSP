import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { getMockContactOrganizationWithOnePerson } from '@/mocks/contacts.mock';
import { getMockExpropriationFile } from '@/mocks/index.mock';
import { act, render, RenderOptions, userEvent } from '@/utils/test-utils';

import ExpropriationForm1, { IExpropriationForm1Props } from './ExpropriationForm1';
import { ExpropriationForm1Model } from '../models';
import { createRef } from 'react';
import { FormikProps } from 'formik';

// mock auth library

vi.mock('@/hooks/pims-api/useApiContacts');
const getContacts = vi.fn();
vi.mocked(useApiContacts).mockReturnValue({
  getContacts,
} as unknown as ReturnType<typeof useApiContacts>);

const onGenerate = vi.fn();
const onError = vi.fn();

describe('Expropriation Form 1', () => {
  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<IExpropriationForm1Props> } = {},
  ) => {
    const formikRef = createRef<FormikProps<ExpropriationForm1Model>>();
    const utils = render(
      <ExpropriationForm1
        {...renderOptions.props}
        acquisitionFile={renderOptions.props?.acquisitionFile ?? getMockExpropriationFile()}
        onGenerate={onGenerate}
        formikRef={renderOptions.props?.formikRef ?? formikRef}
      ></ExpropriationForm1>,
      {
        ...renderOptions,
        useMockAuthentication: true,
      },
    );

    return {
      ...utils,
      formikRef,
      getNatureOfInterest: () =>
        utils.container.querySelector(`input[name="landInterest"]`) as HTMLInputElement,
      getPurpose: () => utils.container.querySelector(`input[name="purpose"]`) as HTMLInputElement,
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

   it('validates form 1 values before generating', async () => {
    const { getByText, formikRef } = await setup();
    await act(async () => formikRef.current.submitForm());

    expect(getByText('Expropriation authority is required')).toBeInTheDocument();
    expect(getByText('At lease one impacted property is required')).toBeInTheDocument();
  });

  it(`submits the form when Generate button is clicked`, async () => {
    const { getByText, getByTestId, getByTitle, getNatureOfInterest, getPurpose, formikRef } = await setup();

    // pick an organization from contact manager
    await act(async () => userEvent.click(getByTitle('Select Contact')));
    await act(async () => userEvent.click(getByTestId('selectrow-O3')));
    await act(async () => userEvent.click(getByText('Select')));

    // fill other form fields
    await act(async () => userEvent.click(getByTestId('selectrow-1')));
    await act(async () => userEvent.paste(getNatureOfInterest(), 'foo'));
    await act(async () => userEvent.paste(getPurpose(), 'bar'));
    await act(async () => formikRef.current.submitForm());

    expect(onGenerate).toHaveBeenCalled();
    expect(onError).not.toHaveBeenCalled();
  });

  it(`clears the form when Clear form button is clicked`, async () => {
    const { getByText, getByTestId, getNatureOfInterest, getPurpose } = await setup();

    await act(async () => userEvent.click(getByTestId('selectrow-1')));
    await act(async () => userEvent.paste(getNatureOfInterest(), 'foo'));
    await act(async () => userEvent.paste(getPurpose(), 'bar'));

    expect(getByTestId('selectrow-1')).toBeChecked();
    expect(getNatureOfInterest()).toHaveValue();
    expect(getPurpose()).toHaveValue();

    await act(async () => userEvent.click(getByText('Clear Form')));

    expect(getByTestId('selectrow-1')).not.toBeChecked();
    expect(getNatureOfInterest()).not.toHaveValue();
    expect(getPurpose()).not.toHaveValue();
  });
});
