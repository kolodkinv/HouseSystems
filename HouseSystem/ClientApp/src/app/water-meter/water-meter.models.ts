import {House} from "../house/house.models";

// Водяной счетчик
export class WaterMeter {
  id: number;         // Заводской номер счетчика
  value: number;      // Показания счетчика
  building: House;    // Строение в котором установлен счетчик
  buildingId:number;  // Id строения в котором установлен счетчик

  constructor(){
    this.id = 0;
    this.value = 0;
    this.building = new House();
  }
}
