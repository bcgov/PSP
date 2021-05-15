import { INVENTORY_POLICY_URL } from 'constants/strings';
import styled from 'styled-components';
import { SresManual } from 'features/projects/common/components/SresManual';

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

export const InventoryPolicy = () => (
  <InventoryPolicyContainer>
    <SresManual hideText={true} clickUrl={INVENTORY_POLICY_URL} />
    <InventoryPolicyLabel className="p-1 mr-2">Inventory Policy</InventoryPolicyLabel>
  </InventoryPolicyContainer>
);
