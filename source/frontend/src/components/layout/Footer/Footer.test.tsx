import renderer from 'react-test-renderer';

import Footer from './Footer';
import IApiVersion from '@/hooks/pims-api/interfaces/IApiVersion';

const defaultVersion: IApiVersion = {
  environment: 'test',
  version: '11.1.1.1',
  fileVersion: '11.1.1.1',
  informationalVersion: '11.1.1-1.999',
};

const mockGetVersion = vi.fn(async () => {
  return Promise.resolve({ data: defaultVersion });
});

vi.mock('@/hooks/pims-api/useApiHealth', () => ({
  useApiHealth: () => ({
    getVersion: mockGetVersion,
  }),
}));

describe('Footer', () => {
  it('renders correctly', () => {
    const tree = renderer.create(<Footer />).toJSON();
    expect(tree).toMatchSnapshot();
  });
});
