import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {WaterMeter} from "./water-meter.models";
import {WaterMeterService} from "./water-meter.service";

@Component({
  selector: 'app-water-meter',
  templateUrl: './water-meter.component.html',
  providers: [WaterMeterService]
})
export class WaterMeterComponent implements OnInit {
  @Input() houseId: number;
  @Output() onCancel = new EventEmitter();
  @Output() onCreate = new EventEmitter<WaterMeter>();
  waterMeter: WaterMeter = new WaterMeter();
  error: string;

  constructor(private waterMeterService: WaterMeterService) { }

  ngOnInit() {
  }

  registerWaterMeter(waterMeter: WaterMeter){
    waterMeter.buildingId = this.houseId;
    this.waterMeterService.addWaterMeter(waterMeter)
      .then(response => {
        this.onCreate.emit(waterMeter);
      })
      .catch(error => {
        this.error = error.error.error;
      });
  }

  cancel(){
    this.onCancel.emit();
  }

  resetError(){
    this.error = '';
  }
}
