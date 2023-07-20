import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';

import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';

const mockAxios = new MockAdapter(axios);

jest.mock('@/hooks/useKeycloakWrapper');
(useKeycloakWrapper as jest.Mock).mockReturnValue({ hasClaim: () => true });

// This is required to mock react-redux so that the App can render.
const mockDispatch = jest.fn();
jest.mock('react-redux', () => ({
  connect:
    (mapStateToProps: any, mapDispatchToProps: (arg0: any, arg1: any) => any) =>
    (reactComponent: any) => ({
      mapStateToProps,
      mapDispatchToProps: jest.fn((dispatch = mockDispatch, ownProps) =>
        mapDispatchToProps(dispatch, ownProps),
      ),
      reactComponent,
      mockDispatch: jest.fn(),
    }),
  useSelector: jest.fn(),
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
