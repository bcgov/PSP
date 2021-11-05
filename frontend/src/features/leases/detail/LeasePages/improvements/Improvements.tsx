import { FormSection } from 'components/common/form/styles';
import { Grid } from 'components/common/Grid';

import * as Styled from './styles';

export interface IImprovementsProps {}

export const Improvements: React.FunctionComponent<IImprovementsProps> = props => {
  return (
    <Styled.ImprovementsContainer>
      <FormSection>
        <Grid columns={2} columnGap="3rem">
          <Grid.Item>AAA</Grid.Item>
          <Grid.Item>BBB</Grid.Item>
        </Grid>
      </FormSection>
      <FormSection>CCC</FormSection>
      <FormSection>DDD</FormSection>
      <FormSection>EEE</FormSection>
    </Styled.ImprovementsContainer>
  );
};

export default Improvements;
