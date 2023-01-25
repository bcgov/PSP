import { Section } from 'features/mapSideBar/tabs/Section';
import { getIn, useFormikContext } from 'formik';
import { IProjectForm } from 'interfaces/IProject';
import { withNameSpace } from 'utils/formUtils';

export interface IProjectDetailInformationProps {
  nameSpace?: string;
}

export const ProjectDetailInformation: React.FunctionComponent<
  React.PropsWithChildren<IProjectDetailInformationProps>
> = ({ nameSpace }) => {
  const { values } = useFormikContext<IProjectForm>();
  const summary = getIn(values, withNameSpace(nameSpace, 'note'));

  return <Section>This works!{summary}</Section>;
};
