import {House} from "../house/house.models";

export class WaterMeter {
  id: number;
  value: number;
  building: House;
  buildingId:number;

  constructor(){
    this.id = 0;
    this.value = 0;
    this.building = new House();
  }
}
