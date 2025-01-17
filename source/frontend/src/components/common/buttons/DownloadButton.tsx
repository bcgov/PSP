import { FaDownload } from 'react-icons/fa';
import { CSSProperties } from 'styled-components';

import TooltipWrapper from '../TooltipWrapper';
import { StyledIconButton } from './IconButton';

interface IDownloadButtonProps {
  /** set the text of the tooltip that appears on hover of this button */
  toolText: string;
  /** set the id of the tool tip use for on hover of the this button */
  toolId: string;
  onClick: () => void;
  title?: string;
  icon?: React.ReactNode;
  'data-testId'?: string;
  style?: CSSProperties | null;
}

export const DownloadButton: React.FunctionComponent<IDownloadButtonProps> = ({
  onClick,
  toolId,
  toolText,
  'data-testId': dataTestId,
  icon,
  style,
  title,
}) => {
  return (
    <TooltipWrapper tooltipId={toolId} tooltip={toolText}>
      <StyledIconButton
        variant="primary"
        title={title ?? 'Download'}
        onClick={onClick}
        data-testid={dataTestId ?? 'download-button'}
        style={style}
      >
        {icon ?? <FaDownload size={20} />}
      </StyledIconButton>
    </TooltipWrapper>
  );
};

export default DownloadButton;
