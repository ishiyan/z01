import { VerticalLayoutBlock } from './vertical-layout-block';

export class VerticalLayout {
  public height = 0;
  public pricePane: VerticalLayoutBlock = { top: 0, height: 0 };
  public indicatorPanes: VerticalLayoutBlock[] = [];
  public timeAxis: VerticalLayoutBlock = { top: 0, height: 0 };
  public navigationPane: VerticalLayoutBlock = { top: 0, height: 0 };
}
