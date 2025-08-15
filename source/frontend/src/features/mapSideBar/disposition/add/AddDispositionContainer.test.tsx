import { FormikHelpers, FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import { createRef } from 'react';

import { IMapStateMachineContext } from '@/components/common/mapFSM/MapStateMachineContext';
import { useDispositionProvider } from '@/hooks/repositories/useDispositionProvider';
import { mockDispositionFileResponse } from '@/mocks/dispositionFiles.mock';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import {
  act,
  createAxiosError,
  getMockRepositoryObj,
  render,
  RenderOptions,
} from '@/utils/test-utils';

import { SideBarContextProvider } from '../../context/sidebarContext';
import { DispositionFormModel } from '../models/DispositionFormModel';
import AddDispositionContainer, { IAddDispositionContainerProps } from './AddDispositionContainer';
import { IAddDispositionContainerViewProps } from './AddDispositionContainerView';

const history = createMemoryHistory();

const onClose = vi.fn();
const onSuccess = vi.fn();

let viewProps: IAddDispositionContainerViewProps | undefined;
const TestView: React.FC<IAddDispositionContainerViewProps> = props => {
  viewProps = props;
  return <span>Content Rendered</span>;
};

const mockCreateDispositionFile = getMockRepositoryObj();

vi.mock('@/hooks/repositories/useDispositionProvider');
vi.mocked(useDispositionProvider, { partial: true }).mockReturnValue({
  addDispositionFileApi: mockCreateDispositionFile,
});

describe('Add Disposition Container component', () => {
  const setup = async (
    renderOptions: RenderOptions & {
      props?: Partial<IAddDispositionContainerProps>;
    } = {},
  ) => {
    const ref = createRef<FormikProps<DispositionFormModel>>();
    const component = render(
      <SideBarContextProvider>
        <AddDispositionContainer View={TestView} onClose={onClose} onSuccess={onSuccess} />
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
    mockCreateDispositionFile.execute.mockResolvedValue(mockDispositionFileResponse());
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders the underlying form', async () => {
    const { getByText } = await setup();
    expect(getByText(/Content Rendered/)).toBeVisible();
  });

  it('calls onCancel when the form is cancelled', async () => {
    await setup();

    await act(async () => {
      viewProps?.onCancel();
    });

    expect(onClose).toHaveBeenCalled();
  });

  it('calls onSuccess when the Disposition is saved successfully', async () => {
    const testMockMachine: IMapStateMachineContext = {
      ...mapMachineBaseMock,
      processCreation: vi.fn(),
      refreshMapProperties: vi.fn(),
    };
    const formikHelpers: Partial<FormikHelpers<DispositionFormModel>> = {
      setSubmitting: vi.fn(),
      resetForm: vi.fn(),
    };
    await setup({ mockMapMachine: testMockMachine });

    await act(async () => {
      viewProps?.onSubmit(
        new DispositionFormModel(1, 'NUMBER', 1),
        formikHelpers as FormikHelpers<DispositionFormModel>,
      );
    });

    expect(onSuccess).toHaveBeenCalled();
    expect(testMockMachine.processCreation).toHaveBeenCalled();
    expect(testMockMachine.refreshMapProperties).toHaveBeenCalled();
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

  it('resets the "draft" markers when the file is opened', async () => {
    const testMockMachine: IMapStateMachineContext = {
      ...mapMachineBaseMock,
    };
    await setup({ mockMapMachine: testMockMachine });
    expect(testMockMachine.setFilePropertyLocations).toHaveBeenCalledWith([]);
  });
});
