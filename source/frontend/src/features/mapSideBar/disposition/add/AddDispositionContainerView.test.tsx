import { FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import { createRef } from 'react';

import Claims from '@/constants/claims';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes/lookupCodesSlice';
import { render, RenderOptions } from '@/utils/test-utils';

import { DispositionFormModel } from '../models/DispositionFormModel';
import AddDispositionContainerView, {
  IAddDispositionContainerViewProps,
} from './AddDispositionContainerView';
import { useApiUsers } from '@/hooks/pims-api/useApiUsers';
import { getUserMock, getMockPagedUsers } from '@/mocks/user.mock';
import { vi } from 'vitest';

const history = createMemoryHistory();

const onCancel = vi.fn();
const onSave = vi.fn();
const onSubmit = vi.fn();
const confirmBeforeAdd = vi.fn();

const initialValues = new DispositionFormModel();

vi.mock('react-visibility-sensor', () => {
  return {
    default: vi.fn().mockImplementation(({ children }) => {
      if (children instanceof Function) {
        return children({ isVisible: true });
      }
      return children;
    }),
  };
});

// mock API service calls
vi.mock('@/hooks/pims-api/useApiUsers');

vi.mocked(useApiUsers).mockReturnValue({
  getUserInfo: vi.fn().mockResolvedValue({}),
} as any);

describe('Add Disposition Container View', () => {
  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<IAddDispositionContainerViewProps> } = {},
  ) => {
    const ref = createRef<FormikProps<DispositionFormModel>>();
    const utils = render(
      <AddDispositionContainerView
        {...renderOptions.props}
        formikRef={ref}
        dispositionInitialValues={renderOptions.props?.dispositionInitialValues ?? initialValues}
        loading={renderOptions.props?.loading ?? false}
        displayFormInvalid={renderOptions.props?.displayFormInvalid ?? false}
        onCancel={onCancel}
        onSave={onSave}
        onSubmit={onSubmit}
        confirmBeforeAdd={confirmBeforeAdd}
      />,
      {
        ...renderOptions,
        useMockAuthentication: true,
        claims: renderOptions?.claims ?? [Claims.DISPOSITION_ADD],
        history: history,
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
        },
      },
    );

    return {
      ...utils,
      getCancelButton: () => utils.getByText(/Cancel/i),
    };
  };

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('matches snapshot', async () => {
    const { asFragment } = await setup();
    expect(asFragment()).toMatchSnapshot();
  });
});
