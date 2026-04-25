import { Band } from '../../entities/band';

/** Describes a band in a pane. */
export class BandData {
  /** A name of the data. */
  public name = '';

  /** Data array. */
  public data: Band[] = [];

  /** An index of an indicator in the output data. */
  public indicator = 0;

  /** An index of an output within an indicator in the output data. */
  public output = 0;

  /** A fill color of the band. */
  public color = 'rgba(0,0,0,0.0667)';

  /**
   *  The fill color of the band may be very transprent and its visibility on a legend may be poor.
   *  This allows to specify a legend color different from the fill color.
   */
  public legendColor?: string;

  /** A band edge interpoltion method:
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
