// eslint-disable-next-line simple-import-sort/imports
import { FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import { forwardRef } from 'react';

import { mockLookups } from '@/mocks/lookups.mock';
import { getMockApiPropertyManagement } from '@/mocks/propertyManagement.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { render, RenderOptions } from '@/utils/test-utils';

import {
  IPropertyManagementDetailContainerProps,
  PropertyManagementDetailContainer,
} from './PropertyManagementDetailContainer';
import { IPropertyManagementDetailViewProps } from './PropertyManagementDetailView';
import { ApiGen_Concepts_PropertyManagement } from '@/models/api/generated/ApiGen_Concepts_PropertyManagement';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const mockGetApi = {
  error: undefined,
  response: undefined as ApiGen_Concepts_PropertyManagement | undefined,
  execute: vi.fn(),
  loading: false,
};

vi.mock('@/hooks/repositories/usePropertyManagementRepository', () => ({
  usePropertyManagementRepository: () => {
    return {
      getPropertyManagement: mockGetApi,
    };
  },
}));

describe('PropertyManagementDetailContainer component', () => {
  let viewProps: IPropertyManagementDetailViewProps;

  const View = forwardRef<FormikProps<any>, IPropertyManagementDetailViewProps>((props, ref) => {
    viewProps = props;
    return <></>;
  });

  const setup = (
    renderOptions?: RenderOptions & { props?: Partial<IPropertyManagementDetailContainerProps> },
  ) => {
    renderOptions = renderOptions ?? {};
    const utils = render(
      <PropertyManagementDetailContainer
        {...renderOptions.props}
        propertyId={renderOptions.props?.propertyId ?? 1}
        View={View}
      />,
      {
        ...renderOptions,
        store: storeState,
        history,
      },
    );

    return {
      ...utils,
    };
  };

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('fetches property management info from the api', () => {
    mockGetApi.execute.mockResolvedValue(getMockApiPropertyManagement(1));
    setup({ props: { propertyId: 1 } });
    expect(mockGetApi.execute).toBeCalledWith(1);
  });

  it('passes property management info to child view as prop', async () => {
    const apiManagement = getMockApiPropertyManagement(1);
    mockGetApi.response = apiManagement;
    setup({ props: { propertyId: 1 } });
    expect(viewProps.isLoading).toBe(false);
    expect(viewProps.propertyManagement).toBe(apiManagement);
  });
});
