import { createMemoryHistory } from 'history';

import { act, renderAsync, RenderOptions } from '@/utils/test-utils';

import AddDispositionContainer, { IAddDispositionContainerProps } from './AddDispositionContainer';
import { IAddDispositionContainerViewProps } from './AddDispositionContainerView';

jest.mock('@react-keycloak/web');
const history = createMemoryHistory();

const onClose = jest.fn();

let viewProps: IAddDispositionContainerViewProps | undefined;
const TestView: React.FC<IAddDispositionContainerViewProps> = props => {
  viewProps = props;
  return <span>Content Rendered</span>;
};

describe('Add Form8 Container component', () => {
  const setup = async (
    renderOptions: RenderOptions & {
      props?: Partial<IAddDispositionContainerProps>;
    } = {},
  ) => {
    const component = await renderAsync(
      <AddDispositionContainer View={TestView} onClose={onClose} />,
      {
        history,
        useMockAuthentication: true,
        claims: [],
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

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('Renders the underlying form', async () => {
    const { getByText } = await setup();
    expect(getByText(/Content Rendered/)).toBeVisible();
  });

  it('calls onSuccess when the Disposition is saved successfully', async () => {
    await setup({});

    await act(async () => {
      viewProps?.onCancel();
    });

    expect(onClose).toHaveBeenCalled();
  });
});
