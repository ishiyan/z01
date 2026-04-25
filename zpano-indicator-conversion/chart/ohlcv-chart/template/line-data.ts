import { Scalar } from '../../../data/entities/scalar';

/** Describes a line in a pane. */
export class LineData {
  /** A name of the data. */
  public name = '';

  /** Data array. */
  public data: Scalar[] = [];

  /** An index of an indicator in the output data. */
  public indicator = 0;

  /** An index of an output within an indicator in the output data. */
  public output = 0;

  /** A color of the line stroke. */
  public color = 'black';

  /** A width of the line stroke in pixels. */
  public width = 1;

  /** A dash array of the line stroke, e.g. '5,5' or '20,10,5,5,5,10' or empty if no dashes. */
  public dash = '';

  /** A line curve interpoltion method:
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
