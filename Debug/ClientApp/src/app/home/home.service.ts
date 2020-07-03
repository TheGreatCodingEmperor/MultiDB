import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { catchError, map, tap } from 'rxjs/operators';
import { Observable, of } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class ApiTemplateService {
  constructor(private http: HttpClient) {

  }
  private serverUrl = "http://localhost:5000/DBLink/";
  dbOption = {
    mariadb: `Server=192.168.99.100;port=3306;User Id=root;Password='Ex70400845%';Database=hd_tmp;`,
    sqlLite: "sqlLite",
    other: "other"
  }
  corsHeaders = new HttpHeaders({
    'Content-Type': 'application/json',
    'Accept': 'application/json',
    'Access-Control-Allow-Origin': '*'
  });

  getData(sql: string, db: string, connString:string) {
    console.log(sql);
    console.log(db);
    return this.http.get<any>(`${this.serverUrl}${db}/?sql=${sql}`, { headers: {connectString:connString} }).pipe(
      catchError(this.handleError<any[]>('getDBData', []))
    );
  }

  getTableData(connString:string, sql: string, db: string, col?:string, keyword?:string) {
    console.log(col);
    console.log(keyword);
    let cmd = (keyword && keyword!='')?`&col=${col}&keyword=${keyword}`:'';
    return this.http.get<any>(`${this.serverUrl}${db}/?sql=${sql}`+cmd, { headers: {connectString:connString} }).pipe(
      catchError(this.handleError<any[]>('getDBData', []))
    );
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {

      // TODO: send the error to remote logging infrastructure
      console.error(error); // log to console instead

      return of(result as T);
    };
  }
}