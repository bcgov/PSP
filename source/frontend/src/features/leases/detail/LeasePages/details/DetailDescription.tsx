import * as React from 'react';

import { TextArea } from '@/components/common/form';
import * as Styled from '@/features/leases/detail/styles';
import { withNameSpace } from '@/utils/formUtils';

export interface IDetailDescriptionProps {
  nameSpace?: string;
  disabled?: boolean;
}

export const DetailDescription: React.FunctionComponent<
  React.PropsWithChildren<IDetailDescriptionProps>
> = ({ nameSpace, disabled }) => {
  return (
    <>
      <Styled.FormDescriptionLabel>Intended Use</Styled.FormDescriptionLabel>
      <TextArea disabled={disabled} field={withNameSpace(nameSpace, 'description')} />
    </>
  );
};

export default DetailDescription;
