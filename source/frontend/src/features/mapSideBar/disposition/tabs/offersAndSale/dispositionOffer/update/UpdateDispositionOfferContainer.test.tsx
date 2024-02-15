import { createMemoryHistory } from 'history';
import React from 'react';

import { Claims } from '@/constants/claims';
import { mockDispositionFileOfferApi } from '@/mocks/dispositionFiles.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { Api_DispositionFileOffer } from '@/models/api/DispositionFile';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, createAxiosError, render, RenderOptions, waitForEffects } from '@/utils/test-utils';

import { IDispositionOfferFormProps } from '../form/DispositionOfferForm';
import { DispositionOfferFormModel } from '../models/DispositionOfferFormModel';
import UpdateDispositionOfferContainer, {
  IUpdateDispositionOfferContainerProps,
} from './UpdateDispositionOfferContainer';

const history = createMemoryHistory();
const mockDispositionOfferApi = mockDispositionFileOfferApi(10, 1);

const mockPutOfferApi = {
  error: undefined,
  response: undefined,
  execute: jest.fn(),
  loading: false,
};

const mockGetOfferApi = {
  error: undefined,
  response: undefined,
  execute: jest.fn().mockResolvedValue(mockDispositionOfferApi),
  loading: false,
};
const onSuccess = jest.fn();

jest.mock('@/hooks/repositories/useDispositionProvider', () => ({
  useDispositionProvider: () => {
    return {
      getDispositionOffer: mockGetOfferApi,
      putDispositionOffer: mockPutOfferApi,
    };
  },
}));

// eslint-disable-next-line @typescript-eslint/no-unused-vars
let viewProps: IDispositionOfferFormProps | undefined;
const TestView: React.FC<IDispositionOfferFormProps> = props => {
  viewProps = props;
  return <span>Content Rendered</span>;
};

describe('Update Disposition Offer Container component', () => {
  const setup = async (
    renderOptions: RenderOptions & {
      props?: Partial<IUpdateDispositionOfferContainerProps>;
    } = {},
  ) => {
    const component = render(
      <UpdateDispositionOfferContainer
        dispositionFileId={1}
        dispositionOfferId={10}
        View={TestView}
        onSuccess={onSuccess}
      />,
      {
        history,
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
        },
        useMockAuthentication: true,
        claims: renderOptions?.claims ?? [Claims.DISPOSITION_VIEW, Claims.DISPOSITION_EDIT],
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

  it('Renders the underlying form', async () => {
    const { getByText } = await setup();
    expect(getByText(/Content Rendered/)).toBeVisible();
    expect(mockGetOfferApi.execute).toHaveBeenCalled();
  });

  it('Loads props with the initial values', async () => {
    mockGetOfferApi.execute.mockResolvedValue(mockDispositionOfferApi);
    await setup();
    await waitForEffects();

    expect(mockGetOfferApi.execute).toHaveBeenCalled();
    const formModel = DispositionOfferFormModel.fromApi(mockDispositionOfferApi);

    expect(viewProps?.initialValues).toStrictEqual(formModel);
  });

  it('makes request to create a new Offer and returns the response', async () => {
    mockGetOfferApi.execute.mockResolvedValue(mockDispositionOfferApi);
    mockPutOfferApi.execute.mockReturnValue(mockDispositionOfferApi);

    await setup();

    let createdOffer: Api_DispositionFileOffer | undefined;
    await act(async () => {
      createdOffer = await viewProps?.onSave({} as Api_DispositionFileOffer);
    });

    expect(mockPutOfferApi.execute).toHaveBeenCalled();
    expect(createdOffer).toStrictEqual({ ...mockDispositionOfferApi });
    expect(history.location.pathname).toBe('/');
  });

  it('navigates back to Offers and Sale tab when form is cancelled', async () => {
    await setup();
    act(() => {
      viewProps?.onCancel();
    });

    expect(history.location.pathname).toBe('/');
    expect(mockPutOfferApi.execute).not.toHaveBeenCalled();
  });

  it('displays error message for duplicate accepted status', async () => {
    await setup({ props: { dispositionFileId: 1, dispositionOfferId: 10 } });
    await waitForEffects();

    await act(async () => {
      const error409 = createAxiosError(409, 'Duplicate');
      viewProps?.onError(error409);
    });

    expect(viewProps?.showOfferStatusError).toBe(true);
  });
});
