import { FormikHelpers, FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import { createRef } from 'react';

import { IMapStateMachineContext } from '@/components/common/mapFSM/MapStateMachineContext';
import { useManagementFileRepository } from '@/hooks/repositories/useManagementFileRepository';
import { mockManagementFileResponse } from '@/mocks/managementFiles.mock';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import {
  act,
  createAxiosError,
  getMockRepositoryObj,
  render,
  RenderOptions,
  screen,
} from '@/utils/test-utils';

import { SideBarContextProvider } from '../../context/sidebarContext';
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

const mockCreateManagementFile = getMockRepositoryObj();

vi.mock('@/hooks/repositories/useManagementFileRepository');
vi.mocked(useManagementFileRepository, { partial: true }).mockReturnValue({
  addManagementFileApi: mockCreateManagementFile,
});

describe('Add Management Container component', () => {
  const setup = async (
    renderOptions: RenderOptions & {
      props?: Partial<IAddManagementContainerProps>;
    } = {},
  ) => {
    const ref = createRef<FormikProps<ManagementFormModel>>();
    const component = render(
      <SideBarContextProvider>
        <AddManagementContainer View={TestView} onClose={onClose} onSuccess={onSuccess} />
      </SideBarContextProvider>,
      {
        history,
        useMockAuthentication: true,
        claims: [],
        ...renderOptions,
      },
    );

    // wait for the component to finish loading
    await act(async () => {});

    return {
      ...component,
      getFormikRef: () => ref,
    };
  };

  beforeEach(() => {
    viewProps = undefined;
    mockCreateManagementFile.execute.mockResolvedValue(mockManagementFileResponse());
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders the underlying form', async () => {
    const { getByText } = await setup();
    expect(getByText(/Content Rendered/)).toBeVisible();
  });

  it('passes correct props to View', async () => {
    await setup();
    expect(viewProps).toBeDefined();
    expect(typeof viewProps?.onCancel).toBe('function');
    expect(typeof viewProps?.onSubmit).toBe('function');
  });

  it('calls onClose when changes are cancelled', async () => {
    await setup();

    await act(async () => {
      viewProps?.onCancel();
    });

    expect(onClose).toHaveBeenCalled();
  });

  it('calls onSuccess when the Management is saved successfully', async () => {
    const testMockMachine: IMapStateMachineContext = {
      ...mapMachineBaseMock,
      processCreation: vi.fn(),
      refreshMapProperties: vi.fn(),
    };
    await setup({ mockMapMachine: testMockMachine });

    await act(async () => {
      await viewProps?.onSubmit(ManagementFormModel.fromApi(mockManagementFileResponse()), {
        setSubmitting: vi.fn(),
        resetForm: vi.fn(),
      } as unknown as FormikHelpers<ManagementFormModel>);
    });

    expect(onSuccess).toHaveBeenCalled();
    expect(testMockMachine.processCreation).toHaveBeenCalled();
    expect(testMockMachine.refreshMapProperties).toHaveBeenCalled();
  });

  it('calls setSubmitting(false) after submit', async () => {
    await setup();

    const setSubmitting = vi.fn();
    const resetForm = vi.fn();
    await act(async () => {
      await viewProps?.onSubmit(ManagementFormModel.fromApi(mockManagementFileResponse()), {
        setSubmitting,
        resetForm,
      } as unknown as FormikHelpers<ManagementFormModel>);
    });

    expect(setSubmitting).toHaveBeenCalledWith(false);
    expect(resetForm).toHaveBeenCalled();
  });

  it('displays error when addManagementFileApi throws', async () => {
    mockCreateManagementFile.execute.mockRejectedValue(createAxiosError(400, 'network error'));
    await setup();

    await act(async () => {
      await viewProps?.onSubmit(ManagementFormModel.fromApi(mockManagementFileResponse()), {
        setSubmitting: vi.fn(),
        resetForm: vi.fn(),
      } as unknown as FormikHelpers<ManagementFormModel>);
    });

    expect(onSuccess).not.toHaveBeenCalled();
    expect(await screen.findByText(/network error/i)).toBeVisible();
  });

  it('calls resetForm after successful submit', async () => {
    await setup();

    const resetForm = vi.fn();
    await act(async () => {
      await viewProps?.onSubmit(ManagementFormModel.fromApi(mockManagementFileResponse()), {
        setSubmitting: vi.fn(),
        resetForm,
      } as unknown as FormikHelpers<ManagementFormModel>);
    });

    expect(resetForm).toHaveBeenCalled();
  });

  it('calls setFilePropertyLocations with empty array on open', async () => {
    const testMockMachine: IMapStateMachineContext = {
      ...mapMachineBaseMock,
      setFilePropertyLocations: vi.fn(),
    };
    await setup({ mockMapMachine: testMockMachine });
    expect(testMockMachine.setFilePropertyLocations).toHaveBeenCalledWith([]);
  });
});
