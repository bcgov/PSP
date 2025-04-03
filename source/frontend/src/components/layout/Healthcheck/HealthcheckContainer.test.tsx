import {
  act,
  render,
  RenderOptions,
} from '@/utils/test-utils';

import { HealthcheckContainer, IHealthcheckContainerProps } from './HealthcheckContainer';
import { useApiHealth } from '@/hooks/pims-api/useApiHealth';
import { IHealthCheckViewProps } from './HealthcheckView';
import IHealthLive from '@/hooks/pims-api/interfaces/IHealthLive';
import { HttpStatusCode } from 'node_modules/axios/index.cjs';

const mockGetLiveApi = vi.fn();
const mockGetSystemCheckApi = vi.fn();
const updateHealthcheckResult = vi.fn();

vi.mock('@/hooks/pims-api/useApiHealth');
vi.mocked(useApiHealth, { partial: true }).mockImplementation(() => ({
  getLive: mockGetLiveApi,
  getSystemCheck: mockGetSystemCheckApi,
}));

let viewProps: IHealthCheckViewProps | undefined;
const TestView = (props: IHealthCheckViewProps) => {
  viewProps = props;
  return <>Content Rendered</>;
};

describe('Healthcheck container', () => {
  const setup = async (
    renderOptions: RenderOptions & {
      props?: Partial<IHealthcheckContainerProps>;
    } = {},
  ) => {
    const utils = render(
      <HealthcheckContainer
        systemDegraded={renderOptions.props?.systemDegraded ?? false}
        updateHealthcheckResult={updateHealthcheckResult}
        View={TestView}
      />,
      {
        ...renderOptions,
        keycloakMock: renderOptions.keycloakMock,
        claims: renderOptions?.claims ?? [],
      },
    );
    return {
      ...utils,
    };
  };

  afterEach(() => {
    vi.clearAllMocks();
  });

  beforeEach(() => {
    viewProps = undefined;

    mockGetLiveApi.mockResolvedValue({
      data: {
        status: 'Unhealthy',
        totalDuration: '00:00:00.0031518',
        entries: {},
      } as unknown as IHealthLive,
    });

    mockGetSystemCheckApi.mockResolvedValue({
      HttpStatusCode: 503,
      data: {
        status: 'Unhealthy',
        totalDuration: '00:00:00.9134453',
        entries: {
          PmbcExternalApi: {
            data: {},
            duration: '00:00:00.2917181',
            status: 'Healthy',
            tags: ['services', 'external', 'system-check'],
          },
          Geoserver: {
            data: {},
            description:
              'An invalid request URI was provided. Either the request URI must be an absolute URI or BaseAddress must be set.',
            duration: '00:00:00.0509285',
            exception:
              'An invalid request URI was provided. Either the request URI must be an absolute URI or BaseAddress must be set.',
            status: 'Unhealthy',
            tags: ['services', 'system', 'system-check'],
          },
          Mayan: {
            data: {},
            duration: '00:00:00.1083108',
            status: 'Healthy',
            tags: ['services', 'system-check'],
          },
          Ltsa: {
            data: {},
            description:
              'LTSA error response: Received error response from LTSA when retrieving authorization token\r\n',
            duration: '00:00:00.9076004',
            status: 'Unhealthy',
            tags: ['services', 'external', 'system-check'],
          },
          Geocoder: {
            data: {},
            duration: '00:00:00.1995340',
            status: 'Healthy',
            tags: ['services', 'external', 'system-check'],
          },
          Cdogs: {
            data: {},
            duration: '00:00:00.1601968',
            status: 'Healthy',
            tags: ['services', 'external', 'system-check'],
          },
        },
      },
    });
  });

  it('renders as expected', async () => {
    await act(async () => {
      const { asFragment } = await setup();
      expect(asFragment()).toMatchSnapshot();
    });

    expect(updateHealthcheckResult).toHaveBeenCalledWith(true);
  });
});
