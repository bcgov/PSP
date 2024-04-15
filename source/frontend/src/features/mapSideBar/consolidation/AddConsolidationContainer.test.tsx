import { Formik, FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import { createRef } from 'react';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { act, renderAsync, RenderOptions, screen, userEvent } from '@/utils/test-utils';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { IAddConsolidationViewProps } from './AddConsolidationView';
import { ConsolidationFormModel } from './AddConsolidationModel';
import AddConsolidationContainer, {
  IAddConsolidationContainerProps,
} from './AddConsolidationContainer';

const history = createMemoryHistory();

const onClose = vi.fn();

let viewProps: IAddConsolidationViewProps | undefined;
const TestView: React.FC<IAddConsolidationViewProps> = props => {
  viewProps = props;
  return (
    <Formik<ConsolidationFormModel>
      innerRef={props.formikRef}
      onSubmit={props.onSubmit}
      initialValues={props.consolidationInitialValues}
    >
      <>
        <span>Content Rendered</span>
      </>
    </Formik>
  );
};

const mockAddPropertyOperation = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
};

vi.mock('@/hooks/repositories/usePropertyOperationRepository', () => ({
  usePropertyOperationRepository: () => {
    return {
      addPropertyOperationApi: mockAddPropertyOperation,
    };
  },
}));

describe('Add Consolidation Container component', () => {
  const setup = async (
    renderOptions: RenderOptions & {
      props?: Partial<IAddConsolidationContainerProps>;
    } = {},
  ) => {
    const ref = createRef<FormikProps<ConsolidationFormModel>>();
    const component = await renderAsync(
      <AddConsolidationContainer View={TestView} onClose={onClose} />,
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
    history.location.pathname = '/';
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('Renders the underlying form', async () => {
    const { getByText } = await setup();
    await act(async () => {});
    expect(getByText(/Content Rendered/)).toBeVisible();
  });

  it('calls onClose when the Disposition is saved successfully', async () => {
    await setup({});

    await act(async () => {
      viewProps?.onCancel();
    });

    expect(onClose).toHaveBeenCalled();
  });

  it('Displays modal when onsave is called and form is valid', async () => {
    await setup({});

    await act(async () => {
      viewProps?.onSave();
    });

    expect(screen.getByText('Are you sure?')).toBeVisible();
  });

  it('Changes the url when the save operation completes successfully when the response does not contain a viable property to navigate', async () => {
    await setup({});

    await act(async () => {
      viewProps?.onSave();
    });

    expect(screen.getByText('Are you sure?')).toBeVisible();

    mockAddPropertyOperation.execute.mockResolvedValue([{}]);
    await act(async () => {
      userEvent.click(screen.getByText('Yes'));
    });

    expect(history.location.pathname).toBe('/mapview');
  });

  it('aborting the modal onsave does not change the url', async () => {
    await setup({});

    await act(async () => {
      viewProps?.onSave();
    });

    expect(screen.getByText('Are you sure?')).toBeVisible();

    mockAddPropertyOperation.execute.mockResolvedValue([{}]);
    await act(async () => {
      userEvent.click(screen.getByText('No'));
    });

    expect(history.location.pathname).toBe('/');
  });

  it('Changes the url when the submit operation completes successfully when the response does not contain a viable property to navigate', async () => {
    await setup({});

    mockAddPropertyOperation.execute.mockResolvedValue([{}]);
    const model = new ConsolidationFormModel();
    model.sourceProperties = [{} as ApiGen_Concepts_Property];
    await act(async () => {
      viewProps?.onSubmit(model, {} as any);
    });

    expect(history.location.pathname).toBe('/mapview');
  });

  it('Changes the url when the submit operation completes successfully when the response contains a viable property to navigate', async () => {
    await setup({});

    mockAddPropertyOperation.execute.mockResolvedValue([{ destinationProperty: { id: 1 } }]);
    const model = new ConsolidationFormModel();
    model.sourceProperties = [{} as ApiGen_Concepts_Property];
    model.destinationProperty = { id: 1 } as ApiGen_Concepts_Property;
    await act(async () => {
      viewProps?.onSubmit(model, {} as any);
    });

    expect(history.location.pathname).toBe(`/mapview/sidebar/property/1`);
  });

  it('Calls onCancel if the onCancel is called', async () => {
    await setup({});

    await act(async () => {
      viewProps?.onCancel();
    });

    expect(onClose).toHaveBeenCalled();
  });

  it('Displays modal if cancelled when form dirty', async () => {
    const { container } = await setup({});

    await act(async () => {
      viewProps?.formikRef.current?.setFieldValue('test', 1);
    });
    await act(async () => {
      viewProps?.onCancel();
    });

    expect(screen.getByText('Confirm Changes')).toBeVisible();
  });

  it('calls onclose if dirty model dismissed', async () => {
    const { container } = await setup({});

    await act(async () => {
      viewProps?.formikRef.current?.setFieldValue('test', 1);
    });
    await act(async () => {
      viewProps?.onCancel();
    });

    expect(screen.getByText('Confirm Changes')).toBeVisible();

    await act(async () => {
      userEvent.click(screen.getByText('Yes'));
    });
  });

  it('does not call onclose if dirty modal cancelled', async () => {
    const { container } = await setup({});

    await act(async () => {
      viewProps?.formikRef.current?.setFieldValue('test', 1);
    });
    await act(async () => {
      viewProps?.onCancel();
    });

    expect(screen.getByText('Confirm Changes')).toBeVisible();

    await act(async () => {
      userEvent.click(screen.getByText('No'));
    });
  });
});
