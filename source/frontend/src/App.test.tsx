import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';

import useKeycloakWrapper, { IKeycloak } from '@/hooks/useKeycloakWrapper';

const mockAxios = new MockAdapter(axios);

vi.mock('@/hooks/useKeycloakWrapper');
vi.mocked(useKeycloakWrapper).mockReturnValue({ hasClaim: () => true } as unknown as IKeycloak);

// This is required to mock react-redux so that the App can render.
const mockDispatch = vi.fn();
vi.mock('react-redux', () => ({
  connect:
    (mapStateToProps: any, mapDispatchToProps: (arg0: any, arg1: any) => any) =>
    (reactComponent: any) => ({
      mapStateToProps,
      mapDispatchToProps: vi.fn((dispatch = mockDispatch, ownProps) =>
        mapDispatchToProps(dispatch, ownProps),
      ),
      reactComponent,
      mockDispatch: vi.fn(),
    }),
  useSelector: vi.fn(),
  useDispatch: () => mockDispatch,
}));

describe('App suite', () => {
  afterEach(() => {
    mockAxios.reset();
  });

  it('fake test', () => {
    expect(1).toBe(1);
  });

  // TODO: PSP-860 Create tests for the App component.
  // it('App snapshot', () => {
  //   const { container } = render(<App />);
  //   expect(container).toMatchSnapshot();
  // });
});
