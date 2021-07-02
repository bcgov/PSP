/**
 * Property classifications, identify the purpose of the property.
 */
export enum Classifications {
  /** The property is currently being used. */
  CoreOperational = 0,
  /** The property is not currently being used but has a strategic purpose. */
  CoreStrategic = 1,
  /** The property has been disposed. */
  Disposed = 4,
  /** The property has been demolished */
  Demolished = 5,
  /** The property has been subdivided */
  Subdivided = 6,
}
