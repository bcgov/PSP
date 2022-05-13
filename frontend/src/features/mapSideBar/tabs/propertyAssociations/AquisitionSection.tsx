import * as React from 'react';

import { Section } from '../Section';

export interface IAquisitionSectionProps {
  aquisitionFiles: any[];
}

const AssociationSection: React.FunctionComponent<IAquisitionSectionProps> = props => {
  return <Section header="Acquisition Files">A list!</Section>;
};

export default AssociationSection;
