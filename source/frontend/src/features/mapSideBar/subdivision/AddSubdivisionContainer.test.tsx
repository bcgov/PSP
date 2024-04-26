import { Formik, FormikHelpers, FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import { createRef } from 'react';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import {
  act,
  createAxiosError,
  fillInput,
  renderAsync,
  RenderOptions,
  screen,
  userEvent,
} from '@/utils/test-utils';
import { IAddSubdivisionViewProps } from './AddSubdivisionView';
import { IAddAcquisitionContainerProps } from '../acquisition/add/AddAcquisitionContainer';
import { SubdivisionFormModel } from './AddSubdivisionModel';
import AddSubdivisionContainer from './AddSubdivisionContainer';
import { ApiGen_Concepts_PropertyOperation } from '@/models/api/generated/ApiGen_Concepts_PropertyOperation';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { Input } from '@/components/common/form';

const history = createMemoryHistory();

const onClose = vi.fn();

let viewProps: IAddSubdivisionViewProps | undefined;
const TestView: React.FC<IAddSubdivisionViewProps> = props => {
  viewProps = props;
  return (
    <Formik<SubdivisionFormModel>
      innerRef={props.formikRef}
      onSubmit={props.onSubmit}
      initialValues={props.subdivisionInitialValues}
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

describe('Add Subdivision Container component', () => {
  const setup = async (
    renderOptions: RenderOptions & {
      props?: Partial<IAddAcquisitionContainerProps>;
    } = {},
  ) => {
    const ref = createRef<FormikProps<SubdivisionFormModel>>();
    const component = await renderAsync(
      <AddSubdivisionContainer View={TestView} onClose={onClose} />,
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
    const model = new SubdivisionFormModel();
    model.destinationProperties = [{} as ApiGen_Concepts_Property];
    await act(async () => {
      viewProps?.onSubmit(model, {} as any);
    });

    expect(history.location.pathname).toBe('/mapview');
  });

  it('Changes the url when the submit operation completes successfully when the response contains a viable property to navigate', async () => {
    await setup({});

    mockAddPropertyOperation.execute.mockResolvedValue([{}]);
    const model = new SubdivisionFormModel();
    model.destinationProperties = [{} as ApiGen_Concepts_Property];
    model.sourceProperty = { id: 1 } as ApiGen_Concepts_Property;
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
