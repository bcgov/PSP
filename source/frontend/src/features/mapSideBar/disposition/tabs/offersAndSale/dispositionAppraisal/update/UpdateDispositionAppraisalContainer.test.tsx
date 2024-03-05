import { createMemoryHistory } from 'history';
import { act } from 'react-dom/test-utils';

import { Claims } from '@/constants/claims';
import { DispositionAppraisalFormModel } from '@/features/mapSideBar/disposition/models/DispositionAppraisalFormModel';
import { mockDispositionAppraisalApi } from '@/mocks/dispositionFiles.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { Api_DispositionFileAppraisal } from '@/models/api/DispositionFile';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { render, RenderOptions, waitForEffects } from '@/utils/test-utils';

import { IDispositionAppraisalFormProps } from '../form/DispositionAppraisalForm';
import UpdateDispositionAppraisalContainer, {
  IUpdateDispositionAppraisalContainerProps,
} from './UpdateDispositionAppraisalContainer';

const history = createMemoryHistory();
const mockAppraisal = mockDispositionAppraisalApi(1);

const mockGetAppraisalApi = {
  error: undefined,
  response: undefined,
  execute: jest.fn().mockResolvedValue(mockAppraisal),
  loading: false,
};

const mockPutAppraisalApi = {
  error: undefined,
  response: undefined,
  execute: jest.fn(),
  loading: false,
};

const mockPostAppraisalApi = {
  error: undefined,
  response: undefined,
  execute: jest.fn(),
  loading: false,
};

const onSuccess = jest.fn();

jest.mock('@/hooks/repositories/useDispositionProvider', () => ({
  useDispositionProvider: () => {
    return {
      getDispositionAppraisal: mockGetAppraisalApi,
      postDispositionAppraisal: mockPostAppraisalApi,
      putDispositionAppraisal: mockPutAppraisalApi,
    };
  },
}));

// eslint-disable-next-line @typescript-eslint/no-unused-vars
let viewProps: IDispositionAppraisalFormProps | undefined;
const TestView: React.FC<IDispositionAppraisalFormProps> = props => {
  viewProps = props;
  return <span>Content Rendered</span>;
};

describe('Update Disposition Appraisal Container component', () => {
  const setup = async (
    renderOptions: RenderOptions & {
      props?: Partial<IUpdateDispositionAppraisalContainerProps>;
    } = {},
  ) => {
    const component = render(
      <UpdateDispositionAppraisalContainer
        dispositionFileId={1}
        View={TestView}
        onSuccess={onSuccess}
      />,
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
    jest.resetAllMocks();
  });

  it('Renders the underlying form', async () => {
    const { getByText } = await setup();
    expect(getByText(/Content Rendered/)).toBeVisible();
    expect(mockGetAppraisalApi.execute).toHaveBeenCalled();
  });

  it('Loads props with the initial values when Appraisal has values', async () => {
    mockGetAppraisalApi.execute.mockResolvedValue(mockAppraisal);
    await setup();
    await waitForEffects();

    expect(mockGetAppraisalApi.execute).toHaveBeenCalled();
    const formModel = DispositionAppraisalFormModel.fromApi(mockAppraisal);

    expect(viewProps?.initialValues).toStrictEqual(formModel);
  });

  it('Loads props with the initial values with default values when no appraisal exists', async () => {
    mockGetAppraisalApi.execute.mockResolvedValue(null);
    await setup();
    await waitForEffects();

    expect(mockGetAppraisalApi.execute).toHaveBeenCalled();
    const formModel = new DispositionAppraisalFormModel(null, 1, null);

    expect(viewProps?.initialValues).toStrictEqual(formModel);
  });

  it('makes POST request to create a NEW Appraisal and returns the response', async () => {
    mockGetAppraisalApi.execute.mockResolvedValue(null);
    mockPostAppraisalApi.execute.mockResolvedValue(mockAppraisal);

    await setup();
    await waitForEffects();

    let createdAppraisal: Api_DispositionFileAppraisal | undefined;
    await act(async () => {
      createdAppraisal = await viewProps?.onSave({
        id: null,
        dispositionFileId: 1,
        appraisedAmount: 20000.0,
        appraisalDate: '2024-01-18',
        bcaValueAmount: 350000.0,
        bcaRollYear: '2024',
        listPriceAmount: 500000.0,
      } as Api_DispositionFileAppraisal);
    });

    expect(mockPostAppraisalApi.execute).toHaveBeenCalled();
    expect(createdAppraisal).toStrictEqual({ ...mockAppraisal });
    expect(history.location.pathname).toBe('/');
  });

  it('makes PUT request to update Appraisal and returns the response', async () => {
    mockGetAppraisalApi.execute.mockResolvedValue(mockAppraisal);
    mockPutAppraisalApi.execute.mockResolvedValue(mockAppraisal);

    await setup({ props: { dispositionFileId: 1 } });
    await waitForEffects();

    let updatedAppraisal: Api_DispositionFileAppraisal | undefined;
    await act(async () => {
      updatedAppraisal = await viewProps?.onSave({} as Api_DispositionFileAppraisal);
    });

    expect(mockPutAppraisalApi.execute).toHaveBeenCalled();
    expect(updatedAppraisal).toStrictEqual({ ...mockAppraisal });
    expect(history.location.pathname).toBe('/');
  });

  it('navigates back to Offers and Sale tab when form is cancelled', async () => {
    await setup();
    act(() => {
      viewProps?.onCancel();
    });

    expect(history.location.pathname).toBe('/');
    expect(mockPutAppraisalApi.execute).not.toHaveBeenCalled();
    expect(mockPostAppraisalApi.execute).not.toHaveBeenCalled();
  });
});
