import { createMemoryHistory } from 'history';

import { mockFinancialCode } from '@/mocks/index.mock';
import { ApiGen_Concepts_FinancialCode } from '@/models/api/generated/ApiGen_Concepts_FinancialCode';
import { ApiGen_Concepts_FinancialCodeTypes } from '@/models/api/generated/ApiGen_Concepts_FinancialCodeTypes';
import { act, createAxiosError, render, RenderOptions, screen } from '@/utils/test-utils';

import UpdateFinancialCodeContainer, {
  IUpdateFinancialCodeFormProps,
} from './UpdateFinancialCodeContainer';

const mockGetApi = {
  error: undefined,
  response: undefined,
  execute: jest.fn(),
  loading: false,
};

const mockUpdateApi = {
  error: undefined,
  response: undefined,
  execute: jest.fn(),
  loading: false,
};

jest.mock('@/hooks/repositories/useFinancialCodeRepository', () => ({
  useFinancialCodeRepository: () => {
    return {
      getFinancialCode: mockGetApi,
      updateFinancialCode: mockUpdateApi,
    };
  },
}));

const history = createMemoryHistory();

let viewProps: IUpdateFinancialCodeFormProps | undefined;

const TestView: React.FC<IUpdateFinancialCodeFormProps> = props => {
  viewProps = props;
  return <span>Content Rendered</span>;
};

describe('UpdateFinancialCode container', () => {
  const setup = (renderOptions: RenderOptions = {}) => {
    const utils = render(
      <UpdateFinancialCodeContainer
        type={ApiGen_Concepts_FinancialCodeTypes.BusinessFunction}
        id={1}
        View={TestView}
      />,
      {
        ...renderOptions,
        history,
      },
    );
    return { ...utils };
  };

  beforeEach(() => {
    viewProps = undefined;
    jest.resetAllMocks();
  });

  it('renders the underlying form and retrieves the financial code to update', async () => {
    const { getByText } = setup();
    expect(getByText(/Content Rendered/)).toBeVisible();
    expect(mockGetApi.execute).toHaveBeenCalled();
  });

  it('displays error message for duplicate codes', async () => {
    const { getByText } = setup();

    await act(async () => {
      const error409 = createAxiosError(409, 'Duplicate code found');
      viewProps?.onError(error409);
    });

    expect(getByText(/Cannot update duplicate financial code/)).toBeVisible();
  });

  it('displays a toast for generic server errors', async () => {
    setup();

    await act(async () => {
      const error500 = createAxiosError(500);
      viewProps?.onError(error500);
    });

    expect(await screen.findByText('Unable to save. Please try again.')).toBeVisible();
  });

  it('makes request to update financial code and returns the response', async () => {
    setup();
    mockUpdateApi.execute.mockResolvedValue(mockFinancialCode());

    let createdCode: ApiGen_Concepts_FinancialCode | undefined;
    await act(async () => {
      createdCode = await viewProps?.onSave({} as ApiGen_Concepts_FinancialCode);
    });

    expect(mockUpdateApi.execute).toHaveBeenCalled();
    expect(createdCode).toStrictEqual({ ...mockFinancialCode() });
  });

  it('navigates back to financial codes list when form is cancelled', async () => {
    setup();
    await act(async () => {
      viewProps?.onCancel();
    });
    expect(history.location.pathname).toBe(`/admin/financial-code/list`);
  });

  it('navigates back to financial codes list and displays a toast when code is saved', async () => {
    setup();
    await act(async () => {
      await viewProps?.onSuccess({} as ApiGen_Concepts_FinancialCode);
    });
    expect(history.location.pathname).toBe(`/admin/financial-code/list`);
    expect(await screen.findByText(/Financial code saved/)).toBeVisible();
  });
});
