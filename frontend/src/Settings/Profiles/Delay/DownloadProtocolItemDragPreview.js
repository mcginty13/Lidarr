import PropTypes from 'prop-types';
import React, { Component } from 'react';
import { DragLayer } from 'react-dnd';
import DragPreviewLayer from 'Components/DragPreviewLayer';
import { DOWNLOAD_PROTOCOL_ITEM } from 'Helpers/dragTypes';
import dimensions from 'Styles/Variables/dimensions.js';
import DownloadProtocolItem from './DownloadProtocolItem';
import styles from './DownloadProtocolItemDragPreview.css';

const formGroupSmallWidth = parseInt(dimensions.formGroupSmallWidth);
const formLabelSmallWidth = parseInt(dimensions.formLabelSmallWidth);
const formLabelRightMarginWidth = parseInt(dimensions.formLabelRightMarginWidth);
const dragHandleWidth = parseInt(dimensions.dragHandleWidth);

function collectDragLayer(monitor) {
  return {
    item: monitor.getItem(),
    itemType: monitor.getItemType(),
    currentOffset: monitor.getSourceClientOffset()
  };
}

class DownloadProtocolItemDragPreview extends Component {

  //
  // Render

  render() {
    const {
      item,
      itemType,
      currentOffset
    } = this.props;

    if (!currentOffset || itemType !== DOWNLOAD_PROTOCOL_ITEM) {
      return null;
    }

    // The offset is shifted because the drag handle is on the right edge of the
    // list item and the preview is wider than the drag handle.

    const { x, y } = currentOffset;
    const handleOffset = formGroupSmallWidth - formLabelSmallWidth - formLabelRightMarginWidth - dragHandleWidth;
    const transform = `translate3d(${x - handleOffset}px, ${y}px, 0)`;

    const style = {
      position: 'absolute',
      WebkitTransform: transform,
      msTransform: transform,
      transform
    };

    const {
      id,
      name,
      allowed,
      delay
    } = item;

    return (
      <DragPreviewLayer>
        <div
          className={styles.dragPreview}
          style={style}
        >
          <DownloadProtocolItem
            isPreview={true}
            id={id}
            name={name}
            allowed={allowed}
            delay={delay}
            isDragging={false}
          />
        </div>
      </DragPreviewLayer>
    );
  }
}

DownloadProtocolItemDragPreview.propTypes = {
  item: PropTypes.object,
  itemType: PropTypes.string,
  currentOffset: PropTypes.shape({
    x: PropTypes.number.isRequired,
    y: PropTypes.number.isRequired
  })
};

export default DragLayer(collectDragLayer)(DownloadProtocolItemDragPreview);