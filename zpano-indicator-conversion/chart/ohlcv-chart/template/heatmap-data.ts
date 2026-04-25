import { Heatmap } from '../../entities/heatmap';

/** Describes a heatmap in a pane. */
export class HeatmapData {
  /** A name of the data. */
  public name = '';

  /** Data array. */
  public data: Heatmap[] = [];

  /** An index of an indicator in the output data. */
  public indicator = 0;

  /** An index of an output within an indicator in the output data. */
  public output = 0;

  /** An intensity gradient:
   * - Viridis
   * - Inferno
   * - Magma
   * - Plasma
   * - Cividis
   * - Warm
   * - Cool
   * - Rainbow
   * - CubehelixDefault
   * - BuGn
   * - BuPu
   * - GnBu
   * - OrRd
   * - PuBuGn
   * - PuBu
   * - PuRd
   * - RdPu
   * - YlGnBu
   * - YlGn
   * - YlOrBr
   * - YlOrRd
   * - Blues
   * - Greens
   * - Greys
   * - Oranges
   * - Purples
   * - Reds
   */
  public gradient = 'viridis';

  /** If to invert the gradient. */
  public invertGradient = false;
}
