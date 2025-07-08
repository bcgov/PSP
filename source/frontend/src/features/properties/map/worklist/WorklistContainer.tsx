import { IWorklistViewProps } from './WorklistView';

export interface IWorklistContainerProps {
  View: React.FC<IWorklistViewProps>;
}

export const WorklistContainer: React.FC<IWorklistContainerProps> = ({ View }) => {
  // FIXME:
  return <View></View>;
};
