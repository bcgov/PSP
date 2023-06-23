import { fireEvent, render, waitFor } from '@/utils/test-utils';

import { IHelpPage, Topics } from '../interfaces';
import HelpBox from './HelpBox';

const topicMap = new Map();
topicMap.set('Map', null);
topicMap.set('Test Section', 'This is some test text');

const mockPage: IHelpPage = {
  name: 'Landing Page',
  topics: topicMap,
};

const setActiceTopic = jest.fn();

const renderHelpBox = () => {
  return render(
    <HelpBox
      setActiveTopic={setActiceTopic}
      activeTopic={Topics.LANDING_MAP}
      helpPage={mockPage}
    />,
  );
};
describe('Help box tests...', () => {
  it('renders correctly..', () => {
    const { container } = renderHelpBox();
    expect(container).toMatchSnapshot();
  });

  it('contains navigation items..', () => {
    const { getByText } = renderHelpBox();
    expect(getByText('Map')).toBeInTheDocument();
    expect(getByText('Test Section')).toBeInTheDocument();
  });

  it('calls setActiveTopic on click of item', async () => {
    const { getByText } = renderHelpBox();
    await waitFor(() => {
      fireEvent.click(getByText('Test Section'));
    });
    expect(setActiceTopic).toBeCalledTimes(1);
  });

  it('displays corresponding text to navigation item', () => {
    const { getByText } = render(
      <HelpBox
        setActiveTopic={setActiceTopic}
        activeTopic={'Test Section' as Topics}
        helpPage={mockPage}
      />,
    );
    expect(getByText('This is some test text')).toBeInTheDocument();
  });
});
