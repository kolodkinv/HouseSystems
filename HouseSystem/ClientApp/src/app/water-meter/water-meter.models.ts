import {House} from "../house/house.models";

export class WaterMeter {
  id: string;
  value: number;
  building: House;

  constructor(){
    this.id = "";
    this.value = 0;
    this.building = new House();
  }
}
