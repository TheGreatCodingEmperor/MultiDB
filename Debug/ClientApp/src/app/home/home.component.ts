import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { ApiTemplateService } from './home.service';
import { MatPaginator } from '@angular/material';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  @ViewChild( MatPaginator, { static: true }) homePaginator: MatPaginator;
  /**
   * @summary 頁面顯示清單(<table mat-table>)欄位
   * @DBTag 換成 API 取出來 資料 Tag 名稱
   */
  cols = [
    { field: 'DBTagA', header: '編號', name: "123" },
    { field: 'DBTagB', header: '姓名' },
    { field: 'DBTagC', header: '性別' },
    { field: 'DBTagD', header: '電話' },
    { field: 'DBTagE', header: 'email' },
    { field: 'DBTagF', header: '薪資' }
  ]
  // /**
  //  * @summary 頁面顯示清單(p-table) 內容，API 資料直接塞進來
  //  * @注意 不再cols裡面的資料不會show出來
  //  */
  // datas = [
  //   { DBTagA: '1', DBTagB: '2', DBTagC: '3', DBTagD: '4', DBTagE: '5', DBTagF: '6', vin: 13 },
  //   { DBTagA: '7', DBTagB: '8', DBTagC: '9', DBTagD: '10', DBTagE: '11', DBTagF: '12' },
  //   { DBTagA: '13', DBTagB: '14', DBTagC: '15', DBTagD: '16', DBTagE: '17', DBTagF: '18' }
  // ]
  /** UI 清單欄位 */
  displayedColumns: string[] = this.cols.map(i => i.field);
  /** UI 清單內容 */
  dataSource = new MatTableDataSource<any>();
  /** DB 選項 */
  dbOptions = [
    { value: 'maria', viewValue: 'mariaDB' },
    { value: 'sqlite', viewValue: 'sqlLite' },
    { value: 'postgre', viewValue: 'postGresdb' },
    { value: 'oracle', viewValue: 'oracle' },
    { value: 'sqlserver', viewValue: 'sql server' }
  ];
  /** 選擇的DB */
  DB = "maria";
  /** sql 語法 */
  sql = "";
  /** 是否顯示'查詢錯誤' */
  erroMessage = false;
  connString="";
  mod = 'sql';

  constructor(
    /** call api 服務(function 集合) */
    private apiService: ApiTemplateService
  ) { }
  /** 頁面初始 */
  ngOnInit() {
    this.dataSource.paginator = this.homePaginator;
  }
  /** 根據使用者 sql、DB 查詢 */
  getDatas() {
    this.apiService.getData(this.sql, this.DB, this.connString).subscribe(
      res => {
        if (!res) {
          this.erroMessage = true;
          return;
        }
        this.cols = Object.keys(res[0]).map((i) => { return { field: i, header: i } });
        this.displayedColumns = this.cols.map(i => i.field);
        this.dataSource.data = res;
      }
    );
  }
}
