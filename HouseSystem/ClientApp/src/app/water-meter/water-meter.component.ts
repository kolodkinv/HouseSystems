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
  waterMeter: WaterMeter = new WaterMeter();

  error: string;

  constructor(private waterMeterService: WaterMeterService) { }

  ngOnInit() {
  }

  registerWaterMeter(waterMeter: WaterMeter){
    debugger;
    waterMeter.building.id = this.houseId;
    this.waterMeterService.addWaterMeter(waterMeter)
      .then(response => {
        debugger;
      })
      .catch(error => {
        this.error = error;
      });
  }

  cancel(){
    this.onCancel.emit();
  }
}
