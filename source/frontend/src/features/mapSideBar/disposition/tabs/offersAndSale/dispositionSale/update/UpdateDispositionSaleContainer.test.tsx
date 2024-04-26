import { createMemoryHistory } from 'history';

import { Claims } from '@/constants/claims';
import { DispositionSaleFormModel } from '@/features/mapSideBar/disposition/models/DispositionSaleFormModel';
import { mockDispositionSaleApi } from '@/mocks/dispositionFiles.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { ApiGen_Concepts_DispositionFileSale } from '@/models/api/generated/ApiGen_Concepts_DispositionFileSale';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions, waitForEffects } from '@/utils/test-utils';

import UpdateDispositionSaleContainer, {
  IUpdateDispositionSaleContainerProps,
} from './UpdateDispositionSaleContainer';
import { IUpdateDispositionSaleViewProps } from './UpdateDispostionSaleView';

const history = createMemoryHistory();
const mockDispositionSale = mockDispositionSaleApi(1, 1);

const mockGetSaleApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn().mockResolvedValue(mockDispositionSale),
  loading: false,
};

const mockPostSaleApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
};

const mockPutSaleApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
};

vi.mock('@/hooks/repositories/useDispositionProvider', () => ({
  useDispositionProvider: () => {
    return {
      getDispositionFileSale: mockGetSaleApi,
      postDispositionFileSale: mockPostSaleApi,
      putDispositionFileSale: mockPutSaleApi,
    };
  },
}));

// eslint-disable-next-line @typescript-eslint/no-unused-vars
let viewProps: IUpdateDispositionSaleViewProps | undefined;
const TestView: React.FC<IUpdateDispositionSaleViewProps> = props => {
  viewProps = props;
  return <span>Content Rendered</span>;
};

describe('Update Disposition Appraisal Container component', () => {
  const setup = async (
    renderOptions: RenderOptions & {
      props?: Partial<IUpdateDispositionSaleContainerProps>;
    } = {},
  ) => {
    const component = render(
      <UpdateDispositionSaleContainer dispositionFileId={1} View={TestView} />,
      {
        history,
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
        },
        useMockAuthentication: true,
        claims: renderOptions?.claims ?? [Claims.DISPOSITION_VIEW, Claims.DISPOSITION_EDIT],
        ...renderOptions,
      },
    );

    return {
      ...component,
    };
  };

  beforeEach(() => {
    viewProps = undefined;
    vi.resetAllMocks();
  });

  it('Renders the underlying form', async () => {
    const { getByText } = await setup();
    expect(getByText(/Content Rendered/)).toBeVisible();
    expect(mockGetSaleApi.execute).toHaveBeenCalled();
  });

  it('Loads props with the initial values when Sale has values', async () => {
    mockGetSaleApi.execute.mockResolvedValue(mockDispositionSale);
    await setup();
    await waitForEffects();

    expect(mockGetSaleApi.execute).toHaveBeenCalled();
    const formModel = DispositionSaleFormModel.fromApi(mockDispositionSale);

    expect(viewProps?.initialValues).toStrictEqual(formModel);
  });

  it('Loads props with the initial values with default values when no sale exists', async () => {
    mockGetSaleApi.execute.mockResolvedValue(null);
    await setup();
    await waitForEffects();

    expect(mockGetSaleApi.execute).toHaveBeenCalled();
    const formModel = new DispositionSaleFormModel(null, 1, null);

    expect(viewProps?.initialValues).toStrictEqual(formModel);
  });

  it('makes POST request to create a NEW Sale and returns the response', async () => {
    mockGetSaleApi.execute.mockResolvedValue(null);
    mockPostSaleApi.execute.mockResolvedValue(mockDispositionSale);

    await setup();
    await waitForEffects();

    let createdSale: ApiGen_Concepts_DispositionFileSale | undefined;
    await act(async () => {
      createdSale = await viewProps?.onSave({
        id: null,
      } as ApiGen_Concepts_DispositionFileSale);
    });

    expect(mockPostSaleApi.execute).toHaveBeenCalled();
    expect(createdSale).toStrictEqual({ ...mockDispositionSale });
    expect(history.location.pathname).toBe('/');
  });

  it('makes PUT request to update Appraisal and returns the response', async () => {
    mockGetSaleApi.execute.mockResolvedValue(mockDispositionSale);
    mockPutSaleApi.execute.mockResolvedValue(mockDispositionSale);

    await setup({ props: { dispositionFileId: 1 } });
    await waitForEffects();

    let updatedAppraisal: ApiGen_Concepts_DispositionFileSale | undefined;
    await act(async () => {
      updatedAppraisal = await viewProps?.onSave({ id: 1 } as ApiGen_Concepts_DispositionFileSale);
    });

    expect(mockPutSaleApi.execute).toHaveBeenCalled();
    expect(updatedAppraisal).toStrictEqual({ ...mockDispositionSale });
    expect(history.location.pathname).toBe('/');
  });

  it('navigates back to Offers and Sale tab when form is cancelled', async () => {
    await setup();
    await act(async () => {
      viewProps?.onCancel();
    });

    expect(history.location.pathname).toBe('/');
    expect(mockPutSaleApi.execute).not.toHaveBeenCalled();
    expect(mockPostSaleApi.execute).not.toHaveBeenCalled();
  });
});
