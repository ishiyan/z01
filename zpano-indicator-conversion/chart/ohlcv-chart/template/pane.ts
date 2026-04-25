import { HeatmapData } from './heatmap-data';
import { BandData } from './band-data';
import { LineAreaData } from './line-area-data';
import { HorizontalData } from './horizontal-data';
import { LineData } from './line-data';
import { ArrowData } from './arrow-data';

/** Describes a pane. */
export class Pane {
  /**
   * A height of the pane.
   * Can be either a positive number of pixels or a percentage string (e.g. '45%') of a reference width.
   */
  public height: string | number = 0;

  /** An optional minimal height of the pane in pixels. */
  public heightMin?: number;

  /** An optional maximal height of the pane in pixels. */
  public heightMax?: number;

  /** A d3.format specifier for value ticks and annotations on the pane. */
  public valueFormat = ',.2f';

  /** An optional number of ticks in the value axis. */
  public valueTicks?: number;

  /**
   * A percentage factor (e.g., 0.05) to add to a lower and upper parts of a value axis.
   * This allows to add space between the top / bottom of the pane and the max / min values.
   */
  public valueMarginPercentageFactor = 0;

  /** An optional heatmap on this pane. */
  public heatmap?: HeatmapData;

  /** An array of indicator bands on this pane. */
  public bands: BandData[] = [];

  /** An array of indicator line areas on this pane. */
  public lineAreas: LineAreaData[] = [];

  /** An array of indicator bands on this pane. */
  public horizontals: HorizontalData[] = [];

  /** An array of indicator lines on this pane. */
  public lines: LineData[] = [];

  /** An array of arrows on this pane. */
  public arrows: ArrowData[] = [];
}
