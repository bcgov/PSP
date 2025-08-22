import { feature } from '@turf/turf';
import { FormikHelpers, FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import { createRef } from 'react';

import { IMapStateMachineContext } from '@/components/common/mapFSM/MapStateMachineContext';
import { useManagementFileRepository } from '@/hooks/repositories/useManagementFileRepository';
import { getMockFullyAttributedParcel } from '@/mocks/faParcelLayerResponse.mock';
import { getMockPolygon } from '@/mocks/geometries.mock';
import { mockManagementFileResponse } from '@/mocks/managementFiles.mock';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { emptyRegion } from '@/models/layers/motRegionalBoundary';
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

  it('should preserve the order of properties when saving', async () => {
    const testMockMachine: IMapStateMachineContext = {
      ...mapMachineBaseMock,
      selectedFeatures: [
        {
          location: { lng: -120.69195885, lat: 50.25163372 },
          fileLocation: null,
          pimsFeature: null,
          parcelFeature: getMockFullyAttributedParcel('111-111-111'),
          regionFeature: feature(getMockPolygon(), {
            ...emptyRegion,
            REGION_NUMBER: 1,
            REGION_NAME: 'South Coast Region',
          }),
          districtFeature: null,
          selectingComponentId: null,
          municipalityFeature: null,
        },
        {
          location: { lng: -120.69195885, lat: 50.25163372 },
          fileLocation: null,
          pimsFeature: null,
          parcelFeature: getMockFullyAttributedParcel('222-222-222'),
          regionFeature: feature(getMockPolygon(), {
            ...emptyRegion,
            REGION_NUMBER: 1,
            REGION_NAME: 'South Coast Region',
          }),
          districtFeature: null,
          selectingComponentId: null,
          municipalityFeature: null,
        },
        {
          location: { lng: -120.69195885, lat: 50.25163372 },
          fileLocation: null,
          pimsFeature: null,
          parcelFeature: getMockFullyAttributedParcel('333-333-333'),
          regionFeature: feature(getMockPolygon(), {
            ...emptyRegion,
            REGION_NUMBER: 1,
            REGION_NAME: 'South Coast Region',
          }),
          districtFeature: null,
          selectingComponentId: null,
          municipalityFeature: null,
        },
      ],
    };
    await setup({ mockMapMachine: testMockMachine });

    expect(viewProps?.managementInitialValues.fileProperties).toHaveLength(3);

    await act(async () => {
      await viewProps?.onSubmit(viewProps?.managementInitialValues, {
        setSubmitting: vi.fn(),
        resetForm: vi.fn(),
      } as unknown as FormikHelpers<ManagementFormModel>);
    });

    expect(mockCreateManagementFile.execute).toHaveBeenCalledWith(
      expect.objectContaining({
        fileProperties: expect.arrayContaining([
          expect.objectContaining({
            property: expect.objectContaining({ pid: 111111111 }),
            displayOrder: 0,
          }),
          expect.objectContaining({
            property: expect.objectContaining({ pid: 222222222 }),
            displayOrder: 1,
          }),
          expect.objectContaining({
            property: expect.objectContaining({ pid: 333333333 }),
            displayOrder: 2,
          }),
        ]),
      }),
      [],
    );
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
