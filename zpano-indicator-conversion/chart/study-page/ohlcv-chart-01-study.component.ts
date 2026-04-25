import { ChangeDetectionStrategy, Component } from '@angular/core';
import { MatButton } from '@angular/material/button';

import { Configuration } from 'projects/mb/src/lib/charts/ohlcv-chart/template/configuration';
import { OhlcvChartComponent } from 'projects/mb/src/lib/charts/ohlcv-chart/ohlcv-chart.component';

import { TestData } from './test-data/test-data';

@Component({
  selector: 'mb-sample-ohlcv-chart-01-study',
  templateUrl: './ohlcv-chart-01-study.component.html',
  styleUrls: ['./ohlcv-chart-01-study.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [MatButton, OhlcvChartComponent]
})
export class OhlcvChart01StudyComponent {
  public showPortal = false;
  public configPrefilled: Configuration = TestData.configDataPrefilled;

  private testData: TestData = new TestData();
  public configModifiable: Configuration = this.testData.config;

  public clearData(): void {
    this.testData.clear();
    this.configModifiable = { ...this.configModifiable };
  }

  public addData1(): void {
    this.testData.addSingle();
    this.configModifiable = { ...this.configModifiable };
  }

  public addData10(): void {
    this.testData.addTen();
    this.configModifiable = { ...this.configModifiable };
  }

  public addDataAll(): void {
    this.testData.addAll();
    this.configModifiable = { ...this.configModifiable };
  }
}
