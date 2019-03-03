import {HttpClient} from "@angular/common/http";
import {Injectable} from "@angular/core";
import {WaterMeter} from "./water-meter.models";

@Injectable()
export class WaterMeterService {
  urlWaterMeters: string = "api/WaterMeters/";

  constructor(private httpClient: HttpClient) { }

  private static responseError(error: any): Promise<any> {
    return Promise.reject(error);
  }

  addWaterMeter(meter: WaterMeter){
    return this.httpClient.post(this.urlWaterMeters, meter)
      .toPromise()
      .catch(WaterMeterService.responseError);
  }
}
