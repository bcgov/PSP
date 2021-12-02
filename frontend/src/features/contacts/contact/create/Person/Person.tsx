import { FormSection } from 'components/common/form/styles';
import { useFormikContext } from 'formik';
import { IFormPerson } from 'interfaces/IFormPerson';

import * as Styled from './styles';

export interface IPersonProps {}

export const Person: React.FunctionComponent<IPersonProps> = props => {
  const { values } = useFormikContext<IFormPerson>();
  // const improvements: ILeaseImprovement[] = getIn(values, 'improvements') ?? [];

  return (
    <Styled.CreatePersonLayout>
      <FormSection></FormSection>

      <FormSection></FormSection>
    </Styled.CreatePersonLayout>
  );
};

export default Person;
