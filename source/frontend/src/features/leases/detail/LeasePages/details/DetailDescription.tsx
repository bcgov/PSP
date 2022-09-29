import { TextArea } from 'components/common/form';
import * as Styled from 'features/leases/detail/styles';
import * as React from 'react';
import { withNameSpace } from 'utils/formUtils';

export interface IDetailDescriptionProps {
  nameSpace?: string;
  disabled?: boolean;
}

export const DetailDescription: React.FunctionComponent<IDetailDescriptionProps> = ({
  nameSpace,
  disabled,
}) => {
  return (
    <>
      <Styled.FormDescriptionLabel>Description</Styled.FormDescriptionLabel>
      <TextArea disabled={disabled} field={withNameSpace(nameSpace, 'description')} />
    </>
  );
};

export default DetailDescription;
