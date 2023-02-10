import { LinkButton } from 'components/common/buttons';
import { Input } from 'components/common/form';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { FieldArray, useFormikContext } from 'formik';
import { useState } from 'react';
import { Row } from 'react-bootstrap';
import styled from 'styled-components';

import { AcquisitionFormModal } from '../../../modals/AcquisitionFormModal';
import { AcquisitionOwnerFormModel, WithAcquisitionOwners } from '../../models';

interface IUpdateAcquisitionOwnersSubFormProps {}

export const UpdateAcquisitionOwnersSubForm: React.FunctionComponent<
  React.PropsWithChildren<IUpdateAcquisitionOwnersSubFormProps>
> = () => {
  const { values, setFieldValue } = useFormikContext<WithAcquisitionOwners>();
  const [removeIndex, setRemoveIndex] = useState<number>(-1);
  const [showRemoveOwnerModal, setShowRemoveOwnerModal] = useState<boolean>(false);

  return (
    <>
      <FieldArray
        name="owners"
        render={arrayHelpers => (
          <>
            {values.owners.map((owner, index) => (
              <Row key={`onwer-parent-${index}`} className="py-3">
                <SectionField label="Given names">
                  <LargeInput field={`owners[${index}].givenName`} />
                </SectionField>
                <SectionField label="Last name/Corporation name">
                  <LargeInput field={`owners[${index}].lastNameOrCorp1`} />
                </SectionField>
                <SectionField label="Other name">
                  <LargeInput field={`owners[${index}].lastNameOrCorp2`} />
                </SectionField>
                <SectionField label="Incorporation number">
                  <LargeInput field={`owners[${index}].incorporationNumber`} />
                </SectionField>
              </Row>
            ))}
            <LinkButton
              data-testid="add-file-owner"
              onClick={() => {
                const owner = new AcquisitionOwnerFormModel();
                arrayHelpers.push(owner);
              }}
            >
              + Add owner
            </LinkButton>

            <AcquisitionFormModal
              message="Are you sure you want to remove this row?"
              title="Remove Owner"
              display={showRemoveOwnerModal}
              handleOk={() => {
                setShowRemoveOwnerModal(false);
                arrayHelpers.remove(removeIndex);
                setRemoveIndex(-1);
              }}
              handleCancel={() => {
                setShowRemoveOwnerModal(false);
                setRemoveIndex(-1);
              }}
            ></AcquisitionFormModal>
          </>
        )}
      ></FieldArray>
    </>
  );
};

export default UpdateAcquisitionOwnersSubForm;

const LargeInput = styled(Input)`
  input.form-control {
    min-width: 50rem;
    max-width: 100%;
  }
`;
