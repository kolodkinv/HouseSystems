import {HttpClient} from "@angular/common/http";
import {Injectable} from "@angular/core";
import {House} from "./house.models";

@Injectable()
export class HouseService {

  urlHouses: string = "api/Houses/";

  constructor(private httpClient: HttpClient) { }

  private static responseError(error: any): Promise<any> {
    return Promise.reject(error);
  }

  getHouses(){
    return this.httpClient.get(this.urlHouses)
      .toPromise()
      .catch(HouseService.responseError);
  }

  getHouseWithMaxWater(){
    return this.httpClient.get(this.urlHouses + "?max=water")
      .toPromise()
      .catch(HouseService.responseError)
  }

  addHouse(house: House){
    return this.httpClient.post(this.urlHouses, house)
      .toPromise()
      .catch(HouseService.responseError)
  }
}
