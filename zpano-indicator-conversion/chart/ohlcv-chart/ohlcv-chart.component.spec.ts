import { provideZonelessChangeDetection } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';

import { OhlcvChartComponent } from './ohlcv-chart.component';
import { Configuration } from './template/configuration';

// ng test mb  --code-coverage --include='**/ohlcv-chart/*.spec.ts'

const getConfig = (): Configuration => ({
  width: '100%', widthMin: 360,
  navigationPane: {
    height: 30, heightMin: 30, heightMax: 30,
    hasLine: false, hasArea: true, hasTimeAxis: false, timeTicks: 0,
  },
  heightNavigationPane: 30,
  timeAnnotationFormat: '%Y-%m-%d',
  axisLeft: true,
  axisRight: false,
  margin: { left: 0, top: 10, right: 20, bottom: 0 },
  ohlcv: { name: '', data: [], candlesticks: false },
  pricePane: {
    height: '30%', heightMin: 300, heightMax: 800,
    valueMarginPercentageFactor: 0.01, valueFormat: ',.2f',
    bands: [],
    lineAreas: [],
    horizontals: [],
    arrows: [],
    lines: []
  },
  indicatorPanes: [],
  crosshair: false,
  volumeInPricePane: false,
  menuVisible: true, downloadSvgVisible: true
});

describe('OhlcvChartComponent', () => {
  let component: OhlcvChartComponent;
  let fixture: ComponentFixture<OhlcvChartComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OhlcvChartComponent],
      providers: [
        provideZonelessChangeDetection(),
        provideHttpClient(),
        provideHttpClientTesting(),
      ]
    }).compileComponents();
  });
  
  beforeEach(async () => {
    fixture = TestBed.createComponent(OhlcvChartComponent);
    fixture.componentRef.setInput('configuration', getConfig());
    await fixture.whenStable();
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
