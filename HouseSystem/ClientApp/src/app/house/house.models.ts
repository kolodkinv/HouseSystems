import {WaterMeter} from "../water-meter/water-meter.models";

// Дом
export class House {
  id: number;             // Ид дома
  address: string;        // Адрес дома
  waterMeter: WaterMeter; // Водяной счетчик, который установлен в доме
}
