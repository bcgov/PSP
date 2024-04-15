import { FormikHelpers, FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import { createRef } from 'react';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { act, createAxiosError, renderAsync, RenderOptions } from '@/utils/test-utils';

import { DispositionFormModel } from '../models/DispositionFormModel';
import AddDispositionContainer, { IAddDispositionContainerProps } from './AddDispositionContainer';
import { IAddDispositionContainerViewProps } from './AddDispositionContainerView';

const history = createMemoryHistory();

const onClose = vi.fn();

let viewProps: IAddDispositionContainerViewProps | undefined;
const TestView: React.FC<IAddDispositionContainerViewProps> = props => {
  viewProps = props;
  return <span>Content Rendered</span>;
};

const mockCreateDispositionFile = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
};

vi.mock('@/hooks/repositories/useDispositionProvider', () => ({
  useDispositionProvider: () => {
    return {
      addDispositionFileApi: mockCreateDispositionFile,
    };
  },
}));

describe('Add Disposition Container component', () => {
  const setup = async (
    renderOptions: RenderOptions & {
      props?: Partial<IAddDispositionContainerProps>;
    } = {},
  ) => {
    const ref = createRef<FormikProps<DispositionFormModel>>();
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

  it('calls onSuccess when the Disposition is saved successfully', async () => {
    await setup({});

    await act(async () => {
      viewProps?.onCancel();
    });

    expect(onClose).toHaveBeenCalled();
  });

  it(`triggers the modal for contractor not in team (400 - Error)`, async () => {
    mockCreateDispositionFile.execute.mockRejectedValue(
      createAxiosError(
        409,
        `As a contractor, you must add yourself as a team member to the file in order to create or save changes`,
      ),
    );
    const mockDispositionValues = new DispositionFormModel(1, 'NUMBER', 1);

    const { getFormikRef, findByText } = await setup();
    const formikHelpers: Partial<FormikHelpers<DispositionFormModel>> = {
      setSubmitting: vi.fn(),
    };

    await act(async () => {
      return viewProps?.onSubmit(
        mockDispositionValues,
        formikHelpers as FormikHelpers<DispositionFormModel>,
      );
    });

    await act(async () => getFormikRef().current?.submitForm());

    const popup = await findByText(/As a contractor, you must add yourself as a team member/i);
    expect(popup).toBeVisible();
  });
});
