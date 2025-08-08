import { FormikHelpers, FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import { createRef } from 'react';

import { act, getMockRepositoryObj, render, RenderOptions } from '@/utils/test-utils';

import { IMapStateMachineContext } from '@/components/common/mapFSM/MapStateMachineContext';
import { useManagementFileRepository } from '@/hooks/repositories/useManagementFileRepository';
import { mockManagementFileResponse } from '@/mocks/managementFiles.mock';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
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
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders the underlying form', async () => {
    const { getByText } = await setup();
    expect(getByText(/Content Rendered/)).toBeVisible();
  });

  it('calls onClose when changes are cancelled', async () => {
    await setup();

    await act(async () => {
      viewProps?.onCancel();
    });

    expect(onClose).toHaveBeenCalled();
  });

  it('calls onSuccess when the Management is saved successfully', async () => {
    mockCreateManagementFile.execute.mockResolvedValue(mockManagementFileResponse());
    await setup();

    await act(async () => {
      viewProps?.onSubmit(ManagementFormModel.fromApi(mockManagementFileResponse()), {
        setSubmitting: vi.fn(),
        resetForm: vi.fn(),
      } as unknown as FormikHelpers<ManagementFormModel>);
    });

    expect(onSuccess).toHaveBeenCalled();
  });

  it('resets the "draft" markers when the file is opened', async () => {
    const testMockMachine: IMapStateMachineContext = {
      ...mapMachineBaseMock,
    };
    await setup({ mockMapMachine: testMockMachine });
    expect(testMockMachine.setFilePropertyLocations).toHaveBeenCalledWith([]);
  });
});
