import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-transactions-table',
  templateUrl: './transactions-table.component.html',
  styleUrls: ['./transactions-table.component.css']
})
export class TransactionsTableComponent implements OnInit {
  public transactions: Transaction[];

  constructor(private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string) { 
    }

  ngOnInit(): void {
    this.reloadData();
  }

  reloadData() {
    this.http.get<Transaction[]>(this.baseUrl + 'api/transactions').subscribe(result => {
      this.transactions = result;
    }, error => console.error(error));
  }
}

interface Transaction {
  id: string;
  type: number;
  amount: number;
  effectiveDate: string;
}
