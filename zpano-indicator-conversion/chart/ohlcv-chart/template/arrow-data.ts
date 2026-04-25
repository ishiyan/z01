/** Describes an vertical arrow in a pane. */
export class ArrowData {
  /** A name of the arrow. */
  public name = '';

  /** Is an arrow points up or down. */
  public down!: boolean;

  /** Arrow time. */
  public time!: Date;

  /** An optional value to which the arrow points. */
  public value?: number;

  /** An index of an indicator in the output data. */
  public indicator = 0;

  /** An index of an output within an indicator in the output data. */
  public output = 0;

  /** A color of the arrow. */
  public color!: string;
}
