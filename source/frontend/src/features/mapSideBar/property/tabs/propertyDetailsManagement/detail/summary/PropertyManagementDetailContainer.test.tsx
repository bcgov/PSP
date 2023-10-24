import { FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import { forwardRef } from 'react';

import { mockLookups } from '@/mocks/lookups.mock';
import { getMockApiPropertyManagement } from '@/mocks/propertyManagement.mock';
import { Api_PropertyManagement } from '@/models/api/Property';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { render, RenderOptions } from '@/utils/test-utils';

import {
  IPropertyManagementDetailContainerProps,
  PropertyManagementDetailContainer,
} from './PropertyManagementDetailContainer';
import { IPropertyManagementDetailViewProps } from './PropertyManagementDetailView';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const mockGetApi = {
  error: undefined,
  response: undefined as Api_PropertyManagement | undefined,
  execute: jest.fn(),
  loading: false,
};

jest.mock('@/hooks/repositories/usePropertyManagementRepository', () => ({
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

  const setEditManagementState = jest.fn();

  const setup = (
    renderOptions?: RenderOptions & { props?: Partial<IPropertyManagementDetailContainerProps> },
  ) => {
    renderOptions = renderOptions ?? {};
    const utils = render(
      <PropertyManagementDetailContainer
        {...renderOptions.props}
        propertyId={renderOptions.props?.propertyId ?? 1}
        setEditManagementState={setEditManagementState}
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
    jest.clearAllMocks();
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
