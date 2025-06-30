import userEvent from '@testing-library/user-event';
import { createMemoryHistory } from 'history';

import { EnumAcquisitionFileType } from '@/constants/acquisitionFileType';
import Claims from '@/constants/claims';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { useExpropriationEventRepository } from '@/hooks/repositories/useExpropriationEventRepository';
import { useInterestHolderRepository } from '@/hooks/repositories/useInterestHolderRepository';
import { getMockExpropriationFile } from '@/mocks/index.mock';
import { act, getMockRepositoryObj, render, RenderOptions,  within } from '@/utils/test-utils';

import {
  ExpropriationTabContainerView,
  IExpropriationTabContainerViewProps,
} from './ExpropriationTabContainerView';
import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { getMockContactOrganizationWithOnePerson } from '@/mocks/contacts.mock';
import { FormikProps } from 'formik';
import { createRef } from 'react';
import { ExpropriationForm1Model, ExpropriationForm5Model, ExpropriationForm9Model } from './models';

const history = createMemoryHistory();

vi.mock('@/hooks/repositories/useExpropriationEventRepository');
const mockGetExpropriationEventsApi = getMockRepositoryObj([]);
const mockAddExpropriationEventsApi = getMockRepositoryObj();
const mockUpdateExpropriationEventsApi = getMockRepositoryObj();
const mockDeleteExpropriationEventsApi = getMockRepositoryObj();

vi.mock('@/hooks/repositories/useAcquisitionProvider');
const mockGetAcquisitionOwnersApi = getMockRepositoryObj([]);

vi.mock('@/hooks/repositories/useInterestHolderRepository');
const mockGetAcquisitionInterestHoldersApi = getMockRepositoryObj([]);

vi.mock('@/hooks/pims-api/useApiContacts');
const getContacts = vi.fn();
vi.mocked(useApiContacts).mockReturnValue({
  getContacts,
} as unknown as ReturnType<typeof useApiContacts>);

const handleGenerateForm1 = vi.fn();
const onError = vi.fn();

describe('Expropriation Tab Container View', () => {
  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<IExpropriationTabContainerViewProps> } = {},
  ) => {
    const formikRefForm1 = createRef<FormikProps<ExpropriationForm1Model>>();
    const formikRefForm5 = createRef<FormikProps<ExpropriationForm5Model>>();
    const formikRefForm9 = createRef<FormikProps<ExpropriationForm9Model>>();
    const rendered = render(
      <ExpropriationTabContainerView
        {...renderOptions.props}
        loading={renderOptions.props?.loading ?? false}
        acquisitionFile={renderOptions.props?.acquisitionFile ?? getMockExpropriationFile()}
        form8s={renderOptions.props?.form8s ?? []}
        onForm8Deleted={vi.fn()}
        isFileFinalStatus={renderOptions?.props?.isFileFinalStatus ?? false}
      />,
      {
        ...renderOptions,
        useMockAuthentication: true,
        claims: renderOptions?.claims ?? [Claims.ACQUISITION_EDIT],
        history: history,
      },
    );
    await act(async () => {});
    return {
      ...rendered,
      getNatureOfInterestForm1: () =>
        rendered.container.querySelector(`input[name="landInterest"]`) as HTMLInputElement,
      getPurposeForm1: () => rendered.container.querySelector(`input[name="purpose"]`) as HTMLInputElement,
    };
  };

  beforeEach(() => {
    vi.mocked(useExpropriationEventRepository, { partial: true }).mockReturnValue({
      getExpropriationEvents: mockGetExpropriationEventsApi,
      addExpropriationEvent: mockAddExpropriationEventsApi,
      updateExpropriationEvent: mockUpdateExpropriationEventsApi,
      deleteExpropriationEvent: mockDeleteExpropriationEventsApi,
    });

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

    vi.mocked(useAcquisitionProvider, { partial: true }).mockReturnValue({
      getAcquisitionOwners: mockGetAcquisitionOwnersApi,
    });
    vi.mocked(useInterestHolderRepository, { partial: true }).mockReturnValue({
      getAcquisitionInterestHolders: mockGetAcquisitionInterestHoldersApi,
    });
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('displays a loading spinner when loading', async () => {
    const { getByTestId } = await setup({ props: { loading: true } });
    const spinner = getByTestId('filter-backdrop-loading');
    expect(spinner).toBeVisible();
  });

  it('shows the sections for Acquisition file type "Section 6"', async () => {
    const { queryByTestId } = await setup({});
    expect(queryByTestId('form-1-section')).toBeInTheDocument();
    expect(queryByTestId('form-5-section')).toBeInTheDocument();
    expect(queryByTestId('form-8-section')).toBeInTheDocument();
    expect(queryByTestId('form-9-section')).toBeInTheDocument();
  });

  it('shows the sections for Acquisition file type "Section 3"', async () => {
    const { queryByTestId } = await setup({
      props: { acquisitionFile: getMockExpropriationFile(EnumAcquisitionFileType.SECTN3) },
    });

    expect(queryByTestId('form-1-section')).not.toBeInTheDocument();
    expect(queryByTestId('form-5-section')).not.toBeInTheDocument();
    expect(queryByTestId('form-8-section')).toBeInTheDocument();
    expect(queryByTestId('form-9-section')).not.toBeInTheDocument();
  });

  it('displays tooltip instead of add button when file in final status', async () => {
    const { queryByTestId, queryByText } = await setup({
      props: {
        acquisitionFile: getMockExpropriationFile(EnumAcquisitionFileType.SECTN3),
        isFileFinalStatus: true,
      },
    });

    expect(queryByText('Add Form 8')).toBeNull();
    expect(queryByTestId('tooltip-icon-deposit-notes-cannot-edit-tooltip')).toBeVisible();
  });

  // it(`calls onError callback when generate form 1 endpoint fails`, async () => {
  //   const error = new Error('Network error');
  //   handleGenerateForm1.mockRejectedValueOnce(error);
  //   const { getByText, getByTestId, getNatureOfInterestForm1, getPurposeForm1 } = await setup();

  //   // pick an organization from contact manager
  //   const form1Wrapper = getByTestId('form-1-section');
  //   const form1SelectContactButton = within(form1Wrapper).getByTitle('Select Contact');
  //   await act(async () => userEvent.click(form1SelectContactButton));
  //   await act(async () => userEvent.click(getByTestId('selectrow-O3')));
  //   await act(async () => userEvent.click(getByText('Select')));

  //   // fill other form fields
  //   await act(async () => userEvent.click(within(form1Wrapper).getByTestId('selectrow-1')));
  //   await act(async () => userEvent.paste(getNatureOfInterestForm1(), 'foo'));
  //   await act(async () => userEvent.paste(getPurposeForm1(), 'bar'));

  //   await act(async () => userEvent.click(getByText(/Generate Form 1/i)));

  //   expect(handleGenerateForm1).toHaveBeenCalled();
  //   expect(onError).toHaveBeenCalledWith(error);
  // });
});
