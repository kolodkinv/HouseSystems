import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {WaterMeter} from "./water-meter.models";
import {WaterMeterService} from "./water-meter.service";

@Component({
  selector: 'app-water-meter',
  templateUrl: './water-meter.component.html',
  providers: [WaterMeterService]
})
export class WaterMeterComponent implements OnInit {
  @Input() houseId: number;                           // Ид дома в котором регистрируется счетчик
  @Output() onCancel = new EventEmitter();            // Событие того регистрация счетчика отменена
  @Output() onCreate = new EventEmitter<WaterMeter>();// Событие успешной регистрации счетчика
  waterMeter: WaterMeter = new WaterMeter();          // Регистрируемый счетчик
  error: string;                                      // Ошибка

  constructor(private waterMeterService: WaterMeterService) { }

  ngOnInit() {
  }

  // Регистрация водяного счетчика
  registerWaterMeter(waterMeter: WaterMeter){
    if(waterMeter.id <= 0){
      this.error = "Некорректный номер счетчика"
    }

    if(waterMeter.value < 0){
      this.error = "Некорректный значение показателей счетчика"
    }

    if(!this.error){
      waterMeter.buildingId = this.houseId;
      this.waterMeterService.addWaterMeter(waterMeter)
        .then(response => {
          this.onCreate.emit(waterMeter);
        })
        .catch(error => {
          this.error = error.error.error;
        });
    }
  }

  // Отмена режима регистрации счетчика
  cancel(){
    this.onCancel.emit();
  }

  // Сбросить ошибку
  resetError(){
    this.error = '';
  }
}
