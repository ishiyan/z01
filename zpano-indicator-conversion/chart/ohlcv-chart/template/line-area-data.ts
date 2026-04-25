import { Scalar } from '../../../data/entities/scalar';

/** Describes an area in a pane between a line and a constant value. */
export class LineAreaData {
  /** A name of the data. */
  public name = '';

  /** Data array. */
  public data: Scalar[] = [];

  /** An index of an indicator in the output data. */
  public indicator = 0;

  /** An index of an output within an indicator in the output data. */
  public output = 0;

  /** A constant limiting value. */
  public value = 0;

  /** A fill color of an area. */
  public color = 'rgba(0,0,0,0.0667)';

  /**
   *  The fill color of an area may be very transprent and its visibility on a legend may be poor.
   *  This allows to specify a legend color different from the fill color.
   */
  public legendColor?: string;

  /** An erea edge interpoltion method:
   * - linear
   * - natural
   * - basis
   * - camullRom
   * - cardinal
   * - step
   * - stepBefore
   * - stepAfter
   */
  public interpolation = 'natural';
}
