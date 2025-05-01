import { FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import { createRef } from 'react';

import { act, renderAsync, RenderOptions } from '@/utils/test-utils';

import { ManagementFormModel } from '../models/ManagementFormModel';
import AddManagementContainer, { IAddManagementContainerProps } from './AddManagementContainer';
import { IAddManagementContainerViewProps } from './AddManagementContainerView';

const history = createMemoryHistory();

const onClose = vi.fn();
const onSuccess = vi.fn();

let viewProps: IAddManagementContainerViewProps | undefined;
const TestView: React.FC<IAddManagementContainerViewProps> = props => {
  viewProps = props;
  return <span>Content Rendered</span>;
};

const mockCreateManagementFile = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
};

vi.mock('@/hooks/repositories/useManagementProvider', () => ({
  useManagementProvider: () => {
    return {
      addManagementFileApi: mockCreateManagementFile,
    };
  },
}));

describe('Add Management Container component', () => {
  const setup = async (
    renderOptions: RenderOptions & {
      props?: Partial<IAddManagementContainerProps>;
    } = {},
  ) => {
    const ref = createRef<FormikProps<ManagementFormModel>>();
    const component = await renderAsync(
      <AddManagementContainer View={TestView} onClose={onClose} onSuccess={onSuccess} />,
      {
        history,
        useMockAuthentication: true,
        claims: [],
        ...renderOptions,
      },
    );

    return {
      ...component,
      getFormikRef: () => ref,
    };
  };

  beforeEach(() => {
    viewProps = undefined;
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('Renders the underlying form', async () => {
    const { getByText } = await setup();
    expect(getByText(/Content Rendered/)).toBeVisible();
  });

  it('calls onSuccess when the Management is saved successfully', async () => {
    await setup({});

    await act(async () => {
      viewProps?.onCancel();
    });

    expect(onClose).toHaveBeenCalled();
  });
});
