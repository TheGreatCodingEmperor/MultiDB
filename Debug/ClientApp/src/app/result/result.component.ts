import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { MatTableDataSource, MatPaginator } from '@angular/material';
import { ApiTemplateService } from '../home/home.service';
export interface Column{
  field: string,
  header: string
}

@Component({
  selector: 'app-result',
  templateUrl: './result.component.html',
  styleUrls: ['./result.component.css']
})
export class ResultComponent implements OnInit {
  /** SQL */
  @Input() sql:string;
  /** 選擇的DB */
  @Input() DB:string;
  @Input() cols:Column[];
  @Input() connString:string;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  /**
   * @summary 頁面顯示清單(<table mat-table>)欄位
   * @DBTag 換成 API 取出來 資料 Tag 名稱
   */
  /** UI 清單欄位 */
  displayedColumns: string[] = []
  /** UI 清單內容 */
  dataSource = new MatTableDataSource<any>();
  /** DB 選項 */
  dbOptions = []
  /** 是否顯示'查詢錯誤' */
  erroMessage = false;

  /** 查詢關鍵字 */
  keyword;
  col=this.displayedColumns[0];

  constructor(
    private apiService: ApiTemplateService
  ) { }
  /** 頁面初始 */
  ngOnInit() {
    /** 設定清單資料分頁 */
    this.dataSource.paginator = this.paginator;
    if(this.sql){
      this.getDatas();
    }
    this.setInit();
  }
  setInit(){
    if(!this.cols)return;
    this.displayedColumns = this.cols.map(i=>i.field);
    this.dbOptions = this.displayedColumns.map((i)=>{return { value: i, viewValue: i }});
    this.col=this.displayedColumns[0];
  }
  /** 根據使用者 sql、DB 查詢 */
  getDatas() {
    /** 去 api 函式庫執行 sql 查詢 */
    let col = this.col?this.col:null;
    let keyword = this.keyword?this.keyword:null;
    this.apiService.getTableData(this.connString, this.sql, this.DB, col,keyword).subscribe(
      res => {
        /** 回傳資料錯誤 */
        if (!res) {
          this.erroMessage = true;
          return;
        }
        /** 重新定義清單欄位 */
        this.cols = Object.keys(res[0]).map((i) => { return { field: i, header: i } });
        /** 重新定義清單欄位 */
        this.displayedColumns = this.cols.map(i => i.field);
        /** 重新輸入清單內容 */
        this.dataSource.data = res;
      }
    );
  }

}
