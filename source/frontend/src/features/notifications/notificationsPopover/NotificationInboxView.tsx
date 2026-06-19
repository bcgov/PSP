import { FC } from 'react';
import styled from 'styled-components';

import { ApiGen_Concepts_NotificationInboxItem } from '@/models/api/generated/ApiGen_Concepts_NotificationInboxItem';

import NotificationRow from './NotificationRow';

export interface INotificationInboxViewProps {
  items: ApiGen_Concepts_NotificationInboxItem[];
  hasMore: boolean;
  isLoading: boolean;
  onLoadMore: () => void;
  onSelect: (notification: ApiGen_Concepts_NotificationInboxItem) => void;
  onToggleRead: (notification: ApiGen_Concepts_NotificationInboxItem) => void;
  onDelete: (notification: ApiGen_Concepts_NotificationInboxItem) => void;
}

export const NotificationInboxView: FC<INotificationInboxViewProps> = ({
  items,
  hasMore,
  isLoading,
  onLoadMore,
  onSelect,
  onToggleRead,
  onDelete,
}) => {
  if (!isLoading && items.length === 0) {
    return <EmptyState>You have no notifications.</EmptyState>;
  }

  return (
    <Container>
      <List>
        {items.map(item => (
          <NotificationRow
            key={item.id}
            notification={item}
            onSelect={onSelect}
            onToggleRead={onToggleRead}
            onDelete={onDelete}
          />
        ))}
        {isLoading && items.length === 0 && <EmptyState>Loading…</EmptyState>}
      </List>

      {hasMore && (
        <Footer>
          <LoadMoreButton type="button" onClick={onLoadMore} disabled={isLoading}>
            {isLoading ? 'Loading…' : 'Load more'}
          </LoadMoreButton>
        </Footer>
      )}
    </Container>
  );
};

const Container = styled.div`
  display: flex;
  flex-direction: column;
`;

const List = styled.div`
  display: flex;
  flex-direction: column;
  min-height: 40rem;
  max-height: 40rem;
  overflow-y: auto;
`;

const EmptyState = styled.div`
  padding: 2rem;
  text-align: center;
  color: ${props => props.theme.css.pimsGrey80 ?? '#555'};
`;

const Footer = styled.div`
  display: flex;
  justify-content: center;
  padding: 0.8rem 0;
  border-top: 1px solid ${props => props.theme.css.borderOutlineColor ?? '#e0e0e0'};
`;

const LoadMoreButton = styled.button`
  background: transparent;
  border: none;
  color: ${props => props.theme.css.linkColor ?? '#1a5a96'};
  font-weight: 600;
  cursor: pointer;
  padding: 0.4rem 1.2rem;

  &:hover:not(:disabled),
  &:focus:not(:disabled) {
    text-decoration: underline;
    outline: none;
  }

  &:disabled {
    color: ${props => props.theme.css.pimsGrey80 ?? '#777'};
    cursor: default;
  }
`;

export default NotificationInboxView;
