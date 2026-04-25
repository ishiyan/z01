export class TimePane {
  group: any;
  timeScale: any;
  timeAxis: any;
  timeAnnotation: any;

  draw(): void {
    this.group.call(this.timeAxis);
  }
}
