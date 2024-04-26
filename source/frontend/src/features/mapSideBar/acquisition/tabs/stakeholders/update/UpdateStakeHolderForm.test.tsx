import { createMemoryHistory } from 'history';

import { mockAcquisitionFileResponse } from '@/mocks/index.mock';
import { getMockApiInterestHolders } from '@/mocks/interestHolders.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions, userEvent } from '@/utils/test-utils';

import { StakeHolderForm } from './models';
import UpdateStakeHolderForm, { IUpdateStakeHolderFormProps } from './UpdateStakeHolderForm';
import { createRef } from 'react';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const onSubmit = vi.fn();

describe('UpdateStakeHolderForm component', () => {
  // render component under test
  const setup = (
    renderOptions: RenderOptions & { props?: Partial<IUpdateStakeHolderFormProps> },
  ) => {
    const utils = render(
      <UpdateStakeHolderForm
        {...renderOptions.props}
        onSubmit={onSubmit}
        formikRef={createRef() as any}
        file={mockAcquisitionFileResponse()}
        interestHolders={
          renderOptions.props?.interestHolders ??
          StakeHolderForm.fromApi(getMockApiInterestHolders())
        }
        loading={renderOptions.props?.loading ?? false}
      />,
      {
        ...renderOptions,
        store: storeState,
        history,
      },
    );

    return {
      ...utils,
    };
  };

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders as expected', () => {
    const { asFragment } = setup({});
    expect(asFragment()).toMatchSnapshot();
  });

  it('displays a loading spinner when loading', () => {
    const { getByTestId } = setup({ props: { loading: true } });
    const spinner = getByTestId('filter-backdrop-loading');
    expect(spinner).toBeVisible();
  });

  it('displays no item messages initially', () => {
    const { getByText } = setup({
      props: { loading: false, interestHolders: new StakeHolderForm() },
    });
    expect(getByText('No Interest holders to display')).toBeVisible();
    expect(getByText('No Non-interest payees to display')).toBeVisible();
  });

  it('can add a interest holder', async () => {
    const { getByText } = setup({
      props: { loading: false, interestHolders: new StakeHolderForm() },
    });
    await act(async () => userEvent.click(getByText('+ Add an Interest holder')));
    expect(getByText('Interest holder:')).toBeVisible();
  });

  it('can add a non-interest payee', async () => {
    const { getByText } = setup({
      props: { loading: false, interestHolders: new StakeHolderForm() },
    });
    await act(async () => userEvent.click(getByText('+ Add a Non-interest payee')));
    expect(getByText('Payee name:')).toBeVisible();
  });

  it('can select acquisition file properties', async () => {
    const { getByText, getByTestId } = setup({
      props: {
        loading: false,
        interestHolders: new StakeHolderForm(),
        file: mockAcquisitionFileResponse(),
      },
    });
    await act(async () => userEvent.click(getByText('+ Add an Interest holder')));
    await act(async () => userEvent.click(getByTestId('selectrow-1')));
    expect(getByTestId('selectrow-1')).toBeChecked();
  });

  it('can add a non-interest payee', async () => {
    const { getByText, getByTestId } = setup({
      props: { loading: false, interestHolders: new StakeHolderForm() },
    });
    await act(async () => userEvent.click(getByText('+ Add a Non-interest payee')));
    await act(async () => userEvent.click(getByTestId('selectrow-1')));
    expect(getByTestId('selectrow-1')).toBeChecked();
  });

  it('it hides the legacy stakeholders', async () => {
    const { queryByTestId } = setup({});

    expect(queryByTestId('acq-file-legacy-stakeholders')).not.toBeInTheDocument();
  });
});
