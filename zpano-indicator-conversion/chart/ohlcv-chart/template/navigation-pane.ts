/** Describes a navigation pane. */
export class NavigationPane {
  /**
   * A height of the pane.
   * Can be either a positive number of pixels or a percentage string (e.g. '45%') of a reference width.
   */
  public height: string | number = 30;

  /** An optional minimal height of the pane in pixels. */
  public heightMin?: number;

  /** An optional maximal height of the pane in pixels. */
  public heightMax?: number;

  /** If navigation pane has closing price line. */
  public hasLine = true;

  /** If navigation pane has closing price area. */
  public hasArea = false;

  /** If navigation pane has time axis. */
  public hasTimeAxis = true;

  /** An optional d3.timeFormat specifier for time axis ticks, e.g. '%Y-%m-%d'. */
  public timeTicksFormat?: string;

  /** An optional number of ticks in the time axis. */
  public timeTicks?: number;
}

