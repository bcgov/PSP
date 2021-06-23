import { INVENTORY_POLICY_URL } from 'constants/strings';
import { SresManual } from 'features/projects/common/components/SresManual';
import styled from 'styled-components';

const InventoryPolicyContainer = styled.div`
  display: flex;
  align-items: center;
  align-content: center;
`;

const InventoryPolicyLabel = styled.small`
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  font-weight: bold;
`;

/**
 * Property Inventory Policy link for CITZ PIMS
 */
export const InventoryPolicy = () => (
  <InventoryPolicyContainer>
    <SresManual hideText={true} clickUrl={INVENTORY_POLICY_URL} />
    <InventoryPolicyLabel className="p-1 mr-2">Inventory Policy</InventoryPolicyLabel>
  </InventoryPolicyContainer>
);
